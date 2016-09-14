﻿using _7days2mod_core.Models;
using _7days2mod_core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace _7days2mod_core
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            if (_env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // App settings service
            services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));

            // Add framework services.
            services.AddMvc();
            services.AddSession();

            // Setup options with DI
            services.AddOptions();

            services.Configure<GitHubOAuthOptions>(options =>
            {
                options.authEndpoint = "https://github.com/login/oauth/authorize";
                options.verifyEndpoint = "https://github.com/login/oauth/access_token";
                options.authRoute = "/Authenticate";
                options.verifyRoute = "/Authenticate/Verify";
                options.baseUrl = "http://localhost:2500";
                options.scope = "user%20public_repo";
                options.redirectURI = "/";
            });

            // Add application services
            services.AddSingleton<IGitHubAPIService, GitHubAPIService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Development Error Pages
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Static Files
            app.UseStaticFiles();

            // Sessions
            app.UseSession();

            // Authentication
            app.UseGitHubOAuth();
//            app.UseMiddleware<GitHubOAuthMiddleware>();

            // MVC Routing
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}