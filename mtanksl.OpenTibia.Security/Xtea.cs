using System;
using System.Linq;

namespace OpenTibia.Security
{
    public static class Xtea
    {
        public const uint Delta = 2654435769;

        public static void EncryptAndReplace(byte[] dataToEncrypt, int offset, uint rounds, uint[] keys)
        {
            byte[] encryptedData = Encrypt(dataToEncrypt, offset, rounds, keys);

            Buffer.BlockCopy(encryptedData, 0, dataToEncrypt, offset, encryptedData.Length);
        }

        public static void EncryptAndReplace(byte[] dataToEncrypt, int offset, int count, uint rounds, uint[] keys)
        {
            byte[] encryptedData = Encrypt(dataToEncrypt, offset, count, rounds, keys);

            Buffer.BlockCopy(encryptedData, 0, dataToEncrypt, offset, encryptedData.Length);
        }
        
        public static byte[] Encrypt(byte[] dataToEncrypt, uint rounds, uint[] keys)
        {
            return Encrypt(dataToEncrypt, 0, dataToEncrypt.Length, rounds, keys);
        }

        public static byte[] Encrypt(byte[] dataToEncrypt, int offset, uint rounds, uint[] keys)
        {
            return Encrypt(dataToEncrypt, offset, dataToEncrypt.Length - offset, rounds, keys);
        }

        public static byte[] Encrypt(byte[] dataToEncrypt, int offset, int count, uint rounds, uint[] keys)
        {
            dataToEncrypt = dataToEncrypt.Skip(offset)
                                         .Take(count)
                                         .ToArray();

            uint[] blocks = new uint[dataToEncrypt.Length / 4];

            Buffer.BlockCopy(dataToEncrypt, 0, blocks, 0, dataToEncrypt.Length);

            for (int i = 0; i < dataToEncrypt.Length / 8; i++)
            {
                Encrypt(ref blocks[i * 2], ref blocks[i * 2 + 1], rounds, keys);
            } 
                       
            byte[] encryptedData = new byte[dataToEncrypt.Length];

            Buffer.BlockCopy(blocks, 0, encryptedData, 0, encryptedData.Length);

            return encryptedData;
        }

        private static void Encrypt(ref uint v0, ref uint v1, uint rounds, uint[] keys)
        {
            uint sum = 0;

            for (int i = 0; i < rounds; i++)
            {
                v0 += ( ( ( (v1 << 4) ^ (v1 >> 5) ) + v1 ) ^ ( sum + keys[sum & 3] ) );

                sum += Delta;

                v1 += ( ( ( (v0 << 4) ^ (v0 >> 5) ) + v0 ) ^ ( sum + keys[ (sum >> 11) & 3 ] ) );
            }
        }

        public static void DecryptAndReplace(byte[] dataToDecrypt, int offset, uint rounds, uint[] keys)
        {
            byte[] decryptedData = Decrypt(dataToDecrypt, offset, rounds, keys);

            Buffer.BlockCopy(decryptedData, 0, dataToDecrypt, offset, decryptedData.Length);
        }

        public static void DecryptAndReplace(byte[] dataToDecrypt, int offset, int count, uint rounds, uint[] keys)
        {
            byte[] decryptedData = Decrypt(dataToDecrypt, offset, count, rounds, keys);

            Buffer.BlockCopy(decryptedData, 0, dataToDecrypt, offset, decryptedData.Length);
        }

        public static byte[] Decrypt(byte[] dataToDecrypt, uint rounds, uint[] keys)
        {
            return Decrypt(dataToDecrypt, 0, dataToDecrypt.Length, rounds, keys);
        }

        public static byte[] Decrypt(byte[] dataToDecrypt, int offset, uint rounds, uint[] keys)
        {
            return Decrypt(dataToDecrypt, offset, dataToDecrypt.Length - offset, rounds, keys);
        }

        public static byte[] Decrypt(byte[] dataToDecrypt, int offset, int count, uint rounds, uint[] keys)
        {
            dataToDecrypt = dataToDecrypt.Skip(offset)
                                         .Take(count)
                                         .ToArray();

            uint[] blocks = new uint[dataToDecrypt.Length / 4];

            Buffer.BlockCopy(dataToDecrypt, 0, blocks, 0, dataToDecrypt.Length);

            for (int i = 0; i < dataToDecrypt.Length / 8; i++)
            {
                Decrypt(ref blocks[i * 2], ref blocks[i * 2 + 1], rounds, keys);
            }

            byte[] decryptedData = new byte[dataToDecrypt.Length];

            Buffer.BlockCopy(blocks, 0, decryptedData, 0, decryptedData.Length);

            return decryptedData;
        }

        private static void Decrypt(ref uint v0, ref uint v1, uint rounds, uint[] keys)
        {
            uint sum = rounds * Delta;
            
            for (int i = 0; i < rounds; i++)
			{
                v1 -= ( ( ( (v0 << 4) ^ (v0 >> 5) ) + v0 ) ^ ( sum + keys[ (sum >> 11) & 3] ) );

                sum -= Delta;

                v0 -= ( ( ( (v1 << 4) ^ (v1 >> 5) ) + v1 ) ^ ( sum + keys[sum & 3] ) );
			}
        }
    }
}