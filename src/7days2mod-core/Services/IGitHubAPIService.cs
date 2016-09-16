using System.Threading.Tasks;

namespace _7days2mod_core.Services
{
    public interface IGitHubAPIService
    {
        Task<dynamic> requestAsync(string mode = "GET");

    }
}
