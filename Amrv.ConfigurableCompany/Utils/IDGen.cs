using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.Utils
{
    public class IDGen(int Bytes)
    {
        public static string DeterministicString(object any)
        {
            throw new NotImplementedException();
        }

        public static int DeterministicIntHash(string str)
        {
            int hash = 0;

            byte[] inBytes = Encoding.UTF8.GetBytes(str);

            byte[] outBytes = new byte[sizeof(int)];

            for (int i = 0; i < inBytes.Length; i++)
            {
                outBytes[i % outBytes.Length] ^= (byte)(inBytes[i] << 7 ^ inBytes[i] >> 3 | inBytes[i] << 5);
            }

            for (int outByte = 0; outByte < outBytes.Length; outByte++)
            {
                hash = hash << 8 | outBytes[outByte];
            }

            return hash;
        }

        private readonly List<int> Hash = [];

        public void AddDeterminant(string any)
        {
            Hash.Add(DeterministicIntHash(any));
        }

        private byte[] GenerateID()
        {
            byte[] uid = new byte[Bytes];
            byte[] temp = new byte[Bytes];

            foreach (int i in Hash)
            {
                new Random(i).NextBytes(temp);

                for (int n = 0; n < temp.Length; n++)
                {
                    uid[n] |= temp[n];
                }
            }

            return uid;
        }

        public byte[] GetIDBytes()
        {
            return GenerateID();
        }

        public string GetIDHex()
        {
            return BitConverter.ToString(GenerateID());
        }

        public string GetIDBaseX(int @base)
        {
            throw new NotImplementedException();
        }

        public string GetIDBaseX(params char[] notation)
        {
            throw new NotImplementedException();
        }

        public string GetIDBase64()
        {
            return Convert.ToBase64String(GenerateID());
        }
    }
}
