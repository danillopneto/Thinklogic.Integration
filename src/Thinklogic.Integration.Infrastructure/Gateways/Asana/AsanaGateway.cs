using Microsoft.Extensions.Logging;
using Thinklogic.Integration.Infrastructure.Configurations;

namespace Thinklogic.Integration.Infrastructure.Gateways.Asana
{
    public abstract class AsanaGateway<T> : BaseGateway<T>
    {
        protected sealed override string BaseClient => NamedHttpClients.AsanaClient;

        public AsanaGateway(IHttpClientFactory httpClientFactory,
                            ILogger<T> logger)
            : base(httpClientFactory, logger)
        {
        }
    }
}
