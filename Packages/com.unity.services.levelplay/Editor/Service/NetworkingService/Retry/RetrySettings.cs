using System;
using System.Net;

namespace Unity.Services.LevelPlay.Editor
{
    internal class RetrySettings
    {
        float m_JitterMagnitude = 1.0f;
        float m_BackoffMagnitude = 0.2f;
        float m_MaxDelay = 8.0f;

        readonly RetryExceptionSettings m_ExceptionSettings = new();
        readonly RetryStatusCodeSettings m_StatusCodeSettings = new();

        public uint MaxAttempts { get; set; } = 3;

        public float JitterMagnitude
        {
            get => m_JitterMagnitude;
            set => m_JitterMagnitude = Math.Clamp(value, 0, 1.0f);
        }

        public float BackoffMagnitude
        {
            get => m_BackoffMagnitude;
            set => m_BackoffMagnitude = Math.Clamp(value, 0.05f, 1.0f);
        }

        public float MaxDelay
        {
            get => m_MaxDelay;
            set => m_MaxDelay = Math.Clamp(value, 0.1f, 30.0f);
        }
        
        public void AddRetryOn<TException>() where TException : Exception
            => m_ExceptionSettings.AddRetryOn<TException>();
        
        public void AddRetryOn<TException>(Func<TException, bool> predicate) where TException : Exception
            => m_ExceptionSettings.AddRetryOn(predicate);
        
        public bool IsRetryableException(Exception ex)
            => m_ExceptionSettings.IsRetryableException(ex);
        
        public bool ShouldRetryForStatusCode(HttpStatusCode statusCode)
            => m_StatusCodeSettings.IsRetryableWithStatusCode((uint)statusCode);
    }
}