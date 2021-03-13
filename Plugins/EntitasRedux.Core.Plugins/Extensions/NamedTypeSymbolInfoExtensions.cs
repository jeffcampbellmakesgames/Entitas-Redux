using System.Collections.Generic;
using System.Linq;
using Genesis.Plugin;
using Microsoft.CodeAnalysis;

namespace EntitasRedux.Core.Plugins
{
	/// <summary>
	/// Helper methods for <see cref="NamedTypeSymbolInfo"/>.
	/// </summary>
	public static class NamedTypeSymbolInfoExtensions
	{
		public static IEnumerable<MemberData> GetPublicMemberData(this NamedTypeSymbolInfo namedTypeSymbolInfo)
		{
			// Get all public fields and create members for each.
			var publicFieldMembers = namedTypeSymbolInfo.AllPublicMembers
				.Where(x =>
					x.IsPublic() &&
					x.IsKind(SymbolKind.Field) &&
					!x.IsStatic)
				.Cast<IFieldSymbol>();

			var publicPropertyMembers = namedTypeSymbolInfo.AllPublicMembers
				.Where(x =>
					x.IsPublic() &&
					x.IsKind(SymbolKind.Property) &&
					!x.IsStatic)
				.Cast<IPropertySymbol>()
				.Where(x => x.GetMethod != null && x.SetMethod != null);

			// Create member data
			var memberData = publicFieldMembers
				.Select(x => new MemberData(x.Type, x.Name))
				.Concat(publicPropertyMembers.Select(x => new MemberData(
					x.Type.GetFullTypeName(),
					x.Name)))
				.ToArray();

			return memberData;
		}
	}
}
