using System.Security.Cryptography;
using System.Text;

namespace McProtoNet.Utils;

public class CryptoHandler
{
    public static RSACryptoServiceProvider DecodeRSAPublicKey(byte[] x509key)
    {
        /* Code from StackOverflow no. 18091460 */

        byte[] SeqOID = { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };

        var ms = new MemoryStream(x509key);
        var reader = new BinaryReader(ms);

        if (reader.ReadByte() == 0x30)
            ReadASNLength(reader); //skip the size
        else
            return null;

        var identifierSize = 0; //total length of Object Identifier section
        if (reader.ReadByte() == 0x30)
            identifierSize = ReadASNLength(reader);
        else
            return null;

        if (reader.ReadByte() == 0x06) //is the next element an object identifier?
        {
            var oidLength = ReadASNLength(reader);
            var oidBytes = new byte[oidLength];
            reader.Read(oidBytes, 0, oidBytes.Length);
            if (oidBytes.SequenceEqual(SeqOID) == false) //is the object identifier rsaEncryption PKCS#1?
                return null;

            var remainingBytes = identifierSize - 2 - oidBytes.Length;
            reader.ReadBytes(remainingBytes);
        }

        if (reader.ReadByte() == 0x03) //is the next element a bit string?
        {
            ReadASNLength(reader); //skip the size
            reader.ReadByte(); //skip unused bits indicator
            if (reader.ReadByte() == 0x30)
            {
                ReadASNLength(reader); //skip the size
                if (reader.ReadByte() == 0x02) //is it an integer?
                {
                    var modulusSize = ReadASNLength(reader);
                    var modulus = new byte[modulusSize];
                    reader.Read(modulus, 0, modulus.Length);
                    if (modulus[0] == 0x00) //strip off the first byte if it's 0
                    {
                        var tempModulus = new byte[modulus.Length - 1];
                        Array.Copy(modulus, 1, tempModulus, 0, modulus.Length - 1);
                        modulus = tempModulus;
                    }

                    if (reader.ReadByte() == 0x02) //is it an integer?
                    {
                        var exponentSize = ReadASNLength(reader);
                        var exponent = new byte[exponentSize];
                        reader.Read(exponent, 0, exponent.Length);

                        var RSA = new RSACryptoServiceProvider();
                        var RSAKeyInfo = new RSAParameters();
                        RSAKeyInfo.Modulus = modulus;
                        RSAKeyInfo.Exponent = exponent;
                        RSA.ImportParameters(RSAKeyInfo);
                        return RSA;
                    }
                }
            }
        }

        return null;
    }


    private static int ReadASNLength(BinaryReader reader)
    {
        int length = reader.ReadByte();
        if ((length & 0x00000080) == 0x00000080) //is the length greater than 1 byte
        {
            var count = length & 0x0000000f;
            var lengthBytes = new byte[4];
            reader.Read(lengthBytes, 4 - count, count);
            Array.Reverse(lengthBytes); //
            length = BitConverter.ToInt32(lengthBytes, 0);
        }

        return length;
    }

    public static byte[] GenerateAESPrivateKey()
    {
        using (AesManaged AES = new())
        {
            AES.KeySize = 128;
            AES.GenerateKey();
            return AES.Key;
        }
    }

    public static string getServerHash(string serverID, byte[] PublicKey, byte[] SecretKey)
    {
        var hash = digest(new[] { Encoding.GetEncoding("iso-8859-1").GetBytes(serverID), SecretKey, PublicKey });
        var negative = (hash[0] & 0x80) == 0x80;
        if (negative) hash = TwosComplementLittleEndian(hash);
        var result = GetHexString(hash).TrimStart('0');
        if (negative) result = "-" + result;
        return result;
    }


    private static byte[] digest(byte[][] tohash)
    {
        var sha1 = new SHA1CryptoServiceProvider();
        for (var i = 0; i < tohash.Length; i++)
            sha1.TransformBlock(tohash[i], 0, tohash[i].Length, tohash[i], 0);
        sha1.TransformFinalBlock(new byte[] { }, 0, 0);
        return sha1.Hash;
    }


    private static string GetHexString(byte[] p)
    {
        var result = string.Empty;
        for (var i = 0; i < p.Length; i++)
            result += p[i].ToString("x2");
        return result;
    }

    /// <summary>
    ///     Compute the two's complement of a little endian byte array
    /// </summary>
    /// <param name="p">Byte array to compute</param>
    /// <returns>Returns the corresponding two's complement</returns>
    private static byte[] TwosComplementLittleEndian(byte[] p)
    {
        int i;
        var carry = true;
        for (i = p.Length - 1; i >= 0; i--)
        {
            p[i] = (byte)~p[i];
            if (carry)
            {
                carry = p[i] == 0xFF;
                p[i]++;
            }
        }

        return p;
    }
}