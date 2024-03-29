﻿using System;
using System.CodeDom.Compiler;
using System.Linq;
using Genesis.Plugin;
using JCMG.EntitasRedux;

namespace EntitasRedux.Core.Plugins
{
	public static class CodeGeneratorExtensions
	{
		public const string LOOKUP = "ComponentsLookup";

		private const string KEYWORD_PREFIX = "@";

		private static readonly CodeDomProvider PROVIDER = CodeDomProvider.CreateProvider("C#");

		public static string ComponentName(this ComponentData data)
		{
			return data.GetTypeName().ToComponentName();
		}

		public static string[] GetComponentNames(this ComponentData data)
		{
			// Attempt to get the actual Type object for this
			var type = data.GetTypeName().ToType();
			if (type != null)
			{
				var attr = Attribute
					.GetCustomAttributes(type)
					.OfType<ComponentNameAttribute>()
					.SingleOrDefault();

				if (attr != null)
				{
					return attr.componentNames;
				}

				// Otherwise if the attribute is inaccessible return the string-calculated type name.
				return new[]
				{
					type.ToCompilableString().ShortTypeName().AddComponentSuffix()
				};
			}

			// Otherwise just return the type name as originally set in the ComponentData instance.
			return new[]
			{
				data.GetTypeName()
			};
		}

		public static string ComponentNameValidLowercaseFirst(this ComponentData data)
		{
			return ComponentName(data).LowercaseFirst().AddPrefixIfIsKeyword();
		}

		public static string ComponentNameValidUppercaseFirst(this ComponentData data)
		{
			return ComponentName(data).UppercaseFirst().AddPrefixIfIsKeyword();
		}

		public static string ComponentNameWithContext(this ComponentData data, string contextName)
		{
			return contextName + data.ComponentName();
		}

		public static string Replace(this string template, string contextName)
		{
			return template
				.Replace("${ContextName}", contextName)
				.Replace("${contextName}", contextName)
				.Replace("${ContextType}", contextName.AddContextSuffix())
				.Replace("${EntityType}", contextName.AddEntitySuffix())
				.Replace("${MatcherType}", contextName.AddMatcherSuffix())
				.Replace("${Lookup}", contextName + LOOKUP);
		}

		public static string Replace(this string template, ComponentData data, string contextName)
		{
			return template
				.Replace(contextName)
				.Replace("${ComponentType}", data.GetTypeName())
				.Replace("${ComponentName}", data.ComponentName())
				.Replace("${componentName}", data.ComponentName().UppercaseFirst())
				.Replace("${validComponentName}", data.ComponentNameValidUppercaseFirst())
				.Replace("${prefixedComponentName}", data.PrefixedComponentName())
				.Replace("${newMethodParameters}", GetMethodParameters(data.GetMemberData(), true))
				.Replace("${methodParameters}", GetMethodParameters(data.GetMemberData(), false))
				.Replace("${newMethodArgs}", GetMethodArgs(data.GetMemberData(), true))
				.Replace("${methodArgs}", GetMethodArgs(data.GetMemberData(), false))
				.Replace("${Index}", contextName + LOOKUP + "." + data.ComponentName());
		}

		public static string Replace(this string template, ComponentData data, string contextName, EventData eventData)
		{
			var eventListener = data.EventListener(contextName, eventData);
			return template
				.Replace(data, contextName)
				.Replace("${EventComponentName}", data.EventComponentName(eventData))
				.Replace("${EventListenerComponent}", eventListener.AddComponentSuffix())
				.Replace("${Event}", data.Event(contextName, eventData))
				.Replace("${EventListener}", eventListener)
				.Replace("${eventListener}", eventListener.UppercaseFirst())
				.Replace("${EventType}", GetEventTypeSuffix(eventData));
		}

		public static string Replace(this string template, MemberData memberData)
		{
			return template
				.Replace("${MemberName}", memberData.name)
				.Replace("${MemberNameUpper}", memberData.name.UppercaseFirst())
				.Replace("${MemberCompilableString}", memberData.compilableTypeString);
		}

		public static string PrefixedComponentName(this ComponentData data)
		{
			return data.GetFlagPrefix().UppercaseFirst() + data.ComponentName();
		}

		public static string Event(this ComponentData data, string contextName, EventData eventData)
		{
			var optionalContextName = data.GetContextNames().Length > 1 ? contextName : string.Empty;
			return optionalContextName + EventComponentName(data, eventData) + GetEventTypeSuffix(eventData);
		}

		public static string EventListener(this ComponentData data, string contextName, EventData eventData)
		{
			return data.Event(contextName, eventData).AddListenerSuffix();
		}

		public static string EventComponentName(this ComponentData data, EventData eventData)
		{
			var componentName = data.GetTypeName().ToComponentName();
			var shortComponentName = data.GetTypeName().ToComponentName();
			var eventComponentName = componentName.Replace(
				shortComponentName,
				eventData.GetEventPrefix() + shortComponentName);
			return eventComponentName;
		}

		public static string GetEventMethodArgs(this ComponentData data, EventData eventData, string args)
		{
			if (data.GetMemberData().Length == 0)
			{
				return string.Empty;
			}

			return eventData.eventType == EventType.Removed
				? string.Empty
				: args;
		}

		public static string GetEventTypeSuffix(this EventData eventData)
		{
			switch (eventData.eventType)
			{
				case EventType.Added:
					return "Added";
				case EventType.Removed:
					return "Removed";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static string GetEventPrefix(this EventData eventData)
		{
			switch (eventData.eventTarget)
			{
				case EventTarget.Any:
					return "Any";
				case EventTarget.Self:
					return string.Empty;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static string GetMethodParameters(this MemberData[] memberData, bool newPrefix)
		{
			return string.Join(
				", ",
				memberData
					.Select(
						info => info.compilableTypeString + (newPrefix ? " new" + info.name.UppercaseFirst() : " " + info.name.LowercaseFirst()))
					.ToArray());
		}

		public static string GetMethodArgs(MemberData[] memberData, bool newPrefix)
		{
			return string.Join(
				", ",
				memberData
					.Select(info => newPrefix ? "new" + info.name.UppercaseFirst() : info.name)
					.ToArray());
		}

		public static string AddPrefixIfIsKeyword(this string name)
		{
			if (!PROVIDER.IsValidIdentifier(name))
			{
				name = KEYWORD_PREFIX + name;
			}

			return name;
		}
	}
}
