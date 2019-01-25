using System;
using System.Text;

namespace AzureTest.Services
{
    public static class HexService
    {
        public static string ToHex(byte[] bytes, bool upperCase = false)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            return result.ToString();
        }
    }
}
