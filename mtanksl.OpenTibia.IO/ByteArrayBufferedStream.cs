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

        /// <exception cref="ObjectDisposedException"></exception>
      
        private void Load()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(ByteArrayBufferedStream) );
            }

            lastPosition = position;

            stream.Seek(lastPosition, SeekOrigin.Begin);

            int length = stream.Read(bytes, 0, bytes.Length);

            lastLength = length;
        }

        /// <exception cref="ObjectDisposedException"></exception>
       
        public byte GetByte()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(ByteArrayBufferedStream) );
            }

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

        /// <exception cref="ObjectDisposedException"></exception>
       
        public void GetBytes(byte[] buffer, int offset, int count)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(ByteArrayBufferedStream) );
            }

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