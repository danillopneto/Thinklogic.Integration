using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net.Http.Headers;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Infrastructure.Gateways.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;
using Thinklogic.Integration.Interfaces.UseCases.Asana;
using Thinklogic.Integration.UseCases.Profiles;
using Thinklogic.Integration.UseCases.Services;

namespace Thinklogic.Integration.CrossCutting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SetupAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AsanaProfile));

            return services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            services.AddSingleton(typeof(DataAppSettings), GetSettings());

            return services;
        }

        public static IServiceCollection AddGateways(this IServiceCollection services)
        {
            services.AddScoped<IAsanaProjectsGateway, AsanaProjectsGateway>();
            services.AddScoped<IAsanaTasksGateway, AsanaTasksGateway>();
            services.AddScoped<IAsanaWorkspacesGateway, AsanaWorkspacesGateway>();

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient(NamedHttpClients.AsanaClient)
                .ConfigureHttpClient(
                    client =>
                    {
                        var provider = services.BuildServiceProvider();
                        var dataAppSettings = provider.GetService<DataAppSettings>();

                        client.BaseAddress = new Uri(dataAppSettings.UrlAsana);

                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {dataAppSettings.AsanaPersonalAccessToken}");

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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IInsertCommentAsanaTaskUseCase, InsertCommentAsanaTaskUseCase>();

            return services;
        }

        private static DataAppSettings GetSettings()
        {
            DataAppSettings settings = new();

            foreach (var setting in settings.GetType().GetProperties())
            {
                setting.SetValue(settings, Environment.GetEnvironmentVariable(setting.Name));
            }

            return settings;
        }
    }
}
