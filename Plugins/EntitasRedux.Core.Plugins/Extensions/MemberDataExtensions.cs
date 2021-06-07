using Genesis.Plugin;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace EntitasRedux.Core.Plugins
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
		/// or deep copy assignment snippet depending on the <paramref name="memberTypeSymbol"/> or it's generic types if present,
		/// and whether or not they implement <see cref="ICloneable"/>.
		/// </summary>
		public static string GetMemberCopyAssignment(this MemberData memberData, ITypeSymbol memberTypeSymbol)
		{
			var result = string.Empty;
			var hasDefaultConstructor = memberTypeSymbol.HasDefaultConstructor();

			// If it's a mutable reference type and not a native Unity Object type, we can likely generate some custom
			// copy code.
			if (memberTypeSymbol.IsMutableReferenceType() && !memberTypeSymbol.IsUnityObject())
			{
				if (memberTypeSymbol.IsList(out var genericListElementType) && hasDefaultConstructor)
				{
					result = GetListMemberCopyAssignment(memberData, genericListElementType);
				}
				else if (memberTypeSymbol.IsDictionary(out var genericKeyType, out var genericValueType) &&
				         hasDefaultConstructor)
				{
					result = GetDictionaryMemberCopyAssignment(memberData, genericKeyType, genericValueType);
				}
				// We only have deep-copy support for rank-1 arrays, otherwise ICloneable.Clone will be used since all
				// arrays implement ICloneable.
				else if (memberTypeSymbol is IArrayTypeSymbol arrayMemberTypeSymbol &&
				         arrayMemberTypeSymbol.Rank == 1)
				{
					var elementType = arrayMemberTypeSymbol.ElementType;
					result = GetArrayMemberCopyAssignment(memberData, elementType);
				}
				// Otherwise if the member type implements ICloneable, clone the instance as assignment
				else if (memberTypeSymbol.ImplementsInterface<ICloneable>())
				{
					result = MEMBER_CLONE_ASSIGNMENT.Replace(memberData);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the member copy assignment code snippet for a member of <see cref="List{T}"/> type.
		/// </summary>
		public static string GetListMemberCopyAssignment(MemberData memberData, ITypeSymbol genericTypeSymbol)
		{
			var result = string.Empty;
			if (genericTypeSymbol.IsReferenceType && genericTypeSymbol.ImplementsInterface<ICloneable>())
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
			ITypeSymbol genericKeyTypeSymbol,
			ITypeSymbol genericValueTypeSymbol)
		{
			var result = string.Empty;

			// Check for more specific deep copy implementation potential
			if (genericValueTypeSymbol.IsList(out var genericListElementTypeSymbol) &&
			    genericListElementTypeSymbol.IsMutableReferenceType() &&
			    genericListElementTypeSymbol.ImplementsInterface<ICloneable>())
			{
				result = DICTIONARY_MEMBER_LIST_DEEP_COPY_ASSIGNMENT.Replace(memberData);
			}
			else if (genericValueTypeSymbol.IsArrayType())
			{
				var elementTypeSymbol = ((IArrayTypeSymbol)genericValueTypeSymbol).ElementType;
				if (elementTypeSymbol.IsReferenceType && elementTypeSymbol.ImplementsInterface<ICloneable>())
				{
					result = DICTIONARY_MEMBER_ARRAY_DEEP_COPY_ASSIGNMENT.Replace(memberData);
				}
			}

			// If we can't find a more specific deep copy...
			if (string.IsNullOrEmpty(result))
			{
				// ...and the value type is capable of deep copy, use a 1-level deep copy,...
				if (genericValueTypeSymbol.IsReferenceType &&
				    genericValueTypeSymbol.ImplementsInterface<ICloneable>())
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
		public static string GetArrayMemberCopyAssignment(MemberData memberData, ITypeSymbol elementTypeSymbol)
		{
			var result = string.Empty;
			if (elementTypeSymbol.IsReferenceType && elementTypeSymbol.ImplementsInterface<ICloneable>())
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
