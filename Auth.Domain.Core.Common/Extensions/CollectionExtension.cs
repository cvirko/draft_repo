namespace Auth.Domain.Core.Common.Extensions
{
    public static class CollectionExtension
    {
        public static bool TryAdd<T>(this ICollection<T> array, T value)
        {
            if (value == null ) return false;
            if (array == null || array.Contains(value)) return false;

            array.Add(value);
            return true;
        }
        public static bool TryRemove<T>(this ICollection<T> array, T value)
        {
            if (value == null) return false;
            if (array == null || array.Count == 0 || !array.Contains(value)) return false;

            array.Remove(value);
            return true;
        }
    }
}
