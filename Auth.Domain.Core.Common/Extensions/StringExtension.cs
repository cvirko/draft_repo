using System.Text;

namespace Auth.Domain.Core.Common.Extensions
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
        public static bool ContainsAny(this string text, string[] substrings, int startIndex, int endIndex)
        {
            int count = endIndex - startIndex - 1;
            for (int i = 0; i < substrings.Length; i++)
                if (text.IsContains(substrings[i], startIndex, count))
                    return true;
            return false;
        }
        public static bool IsContains(this string text, string substring, int startIndex, int count)
            => text.IndexOf(substring, startIndex, count) > -1;
        public static bool TryIndexOf(this string text, string substring, int startIndex, out int index)
        {
            index = text.IndexOf(substring, startIndex);
            return index > -1;
        }
        public static bool IsStartAt(this string text, string substring, int startIndex)
        {
            if (text.Length - startIndex < substring.Length)
                return false;

            for (int i = 0; i < substring.Length; i++)
            {
                if (text[startIndex + i] != substring[i])
                    return false;
            }
            return true;
        }
        public static bool StartAny(this string text, string[] substrings, int startIndex)
        {
            for (int i = 0; i < substrings.Length; i++)
                if (text.IsStartAt(substrings[i], startIndex))
                    return true;
            return false;
        }
        public static string GetBetween(this string text, string start, string end)
        {
            int startIndex = text.IndexOf(start) + start.Length;
            int endIndex = text.IndexOf(end, startIndex);
            return text.Substring(startIndex, endIndex - startIndex);
        }
    }
}
