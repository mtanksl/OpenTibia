using System;
using System.Linq;
using System.Numerics;

namespace OpenTibia.Security
{
    public static class Rsa
    {
        public static BigInteger D = BigInteger.Parse("46730330223584118622160180015036832148732986808519344675210555262940258739805766860224610646919605860206328024326703361630109888417839241959507572247284807035235569619173792292786907845791904955103601652822519121908367187885509270025388641700821735345222087940578381210879116823013776808975766851829020659073");

        //public static BigInteger DP = BigInteger.Parse("11141736698610418925078406669215087697114858422461871124661098818361832856659225315773346115219673296375487744032858798960485665997181641221483584094519937");

        //public static BigInteger DQ = BigInteger.Parse("4886309137722172729208909250386672706991365415741885286554321031904881408516947737562153523770981322408725111241551398797744838697461929408240938369297973");
        
        public static BigInteger Exponent = BigInteger.Parse("65537");

        //public static BigInteger InverseQ = BigInteger.Parse("5610960212328996596431206032772162188356793727360507633581722789998709372832546447914318965787194031968482458122348411654607397146261039733584248408719418");

        public static BigInteger Modulus = BigInteger.Parse("109120132967399429278860960508995541528237502902798129123468757937266291492576446330739696001110603907230888610072655818825358503429057592827629436413108566029093628212635953836686562675849720620786279431090218017681061521755056710823876476444260558147179707119674283982419152118103759076030616683978566631413");

        //public static BigInteger P = BigInteger.Parse("14299623962416399520070177382898895550795403345466153217470516082934737582776038882967213386204600674145392845853859217990626450972452084065728686565928113");

        //public static BigInteger Q = BigInteger.Parse("7630979195970404721891201847792002125535401292779123937207447574596692788513647179235335529307251350570728407373705564708871762033017096809910315212884101");
        
        public static void EncryptAndReplace(byte[] dataToEncrypt, int offset)
        {
            byte[] encryptedData = Encrypt(dataToEncrypt, offset);

            Buffer.BlockCopy(encryptedData, 0, dataToEncrypt, offset, encryptedData.Length);
        }

        public static void EncryptAndReplace(byte[] dataToEncrypt, int offset, int count)
        {
            byte[] encryptedData = Encrypt(dataToEncrypt, offset, count);

            Buffer.BlockCopy(encryptedData, 0, dataToEncrypt, offset, encryptedData.Length);
        }

        public static byte[] Encrypt(byte[] dataToEncrypt)
        {
            return Encrypt(dataToEncrypt, 0, dataToEncrypt.Length);
        }

        public static byte[] Encrypt(byte[] dataToEncrypt, int offset)
        {
            return Encrypt(dataToEncrypt, offset, dataToEncrypt.Length - offset);
        }

        public static byte[] Encrypt(byte[] dataToEncrypt, int offset, int count)
        {
            dataToEncrypt = dataToEncrypt.Skip(offset)
                                         .Take(count)
                                         .Reverse()
                                         .Concat(new byte[] { 0 } )
                                         .ToArray();

            /*
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() )
            {
                rsa.ImportParameters(new RSAParameters()
                {
                    Exponent = Exponent.ToByteArray(),

                    Modulus = Modulus.ToByteArray()
                } );

                return rsa.Encrypt(dataToEncrypt, false);
            }
            */

            
            byte[] encryptedData = BigInteger.ModPow(new BigInteger(dataToEncrypt), Exponent, Modulus).ToByteArray(); 

            return encryptedData.Reverse()                
                                .ToArray();
        }

        public static void DecryptAndReplace(byte[] dataToDecrypt, int offset)
        {
            byte[] decryptedData = Decrypt(dataToDecrypt, offset);

            Buffer.BlockCopy(decryptedData, 0, dataToDecrypt, offset, decryptedData.Length);
        }

        public static void DecryptAndReplace(byte[] dataToDecrypt, int offset, int count)
        {
            byte[] decryptedData = Decrypt(dataToDecrypt, offset, count);

            Buffer.BlockCopy(decryptedData, 0, dataToDecrypt, offset, decryptedData.Length);
        }

        public static byte[] Decrypt(byte[] dataToDecrypt)
        {
            return Decrypt(dataToDecrypt, 0, dataToDecrypt.Length);
        }

        public static byte[] Decrypt(byte[] dataToDecrypt, int offset)
        {
            return Decrypt(dataToDecrypt, offset, dataToDecrypt.Length - offset);
        }

        public static byte[] Decrypt(byte[] dataToDecrypt, int offset, int count)
        {
            dataToDecrypt = dataToDecrypt.Skip(offset)
                                         .Take(count)
                                         .Reverse()
                                         .Concat(new byte[] { 0 } )
                                         .ToArray();

            /* Does not work
            
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() )
            {
                rsa.ImportParameters(new RSAParameters()
                {
                    D = D.ToByteArray(),

                    DP = DP.ToByteArray(),

                    DQ = DQ.ToByteArray(),

                    Exponent = Exponent.ToByteArray(),

                    InverseQ = InverseQ.ToByteArray(),

                    Modulus = Modulus.ToByteArray(),

                    P = P.ToByteArray(),

                    Q = Q.ToByteArray()
                } );

                return rsa.Decrypt(dataToDecrypt, false);
            }
            */
            
            byte[] decryptedData = BigInteger.ModPow(new BigInteger(dataToDecrypt), D, Modulus).ToByteArray();

            if (decryptedData.Length == 127)
	        {
                return decryptedData.Concat(new byte[] { 0 } )
                                    .Reverse()
                                    .ToArray();
	        }

            return decryptedData.Reverse()
                                .ToArray();
        }
    }
}