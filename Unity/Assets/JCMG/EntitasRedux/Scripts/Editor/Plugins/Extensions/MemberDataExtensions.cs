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
	/// Additional extension methods for <see cref="MemberData"/>
	/// </summary>
	public static class MemberDataExtensions
	{
		// Code templates
		private const string MEMBER_ASSIGNMENT
			= "		component.${MemberName} = new${MemberNameUpper};";

		private const string DEFAULT_MEMBER_COPY_ASSIGNMENT
			= "		component.${MemberName} = copyComponent.${MemberName};";

		private const string MEMBER_CLONE_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})copyComponent.${MemberName}.Clone();";

		private const string LIST_MEMBER_DEEP_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})JCMG.EntitasRedux.ListTools.DeepCopy(copyComponent.${MemberName});";

		private const string LIST_MEMBER_SHALLOW_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})JCMG.EntitasRedux.ListTools.ShallowCopy(copyComponent.${MemberName});";

		private const string DICTIONARY_MEMBER_SHALLOW_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})JCMG.EntitasRedux.DictionaryTools.ShallowCopy(copyComponent.${MemberName});";

		private const string DICTIONARY_MEMBER_DEEP_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})JCMG.EntitasRedux.DictionaryTools.DeepCopy(copyComponent.${MemberName});";

		private const string DICTIONARY_MEMBER_LIST_DEEP_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})JCMG.EntitasRedux.DictionaryTools.DeepCopyListValue(copyComponent.${MemberName});";

		private const string DICTIONARY_MEMBER_ARRAY_DEEP_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})JCMG.EntitasRedux.DictionaryTools.DeepCopyArrayValue(copyComponent.${MemberName});";

		private const string ARRAY_MEMBER_DEEP_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})JCMG.EntitasRedux.ArrayTools.DeepCopy(copyComponent.${MemberName});";

		private const string ARRAY_MEMBER_SHALLOW_COPY_ASSIGNMENT
			= "		component.${MemberName} = (${MemberCompilableString})copyComponent.${MemberName}.Clone();";

		/// <summary>
		/// Returns a snippet of code for direct member assignment for <paramref name="memberData"/> from a parameter.
		/// </summary>
		public static string GetMemberAssignment(this MemberData memberData)
		{
			return MEMBER_ASSIGNMENT.Replace(memberData);
		}

		/// <summary>
		/// Returns a snippet of code for direct member assignment for <paramref name="memberData"/> from another
		/// component.
		/// </summary>
		public static string GetMemberCopyAssignment(this MemberData memberData)
		{
			return DEFAULT_MEMBER_COPY_ASSIGNMENT.Replace(memberData);
		}

		/// <summary>
		/// Returns a snippet of code for copy member assignment for <paramref name="memberData"/>;  this may be a shallow
		/// or deep copy assignment snippet depending on the <paramref name="memberType"/> or it's generic types if present,
		/// and whether or not they implement <see cref="ICloneable"/>.
		/// </summary>
		public static string GetMemberCopyAssignment(this MemberData memberData, Type memberType)
		{
			var result = string.Empty;

			// In this case, a mutable reference type
			var isUnityObjectType = memberType.IsAssignableFrom(typeof(UnityEngine.Object));
			if (memberType.IsMutableReferenceType() && !isUnityObjectType)
			{
				var hasDefaultConstructor = memberType.HasDefaultConstructor();
				if (memberType.IsList(out var genericType) && hasDefaultConstructor)
				{
					result = GetListMemberCopyAssignment(memberData, genericType);
				}
				else if (memberType.IsDictionary(out var genericKeyType, out var genericValueType) &&
						 hasDefaultConstructor)
				{
					result = GetDictionaryMemberCopyAssignment(memberData, genericKeyType, genericValueType);
				}
				else if (memberType.IsArray)
				{
					var elementType = memberType.GetElementType();
					result = GetArrayMemberCopyAssignment(memberData, elementType);
				}
				// Otherwise if the member type implements ICloneable, clone the instance as assignment
				else if (memberType.ImplementsInterface<ICloneable>())
				{
					result = MEMBER_CLONE_ASSIGNMENT.Replace(memberData);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the member copy assignment code snippet for a member of <see cref="List{T}"/> type.
		/// </summary>
		public static string GetListMemberCopyAssignment(MemberData memberData, Type genericType)
		{
			var result = string.Empty;
			if (genericType.IsMutableReferenceType() && genericType.ImplementsInterface<ICloneable>())
			{
				result = LIST_MEMBER_DEEP_COPY_ASSIGNMENT.Replace(memberData);
			}
			else
			{
				result = LIST_MEMBER_SHALLOW_COPY_ASSIGNMENT.Replace(memberData);
			}

			return result;
		}

		/// <summary>
		/// Returns the member copy assignment code snippet for a member of <see cref="Dictionary{TKey,TValue}"/> type.
		/// </summary>
		public static string GetDictionaryMemberCopyAssignment(
			MemberData memberData,
			Type genericKeyType,
			Type genericValueType)
		{
			var result = string.Empty;
			// Check for more specific deep copy implementation potential
			if (genericValueType.IsList(out var genericListType) &&
				genericListType.IsMutableReferenceType() &&
				genericListType.ImplementsInterface<ICloneable>())
			{
				result = DICTIONARY_MEMBER_LIST_DEEP_COPY_ASSIGNMENT.Replace(memberData);
			}
			else if (genericValueType.IsArray)
			{
				var elementType = genericValueType.GetElementType();
				if (elementType.IsMutableReferenceType() && elementType.ImplementsInterface<ICloneable>())
				{
					result = DICTIONARY_MEMBER_ARRAY_DEEP_COPY_ASSIGNMENT.Replace(memberData);
				}
			}

			// If we can't find a more specific deep copy...
			if (string.IsNullOrEmpty(result))
			{
				// ...and the value type is capable of deep copy, use a 1-level deep copy,...
				if (genericValueType.IsMutableReferenceType() &&
					genericValueType.ImplementsInterface<ICloneable>())
				{
					result = DICTIONARY_MEMBER_DEEP_COPY_ASSIGNMENT.Replace(memberData);
				}
				// ...otherwise if the value ends up not being capable of a deep copy, use shallow copy instead
				else
				{
					result = DICTIONARY_MEMBER_SHALLOW_COPY_ASSIGNMENT.Replace(memberData);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the member copy assignment code snippet for a member of <see cref="Array"/> type.
		/// </summary>
		public static string GetArrayMemberCopyAssignment(MemberData memberData, Type elementType)
		{
			var result = string.Empty;
			if (elementType.IsMutableReferenceType() && elementType.ImplementsInterface<ICloneable>())
			{
				result = ARRAY_MEMBER_DEEP_COPY_ASSIGNMENT.Replace(memberData);
			}
			else
			{
				result = ARRAY_MEMBER_SHALLOW_COPY_ASSIGNMENT.Replace(memberData);
			}

			return result;
		}
	}
}
