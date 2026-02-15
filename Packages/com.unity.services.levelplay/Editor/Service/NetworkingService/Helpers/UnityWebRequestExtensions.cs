using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Unity.Services.LevelPlay.Editor
{
    internal static class UnityWebRequestExtensions
    {
        internal static TaskAwaiter<UnityWebRequest> GetAwaiter(this UnityWebRequestAsyncOperation op)
        {
            var tcs = new TaskCompletionSource<UnityWebRequest>(TaskCreationOptions.RunContinuationsAsynchronously);
            op.completed += _ => tcs.TrySetResult(op.webRequest);
            return tcs.Task.GetAwaiter();
        }
    }
}