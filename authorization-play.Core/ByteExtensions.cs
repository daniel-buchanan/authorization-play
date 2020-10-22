using System;
using System.Text;

namespace authorization_play.Core
{
    public static class ByteExtensions
    {
        public static string ToHexString(this byte[] bytes)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static byte[] ToByteArrayFromHex(this string input)
        {
            if (input.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            var arr = new byte[input.Length >> 1];

            for (var i = 0; i < input.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(input[i << 1]) << 4) + (GetHexVal(input[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            var val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
