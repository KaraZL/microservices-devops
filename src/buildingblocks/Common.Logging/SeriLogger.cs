using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure => (context, config) =>
        {
            var elasticUri = context.Configuration["ConnectionStrings:ElasticUri"];
            var date = DateTime.UtcNow.AddHours(2).ToString("dd-MM-yyyy-HH-mm-ss");

            config
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                IndexFormat = $"applogs-" +
                                $"{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-" +
                                $"{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-" +
                                $"{date}",

                AutoRegisterTemplate = true, //register index template
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                NumberOfShards = 2,
                NumberOfReplicas = 1,
                //EmitEventFailure = EmitEventFailureHandling.ThrowException|EmitEventFailureHandling.RaiseCallback|EmitEventFailureHandling.WriteToSelfLog,
                //FailureCallback = e =>
                //{
                //    Console.WriteLine("---------> Unable to submit event " + e.MessageTemplate);
                //}
            })
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
            .ReadFrom.Configuration(context.Configuration); //appsettings.json du service - config du logging
        };

    }
}
