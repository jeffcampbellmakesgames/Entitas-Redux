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

using Genesis.Plugin;
using Genesis.Shared;
using JCMG.EntitasRedux;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntitasRedux.Core.Plugins
{
	internal sealed class EntityIndexDataProvider : IDataProvider,
													IConfigurable,
													ICacheable
	{
		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Entity Index";

		private readonly ContextsComponentDataProvider _contextsComponentDataProvider;
		private IMemoryCache _memoryCache;
		private AssembliesConfig _assembliesConfig;

		public EntityIndexDataProvider()
		{
			_contextsComponentDataProvider = new ContextsComponentDataProvider();
		}

		public void Configure(IGenesisConfig genesisConfig)
		{
			_contextsComponentDataProvider.Configure(genesisConfig);
			_assembliesConfig = genesisConfig.CreateAndConfigure<AssembliesConfig>();
		}

		/// <inheritdoc />
		public void SetCache(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public CodeGeneratorData[] GetData()
		{
			var componentName = nameof(IComponent);
			var namedTypeSymbols =
				_assembliesConfig.FilterTypeSymbols(_memoryCache.GetNamedTypeSymbols());
			var entityIndexData = namedTypeSymbols
				.Where(cachedNamedTypeSymbol => !cachedNamedTypeSymbol.NamedTypeSymbol.IsAbstract)
				.Where(cachedNamedTypeSymbol => cachedNamedTypeSymbol.ImplementsInterface(componentName))
				.ToDictionary(
					cachedNamedTypeSymbol => cachedNamedTypeSymbol,
					cachedNamedTypeSymbol => cachedNamedTypeSymbol.GetPublicMemberData())
				.Where(kv => kv.Value.Any(info =>
				{
					var hasIndexAttr = info.memberFieldSymbol != null &&
					                   info.memberFieldSymbol.GetAttributes()
						                   .HasAttribute(nameof(AbstractEntityIndexAttribute), canInherit: true);
						return hasIndexAttr;
				}))
				.SelectMany(kv => CreateEntityIndexData(kv.Key, kv.Value));

			var customEntityIndexData = namedTypeSymbols
				.Where(cachedNamedTypeSymbol => !cachedNamedTypeSymbol.NamedTypeSymbol.IsAbstract)
				.Where(cachedNamedTypeSymbol => cachedNamedTypeSymbol.HasAttribute<CustomEntityIndexAttribute>())
				.Select(CreateCustomEntityIndexData);

			return entityIndexData
				.Concat(customEntityIndexData)
				.ToArray();
		}

		private EntityIndexData[] CreateEntityIndexData(ICachedNamedTypeSymbol cachedNamedTypeSymbol, IEnumerable<MemberData> memberData)
		{
			var hasMultiple = memberData
				.Count(i => i.memberFieldSymbol.GetAttributes()
					.HasAttribute(nameof(AbstractEntityIndexAttribute), canInherit:true)) > 1;

			return memberData
				.Where(i => i.memberFieldSymbol.GetAttributes()
					.HasAttribute(nameof(AbstractEntityIndexAttribute), canInherit:true))
				.Select(
					info =>
					{
						var data = new EntityIndexData();
						var attribute = info.memberFieldSymbol.GetAttributes()
							.GetAttributes(nameof(AbstractEntityIndexAttribute), canInherit:true)
							.Single();

						data.SetEntityIndexType(GetEntityIndexType(attribute));
						data.IsCustom(false);
						data.SetEntityIndexName(cachedNamedTypeSymbol.FullTypeName.ToComponentName());
						data.SetKeyType(info.compilableTypeString);
						data.SetComponentType(cachedNamedTypeSymbol.FullTypeName);
						data.SetMemberName(info.name);
						data.SetHasMultiple(hasMultiple);
						data.SetContextNames(_contextsComponentDataProvider.GetContextNamesOrDefault(cachedNamedTypeSymbol));

						return data;
					})
				.ToArray();
		}

		private EntityIndexData CreateCustomEntityIndexData(ICachedNamedTypeSymbol cachedNamedTypeSymbol)
		{
			var data = new EntityIndexData();
			var attribute = cachedNamedTypeSymbol.GetAttributes(nameof(CustomEntityIndexAttribute)).Single();
			var contextType = (ITypeSymbol)attribute.ConstructorArguments[0].Value;

			var fullTypeName = cachedNamedTypeSymbol.FullTypeName;
			data.SetEntityIndexType(fullTypeName);
			data.IsCustom(true);
			data.SetEntityIndexName(fullTypeName.RemoveDots());
			data.SetHasMultiple(false);
			data.SetContextNames(
				new[]
				{
					contextType.GetFullTypeName().ShortTypeName().RemoveContextSuffix()
				});

			var getMethods = contextType.GetAllMembers()
				.Where(method => method.HasAttribute<EntityIndexGetMethodAttribute>())
				.Select(
					method => new MethodData(
						method.GetReturnType().GetFullTypeName(),
						method.Name,
						method.GetParameters()
							.Select(p => new MemberData(p.Type.GetFullTypeName(), p.Name))
							.ToArray()))
				.ToArray();

			data.SetCustomMethods(getMethods);

			return data;
		}

		private string GetEntityIndexType(AttributeData attribute)
		{
			return attribute.AttributeClass.Name switch
			{
				nameof(EntityIndexAttribute) => "JCMG.EntitasRedux.EntityIndex",
				nameof(PrimaryEntityIndexAttribute) => "JCMG.EntitasRedux.PrimaryEntityIndex",
				_ => throw new Exception(
					$"Unhandled EntityIndex Type: {attribute.AttributeClass.GetFullTypeName()}")
			};
		}
	}
}
