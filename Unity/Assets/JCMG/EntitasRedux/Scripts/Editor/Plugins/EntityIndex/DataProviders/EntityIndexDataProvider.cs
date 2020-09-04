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
using System.Reflection;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class EntityIndexDataProvider : IDataProvider,
	                                                IConfigurable,
	                                                ICacheable
	{
		private readonly ContextsComponentDataProvider _contextsComponentDataProvider;
		private readonly AssembliesConfig _assembliesConfig;
		private readonly Type[] _types;

		public EntityIndexDataProvider() : this(null)
		{
		}

		internal EntityIndexDataProvider(Type[] types)
		{
			_types = types;
			_contextsComponentDataProvider = new ContextsComponentDataProvider();
			_assembliesConfig = new AssembliesConfig();
		}

		private EntityIndexData[] CreateEntityIndexData(Type type, List<PublicMemberInfo> infos)
		{
			var hasMultiple = infos.Count(i => i.attributes.Count(attr => attr.attribute is AbstractEntityIndexAttribute) == 1) >
			                  1;
			return infos
				.Where(i => i.attributes.Count(attr => attr.attribute is AbstractEntityIndexAttribute) == 1)
				.Select(
					info =>
					{
						var data = new EntityIndexData();
						var attribute = (AbstractEntityIndexAttribute)info.attributes
							.Single(attr => attr.attribute is AbstractEntityIndexAttribute)
							.attribute;

						data.SetEntityIndexType(GetEntityIndexType(attribute));
						data.IsCustom(false);
						data.SetEntityIndexName(type.ToCompilableString().ToComponentName());
						data.SetKeyType(info.type.ToCompilableString());
						data.SetComponentType(type.ToCompilableString());
						data.SetMemberName(info.name);
						data.SetHasMultiple(hasMultiple);
						data.SetContextNames(_contextsComponentDataProvider.GetContextNamesOrDefault(type));

						return data;
					})
				.ToArray();
		}

		private EntityIndexData CreateCustomEntityIndexData(Type type)
		{
			var data = new EntityIndexData();

			var attribute = (CustomEntityIndexAttribute)type.GetCustomAttributes(typeof(CustomEntityIndexAttribute), false)[0];

			data.SetEntityIndexType(type.ToCompilableString());
			data.IsCustom(true);
			data.SetEntityIndexName(type.ToCompilableString().RemoveDots());
			data.SetHasMultiple(false);
			data.SetContextNames(
				new[]
				{
					attribute.contextType.ToCompilableString().ShortTypeName().RemoveContextSuffix()
				});

			var getMethods = type
				.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				.Where(method => Attribute.IsDefined(method, typeof(EntityIndexGetMethodAttribute)))
				.Select(
					method => new MethodData(
						method.ReturnType.ToCompilableString(),
						method.Name,
						method.GetParameters()
							.Select(p => new MemberData(p.ParameterType, p.Name))
							.ToArray()))
				.ToArray();

			data.SetCustomMethods(getMethods);

			return data;
		}

		private string GetEntityIndexType(AbstractEntityIndexAttribute attribute)
		{
			switch (attribute.entityIndexType)
			{
				case EntityIndexType.EntityIndex:
					return "JCMG.EntitasRedux.EntityIndex";
				case EntityIndexType.PrimaryEntityIndex:
					return "JCMG.EntitasRedux.PrimaryEntityIndex";
				default:
					throw new Exception("Unhandled EntityIndexType: " + attribute.entityIndexType);
			}
		}

		public Dictionary<string, object> ObjectCache { get; set; }

		public void Configure(GenesisSettings settings)
		{
			_contextsComponentDataProvider.Configure(settings);
			_assembliesConfig.Configure(settings);
		}

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Entity Index";

		public CodeGeneratorData[] GetData()
		{
			var assemblies = _assembliesConfig.DoUseWhitelistOfAssemblies
				? ReflectionTools.GetAvailableAssemblies(_assembliesConfig.WhiteListedAssemblies)
				: ReflectionTools.GetAvailableAssemblies();

			var types = _types ??
			            assemblies.SelectMany(x => x.GetTypes());

			var entityIndexData = types
				.Where(type => !type.IsAbstract)
				.Where(type => type.ImplementsInterface<IComponent>())
				.ToDictionary(
					type => type,
					type => type.GetPublicMemberInfos())
				.Where(kv => kv.Value.Any(info => info.attributes.Any(attr => attr.attribute is AbstractEntityIndexAttribute)))
				.SelectMany(kv => CreateEntityIndexData(kv.Key, kv.Value));

			var customEntityIndexData = types
				.Where(type => !type.IsAbstract)
				.Where(type => Attribute.IsDefined(type, typeof(CustomEntityIndexAttribute)))
				.Select(CreateCustomEntityIndexData);

			return entityIndexData
				.Concat(customEntityIndexData)
				.ToArray();
		}
	}

	public static class EntityIndexDataExtension
	{
		public const string ENTITY_INDEX_TYPE = "EntityIndex.Type";

		public const string ENTITY_INDEX_IS_CUSTOM = "EntityIndex.Custom";
		public const string ENTITY_INDEX_CUSTOM_METHODS = "EntityIndex.CustomMethods";

		public const string ENTITY_INDEX_NAME = "EntityIndex.Name";
		public const string ENTITY_INDEX_CONTEXT_NAMES = "EntityIndex.ContextNames";

		public const string ENTITY_INDEX_KEY_TYPE = "EntityIndex.KeyType";
		public const string ENTITY_INDEX_COMPONENT_TYPE = "EntityIndex.ComponentType";
		public const string ENTITY_INDEX_MEMBER_NAME = "EntityIndex.MemberName";
		public const string ENTITY_INDEX_HAS_MULTIPLE = "EntityIndex.HasMultiple";

		public static string GetEntityIndexType(this EntityIndexData data)
		{
			return (string)data[ENTITY_INDEX_TYPE];
		}

		public static void SetEntityIndexType(this EntityIndexData data, string type)
		{
			data[ENTITY_INDEX_TYPE] = type;
		}

		public static bool IsCustom(this EntityIndexData data)
		{
			return (bool)data[ENTITY_INDEX_IS_CUSTOM];
		}

		public static void IsCustom(this EntityIndexData data, bool isCustom)
		{
			data[ENTITY_INDEX_IS_CUSTOM] = isCustom;
		}

		public static MethodData[] GetCustomMethods(this EntityIndexData data)
		{
			return (MethodData[])data[ENTITY_INDEX_CUSTOM_METHODS];
		}

		public static void SetCustomMethods(this EntityIndexData data, MethodData[] methods)
		{
			data[ENTITY_INDEX_CUSTOM_METHODS] = methods;
		}

		public static string GetEntityIndexName(this EntityIndexData data)
		{
			return (string)data[ENTITY_INDEX_NAME];
		}

		public static void SetEntityIndexName(this EntityIndexData data, string name)
		{
			data[ENTITY_INDEX_NAME] = name;
		}

		public static string[] GetContextNames(this EntityIndexData data)
		{
			return (string[])data[ENTITY_INDEX_CONTEXT_NAMES];
		}

		public static void SetContextNames(this EntityIndexData data, string[] contextNames)
		{
			data[ENTITY_INDEX_CONTEXT_NAMES] = contextNames;
		}

		public static string GetKeyType(this EntityIndexData data)
		{
			return (string)data[ENTITY_INDEX_KEY_TYPE];
		}

		public static void SetKeyType(this EntityIndexData data, string type)
		{
			data[ENTITY_INDEX_KEY_TYPE] = type;
		}

		public static string GetComponentType(this EntityIndexData data)
		{
			return (string)data[ENTITY_INDEX_COMPONENT_TYPE];
		}

		public static void SetComponentType(this EntityIndexData data, string type)
		{
			data[ENTITY_INDEX_COMPONENT_TYPE] = type;
		}

		public static string GetMemberName(this EntityIndexData data)
		{
			return (string)data[ENTITY_INDEX_MEMBER_NAME];
		}

		public static void SetMemberName(this EntityIndexData data, string memberName)
		{
			data[ENTITY_INDEX_MEMBER_NAME] = memberName;
		}

		public static bool GetHasMultiple(this EntityIndexData data)
		{
			return (bool)data[ENTITY_INDEX_HAS_MULTIPLE];
		}

		public static void SetHasMultiple(this EntityIndexData data, bool hasMultiple)
		{
			data[ENTITY_INDEX_HAS_MULTIPLE] = hasMultiple;
		}
	}
}
