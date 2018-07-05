using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.Types
{
    class LoginReply
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public OnstarAccountInfo onstar_account_info { get; set; }
        public UserInfo user_info { get; set; }
        public string id_token { get; set; }
    }
}
