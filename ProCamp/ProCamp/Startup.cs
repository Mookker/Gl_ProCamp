using System;
using System.IO;
using System.Reflection;
using System.Xml.XPath;
using AutoMapper;
using CommonLibrary.Cache.Implementations;
using CommonLibrary.Cache.Interfaces;
using CommonLibrary.Config;
using CommonLibrary.Models.Requests;
using CommonLibrary.Models.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ProCamp.Managers;
using ProCamp.Managers.Cache;
using ProCamp.Managers.Cache.Interfaces;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
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


            services.AddSwaggerGen(options => { SetupSwagger(options, ApiName, _hostingEnv); });

            Mapper.Initialize(c =>
            {
                c.CreateMissingTypeMaps = true;
                c.CreateMap<Fixture, FixtureResponse>();
                c.CreateMap<CreateFixtureRequest, Fixture>().ForMember(m => m.Id, expression => expression.Ignore());
            });

            services.Configure<RedisCacheConfiguration>(x =>
            {
                x.ConnectionString = RedisConnectionString(Configuration);
                x.Environment = "Development";
                x.ApiName = ApiName;
            });

            services.AddSingleton<IBaseCache, BaseCache>();
            services.AddSingleton<IFixturesRepository, FixturesRepository>();
            services.AddSingleton<IFixturesCacheManager, FixturesCacheManager>();
            services.AddSingleton<IFixtureManager, FixtureManager>();
            services.Scan(scan => scan
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.Where(c =>
                    c.Name.EndsWith("Command") || c.Name.EndsWith("Query") || c.Name.EndsWith("Handler")))
                .AsSelf()
                .WithScopedLifetime());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
                Type = "apiKey"
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
    }
}

