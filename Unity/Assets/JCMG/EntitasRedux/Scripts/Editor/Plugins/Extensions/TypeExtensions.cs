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
using System.Collections.Generic;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	/// <summary>
	/// Helper methods for <see cref="Type"/>
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Returns true if <paramref name="type"/> implements either <see cref="IList{T}"/> or
		/// <see cref="IReadOnlyList{T}"/>. If true, <paramref name="genericType"/> will be initialized with the list's
		/// generic type value.
		/// </summary>
		/// <param name="type"></param>
		/// <param name = "genericType"> </param>
		/// <returns></returns>
		public static bool IsList(this Type type, out Type genericType)
		{
			genericType = null;

			var typeToCheck = type;
			while (typeToCheck != null)
			{
				// If not a generic type, continue checking the base type
				if (!typeToCheck.IsGenericType)
				{
					typeToCheck = typeToCheck.BaseType;
					continue;
				}

				// If not a generic list or derived from a generic list, continue checking the base type
				var genericTypeDef = typeToCheck.GetGenericTypeDefinition();
				if (genericTypeDef != typeof(List<>))
				{
					typeToCheck = typeToCheck.BaseType;
					continue;
				}

				genericType = typeToCheck.GetGenericArguments()[0];
				break;
			}

			return genericType != null;
		}

		/// <summary>
		/// Returns true if <paramref name="type"/> is a <see cref="Dictionary{TKey,TValue}"/>, otherwise false. If true,
		/// <paramref name="genericValueType"/> and <paramref name="genericValueType"/> will be initialized to the value
		/// and key type of the dictionary.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="genericKeyType"></param>
		/// <param name="genericValueType"></param>
		/// <returns></returns>
		public static bool IsDictionary(this Type type, out Type genericKeyType, out Type genericValueType)
		{
			genericKeyType = null;
			genericValueType = null;

			var typeToCheck = type;
			while (typeToCheck != null)
			{
				// If not a generic type, continue checking the base type
				if (!typeToCheck.IsGenericType)
				{
					typeToCheck = typeToCheck.BaseType;
					continue;
				}

				// If not a generic list or derived from a generic list, continue checking the base type
				var genericTypeDef = typeToCheck.GetGenericTypeDefinition();
				if (genericTypeDef != typeof(Dictionary<,>))
				{
					typeToCheck = typeToCheck.BaseType;
					continue;
				}

				var genericArguments = typeToCheck.GetGenericArguments();
				genericKeyType = genericArguments[0];
				genericValueType = genericArguments[1];
				break;
			}

			return genericKeyType != null && genericValueType != null;
		}

		/// <summary>
		/// Returns true if <paramref name="type"/> has a default empty constructor, otherwise false.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool HasDefaultConstructor(this Type type)
		{
			return type.GetConstructor(Type.EmptyTypes) != null;
		}

		/// <summary>
		/// Returns true if <paramref name="type"/> is a mutable reference type, otherwise false.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsMutableReferenceType(this Type type)
		{
			return !type.IsValueType && type != typeof(string);
		}
	}
}
