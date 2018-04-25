using System;
using System.Collections.Generic;

namespace OpenTibia.IO
{
    public class ByteArrayMemoryStream : ByteArrayStream
    {
        private class Operation
        {
            public int Position { get; set; }

            public byte[] Buffer { get; set; }

            public int Offset { get; set; }

            public int Count { get; set; }
        }

        private List<Operation> operations = new List<Operation>();

        public override byte ReadByte()
        {
            throw new NotSupportedException();
        }

        public override void Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void WriteByte(byte value)
        {
            Write(new byte[] { value }, 0, 1);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            operations.Add(new Operation()
                {
                    Position = position,

                    Buffer = buffer,

                    Offset = offset,

                    Count = count
                }
            );

            Seek(Origin.Current, count);
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[length];

            ByteArrayArrayStream stream = new ByteArrayArrayStream(bytes);

            foreach (var operation in operations)
            {
                stream.Seek(Origin.Begin, operation.Position);

                stream.Write(operation.Buffer, operation.Offset, operation.Count); 
            }

            return bytes;
        }
    }
}