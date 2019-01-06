using NOnStar.Types;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NOnStar.CommandAndControl;
using Newtonsoft.Json;

namespace NOnStar
{
    public class OnStarClient
    {
        static string ClientId = "OMB_CVY_AND_3F1";
        static string JwtSecretKey = "ZYvGs3YCGrWn7DAd4Eho";
        static string DeviceId = "bf479a40-1f6a-4677-a6de-88b45a46fbae"; //Make up your own GUID
        string username;
        string password;
        string pin;

        IJsonSerializer serializer;
        IDateTimeProvider provider;
        IJwtValidator validator;
        IBase64UrlEncoder urlEncoder;
        IJwtDecoder decoder;
        HttpClient client;
        Action<string> logger = (message) => Console.WriteLine(message);

        public OnStarClient(string username, string password, string pin)
        {
            this.username = username;
            this.password = password;
            this.pin = pin;

            serializer = new JsonNetSerializer();
            provider = new UtcDateTimeProvider();
            validator = new JwtValidator(serializer, provider);
            urlEncoder = new JwtBase64UrlEncoder();
            decoder = new JwtDecoder(serializer, validator, urlEncoder);
        }

        public void SetupLogging(Action<string> logger)
        {
            this.logger = logger;
        }

        #region Public Interface
        public async Task<CommandRequestStatus> StartVehical()
        {
            return await RequestCommand(KnownCommand.Start);
        }

        public async Task<CommandRequestStatus> StopVehical()
        {
            return await RequestCommand(KnownCommand.CancelStart);
        }

        public async Task<CommandRequestStatus> LockVehical()
        {
            return await RequestCommand(KnownCommand.LockDoor);
        }

        public async Task<CommandRequestStatus> UnlockVehical()
        {
            return await RequestCommand(KnownCommand.UnlockDoor);
        }

        #endregion

        //Model: Request command maps input command to type received from server, then Triggers command which involves monitoring it's success
        private async Task<CommandRequestStatus> RequestCommand(KnownCommand command)
        {
            try
            {
                var loginResponse = await LogInAndGetDetailedVehicalInfo();
                var detailedVehicalInfoStatus = loginResponse as CommandRequestStatus<DetailedVehialInfo>;

                if (detailedVehicalInfoStatus == null || detailedVehicalInfoStatus.Successful == false)
                {
                    return loginResponse;
                }
                else
                {
                    var startCommand = detailedVehicalInfoStatus.Content.vehicle.commands.command.FirstOrDefault(c => c.name == command);
                    await TriggerCommand(startCommand);
                }
            }
            catch (Exception ex)
            {
                logger($"Failure executing command {command.Key} - {ex}");
            }

            return CommandRequestStatus.GetSuccessful();
        }

        private async Task<CommandRequestStatus<DetailedVehialInfo>> LogInAndGetDetailedVehicalInfo()
        {
            var authObject = new DeviceAuth()
            {
                client_id = ClientId,
                device_id = DeviceId,
                grant_type = "password",
                username = username,
                password = password,
            };

            CreateHttClient();

            var loginResponse = await Login(authObject);
            if (loginResponse.Successful == false)
            {
                return CommandRequestStatus<DetailedVehialInfo>.GetFailed(loginResponse.ErrorMessage);
            }

            logger("Getting vehicals...");
            var getResponse = await client.GetAsync("https://api.gm.com/api/v1/account/vehicles");
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var vehicals = serializer.Deserialize<VehicalList>(responseBody);

            var firstCar = vehicals.vehicles.vehicle.First();

            logger($"Getting detailed info on {firstCar.vin}");
            var vehicalDetailsReply = await client.GetAsync($"https://api.gm.com/api/v1/account/vehicles/{firstCar.vin}?includeCommands=true&includeFeatures=true&includeDealers=true&includeCarrierAccount=true");
            var vehicalDetailsString = await vehicalDetailsReply.Content.ReadAsStringAsync();
            var detailedVehicalInfo = serializer.Deserialize<DetailedVehialInfo>(vehicalDetailsString);
            foreach (var item in detailedVehicalInfo.vehicle.commands.command)
            {
                logger(item.name);
                logger(item.description);
            }

            return CommandRequestStatus<DetailedVehialInfo>.GetSuccessful(detailedVehicalInfo);
        }

        private async Task TriggerCommand(Command command)
        {
            if (command.isPrivSessionRequired == true)
            {
                await Upgrade();
            }

            var monitoringUrl = await ExecuteCommand(command);

            await MonitorCommand(monitoringUrl);
        }

        private async Task<string> ExecuteCommand(Command command)
        {
            var content = GetStringContentBasedOnCommandType(command);
            var startResult = await client.PostAsync(command.url, content);
            var startResultStr = await startResult.Content.ReadAsStringAsync();

            logger($"Command reply: {startResultStr}");

            var commandResponse = serializer.Deserialize<OuterCommandResponse>(startResultStr);

            return commandResponse.commandResponse.url;
        }

        private async Task<CommandRequestStatus> MonitorCommand(string monitoringUrl)
        {
            OuterCommandResponse commandResponse = null;
            do
            {
                System.Threading.Thread.Sleep(5 * 1000);
                var commandQueryResponse = await client.GetAsync(monitoringUrl);
                var commandQueryResponseStr = await commandQueryResponse.Content.ReadAsStringAsync();
                logger(commandQueryResponseStr);
                commandResponse = serializer.Deserialize<OuterCommandResponse>(commandQueryResponseStr);
            } while (commandResponse.commandResponse.status == "inProgress");

            if( commandResponse.commandResponse.status == "failure")
            {
                logger(commandResponse.commandResponse.body.error.description);

                return CommandRequestStatus.GetFailed(commandResponse.commandResponse.body.error.description);
            }
            else
            {
                return CommandRequestStatus.GetSuccessful();
            }
        }

        private StringContent GetStringContentBasedOnCommandType(Command command)
        {
            switch(command.name)
            {
                case "start":
                case "cancelStart":
                    return new StringContent("{}");
                case "lockDoor":
                    return new StringContent("{\"lockDoorRequest\":{\"delay\":0}}");
                case "unlockDoor":
                    return new StringContent("{\"unlockDoorRequest\":{\"delay\":0}}");
                default:
                    return new StringContent("{}");
            }
        }

        private async Task Upgrade()
        {
            var pinPayload = new PinAuth() { client_id = ClientId, credential = pin, device_id = DeviceId };
            var pinToken = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .AddClaims(pinPayload)
                .WithSecret(JwtSecretKey)
                .Build();

            var pinUpgradeResult = await client.PostAsync("https://api.gm.com/api/v1/oauth/token/upgrade", new StringContent(pinToken));
            var pinUpgradeResultStr = await pinUpgradeResult.Content.ReadAsStringAsync();

            if (pinUpgradeResultStr != string.Empty)
            {
                var errorMessage = JsonConvert.DeserializeObject<UpgradeError>(pinUpgradeResultStr);

                throw new InvalidOperationException($"{errorMessage.Error} - {errorMessage.Description}");
            }
        }

        private async Task<CommandRequestStatus> Login( DeviceAuth authObject)
        {
            var loginToken = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .AddClaims(authObject)
                .WithSecret(JwtSecretKey)
                .Build();

            logger("Logging in...");
            var result = await client.PostAsync("https://api.gm.com/api/v1/oauth/token", new StringContent(loginToken));

            var loginResponse = await result.Content.ReadAsStringAsync();
            logger($"Response Token: {loginResponse}");

            if(loginResponse.Contains("error"))
            {
                var loginErrorResponse = serializer.Deserialize<LoginError>(loginResponse);
                return new CommandRequestStatus() { Successful = false, ErrorMessage = loginErrorResponse.error };
            }

            logger(decoder.Decode(loginResponse, JwtSecretKey, verify: true));
            var json = decoder.DecodeToObject<LoginReply>(loginResponse, JwtSecretKey, verify: true);

            AddAccessToken(json);

            return new CommandRequestStatus() { Successful = true};
        }

        private void AddAccessToken(LoginReply json)
        {
            logger($"Token found: {json.access_token}");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {json.access_token}");
        }

        private void CreateHttClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Language", "en");
            client.DefaultRequestHeaders.Add("Host", "api.gm.com");
            client.DefaultRequestHeaders.Add("Connection", "close");
            client.DefaultRequestHeaders.Add("User-Agent", "okhttp/3.9.0");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}
