using GreenPipes;
using TestLocalization.BLL.Service;
using TestLocalization.Worker.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Reflection;
using TestLocalization.Worker;
using System.IO;
using TestLocalization.BLL;

namespace TestLocalization.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDomainServices();
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();
                        x.AddConsumers(Assembly.GetEntryAssembly());
                        x.UsingRabbitMq((context, config) =>
                        {
                            config.ConfigureJsonSerializer(options =>
                            {
                                options.DefaultValueHandling = DefaultValueHandling.Include;
                                return options;
                            });
                            config.Host("rabbitmq://localhost", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            #region Messaging Endpoints
                            config.ReceiveEndpoint("get_countries", e =>
                            {
                                e.ConfigureConsumer<GetCountriesConsumer>(context);
                            });
                            #endregion

                            config.ConfigureEndpoints(context);
                            config.UseRetry(retry =>
                            {
                                // introduce some randomness to avoid from DDoSing the service
                                var random = new Random();
                                var jitter = random.Next(500, 551);
                                var interval = TimeSpan.FromMilliseconds(100);
                                interval.Add(TimeSpan.FromMilliseconds(jitter));
                                retry.Interval(3, interval);
                            });
                        });
                    });
                    services.AddMassTransitHostedService(true);
                });
    }
}
