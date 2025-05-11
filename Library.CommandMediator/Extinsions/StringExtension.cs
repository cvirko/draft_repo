namespace Library.CommandMediator.Extinsions
{
    internal static class StringExtension
    {
        public static string ToLowerFirstChar(this string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
                return value;

            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}
