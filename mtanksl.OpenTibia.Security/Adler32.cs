namespace OpenTibia.Security
{
    public static class Adler32
    {
        public const uint Modulo = 65521;

        public const uint Multiplier = 65536;

        public static uint Generate(byte[] dataToChecksum)
        {
            return Generate(dataToChecksum, 0, dataToChecksum.Length);
        }

        public static uint Generate(byte[] dataToChecksum, int offset)
        {
            return Generate(dataToChecksum, offset, dataToChecksum.Length - offset);
        }

        public static uint Generate(byte[] dataToChecksum, int offset, int count)
        {
            uint a = 1;
            
            uint b = 0;

            for (int i = 0; i < count; i++)
            {
                a = ( a + dataToChecksum[i + offset] ) % Modulo;

                b = ( b + a ) % Modulo;
            }

            return a + b * Multiplier;
        }
    }
}