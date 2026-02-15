using System;
using System.Collections.Generic;

namespace Unity.Services.LevelPlay.Editor
{
    internal sealed class RetryExceptionSettings
    {
        readonly IList<Func<Exception, bool>> m_ExceptionsFilters = new List<Func<Exception, bool>>();
        
        public void AddRetryOn<TException>() where TException : Exception
            => m_ExceptionsFilters.Add(ex => ex is TException); 
        
        public void AddRetryOn<TException>(Func<TException, bool> predicate) where TException : Exception
            => m_ExceptionsFilters.Add(ex => ex is TException t && predicate(t));

        public bool IsRetryableException(Exception ex)
        {
            if (m_ExceptionsFilters is not { Count: > 0 }) return false;
            
            foreach (var exceptionsFilter in m_ExceptionsFilters)
                if (exceptionsFilter(ex)) return true;

            return false;
        }
    }
}