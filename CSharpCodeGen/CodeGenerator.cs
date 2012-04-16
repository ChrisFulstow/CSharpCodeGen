using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace CSharpCodeGen
{
    public static class CodeGenerator
    {
        public static string InitializationCode(this object obj, int indentLevel = 0)
        {
            var type = obj.GetType();

            // string
            if (obj is string)
                return obj.ToString().PrimitiveLiteral();

            // bool
            if (obj is bool)
                return obj.ToString().ToLower();

            // numeric
            var numericLiteral = obj.ToNumericLiteral();
            if (numericLiteral != null)
                return numericLiteral;

            // dictionary
            if (ImplementsGenericType(type, typeof(IDictionary<,>)))
                return ((IDictionary)obj).ToDictionaryLiteral(indentLevel);

            // collection 
            if (ImplementsGenericType(type, typeof(IEnumerable<>)))
                return ((IEnumerable)obj).ToCollectionLiteral();

            // general object
            return obj.ToObjectLiteral(indentLevel);
        }


        /// <summary>
        /// Returns the C# literal representation of a given primitive expression.
        /// (Useful for escaping strings)
        /// </summary>
        private static string PrimitiveLiteral(this string input)
        {
            var writer = new StringWriter();
            var provider = new CSharpCodeProvider();
            provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
            return writer.ToString();
        }


        /// <summary>
        /// Returns C# initialisation code for the given IDictionary
        /// </summary>
        private static string ToDictionaryLiteral(this IDictionary dictionary, int indentLevel = 0)
        {
            var type = dictionary.GetType();
            var entryList = new List<string>();

            foreach (var key in dictionary.Keys)
            {
                var keyString = key.InitializationCode();
                var valueString = dictionary[key].InitializationCode();
                var entryString = string.Format("{0}{{ {1}, {2} }}", Indent(indentLevel + 1), keyString, valueString);
                entryList.Add(entryString);
            }

            var sb = new StringBuilder();
            sb.AppendLine("new " + type.ToGenericTypeString());
            sb.AppendLine(Indent(indentLevel) + "{");
            sb.AppendLine(string.Join(",\n", entryList.ToArray()));
            sb.Append(Indent(indentLevel) + "}");
            return sb.ToString();
        }


        /// <summary>
        /// Returns an indent string of requested size
        /// </summary>
        private static string Indent(int size)
        {
            return new string('\t', size);
        }


        /// <summary>
        /// Returns C# initialisation code for the given IEnumerable
        /// </summary>
        private static string ToCollectionLiteral(this IEnumerable enumerable)
        {
            var type = enumerable.GetType();
            var itemList = new List<string>();

            foreach (var item in enumerable)
            {
                itemList.Add(item.InitializationCode());
            }
            var values = string.Join(", ", itemList.ToArray());
            return string.Format("new {0} {{ {1} }}", type.ToGenericTypeString(), values);
        }


        /// <summary>
        /// Returns C# object initialiser code for the given object
        /// </summary>
        private static string ToObjectLiteral(this object obj, int indentLevel = 0)
        {
            var type = obj.GetType();
            var typeProperties = type.GetProperties();
            var propertyList = new List<string>();
            foreach (var propertyInfo in typeProperties)
            {
                var value = propertyInfo.GetValue(obj, BindingFlags.Public, null, null, null);
                var indent = new string('\t', indentLevel + 1);
                propertyList.Add(string.Format("{0}{1} = {2}", indent, propertyInfo.Name, value.InitializationCode(indentLevel + 1)));
            }
            var braceIndent = new string('\t', indentLevel);
            var propertiesString = "new " + type.Name + "\n" + braceIndent + "{\n" + string.Join(",\n", propertyList.ToArray()) + "\n" + braceIndent + "}";
            return propertiesString;

        }


        /// <summary>
        /// Returns the C# literal representation of the given numeric object
        /// </summary>
        private static string ToNumericLiteral(this object obj)
        {
            var type = obj.GetType();
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.SByte:
                case TypeCode.UInt16: return obj.ToString();
                case TypeCode.Decimal: return obj + "M";
                case TypeCode.Double: return obj + "D";
                case TypeCode.Int64: return obj + "L";
                case TypeCode.Single: return obj + "F";
                case TypeCode.UInt32: return obj + "U";
                case TypeCode.UInt64: return obj + "UL";
                case TypeCode.Object:
                    var isNullableType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
                    if (isNullableType) return Nullable.GetUnderlyingType(type).ToNumericLiteral();
                    break;
            }
            return null;
        }


        /// <summary>
        /// Determines whether the given type implements a particular generic type
        /// </summary>
        private static bool ImplementsGenericType(this Type type, Type genericType)
        {
            var genericInterface = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)
                .FirstOrDefault();

            return (genericInterface != null);
        }


        /// <summary>
        /// Converts a CLR generic type name to its C# representation
        /// </summary>
        private static string ToGenericTypeString(this Type t)
        {
            if (!t.IsGenericType) return t.Name;
            var genericTypeName = t.GetGenericTypeDefinition().Name;
            genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
            var genericArgs = string.Join(",", t.GetGenericArguments().Select(ToGenericTypeString).ToArray());
            return genericTypeName + "<" + genericArgs + ">";
        }
    }
}