using System.Reflection;

namespace Auth.Domain.Core.Common.Extensions
{
    public static class AssemblyExtension
    {
        public static Type[] GetIocTypes(this Assembly assembly) => assembly.GetTypes();
        public static Type[] GetClasses<T>(this Type[] types)
        {
            var t = typeof(T);
            return types
                .Where(type => type.GetInterfaces().Any(i => i.Name == t.Name))
                .Where(type =>
                    !type.IsAbstract &&
                    !type.IsInterface
                    )
                .Distinct()
                .ToArray();
        }
    }
}
