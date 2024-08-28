namespace Console;

using Polly;
using Polly.Retry;
using Polly.Timeout;

internal static class NoDependencyInjection
{
    public static async Task Run(string[] args)
    {
        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(
                new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<ArgumentException>(),
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 2,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true
                })
            .AddTimeout(
                new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(10)
                })
            .Build();

        var result = await pipeline.ExecuteAsync(
            async ct => await DummyService.Execute<Result>(null, new ArgumentException("Some argument Exception")));
    }
}