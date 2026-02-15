using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Unity.Services.LevelPlay.Editor
{
    internal interface IRetryPolicy<TState, TResult>
    {
        IRetryPolicy<TState, TResult> UpTo(uint maxAttempts);
        IRetryPolicy<TState, TResult> WithJitter(float magnitude);
        IRetryPolicy<TState, TResult> WithBackoff(float magnitude);
        IRetryPolicy<TState, TResult> WithMaxDelay(float time);
        IRetryPolicy<TState, TResult> HandleException<TException>() where TException : Exception;
        IRetryPolicy<TState, TResult> HandleException<TException>(Func<TException, bool> predicate) where TException : Exception;
        IRetryPolicy<TState, TResult> WithRetryPredicate(Func<TResult, bool> predicate);
        IRetryPolicy<TState, TResult> CreateRetryOperation(Func<TState, CancellationToken, Task<TResult>> operation);
        Task<TResult> RunAsync(TState state, TimeSpan? timeout, CancellationToken cancellationToken);
    }
}
