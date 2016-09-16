using _7days2mod_core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace _7days2mod_core.Services
{
    public class GitHubAPIService : IGitHubAPIService
    {
        public const string CacheKey = nameof(GitHubAPIService);

        private readonly IHostingEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public GitHubAPIService (
            IHostingEnvironment env,
            IOptions<AppSettings> appSettings,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _env = env;
            _appSettings = appSettings.Value;
            _cache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<dynamic> requestAsync(string mode = "GET")
        {
            using (var client = new HttpClient())
            {
                string token = null;
                try
                {
                     token = _session.GetString("access_token");
                }
                catch
                {
                    //TODO: return exception, auth required
                    return null;
                }

                if (!string.IsNullOrEmpty(token))
                {

                    client.BaseAddress = new Uri("https://api.github.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(_appSettings.GitHubAppName);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = null;
                    if (mode == "GET") {
                        response = await client.GetAsync("user");
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return JObject.Parse(responseString);
                    }
                }
            }
            return null;
        }

        public void getOrg ()
        {

        }
        public void getRepo()
        {

        }
        public void getUser()
        {

        }
        public void getAuthUser()
        {

        }
    }
}
