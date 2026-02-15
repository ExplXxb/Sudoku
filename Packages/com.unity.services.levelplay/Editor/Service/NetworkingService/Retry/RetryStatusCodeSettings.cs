using System.Collections.Generic;

namespace Unity.Services.LevelPlay.Editor
{
    internal sealed class RetryStatusCodeSettings
    {
        readonly HashSet<uint> m_RetryableCodes = new()
        {
            408, // Request Timeout
            409, // Conflict
            429  // Too Many Requests
        };

        public void AddRetryableCode(uint code) => m_RetryableCodes.Add(code);
        public void RemoveRetryableCode(uint code) => m_RetryableCodes.Remove(code);
        public bool IsRetryableWithStatusCode(uint code) 
            => m_RetryableCodes.Contains(code) || IsCodeGreaterThan500(code);

        static bool IsCodeGreaterThan500(uint code) => code >= 500;
    }
}