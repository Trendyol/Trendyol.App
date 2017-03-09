using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Trendyol.App.EntityFramework.Helpers
{
    internal static class RuntimeTypeBuilder
    {
        private static readonly ModuleBuilder moduleBuilder;
        private static readonly IDictionary<string, Type> builtTypes;

        static RuntimeTypeBuilder()
        {
            var assemblyName = new AssemblyName { Name = "DynamicLinqTypes" };
            moduleBuilder = Thread.GetDomain()
                    .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run)
                    .DefineDynamicModule(assemblyName.Name);
            builtTypes = new Dictionary<string, Type>();
        }

        internal static Type GetRuntimeType(IDictionary<string, PropertyInfo> fields)
        {
            var typeKey = GetTypeKey(fields);
            if (!builtTypes.ContainsKey(typeKey))
            {
                lock (moduleBuilder)
                {
                    builtTypes[typeKey] = GetRuntimeType(typeKey, fields);
                }
            }

            return builtTypes[typeKey];
        }

        private static Type GetRuntimeType(string typeName, IEnumerable<KeyValuePair<string, PropertyInfo>> properties)
        {
            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);
            foreach (var property in properties)
            {
                typeBuilder.DefineField(property.Key, property.Value.PropertyType, FieldAttributes.Public);
            }

            return typeBuilder.CreateType();
        }

        private static string GetTypeKey(IEnumerable<KeyValuePair<string, PropertyInfo>> fields)
        {
            return fields.Aggregate(string.Empty, (current, field) => current + (field.Key + ";" + field.Value.Name + ";"));
        }
    }
}