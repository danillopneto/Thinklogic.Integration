using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Thinklogic.Integration.CrossCutting;

[assembly: FunctionsStartup(typeof(Thinklogic.Integration.Functions.Startup))]

namespace Thinklogic.Integration.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddConfigurations()
                            .AddGateways()
                            .AddHttpClients()
                            .AddServices()
                            .SetupAutoMapper();
        }
    }
}
