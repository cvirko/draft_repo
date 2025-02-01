using System.Text;

namespace Auth.Domain.Core.Common.Extensions
{
    public static class StringExtension
    {
        public static byte[] ToByteArray(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
        public static string ToLowerFirstChar(this string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
                return value;

            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}
