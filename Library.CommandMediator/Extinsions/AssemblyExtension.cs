using System.Reflection;

namespace Library.CommandMediator.Extinsions
{
    internal static class AssemblyExtension
    {
        public static Type[] GetIocTypes(this Assembly assembly) => assembly.GetTypes();
        public static Type[] GetClasses<T>(this Type[] types)
            => types.GetClasses(typeof(T));
        public static Type[] GetClasses(this Type[] types, Type t)
        {
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
