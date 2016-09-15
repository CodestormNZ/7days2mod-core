using _7days2mod_core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace _7days2mod_core.ViewComponents
{
    public class AuthNavData
    {
        public string token;
        public string accessURL;
        public string logoutURL;
        public string profileURL;
        public string avatarURL;
        public string username;
    }

    public class AuthNavViewComponent : ViewComponent
    {
        private readonly AppSettings _appSettings;

        public AuthNavViewComponent(
            IOptions<AppSettings> appSettings
            )
        {
            _appSettings = appSettings.Value;
        }

        public IViewComponentResult Invoke()
        {
            AuthNavData _authNavData = new AuthNavData();
            try
            {
                _authNavData.token = HttpContext.Session.GetString("access_token");
            }
            catch
            {
                return View("NoAuth");
            }
            if (String.IsNullOrEmpty(_authNavData.token))
            {
                return View("NoAuth");
            } else
            {
                _authNavData.accessURL = "https://github.com/settings/connections/applications/" + _appSettings.GitHubClientId;
                _authNavData.logoutURL = "/logout";

                return View("Authorized", _authNavData);
            }
        }
    }
}
