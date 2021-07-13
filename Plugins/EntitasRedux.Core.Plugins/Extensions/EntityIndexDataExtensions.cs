using Genesis.Plugin;

namespace EntitasRedux.Core.Plugins
{
	public static class EntityIndexDataExtensions
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

		public static string GetMemberEntityIndexName(this EntityIndexData data)
		{
			return data.GetHasMultiple()
				? (string)data[ENTITY_INDEX_NAME] + data.GetMemberName().UppercaseFirst()
				: (string)data[ENTITY_INDEX_NAME];
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
