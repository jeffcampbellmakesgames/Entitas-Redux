using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Extension methods for <see cref="Type"/>.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// This is the set of types from the C# keyword list to their alias.
		/// </summary>
		private static readonly Dictionary<Type, string> TYPE_ALIAS = new Dictionary<Type, string>
		{
			{ typeof(bool), "bool" },
			{ typeof(byte), "byte" },
			{ typeof(char), "char" },
			{ typeof(decimal), "decimal" },
			{ typeof(double), "double" },
			{ typeof(float), "float" },
			{ typeof(int), "int" },
			{ typeof(long), "long" },
			{ typeof(object), "object" },
			{ typeof(sbyte), "sbyte" },
			{ typeof(short), "short" },
			{ typeof(string), "string" },
			{ typeof(uint), "uint" },
			{ typeof(ulong), "ulong" },
		    // Yes, this is an odd one.  Technically it's a type though.
		    { typeof(void), "void" }
		};

		/// <summary>
		/// This is the set of types from the C# keyword list to their alias.
		/// </summary>
		private static readonly Dictionary<string, string> STR_TYPE_ALIAS = new Dictionary<string, string>
		{
			{ typeof(bool).FullName, "bool" },
			{ typeof(byte).FullName, "byte" },
			{ typeof(char).FullName, "char" },
			{ typeof(decimal).FullName, "decimal" },
			{ typeof(double).FullName, "double" },
			{ typeof(float).FullName, "float" },
			{ typeof(int).FullName, "int" },
			{ typeof(long).FullName, "long" },
			{ typeof(object).FullName, "object" },
			{ typeof(sbyte).FullName, "sbyte" },
			{ typeof(short).FullName, "short" },
			{ typeof(string).FullName, "string" },
			{ typeof(uint).FullName, "uint" },
			{ typeof(ulong).FullName, "ulong" },
		    // Yes, this is an odd one.  Technically it's a type though.
		    { typeof(void).FullName, "void" }
		};

		public const string ARRAY_SHORT_NAME = "{0}Array";
		public const char BACKTICK_CHAR = '`';
		public const char LEFT_CHEVRON_CHAR = '<';

		/// <summary>
		/// Returns true if <typeparamref name="T"/> implements interface <paramref name="type"/>.
		/// </summary>
		public static bool ImplementsInterface<T>(this Type type)
		{
			var interfaceType = typeof(T);

			Debug.Assert(interfaceType.IsInterface);

			var interfaces = type.GetInterfaces();

			return interfaces.Any(x => x == interfaceType);
		}

		/// <summary>
		/// Returns a list of <see cref="PublicMemberInfo"/> instances for all public members on this
		/// <paramref name="type"/>.
		/// </summary>
		public static List<PublicMemberInfo> GetPublicMemberInfos(this Type type)
		{
			var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			var publicMemberInfoList = new List<PublicMemberInfo>(fields.Length + properties.Length);
			for (var index = 0; index < fields.Length; ++index)
			{
				publicMemberInfoList.Add(new PublicMemberInfo(fields[index]));
			}

			for (var index = 0; index < properties.Length; ++index)
			{
				var info = properties[index];
				if (info.CanRead && info.CanWrite && info.GetIndexParameters().Length == 0)
				{
					publicMemberInfoList.Add(new PublicMemberInfo(info));
				}
			}

			return publicMemberInfoList;
		}

		/// <summary>
		/// Returns a safe-readable version of a short type name, without generic or array characters.
		/// </summary>
		public static string GetHumanReadableName(this Type type)
		{
			var result = type.GetTypeNameOrAlias().UppercaseFirst();
			if (type.IsArray)
			{
				var elementType = type.GetElementType();
				result = string.Format(ARRAY_SHORT_NAME, elementType.GetTypeNameOrAlias());
			}
			else if (type.IsGenericType)
			{
				var backTickIndex = result.IndexOf(BACKTICK_CHAR);
				if (backTickIndex > 0)
				{
					result = result.Remove(backTickIndex);
				}

				var genericTypeParameters = type.GetGenericArguments();
				for (var i = 0; i < genericTypeParameters.Length; i++)
				{
					result += genericTypeParameters[i].GetHumanReadableName();
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the short type name or the C# alias for this type.
		/// </summary>
		public static string GetTypeNameOrAlias(this Type type)
		{
			// Lookup alias for type
			if (TYPE_ALIAS.TryGetValue(type, out var alias))
			{
				return alias;
			}

			// Default to CLR type name
			return type.Name;
		}

		/// <summary>
		/// Returns the type name or the C# alias for this type.
		/// </summary>
		public static string GetTypeNameOrAlias(this string fullTypeName)
		{
			// Lookup alias for type
			if (STR_TYPE_ALIAS.TryGetValue(fullTypeName, out var alias))
			{
				return alias;
			}

			// Default to CLR type name
			return fullTypeName;
		}

		/// <summary>
		/// Returns the full type name for this type.
		/// </summary>
		public static string GetFullTypeName(this Type type)
		{
			var result = type.FullName;
			if (type.IsGenericType)
			{
				var backTickIndex = result.IndexOf(BACKTICK_CHAR);
				if (backTickIndex > 0)
				{
					result = result.Remove(backTickIndex);
				}

				result += "<";
				var genericTypeParameters = type.GetGenericArguments();
				for (var i = 0; i < genericTypeParameters.Length; ++i)
				{
					var typeParamName = genericTypeParameters[i].GetFullTypeName();
					result += (i == 0 ? typeParamName : "," + typeParamName);
				}
				result += ">";
			}

			return result.Replace('+', '.');
		}
	}
}
