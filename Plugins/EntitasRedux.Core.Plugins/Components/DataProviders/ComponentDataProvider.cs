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
using Serilog;
using System.Linq;

namespace EntitasRedux.Core.Plugins
{
	internal sealed class ComponentDataProvider : IDataProvider,
												  IConfigurable,
												  ICacheable
	{
		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private readonly ILogger _logger;
		private readonly IComponentDataProvider[] _dataProviders;
		private readonly ContextsComponentDataProvider _contextsComponentDataProvider;
		private readonly AssembliesConfig _assembliesConfig;

		private IMemoryCache _memoryCache;

		private const string NAME = "Component";

		public ComponentDataProvider() : this(GetComponentDataProviders())
		{
		}

		public ComponentDataProvider(IComponentDataProvider[] dataProviders)
		{
			_dataProviders = dataProviders;
			_contextsComponentDataProvider = new ContextsComponentDataProvider();

			_logger = Log.ForContext<ComponentDataProvider>();
			_assembliesConfig = new AssembliesConfig();
		}

		public CodeGeneratorData[] GetData()
		{
			var namedTypeSymbols =
				_assembliesConfig.FilterTypeSymbols(_memoryCache.GetNamedTypeSymbols());
			var dontGenerateAttributeName = nameof(DontGenerateAttribute);
			var generateNamedTypeSymbols = namedTypeSymbols
				.Where(namedTypeSymbol => !namedTypeSymbol.HasAttribute(dontGenerateAttributeName));

			var componentName = nameof(IComponent);
			var dataFromComponents = generateNamedTypeSymbols
				.Where(cachedNamedTypeSymbol => cachedNamedTypeSymbol.ImplementsInterface(componentName))
				.Where(cachedNamedTypeSymbol => !cachedNamedTypeSymbol.NamedTypeSymbol.IsAbstract)
				.SelectMany(CreateDataForComponents)
				.ToArray();

			var dataFromNonComponents = generateNamedTypeSymbols
				.Where(cachedNamedTypeSymbol => !cachedNamedTypeSymbol.ImplementsInterface(componentName))
				.Where(cachedNamedTypeSymbol => !cachedNamedTypeSymbol.NamedTypeSymbol.IsGenericType)
				.Where(HasContexts)
				.SelectMany(CreateDataForNonComponents)
				.ToArray();

			var mergedData = Merge(dataFromNonComponents, dataFromComponents);
			var dataFromEvents = mergedData
				.Where(data => data.IsEvent())
				.SelectMany(CreateDataForEvents)
				.ToArray();

			var finalMergedData = Merge(dataFromEvents, mergedData).ToArray();

			ComponentValidationTools.ValidateComponentData(_logger, finalMergedData);

			return finalMergedData;
		}

		/// <inheritdoc />
		public void SetCache(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		private static IComponentDataProvider[] GetComponentDataProviders()
		{
			return new IComponentDataProvider[]
			{
				new ComponentTypeComponentDataProvider(),
				new MemberDataComponentDataProvider(),
				new ContextsComponentDataProvider(),
				new IsUniqueComponentDataProvider(),
				new FlagPrefixComponentDataProvider(),
				new ShouldGenerateComponentComponentDataProvider(),
				new ShouldGenerateMethodsComponentDataProvider(),
				new ShouldGenerateComponentIndexComponentDataProvider(),
				new EventComponentDataProvider(),
				new CleanupComponentDataProvider()
			};
		}

		private ComponentData[] Merge(ComponentData[] priorData, ComponentData[] redundantData)
		{
			return redundantData
				.Concat(priorData)
				.Distinct(new ComponentDataEqualityComparer())
				.ToArray();
		}

		private ComponentData[] CreateDataForComponents(ICachedNamedTypeSymbol cachedNamedTypeSymbol)
		{
			return GetComponentNames(cachedNamedTypeSymbol)
				.Select(
					componentName =>
					{
						var data = CreateDataForComponent(cachedNamedTypeSymbol);
						return data;
					})
				.ToArray();
		}

		private ComponentData[] CreateDataForNonComponents(ICachedNamedTypeSymbol cachedNamedTypeSymbol)
		{
			return GetComponentNames(cachedNamedTypeSymbol)
				.Select(
					componentName =>
					{
						var data = CreateDataForComponent(cachedNamedTypeSymbol);
						data.SetTypeName(componentName.AddComponentSuffix());
						data.SetMemberData(
							new[]
							{
								new MemberData(cachedNamedTypeSymbol, "value")
							});

						return data;
					})
				.ToArray();
		}

		private ComponentData CreateDataForComponent(ICachedNamedTypeSymbol namedTypeSymbol)
		{
			var data = new ComponentData();
			foreach (var provider in _dataProviders)
			{
				provider.Provide(namedTypeSymbol, data);
			}

			return data;
		}

		private ComponentData[] CreateDataForEvents(ComponentData data)
		{
			return data.GetContextNames()
				.SelectMany(
					contextName =>
						data.GetEventData()
							.Select(
								eventData =>
								{
									var dataForEvent = new ComponentData(data);
									dataForEvent.IsEvent(false);
									dataForEvent.IsUnique(false);
									dataForEvent.ShouldGenerateComponent(false);
									dataForEvent.RemoveCleanupData();
									var eventComponentName = data.EventComponentName(eventData);
									var eventTypeSuffix = eventData.GetEventTypeSuffix();
									var optionalContextName =
										dataForEvent.GetContextNames().Length > 1 ? contextName : string.Empty;
									var listenerComponentName =
										optionalContextName + eventComponentName + eventTypeSuffix.AddListenerSuffix();
									dataForEvent.SetTypeName(listenerComponentName.AddComponentSuffix());
									dataForEvent.SetMemberData(
										new[]
										{
											new MemberData(
												"System.Collections.Generic.List<I" + listenerComponentName + ">",
												"value")
										});
									dataForEvent.SetContextNames(
										new[]
										{
											contextName
										});
									return dataForEvent;
								})
							.ToArray())
				.ToArray();
		}

		private bool HasContexts(ICachedNamedTypeSymbol cachedNamedTypeSymbol)
		{
			return cachedNamedTypeSymbol.NamedTypeSymbol.GetContextNames().Length != 0;
		}

		private string[] GetComponentNames(ICachedNamedTypeSymbol namedTypeSymbol)
		{
			// if there are not any
			var componentAttrTypeSymbols = namedTypeSymbol.GetAttributes(nameof(ComponentNameAttribute));
			if (!componentAttrTypeSymbols.Any())
			{
				var componentNames = new[]
				{
					namedTypeSymbol.TypeName
				};

				return componentNames;
			}

			return componentAttrTypeSymbols
				.SelectMany(x => x.ConstructorArguments)
				.SelectMany(x => x.Values)
				.Select(x => x.Value.ToString())
				.ToArray();
		}

		public void Configure(IGenesisConfig genesisConfig)
		{
			foreach (var dataProvider in _dataProviders.OfType<IConfigurable>())
			{
				dataProvider.Configure(genesisConfig);
			}

			_contextsComponentDataProvider.Configure(genesisConfig);
			_assembliesConfig.Configure(genesisConfig);
		}
	}
}
