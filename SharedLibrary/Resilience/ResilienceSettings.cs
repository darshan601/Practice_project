namespace SharedLibrary.Resilience;

public class ResilienceSettings
{
    public RetrySettings ServiceRetry { get; set; } = new RetrySettings();

    public CircuitBreakerSettings CircuitBreaker { get; set; } = new CircuitBreakerSettings();

    public TimeoutSettings Timeout { get; set; } = new TimeoutSettings();

    public BulkheadSettings Bulkhead { get; set; } = new BulkheadSettings();

    public RateLimiterSettings RateLimiter { get; set; } = new RateLimiterSettings();


    public class RetrySettings
    {
        public int MaxAttempts { get; set; } = 3;
        public int InitialDelayMs { get; set; } = 200;
        public int MaxDelaySeconds { get; set; } = 10;
        
        // these could be used for more advanced configurations
        public bool UseJitter { get; set; } = true;
        public string BackoffType { get; set; } = "Exponential"; //can be exponential, linear or constant

    }

    public class CircuitBreakerSettings
    {
        public int FailureThresholdPercentage { get; set; } = 50;
        public int SamplingDurationSeconds { get; set; } = 30;
        public int MinimumThroughput { get; set; } = 10;
        public int BreakDurationSeconds { get; set; } = 15;
        
        // advanced configuration options
        public bool AutomaticTransitionFromOpenToHalfOpen { get; set; } = true;
        public int HalfOpenMaximumThroughput { get; set; } = 5;


    }

    public class TimeoutSettings
    {
        public int DurationMs { get; set; } = 2500;
        public string TimeoutStrategy { get; set; } = "Optimistic"; //Optimistic or Pessimistic
    }

    public class BulkheadSettings
    {
        public int MaxConcurrentCalls { get; set; } = 100;
        public int MaxQueueSize { get; set; } = 50;
        public string QueueProcessingOrder { get; set; } = "OldestFirst"; //OldestFirst or NewestFirst

    }

    public class RateLimiterSettings
    {
        public int PermitLimit { get; set; } = 200;
        public int WindowSeconds { get; set; } = 1;
        public int QueueLimit { get; set; } = 50;
        public bool AutoReplenishment { get; set; } = true;
        public string ReplenishmentStrategy { get; set; } = "Fixed"; //Fixed or Gradient


    }
    
    
    
}
