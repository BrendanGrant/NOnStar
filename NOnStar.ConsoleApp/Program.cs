using System;

namespace NOnStar.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //must specify email address, password & PIN used for OnStar
            var client = new OnStarClient("your_email_address", "onstar_password", "pin");

            //Uncomment and try one:
            //client.UnlockVehical().GetAwaiter().GetResult();
            //client.LockVehical().GetAwaiter().GetResult();
            //client.StartVehical().GetAwaiter().GetResult();
            //client.StopVehical().GetAwaiter().GetResult();

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
