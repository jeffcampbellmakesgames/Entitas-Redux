/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

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
