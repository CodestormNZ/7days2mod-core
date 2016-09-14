using _7days2mod_core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace _7days2mod_core
{
    public class GitHubOAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GitHubOAuthOptions _options;
        private readonly AppSettings _appSettings;
        public string access_token { get; set; }


        public GitHubOAuthMiddleware(
            RequestDelegate next,
            IOptions<GitHubOAuthOptions> options,
            IOptions<AppSettings> appSettings
            )
        {
            _next = next;
            _options = options.Value;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            //authentication requested
            if (context.Request.Path.Value == _options.authRoute)
            {
                //create CSRF state key
                var rsa = new RSACryptoServiceProvider(512);
                var modulus = rsa.ExportParameters(false).Modulus;
                var stateKey = UrlEncoder.Default.Encode(Convert.ToBase64String(modulus));
                context.Session.Set("CSRF:State", modulus);

                //create request string
                string baseUrl = _options.baseUrl;
                var requestURL = _options.authEndpoint
                    + "?client_id=" + _appSettings.GitHubClientId
                    // TODO: add return url to redirect to current page after auth
                    + "&redirect_uri=" + baseUrl + _options.verifyRoute + "/"
                    + "&state=" + stateKey
                    + "&scope=" + _options.scope;

                //redirect to authentication endpoint
                context.Response.Redirect(requestURL);

            }
            else if (context.Request.Path.StartsWithSegments(_options.verifyRoute))
            {
                //get code and state from response
                var query = context.Request.Query;
                var code = query["code"];
                var state = query["state"];
                byte[] modulus = null;

                //get CSRF from session
                context.Session.TryGetValue("CSRF:State", out modulus);
                var returnedState = Convert.ToBase64String(modulus);

                if (state == returnedState)
                {
                    // state matches session CSRF, proceed with token aquisition
                    using (var client = new HttpClient())
                    {
                        //set headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //set post variables
                        var values = new Dictionary<string, string>
                            {
                                { "client_id", _appSettings.GitHubClientId },
                                { "client_secret", _appSettings.GitHubClientSecret },
                                { "code", code },
                                { "state", state },
                            };
                        var content = new FormUrlEncodedContent(values);

                        //post request to verification endpoint
                        HttpResponseMessage response = await client.PostAsync(_options.verifyEndpoint, content);

                        if (response.IsSuccessStatusCode)
                        {
                            //get response data
                            var responseString = await response.Content.ReadAsStringAsync();
                            dynamic responseData = JObject.Parse(responseString);

                            if (responseData.error != "")
                            {
                                //response error code detected
                                //TODO: Throw exception, log error
                                context.Response.Redirect("/Error");
                            }
                            else
                            {
                                //store response data in session, clear CSRF
                                context.Session.SetString("access_token", (string)responseData.access_token);
                                context.Session.SetString("scope", (string)responseData.scope);
                                context.Session.Remove("CSRF:State");
                            }
                        }
                    }
                }
                //post auth redirect endpoint
                context.Response.Redirect(_options.redirectURI);
            }
            else
            {
                //continue to next middleware since no auth uri detected.
                await _next.Invoke(context);
            }
        }
    }
    public static class GitHubOAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseGitHubOAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GitHubOAuthMiddleware>();
        }
    }
}
