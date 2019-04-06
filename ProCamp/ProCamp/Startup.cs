using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using AutoMapper;
using CommonLibrary.Cache.Implementations;
using CommonLibrary.Cache.Interfaces;
using CommonLibrary.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using ProCamp.Managers;
using ProCamp.Managers.Cache;
using ProCamp.Managers.Cache.Interfaces;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
using ProCamp.Models.Requests;
using ProCamp.Models.Responses;
using ProCamp.Repositories.Implementations;
using ProCamp.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProCamp
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly string ApiName = "ProCampApi";

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

        /// <summary>
        /// Cfg
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters.ValidateIssuer = true;
                o.TokenValidationParameters.ValidIssuer = Configuration.GetValue<string>("iss");
                o.TokenValidationParameters.ValidateIssuerSigningKey = true;
                o.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSecret")));
                o.TokenValidationParameters.ValidateAudience = false;
                o.TokenValidationParameters.ValidateLifetime = true;
                o.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            services.Configure<JwtOptions>(o =>
            {
                o.Iss = Configuration.GetValue<string>("iss");
                o.Key = Configuration.GetValue<string>("JwtSecret");
            });
            
            services.Configure<ApiKeyManagerOptions>(o => 
            {
                o.Password = Configuration.GetValue<string>("ApiKeyManagerPassword");
            });
            services.AddSwaggerGen(options => { SetupSwagger(options, ApiName, _hostingEnv); });

            Mapper.Initialize(c =>
            {
                c.CreateMissingTypeMaps = true;
                c.CreateMap<Fixture, FixturesResponse>();
                c.CreateMap<CreateFixtureRequest, Fixture>().ForMember(m => m.Id, expression => expression.Ignore());
            });

            services.Configure<RedisCacheConfiguration>(x =>
            {
                x.ConnectionString = RedisConnectionString(Configuration);
                x.Environment = "Development";
                x.ApiName = ApiName;
            });

            services.Configure<MongoConfiguration>(x =>
                x.DbName = Configuration.GetValue<string>("MongoDbName") ?? "football");

            services.AddSingleton<IMongoClient>(x => new MongoClient(MongoConnectionString(Configuration)));

            services.AddSingleton<IBaseCache, BaseCache>();
            services.AddSingleton<IFixturesRepository, MongoDbFixturesRepository>();
            services.AddSingleton<IFixturesCacheManager, FixturesCacheManager>();
            services.AddSingleton<IFixtureManager, FixtureManager>();
            services.AddSingleton<IRoleManager, RoleManager>();
            services.AddSingleton<IUserManager, UserManager>();
            services.AddSingleton<IApiKeyManager, ApiKeyManager>();
            services.AddSingleton<IApiKeyRepository, MongoDbApiKeyRepository>();
            services.AddSingleton<IUsersRepository, MongoDbUsersRepository>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName));
        }

        private static void SetupSwagger(SwaggerGenOptions options, string apiName, IHostingEnvironment hostingEnv)
        {
            options.SwaggerDoc("v1", new Info
            {
                Title = apiName,
                Description = $"{apiName} (ASP.NET Core 2.2)",
                Version = "1.0.BUILD_NUMBER"
            });

            options.DescribeAllEnumsAsStrings();

            var comments =
                new XPathDocument(
                    $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{hostingEnv.ApplicationName}.xml");
            options.OperationFilter<XmlCommentsOperationFilter>(comments);

            options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = "header",
                Type = "apiKey",
            });
            options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            {
                {"Bearer", Enumerable.Empty<string>()}
            });
            options.IgnoreObsoleteActions();
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

