using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine.Networking;

#nullable enable

namespace Unity.Services.LevelPlay.Editor
{
    internal sealed class PooledByteArrayHttpContent : HttpContent
    {
        byte[]? m_Buffer;
        readonly int m_Length;
        volatile bool m_IsDisposed;
        readonly object m_DisposeLock;

        PooledByteArrayHttpContent(byte[] buffer, int length)
        {
            m_Buffer = buffer;
            m_Length = length;
            m_DisposeLock = new object();
        }

        static PooledByteArrayHttpContent FromNativeArray(NativeArray<byte>.ReadOnly array)
        {
            var length = array.Length;
            var buffer = ArrayPool<byte>.Shared.Rent(length);
#if UNITY_2022_2_OR_NEWER
            array.AsReadOnlySpan().CopyTo(buffer);
#else
            var tmp = array.ToArray();
            Buffer.BlockCopy(tmp, 0, buffer, 0, length);
#endif
            return new PooledByteArrayHttpContent(buffer, length);
        }

        static PooledByteArrayHttpContent FromBuffer(byte[] buffer)
        {
            var length = buffer.Length;
            var bufferPool = ArrayPool<byte>.Shared.Rent(length);
#if UNITY_2022_2_OR_NEWER
            buffer.AsSpan().CopyTo(bufferPool);
#else
            Buffer.BlockCopy(buffer, 0, bufferPool, 0, length);
#endif
            return new PooledByteArrayHttpContent(bufferPool, length);
        }

        public static HttpContent Create(byte[] buffer) => FromBuffer(buffer);

        public static HttpContent Create(NativeArray<byte>.ReadOnly array) => FromNativeArray(array);

        internal static HttpContent ToHttpContent(DownloadHandler downloadHandler) =>
            downloadHandler is DownloadHandlerBuffer buffer
                ? FromNativeArray(buffer.nativeData)
                : FromBuffer(downloadHandler.data);

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            ThrowIfDisposed();
            return stream.WriteAsync(m_Buffer, 0, m_Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            if (m_IsDisposed)
            {
                length = 0;
                return false;
            }

            length = m_Length;
            return true;
        }

        void ThrowIfDisposed()
        {
            if (m_IsDisposed) throw new ObjectDisposedException(GetType().Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !m_IsDisposed)
            {
                lock (m_DisposeLock)
                {
                    if (!m_IsDisposed && m_Buffer != null)
                    {
                        ArrayPool<byte>.Shared.Return(m_Buffer);
                        m_Buffer = null;
                        m_IsDisposed = true;
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
