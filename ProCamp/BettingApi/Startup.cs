using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Constants;
using CommonLibrary.Helpers;
using CommunicationLibrary.Services;
using CommunicationLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BettingApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly string ApiName = "BettingApi";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostingEnv"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
            Configuration = configuration;

            var builder = new ConfigurationBuilder();

            if (_hostingEnv.IsEnvironment("Development"))
            {
                builder.AddUserSecrets<Startup>();
            }
            
            builder.AddConfiguration(configuration);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(options => { AppSetup.SetupSwagger(options, ApiName, _hostingEnv); });
            services.AddHttpClient(ServiceNames.Fixtures, cfg =>
            {
                cfg.BaseAddress = new Uri(FixtruesAddress);
                cfg.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient<AuthService>(ServiceNames.Auth,cfg =>
            {
                cfg.BaseAddress = new Uri(AuthAddress);
                cfg.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddSingleton<IFixtureService, FixturesService>();
            services.AddSingleton<IAuthService, AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName));
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private string FixtruesAddress => $"http://localhost:{Ports.FixturesPort}/api/v1/fixtures/";
        private string AuthAddress => $"http://localhost:{Ports.AuthPort}/api/v1/auth/";
    }
}