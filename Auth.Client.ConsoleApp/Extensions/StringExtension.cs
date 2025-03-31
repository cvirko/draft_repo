using System.Text;

namespace Auth.Client.ConsoleApp.Extensions
{
    public static class StringExtension
    {
        public static byte[] ToByteArray(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
        public static string ToUTF8String(this byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
        public static string ToUTF8String(this ReadOnlyMemory<byte> value)
        {
            return value.ToArray().ToUTF8String();
        }
        public static string ToLowerFirstChar(this string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
                return value;

            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}
