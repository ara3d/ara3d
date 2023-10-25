using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ara3D.Utils
{

    public static class ReflectionUtils
    {
        /// <summary>
        /// Returns true if the type self is an instance of the given generic type
        /// </summary>
        public static bool InstanceOfGenericType(this Type self, Type genericType)
            => self.IsGenericType && self.GetGenericTypeDefinition() == genericType;

        /// <summary>
        /// Returns true if the type self is an instance of the given generic interface, or implements the interface
        /// </summary>
        public static bool InstanceOfGenericInterface(this Type self, Type ifaceType)
            => self.InstanceOfGenericType(ifaceType)
               || self.GetInterfaces().Any(i => InstanceOfGenericType(i, ifaceType));

        /// <summary>
        /// https://stackoverflow.com/questions/4963160/how-to-determine-if-a-type-implements-an-interface-with-c-sharp-reflection
        /// </summary>
        public static bool ImplementsInterface(this Type self, Type ifaceType)
            => ifaceType.IsAssignableFrom(self) || self.GetInterfaces().Contains(ifaceType);

        public static bool ImplementsInterface<TInterface>(this Type self)
            => self.ImplementsInterface(typeof(TInterface));

        public static ConstructorInfo GetDefaultConstructor(this Type self)
            => self.GetConstructor(Array.Empty<Type>());

        public static bool HasDefaultConstructor(this Type self)
            => self.GetDefaultConstructor() != null;

        /// <summary>
        /// Returns true if the type implements IList with a generic parmaeter.
        /// </summary>
        public static bool ImplementsIList(this Type t)
            => t.InstanceOfGenericInterface(typeof(IList<>));

        /// <summary>
        /// Returns true if the source type can be cast to doubles.
        /// </summary>
        public static bool CanCastToDouble(this Type typeSrc)
            => typeSrc.IsPrimitive
               && typeSrc != typeof(char)
               && typeSrc != typeof(decimal)
               && typeSrc != typeof(bool);


        public static IEnumerable<Type> GetAllTypes()
            => AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()).Distinct();

        // https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
        public static IEnumerable<Type> GetAllSubclassesOf(Type t)
            => GetAllTypes().Where(x => x.IsSubclassOf(t));

        public static IEnumerable<Type> GetAllSubclassesOf(Assembly asm, Type t)
            => asm.GetTypes().Where(x => x.IsSubclassOf(t));


        public static IEnumerable<Assembly> GetReferencedAssemblies(this Type type)
        {
            yield return type.Assembly;

            foreach (var assemblyName in type.Assembly.GetReferencedAssemblies())
            {
                yield return Assembly.ReflectionOnlyLoad(assemblyName.FullName);
            }
        }

        // https://stackoverflow.com/questions/1582510/get-paths-of-assemblies-used-in-type
        public static IEnumerable<string> GetReferencedAssemblyPaths(this Type type)
            => type.GetReferencedAssemblies().Select(asm => asm.Location);

        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(this Type type, Type attrType)
            => type.GetMethods().Where(m => m.GetCustomAttributes(attrType, false).Length > 0);


        /// <summary>
        /// Converts properties to an enumerable of strings.
        /// </summary>
        public static IEnumerable<string> PropertiesToStrings(this object self)
            => self.PropertiesToDictionary().Select(kv => $"{kv.Key}: {kv.Value}");

        /// <summary>
        /// Returns true if the type is a "plain old data" type (is a struct type that contains no references).
        /// This means that we should be able to create pointers to the type, and copying
        /// arrays of them into buffers makes sense.
        /// </summary>
        public static bool ContainsNoReferences(this Type t)
            => t.IsPrimitive || t.GetAllFields().Select(f => f.FieldType).All(ContainsNoReferences);

        /// <summary>
        /// Returns the size of the managed type.
        /// </summary>
        public static int SizeOf(this Type t)
            => Marshal.SizeOf(t);

        /// <summary>
        /// Returns the size of the managed type.
        /// </summary>
        public static int SizeOf<T>()
            => SizeOf(typeof(T));

        /// <summary>
        /// Returns all instance fields, public and private.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetAllFields(this Type self)
            => self.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Converts fields to a dictionary of Vstring/object pair.
        /// </summary>
        public static IDictionary<string, object> FieldsToDictionary(this object self)
            => self.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(fi => fi.Name, fi => fi.GetValue(self));

        /// <summary>
        /// Converts properties to a dictionary of string/object pair.
        /// </summary>
        public static IDictionary<string, object> PropertiesToDictionary(this object self)
            => self.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(fi => fi.Name, fi => fi.GetValue(self));

        /// <summary>
        /// Given a method info, an object to invoke it on, and args, returns a func object (lambda)
        /// </summary>
        public static Func<T> InvokableMethod<T>(this MethodInfo mi, object self, params object[] args)
            => () => (T)mi.Invoke(self, args);

        /// <summary>
        /// Given a property info, an object to invoke it on, returns a func object (lambda)
        /// </summary>
        public static Func<T> InvokableProperty<T>(this PropertyInfo pi, object self)
            => () => (T)pi.GetValue(self);

        /// <summary>
        /// Given a method info, an object to invoke it on, and args, returns an action object (lambda)
        /// </summary>
        public static Action InvokableMethod(this MethodInfo mi, object self, params object[] args)
            => () => mi.Invoke(self, args);
    }
}