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

using System.CodeDom.Compiler;
using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
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
			return eventData.eventType == EventType.Removed ? "Removed" : string.Empty;
		}

		public static string GetEventPrefix(this EventData eventData)
		{
			return eventData.eventTarget == EventTarget.Any ? "Any" : string.Empty;
		}

		public static string GetMethodParameters(this MemberData[] memberData, bool newPrefix)
		{
			return string.Join(
				", ",
				memberData
					.Select(
						info => info.type + (newPrefix ? " new" + info.name.UppercaseFirst() : " " + info.name.LowercaseFirst()))
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
