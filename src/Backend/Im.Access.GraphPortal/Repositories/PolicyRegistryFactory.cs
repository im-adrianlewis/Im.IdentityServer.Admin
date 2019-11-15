using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;

namespace Im.Access.GraphPortal.Repositories
{
    public class PolicyRegistryFactory : IPolicyRegistryFactory
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public PolicyRegistryFactory(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public PolicyRegistry Create()
        {
            var registry = new PolicyRegistry();

            var sqlServerCircuitBreakerPolicy = Policy
                .Handle<Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn)
                .AdvancedCircuitBreakerAsync(
                    0.5,
                    TimeSpan.FromMinutes(1),
                    10,
                    TimeSpan.FromMinutes(5),
                    CircuitBreakerOnBreak,
                    CircuitBreakerOnReset,
                    CircuitBreakerOnHalfOpen);
            registry.Add("CircuitBreaker:SqlServer", sqlServerCircuitBreakerPolicy);

            var sqlServerBulkheadPolicy = Policy.BulkheadAsync(30);
            registry.Add("Bulkhead:SqlServer", sqlServerBulkheadPolicy);

            registry.Add("SqlConnection", Policy.WrapAsync(
                // Retry: with exponential retry with jitter
                Policy
                    .Handle<Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn)
                    .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                        medianFirstRetryDelay: TimeSpan.FromMilliseconds(200),
                        retryCount: 5,
                        fastFirst: true)),

                // Circuit breaker: fails only on SQL Server transient exceptions
                sqlServerCircuitBreakerPolicy,

                // Timeout: 5 seconds per try - long running queries go elsewhere
                Policy.TimeoutAsync(5),

                // Bulkhead: At most 20 simultaneous threads
                sqlServerBulkheadPolicy));

            registry.Add("HttpRetryWithJitter", Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r =>
                {
                    var transientHttpStatusCodes =
                        new[]
                        {
                            HttpStatusCode.RequestTimeout,
                            HttpStatusCode.InternalServerError,
                            HttpStatusCode.BadGateway,
                            HttpStatusCode.ServiceUnavailable,
                            HttpStatusCode.GatewayTimeout
                        };
                    return transientHttpStatusCodes.Contains(r.StatusCode);
                })
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                    medianFirstRetryDelay: TimeSpan.FromMilliseconds(30),
                    retryCount: 5,
                    fastFirst: true)));

            return registry;
        }

        private void CircuitBreakerOnBreak(Exception exception, CircuitState state, TimeSpan timeSpan, Context context)
        {
            if (exception != null)
            {
                _logger.LogWarning(exception, "Circuit breaker [{0}:BREAK] due to exception", context.PolicyKey);
            }
            else
            {
                _logger.LogWarning("Circuit breaker [{0}:BREAK] due to manual", context.PolicyKey);
            }
        }

        private void CircuitBreakerOnReset(Context context)
        {
            _logger.LogWarning("Circuit breaker [{0}:RESET]", context.PolicyKey);
        }

        private void CircuitBreakerOnHalfOpen()
        {
            _logger.LogWarning("Circuit breaker [HALF-OPEN]");
        }
    }
}