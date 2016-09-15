namespace _7days2mod_core
{
    public class GitHubOAuthOptions
    {
        public string authEndpoint { get; set; }
        public string verifyEndpoint { get; set; }
        public string authRoute { get; set; }
        public string verifyRoute { get; set; }
        public string logoutRoute { get; set; }
        public string baseUrl { get; set; }
        public string scope { get; set; }
        public string redirectURI { get; set; }

        public GitHubOAuthOptions ()
        {
            authEndpoint = "https://github.com/login/oauth/authorize";
            verifyEndpoint = "https://github.com/login/oauth/access_token";
            authRoute = "/login";
            verifyRoute = "/verify";
            logoutRoute = "/logout";
            baseUrl = null;
            scope = "user%20public_repo";
            redirectURI = "/";
        }
    }
}
