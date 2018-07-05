using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar
{
    abstract class BaseAuth
    {
        public string nonce = GetNonce();
        public string timestamp = GetTimestamp();

        protected static string GetNonce()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }

        protected static string GetTimestamp()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
    }

    class DeviceAuth : BaseAuth
    {
        public string client_id;
        public string device_id;
        public string grant_type;
        public string username;
        public string password;
        public string scope = "onstar gmoc commerce msso";
    }


    class PinAuth : BaseAuth
    {
        public string credential;
        public string device_id;
        public string scope = "onstar commerce";
        public string credential_type = "PIN";
        public string client_id;
    }
}
