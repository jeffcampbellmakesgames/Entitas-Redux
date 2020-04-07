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
using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class ComponentDataProvider : IDataProvider,
	                                              IConfigurable,
	                                              ICacheable
	{
		private readonly IComponentDataProvider[] _dataProviders;
		private readonly ContextsComponentDataProvider _contextsComponentDataProvider;
		private readonly AssembliesConfig _assembliesConfig;

		private readonly Type[] _types;

		public ComponentDataProvider() : this(null)
		{
		}

		/// <summary>
		/// Constructor for unit-testing specific component type generation
		/// </summary>
		/// <param name="types"></param>
		internal ComponentDataProvider(Type[] types) : this(types, GetComponentDataProviders())
		{
		}

		internal ComponentDataProvider(Type[] types, IComponentDataProvider[] dataProviders)
		{
			_types = types;
			_dataProviders = dataProviders;
			_contextsComponentDataProvider = new ContextsComponentDataProvider();
			_assembliesConfig = new AssembliesConfig();
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
				new EventComponentDataProvider()
			};
		}

		private ComponentData[] Merge(ComponentData[] priorData, ComponentData[] redundantData)
		{
			var lookup = priorData.ToLookup(data => data.GetTypeName());
			return redundantData
				.Where(data => !lookup.Contains(data.GetTypeName()))
				.Concat(priorData)
				.ToArray();
		}

		private ComponentData CreateDataForComponent(Type type)
		{
			var data = new ComponentData();
			foreach (var provider in _dataProviders)
			{
				provider.Provide(type, data);
			}

			return data;
		}

		private ComponentData[] CreateDataForNonComponent(Type type)
		{
			return GetComponentNames(type)
				.Select(
					componentName =>
					{
						var data = CreateDataForComponent(type);
						data.SetTypeName(componentName.AddComponentSuffix());
						data.SetMemberData(
							new[]
							{
								new MemberData(type.ToCompilableString(), "value")
							});

						return data;
					})
				.ToArray();
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

		private bool HasContexts(Type type)
		{
			return _contextsComponentDataProvider.GetContextNames(type).Length != 0;
		}

		private string[] GetComponentNames(Type type)
		{
			var attr = Attribute
				.GetCustomAttributes(type)
				.OfType<ComponentNameAttribute>()
				.SingleOrDefault();

			if (attr == null)
			{
				return new[]
				{
					type.ToCompilableString().ShortTypeName().AddComponentSuffix()
				};
			}

			return attr.componentNames;
		}

		public Dictionary<string, object> ObjectCache { get; set; }

		public void Configure(GenesisSettings settings)
		{
			foreach (var dataProvider in _dataProviders.OfType<IConfigurable>())
			{
				dataProvider.Configure(settings);
			}

			_contextsComponentDataProvider.Configure(settings);
			_assembliesConfig.Configure(settings);
		}

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Component";

		public CodeGeneratorData[] GetData()
		{
			var assemblies = _assembliesConfig.DoUseWhitelistOfAssemblies
				? ReflectionTools.GetAvailableAssemblies(_assembliesConfig.WhiteListedAssemblies)
				: ReflectionTools.GetAvailableAssemblies();

			var types = _types ?? assemblies.SelectMany(x => x.GetTypes());

			var dataFromComponents = types
				.Where(type => type.ImplementsInterface<IComponent>())
				.Where(type => !type.IsAbstract)
				.Select(CreateDataForComponent)
				.ToArray();

			var dataFromNonComponents = types
				.Where(type => !type.ImplementsInterface<IComponent>())
				.Where(type => !type.IsGenericType)
				.Where(HasContexts)
				.SelectMany(CreateDataForNonComponent)
				.ToArray();

			var mergedData = Merge(dataFromNonComponents, dataFromComponents);

			var dataFromEvents = mergedData
				.Where(data => data.IsEvent())
				.SelectMany(CreateDataForEvents)
				.ToArray();

			return Merge(dataFromEvents, mergedData);
		}
	}
}
