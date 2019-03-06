﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;
using MQTTnet.Server.Logging;
using MQTTnet.Server.Mqtt;
using MQTTnet.Server.Scripting;
using MQTTnet.Server.Scripting.DataSharing;

namespace MQTTnet.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables();

            //Configuration = builder.Build();
        }
        
        public IConfigurationRoot Configuration { get; }

        public void Configure(
            IApplicationBuilder application, 
            IHostingEnvironment environment, 
            MqttServerService mqttServerService,
            PythonScriptHostService pythonScriptHostService,
            DataSharingService dataSharingService)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseHsts();
            }

            application.UseHttpsRedirection();
            application.UseMvc();

            dataSharingService.Configure();
            pythonScriptHostService.Configure();
            mqttServerService.Configure();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<PythonIOStream>();
            services.AddSingleton<PythonScriptHostService>();
            services.AddSingleton<DataSharingService>();

            services.AddSingleton<MqttNetLoggerWrapper>();
            services.AddSingleton<MqttWebSocketServerAdapter>();
            services.AddSingleton<CustomMqttFactory>();
            services.AddSingleton<MqttServerService>();

            services.AddSingleton<MqttClientConnectedHandler>();
            services.AddSingleton<MqttClientDisconnectedHandler>();
            services.AddSingleton<MqttClientSubscribedTopicHandler>();
            services.AddSingleton<MqttClientUnsubscribedTopicHandler>();
            services.AddSingleton<MqttConnectionValidator>();
            services.AddSingleton<MqttSubscriptionInterceptor>();
            services.AddSingleton<MqttApplicationMessageInterceptor>();
        }
    }
}