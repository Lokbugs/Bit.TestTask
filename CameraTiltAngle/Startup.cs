using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraTiltAngle.Repository;
using CameraTiltAngle.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace CameraTiltAngle
{
    public class Startup
    {
        public const string ASPNETCORE_ENVIRONMENT_ENV_NAME = "ASPNETCORE_ENVIRONMENT";
        
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
             
            Configuration = builder.Build();
        }
        
        /// <summary>
        /// Конфигурация.
        /// </summary>
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSwaggerGen();

            var connectionString = Configuration.GetSection("ConnectionString").Value;
            
            services.AddSingleton<ICameraPositionRepository, CameraPositionRepository>(x => new CameraPositionRepository(connectionString));
            services.AddSingleton<ICameraPositionService, CameraPositionService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureLogger();
            
            app.UseSwagger();
                
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CameraTiltController");
            });
            
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{area=App}/{controller=App}/{action=Index}");
            });
        }
        
        /// <summary>
        /// Конфигурирует логгер.
        /// </summary>
        private void ConfigureLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logConsole = new NLog.Targets.ColoredConsoleTarget
            {
                Layout =
                    "[${longdate} ${level:uppercase=true}] ${logger} | ${message}${onexception:${newline}${exception:format=tostring}}"
            };
            
            config.AddRuleForAllLevels(logConsole);
            LogManager.Configuration = config;
            
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(logConsole, 
                LogLevel.FromString(Configuration.GetSection("LogLevel").Value));
        }
    }
}
