using _7days2mod_core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace _7days2mod_core.Services
{
    public class GitHubAPIService : IGitHubAPIService
    {
        public const string CacheKey = nameof(GitHubAPIService);

        private readonly IHostingEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _cache;

        public string access_token { get; set; }

        public GitHubAPIService (
            IHostingEnvironment env,
            IOptions<AppSettings> appSettings,
            IMemoryCache memoryCache
            )
        {
            _env = env;
            _appSettings = appSettings.Value;
            _cache = memoryCache;
        }

    }
}
