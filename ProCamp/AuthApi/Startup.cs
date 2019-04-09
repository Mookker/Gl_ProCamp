using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Managers.Implementations;
using AuthApi.Managers.Interfaces;
using AuthApi.Models;
using AuthApi.Repositories.Implementations;
using AuthApi.Repositories.Interfaces;
using AutoMapper;
using CommonLibrary.Config;
using CommonLibrary.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly string ApiName = "AuthApi";

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
            
            services.Configure<ApiKeyManagerOptions>(o => 
            {
                o.Password = Configuration.GetValue<string>("ApiKeyManagerPassword");
            });

            services.Configure<JwtOptions>(o =>
            {
                o.Iss = Configuration.GetValue<string>("iss");
                o.Key = Configuration.GetValue<string>("JwtSecret");
            });
            services.AddSwaggerGen(options => { AppSetup.SetupSwagger(options, ApiName, _hostingEnv); });

            services.Configure<RedisCacheConfiguration>(x =>
            {
                x.ConnectionString = RedisConnectionString(Configuration);
                x.Environment = "Development";
                x.ApiName = ApiName;
            });

            services.Configure<MongoConfiguration>(x =>
                x.DbName = Configuration.GetValue<string>("MongoDbName") ?? "football");
            services.AddSingleton<IMongoClient>(x => new MongoClient(MongoConnectionString(Configuration)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IRoleManager, RoleManager>();
            services.AddSingleton<IUserManager, UserManager>();
            services.AddSingleton<IApiKeyManager, ApiKeyManager>();
            services.AddSingleton<IApiKeyRepository, MongoDbApiKeyRepository>();
            services.AddSingleton<IUsersRepository, MongoDbUsersRepository>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();

            Mapper.Initialize(c =>
            {
                c.CreateMissingTypeMaps = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName));
        }

        private string RedisConnectionString(IConfiguration configuration)
        {
            var redisUri = "127.0.0.1:6379";
            if (configuration != null)
            {
                var configValue = configuration["SecretRedisConnectionString"] ??
                                  configuration.GetConnectionString("RedisConnectionString");
                if (!string.IsNullOrEmpty(configValue))
                {
                    redisUri = configValue;
                }
            }

            return redisUri;
        }

        private string MongoConnectionString(IConfiguration configuration)
        {
            var mongoConnectionString = "127.0.0.1:27017";
            if (configuration != null)
            {
                var configValue = configuration["SecretMongoConnectionString"] ??
                                  configuration.GetConnectionString("MongoConnectionString");
                if (!string.IsNullOrEmpty(configValue))
                {
                    mongoConnectionString = configValue;
                }
            }

            return mongoConnectionString;
        }
    }
}