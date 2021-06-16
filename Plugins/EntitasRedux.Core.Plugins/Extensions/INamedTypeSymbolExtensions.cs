using System.Linq;
using Genesis.Plugin;
using JCMG.EntitasRedux;
using Microsoft.CodeAnalysis;

namespace EntitasRedux.Core.Plugins
{
	public static class INamedTypeSymbolExtensions
	{
		public static bool HasContexts(this INamedTypeSymbol namedTypeSymbol)
		{
			return namedTypeSymbol.GetContextNames().Any();
		}

		public static string[] GetContextNames(this INamedTypeSymbol namedTypeSymbol)
		{
			return namedTypeSymbol.GetAttributes()
				.Where(x => x.AttributeClass.GetBaseTypesAndThis().Any(x => x.Name == nameof(ContextAttribute)))
				.Select(attributeData =>
				{
					if (attributeData.AttributeClass.Name == nameof(ContextAttribute))
					{
						return attributeData.ConstructorArguments[0].Value.ToString();
					}
					else
					{
						return attributeData.AttributeClass.Name.RemoveAttributeSuffix();
					}
				})
				.ToArray();
		}
	}
}
