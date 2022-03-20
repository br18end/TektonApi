using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using TektonApi.Interface;

namespace TektonApi.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;

        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is ICacheable cacheable)
            {
                if (string.IsNullOrEmpty(cacheable.Key))
                {
                    throw new ArgumentNullException(nameof(cacheable.Key));
                }

                TResponse response;

                var cachedResponse = await _cache.GetAsync(cacheable.Key, cancellationToken);

                if (cachedResponse != null)
                {
                    response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                    _logger.LogInformation($"Fetched: {cacheable.Key}");
                }
                else
                {
                    response = await next();
                    var serialized = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
                    await _cache.SetAsync(cacheable.Key, serialized, cancellationToken);
                    _logger.LogInformation($"Added: {cacheable.Key}");
                }

                return response;
            }

            return await next();
        }
    }
}
