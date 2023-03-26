using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PongServer.Domain.HelperMethods
{
    public static class ExpandoObjectHelper
    {
        public static ExpandoObject ToExpandoObject(object obj)
        {
            if (obj is ExpandoObject exp)
            {
                return exp;
            }

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var propertyDescriptor in obj.GetType().GetTypeInfo().GetProperties())
            {
                var value = propertyDescriptor.GetValue(obj);
                if (value is not null && IsAnonymousType(value.GetType()))
                {
                    value = ToExpandoObject(value);
                }
                expando.Add(propertyDescriptor.Name, value);
            }

            return (ExpandoObject)expando;
        }

        private static bool IsAnonymousType(Type type)
        {
            bool hasColpilerGeneratedAttribute = type
                .GetTypeInfo()
                .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
                .Any();

            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasColpilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }
    }
}
