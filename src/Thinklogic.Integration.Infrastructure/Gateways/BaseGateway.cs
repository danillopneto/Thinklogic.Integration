using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Thinklogic.Integration.Infrastructure.Gateways
{
    public abstract class BaseGateway<T>
    {
        protected abstract string BaseClient { get; }

        protected virtual string Client { get; set; }

        protected IHttpClientFactory HttpClientFactory { get; }

        protected virtual JsonSerializerSettings JsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
            }
        }

        protected ILogger<T> Logger { get; }

        protected BaseGateway(IHttpClientFactory httpClientFactory,
                              ILogger<T> logger)
        {
            HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task<TResponse> SendGetRequest<TResponse>(string url,
                                                                       CancellationToken cancellationToken = default)
        {
            string responseContent = string.Empty;

            try
            {
                using var client = GetClient();

                HttpResponseMessage clientResponse = await client.GetAsync(url, cancellationToken);
                responseContent = await clientResponse.Content.ReadAsStringAsync(cancellationToken);

                return JsonConvert.DeserializeObject<TResponse>(responseContent, JsonSettings);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error trying to reach {Url}: {ResponseContent}", url, responseContent);
                throw;
            }
        }

        public virtual async Task<TResponse> SendPostRequest<TRequest, TResponse>(string url,
                                                                                  TRequest body,
                                                                                  CancellationToken cancellationToken = default)
        {
            var responseContent = string.Empty;

            try
            {
                using var client = GetClient();
                string json = JsonConvert.SerializeObject(body, JsonSettings);
                StringContent content = new(json, Encoding.UTF8, "application/json");

                HttpResponseMessage clientResponse = await client.PostAsync(url, content, cancellationToken);
                responseContent = await clientResponse.Content.ReadAsStringAsync(cancellationToken);
                clientResponse.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<TResponse>(responseContent, JsonSettings);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error trying to reach {Url}: {ResponseContent}", url, responseContent);
                throw;
            }
        }

        public virtual async Task SendPutRequest<TRequest>(string url,
                                                           TRequest body,
                                                           CancellationToken cancellationToken = default)
        {
            var responseContent = string.Empty;

            try
            {
                using var client = GetClient();
                string json = JsonConvert.SerializeObject(body, JsonSettings);
                StringContent content = new(json, Encoding.UTF8, "application/json");

                HttpResponseMessage clientResponse = await client.PutAsync(url, content, cancellationToken);
                responseContent = await clientResponse.Content.ReadAsStringAsync(cancellationToken);
                clientResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error trying to reach {Url}: {ResponseContent}", url, responseContent);
                throw;
            }
        }

        protected virtual HttpClient GetClient() => HttpClientFactory.CreateClient(BaseClient);
    }
}
