using UnityEngine;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GGJRuntime
{
    public static class HashCodeUtility
    {
        public static string CalculateHashCode(int[][] grid)
        {
            byte[] tmpSource = grid.SelectMany(x => GetByteArrayFromIntArray(x)).ToArray();
            byte[] tempHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            return ByteArrayToString(tempHash);
        }

        private static byte[] GetByteArrayFromIntArray(int[] intArray)
        {
            byte[] data = new byte[intArray.Length * 4];

            for(int i=0; i < intArray.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(intArray[i]), 0, data, i * 4, 4);
            }

            return data;
        }


        private static string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder output = new StringBuilder(arrInput.Length);

            for(int i=0; i < arrInput.Length; i++)
            {
                output.Append(arrInput[i].ToString("X2"));
            }

            return output.ToString();
        }
    }
}
