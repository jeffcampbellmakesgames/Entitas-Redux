using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace JCMG.EntitasRedux.Editor
{
	public static class SerializationTypeExtensions
	{
		public static string ToCompilableString(this Type type)
		{
			if (SerializationTools.TryGetBuiltInTypeToString(type, out var str))
			{
				return str;
			}
			else if (type.IsGenericType)
			{
				return type.FullName.Split('`')[0] +
					   "<" +
					   string.Join(", ", type.GetGenericArguments().Select(argType => argType.ToCompilableString()).ToArray()) +
					   ">";
			}
			else if (type.IsArray)
			{
				return type.GetElementType().ToCompilableString() + "[" + new string(',', type.GetArrayRank() - 1) + "]";
			}

			return type.IsNested ? type.FullName.Replace('+', '.') : type.FullName;
		}

		public static Type ToType(this string typeString)
		{
			var typeString1 = GenerateTypeString(typeString);
			var type1 = Type.GetType(typeString1);
			if (type1 != null)
			{
				return type1;
			}

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				var type2 = assembly.GetType(typeString1);
				if (type2 != null)
				{
					return type2;
				}
			}

			return null;
		}

		public static string ShortTypeName(this string fullTypeName)
		{
			var strArray = fullTypeName.Split('.');
			return strArray.Last();
		}

		public static string RemoveDots(this string fullTypeName)
		{
			return fullTypeName.Replace(".", string.Empty);
		}

		private static string GenerateTypeString(string typeString)
		{
			if (SerializationTools.TryGetBuiltInTypeToString(typeString, out var str) ||
				SerializationTools.TryGetBuiltInTypeString(typeString, out str))
			{
				return str;
			}

			typeString = GenerateGenericArguments(typeString);
			typeString = GenerateArray(typeString);

			return typeString;
		}

		private static string GenerateGenericArguments(string typeString)
		{
			var separator = new[] { ", " };
			typeString = Regex.Replace(
				typeString,
				"<(?<arg>.*)>",
				m =>
				{
					var typeString1 = GenerateTypeString(m.Groups["arg"].Value);
					return $"`{typeString1.Split(separator, StringSplitOptions.None).Length}[{typeString1}]";
				});
			return typeString;
		}

		private static string GenerateArray(string typeString)
		{
			typeString = Regex.Replace(
				typeString,
				"(?<type>[^\\[]*)(?<rank>\\[,*\\])",
				m => GenerateTypeString(m.Groups["type"].Value) + m.Groups["rank"].Value);
			return typeString;
		}
	}
}
