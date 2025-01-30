namespace OpenTibia.IO
{
    public abstract class ByteArrayStream : IByteArrayStream
    {
        protected int position;

        public int Position
        {
            get
            {
                return position;
            }
        }

        protected int length;

        public int Length
        {
            get
            {
                return length;
            }
        }

        public virtual void Seek(Origin origin, int offset)
        {
            switch (origin)
            {
                case Origin.Begin:

                    position = offset;

                    break;

                case Origin.Current:

                    position += offset;

                    break;
            }

            if (position > length)
            {
                length = position;
            }
        }

        public abstract byte ReadByte();

        public abstract void Read(byte[] buffer, int offset, int count);

        public abstract void WriteByte(byte value);

        public abstract void Write(byte[] buffer, int offset, int count);
    }
}