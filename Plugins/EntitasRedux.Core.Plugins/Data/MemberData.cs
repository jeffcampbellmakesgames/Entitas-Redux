using System;
using Genesis.Plugin;
using Microsoft.CodeAnalysis;

namespace EntitasRedux.Core.Plugins
{
	public class MemberData
	{
		public readonly ITypeSymbol memberTypeSymbol;
		public readonly Type memberType;
		public readonly string name;
		public readonly string compilableTypeString;

		public MemberData(Type type, string memberName)
		{
			memberType = type;
			compilableTypeString = memberType.ToCompilableString();
			name = memberName;
		}

		public MemberData(string typeString, string memberName)
		{
			compilableTypeString = typeString;
			name = memberName;
		}

		public MemberData(ITypeSymbol typeSymbol, string memberName)
		{
			memberTypeSymbol = typeSymbol;
			compilableTypeString = typeSymbol.GetFullTypeName();
			name = memberName;
		}

		public MemberData(ICachedNamedTypeSymbol cachedNamedTypeSymbol, string memberName)
		{
			memberTypeSymbol = cachedNamedTypeSymbol.NamedTypeSymbol;
			compilableTypeString = cachedNamedTypeSymbol.FullTypeName;
			name = memberName;
		}
	}
}
