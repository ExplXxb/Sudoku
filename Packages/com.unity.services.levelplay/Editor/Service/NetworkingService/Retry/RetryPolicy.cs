using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Services.LevelPlay.Editor
{
    internal class RetryPolicy<TState, TResult> : IRetryPolicy<TState, TResult>
    {
        Func<TResult, bool> m_ShouldRetry;
        Func<TState, CancellationToken, Task<TResult>> m_RetryOperation;
     
        internal readonly RetrySettings Settings = new();
        readonly Random m_Random = new();
        readonly TimeSpan m_DefaultTimeout = TimeSpan.FromSeconds(30);

        public IRetryPolicy<TState, TResult> UpTo(uint maxAttempts)
        {
            Settings.MaxAttempts = maxAttempts;
            return this;
        }

        public IRetryPolicy<TState, TResult> WithJitter(float magnitude)
        {
            Settings.JitterMagnitude = magnitude;
            return this;
        }

        public IRetryPolicy<TState, TResult> WithBackoff(float magnitude)
        {
            Settings.BackoffMagnitude = magnitude;
            return this;
        }

        public IRetryPolicy<TState, TResult> WithMaxDelay(float time)
        {
            Settings.MaxDelay = time;
            return this;
        }

        public IRetryPolicy<TState, TResult> HandleException<TException>() where TException : Exception
        {
            Settings.AddRetryOn<TException>();
            return this;
        }

        public IRetryPolicy<TState, TResult> HandleException<TException>(Func<TException, bool> predicate) where TException : Exception
        {
            Settings.AddRetryOn(predicate);
            return this;
        }

        public IRetryPolicy<TState, TResult> WithRetryPredicate(Func<TResult, bool> predicate)
        {
            m_ShouldRetry = predicate;
            return this;
        }

        public IRetryPolicy<TState, TResult> CreateRetryOperation(Func<TState, CancellationToken, Task<TResult>> operation)
        {
            m_RetryOperation = operation;
            return this;
        }

        public async Task<TResult> RunAsync(TState state, TimeSpan? timeout, CancellationToken cancellationToken)
        {
            var operationResult = default(TResult);
            Exception lastException = null;
            var localTimeout = timeout ?? m_DefaultTimeout;

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(localTimeout);
                
            for (uint attempt = 0; attempt < Settings.MaxAttempts; ++attempt)
            {
                try
                {
                    operationResult = await m_RetryOperation(state, cts.Token);
                    
                    if (m_ShouldRetry == null || (operationResult != null && !m_ShouldRetry(operationResult)))
                        return operationResult;
                }
                catch (Exception e)
                {
                    lastException = e;
                    if (!Settings.IsRetryableException(e) || attempt == Settings.MaxAttempts - 1) 
                        throw;
                }

                var delayTime = CalculateDelayTime(
                    attempt, 
                    Settings.MaxDelay, 
                    Settings.BackoffMagnitude, 
                    Settings.JitterMagnitude);
                await Task.Delay(TimeSpan.FromSeconds(delayTime), cts.Token);
            }

            if (lastException != null) throw lastException;
            return operationResult;
        }

        float CalculateDelayTime(uint attempt, float maxDelayTime, float backoff, float jitterMagnitude)
        {
            var delayTime = (float)(Math.Pow(2, attempt) * backoff);
            delayTime += (float)m_Random.NextDouble() * jitterMagnitude;
            delayTime = Math.Min(delayTime, maxDelayTime);
            return delayTime;
        }
    }
}