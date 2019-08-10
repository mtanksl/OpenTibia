using System;
using System.IO;

namespace OpenTibia.IO
{
    public abstract class ByteArrayBufferedStream : ByteArrayStream, IDisposable
    {
        private Stream stream;
        
        public ByteArrayBufferedStream(Stream stream)
        {
            this.stream = stream;
        }

        ~ByteArrayBufferedStream()
        {
            Dispose(false);
        }

        private byte[] bytes = new byte[4 * 1024];

        private int lastPosition;

        private int lastLength;
        
        private void Load()
        {
            lastPosition = position;

            stream.Seek(lastPosition, SeekOrigin.Begin);

            int length = stream.Read(bytes, 0, bytes.Length);

            lastLength = length;
        }
        
        public byte GetByte()
        {
            int index = position - lastPosition;

            if (index < 0 || lastLength - index < 1)
            {
                Load();

                index = 0;
            }

            byte value = bytes[index];

            Seek(Origin.Current, 1);

            return value;
        }

        public void GetBytes(byte[] buffer, int offset, int count)
        {
            int index = position - lastPosition;

            if (index < 0 || lastLength - index < count)
            {
                Load();

                index = 0;
            }

            Buffer.BlockCopy(bytes, index, buffer, offset, count);

            Seek(Origin.Current, count);
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }
            }
        }
    }
}