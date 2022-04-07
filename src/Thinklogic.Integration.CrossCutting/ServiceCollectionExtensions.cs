using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net.Http.Headers;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Infrastructure.Gateways.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;

namespace Thinklogic.Integration.CrossCutting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGateways(this IServiceCollection services)
        {
            services.AddScoped<IAsanaProjectsGateway, AsanaProjectsGateway>();

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient(NamedHttpClients.AsanaClient)
                .ConfigureHttpClient(
                    client =>
                    {
                        var provider = services.BuildServiceProvider();
                        var apiSettings = provider.GetService<ApiSettings>();

                        client.BaseAddress = new Uri(apiSettings.UrlAsana);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }));

            return services;
        }

    }
}
