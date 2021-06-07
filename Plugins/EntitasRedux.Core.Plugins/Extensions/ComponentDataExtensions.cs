using System.Diagnostics;
using System.Linq;
using Genesis.Plugin;
using JCMG.EntitasRedux;

namespace EntitasRedux.Core.Plugins
{
	public static class ComponentDataExtensions
	{
		#region TypeName

		public const string COMPONENT_TYPE = "Component.TypeName";

		public static string ToComponentName(this string fullTypeName)
		{
			return fullTypeName.ShortTypeName().RemoveComponentSuffix();
		}

		public static string GetTypeName(this ComponentData data)
		{
			return (string)data[COMPONENT_TYPE];
		}

		public static void SetTypeName(this ComponentData data, string fullTypeName)
		{
			data[COMPONENT_TYPE] = fullTypeName;
		}

		#endregion

		#region MemberInfos

		public const string COMPONENT_MEMBER_DATA = "Component.MemberData";

		public static MemberData[] GetMemberData(this ComponentData data)
		{
			return (MemberData[])data[COMPONENT_MEMBER_DATA];
		}

		public static void SetMemberData(this ComponentData data, MemberData[] memberInfos)
		{
			data[COMPONENT_MEMBER_DATA] = memberInfos;
		}

		#endregion

		#region Cleanup

		private const string CLEANUP_DATA_KEY = "Component.Cleanup.Data";

		public static void SetCleanupData(this ComponentData data, CleanupMode[] cleanupData)
		{
			data[CLEANUP_DATA_KEY] = cleanupData;
		}

		public static bool HasCleanupData(this ComponentData data)
		{
			return data.ContainsKey(CLEANUP_DATA_KEY);
		}

		public static void RemoveCleanupData(this ComponentData data)
		{
			data.Remove(CLEANUP_DATA_KEY);
		}

		public static bool HasCleanupRemoveComponentData(this ComponentData data)
		{
			return data.ContainsKey(CLEANUP_DATA_KEY) &&
			       ((CleanupMode[])data[CLEANUP_DATA_KEY]).Any(x => x == CleanupMode.RemoveComponent);
		}

		public static bool HasCleanupDestroyEntityData(this ComponentData data)
		{
			return data.ContainsKey(CLEANUP_DATA_KEY) &&
			       ((CleanupMode[])data[CLEANUP_DATA_KEY]).Any(x => x == CleanupMode.DestroyEntity);
		}

		public static string GetCleanupRemoveSystemClassName(this ComponentData data, string contextName)
		{
			Debug.Assert(data.HasCleanupRemoveComponentData());

			const string CLASS_NAME = "Remove${componentName}From${ContextName}EntitiesSystem";

			return CLASS_NAME.Replace(contextName).Replace(data, contextName);
		}

		public static string GetCleanupDestroySystemClassName(this ComponentData data, string contextName)
		{
			Debug.Assert(data.HasCleanupDestroyEntityData());

			const string CLASS_NAME = "Destroy${ContextName}EntitiesWith${componentName}System";

			return CLASS_NAME.Replace(contextName).Replace(data, contextName);
		}

		#endregion

		#region ContextName

		public const string COMPONENT_CONTEXTS = "Component.ContextNames";

		public static string[] GetContextNames(this ComponentData data)
		{
			return (string[])data[COMPONENT_CONTEXTS];
		}

		public static void SetContextNames(this ComponentData data, string[] contextNames)
		{
			data[COMPONENT_CONTEXTS] = contextNames;
		}

		#endregion

		#region Index

		public const string COMPONENT_GENERATE_INDEX = "Component.Generate.Index";

		public static bool ShouldGenerateIndex(this ComponentData data)
		{
			return (bool)data[COMPONENT_GENERATE_INDEX];
		}

		public static void ShouldGenerateIndex(this ComponentData data, bool generate)
		{
			data[COMPONENT_GENERATE_INDEX] = generate;
		}

		#endregion

		#region Event

		public const string COMPONENT_EVENT = "Component.Event";
		public const string COMPONENT_EVENT_DATA = "Component.Event.Data";

		public static bool IsEvent(this ComponentData data)
		{
			return (bool)data[COMPONENT_EVENT];
		}

		public static void IsEvent(this ComponentData data, bool isEvent)
		{
			data[COMPONENT_EVENT] = isEvent;
		}

		public static EventData[] GetEventData(this ComponentData data)
		{
			return (EventData[])data[COMPONENT_EVENT_DATA];
		}

		public static void SetEventData(this ComponentData data, EventData[] eventData)
		{
			data[COMPONENT_EVENT_DATA] = eventData;
		}

		#endregion

		#region ShouldGenerateObject

		public const string COMPONENT_GENERATE_COMPONENT = "Component.Generate.Object";
		public const string COMPONENT_OBJECT_TYPE = "Component.ObjectTypeName";

		public static bool ShouldGenerateComponent(this ComponentData data)
		{
			return (bool)data[COMPONENT_GENERATE_COMPONENT];
		}

		public static void ShouldGenerateComponent(this ComponentData data, bool generate)
		{
			data[COMPONENT_GENERATE_COMPONENT] = generate;
		}

		public static string GetObjectTypeName(this ComponentData data)
		{
			return (string)data[COMPONENT_OBJECT_TYPE];
		}

		public static void SetObjectTypeName(this ComponentData data, string type)
		{
			data[COMPONENT_OBJECT_TYPE] = type;
		}

		#endregion

		#region Flag

		public const string COMPONENT_FLAG_PREFIX = "Component.FlagPrefix";

		public static string GetFlagPrefix(this ComponentData data)
		{
			return (string)data[COMPONENT_FLAG_PREFIX];
		}

		/// <summary>
		/// Returns true if this component is a flag, otherwise false.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool IsFlag(this ComponentData data)
		{
			return data.GetMemberData().Length == 0;
		}

		public static void SetFlagPrefix(this ComponentData data, string prefix)
		{
			data[COMPONENT_FLAG_PREFIX] = prefix;
		}

		#endregion

		#region Unique

		public const string COMPONENT_IS_UNIQUE = "Component.Unique";

		public static bool IsUnique(this ComponentData data)
		{
			return (bool)data[COMPONENT_IS_UNIQUE];
		}

		public static void IsUnique(this ComponentData data, bool isUnique)
		{
			data[COMPONENT_IS_UNIQUE] = isUnique;
		}

		#endregion

		#region ShouldGenerateMethods

		public const string COMPONENT_GENERATE_METHODS = "Component.Generate.Methods";

		public static bool ShouldGenerateMethods(this ComponentData data)
		{
			return (bool)data[COMPONENT_GENERATE_METHODS];
		}

		public static void ShouldGenerateMethods(this ComponentData data, bool generate)
		{
			data[COMPONENT_GENERATE_METHODS] = generate;
		}

		#endregion
	}
}
