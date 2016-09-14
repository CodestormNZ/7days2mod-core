using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _7days2mod_core
{
    public class GitHubOAuthOptions
    {
        public string authEndpoint { get; set; } //"https://github.com/login/oauth/authorize"
        public string verifyEndpoint { get; set; } //"https://github.com/login/oauth/access_token"
        public string authRoute { get; set; } //"/Authenticate/"
        public string verifyRoute { get; set; } //"/Authenticate/Verify/"
        public string baseUrl { get; set; }
        public string scope { get; set; }
        public string redirectURI { get; set; }
    }
}
