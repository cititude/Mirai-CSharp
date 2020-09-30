using System;
using System.IO;
using System.Net;
using System.Text;

#pragma warning disable CA1063 // Implement IDisposable Correctly
namespace CcHelperCore.Apis
{

    public static partial class CoolQApi
    {
        private class Unpacker : IDisposable
        {
            public MemoryStream Stream { get; private set; }
            public bool LeaveOpen { get; private set; }
            private bool Disposed = false;
            public Unpacker(byte[] source, bool leaveOpen = false)
            {
                Stream = new MemoryStream(source);
                LeaveOpen = leaveOpen;
            }
            public Unpacker(MemoryStream ms, bool leaveOpen = false)
            {
                Stream = ms;
                LeaveOpen = leaveOpen;
            }
            public void Dispose()
            {
                if (!Disposed)
                {
                    if (!LeaveOpen)
                    {
                        Stream?.Close();
                    }
                    Stream = null;
                    Disposed = true;
                }
            }
            public string GetString()
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("Stream");
                }
                int length = GetInt16();
                byte[] buffer = new byte[length];
                Stream.Read(buffer, 0, length);
                return Encoding.Default.GetString(buffer, 0, length);
            }
            public short GetInt16()
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("Stream");
                }
                byte[] buffer = new byte[2];
                Stream.Read(buffer, 0, 2);
                short result = BitConverter.ToInt16(buffer, 0);
                return IPAddress.NetworkToHostOrder(result);
            }
            public int GetInt32()
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("Stream");
                }
                byte[] buffer = new byte[4];
                Stream.Read(buffer, 0, 4);
                int result = BitConverter.ToInt32(buffer, 0);
                return IPAddress.NetworkToHostOrder(result);
            }
            public long GetInt64()
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("Stream");
                }
                byte[] buffer = new byte[8];
                Stream.Read(buffer, 0, 8);
                long result = BitConverter.ToInt64(buffer, 0);
                return IPAddress.NetworkToHostOrder(result);
            }
        }
    }
}