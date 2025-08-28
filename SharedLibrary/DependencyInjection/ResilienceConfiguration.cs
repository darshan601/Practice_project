using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using SharedLibrary.Logs;
using SharedLibrary.Resilience;

namespace SharedLibrary.DependencyInjection;

public static class ResilienceConfiguration
{
    public static IServiceCollection AddServiceResiliencePipelines(this IServiceCollection services,
        IConfiguration configuration)
    {
        // load configuration from settings
        var settings = new ResilienceSettings();
        
        // register strategy factories with proper telemetry
        services.AddResiliencePipeline("api-pipeline", builder =>
        {
            // add retry policy
            builder.AddRetry(new RetryStrategyOptions
            {
                // handle common http and timeout exceptions
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>()
                    .Handle<TaskCanceledException>(),
                // exponential backoff with jitter
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                MaxRetryAttempts = settings.ServiceRetry.MaxAttempts,
                Delay = TimeSpan.FromMilliseconds(settings.ServiceRetry.InitialDelayMs),

                // simple logging
                OnRetry = args =>
                {
                    string message =
                        $"Retry attempt {args.AttemptNumber} after {args.RetryDelay.TotalMilliseconds} ms , Outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }

            });
            
            // add circuit breaker
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                // Handle server error
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>(),
                    
                
                FailureRatio = settings.CircuitBreaker.FailureThresholdPercentage /100.0,
                SamplingDuration = TimeSpan.FromSeconds(settings.CircuitBreaker.SamplingDurationSeconds),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(settings.CircuitBreaker.BreakDurationSeconds),
                
                // state changed logging
                OnOpened = args =>
                {
                    string message =
                        $"Circuit Breaker opened for {args.BreakDuration} s";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                },
                
                OnClosed = args =>
                {
                    string message =
                        $"Circuit breaker closed.......";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }
            });
            
            
            
            // add timeout
            builder.AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromMilliseconds(settings.Timeout.DurationMs),
                
                
                
                OnTimeout = args =>
                {
                    string message =
                        $"Timeout for {args.Timeout.TotalMilliseconds} ms.....";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }
            });

        });

        return services;

    }
    
    
}