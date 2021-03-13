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

using System.IO;
using System.Linq;
using Genesis.Plugin;
using JCMG.EntitasRedux;

namespace EntitasRedux.Core.Plugins
{
	internal sealed class EventSystemGenerator : AbstractGenerator
	{
		public override string Name => NAME;

		private const string NAME = "Event (System)";

		private const string ANY_TARGET_TEMPLATE =
@"public sealed class ${Event}EventSystem : JCMG.EntitasRedux.ReactiveSystem<${EntityType}>
{
	readonly JCMG.EntitasRedux.IGroup<${EntityType}> _listeners;
	readonly System.Collections.Generic.List<${EntityType}> _entityBuffer;
	readonly System.Collections.Generic.List<I${EventListener}> _listenerBuffer;

	public ${Event}EventSystem(Contexts contexts) : base(contexts.${contextName})
	{
		_listeners = contexts.${contextName}.GetGroup(${MatcherType}.${EventListener});
		_entityBuffer = new System.Collections.Generic.List<${EntityType}>();
		_listenerBuffer = new System.Collections.Generic.List<I${EventListener}>();
	}

	protected override JCMG.EntitasRedux.ICollector<${EntityType}> GetTrigger(JCMG.EntitasRedux.IContext<${EntityType}> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.${GroupEvent}(${MatcherType}.${ComponentName})
		);
	}

	protected override bool Filter(${EntityType} entity)
	{
		return ${filter};
	}

	protected override void Execute(System.Collections.Generic.List<${EntityType}> entities)
	{
		foreach (var e in entities)
		{
			${cachedAccess}
			foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
			{
				_listenerBuffer.Clear();
				_listenerBuffer.AddRange(listenerEntity.${eventListener}.value);
				foreach (var listener in _listenerBuffer)
				{
					listener.On${EventComponentName}${EventType}(e${methodArgs});
				}
			}
		}
	}
}
";

		private const string SELF_TARGET_TEMPLATE =
@"public sealed class ${Event}EventSystem : JCMG.EntitasRedux.ReactiveSystem<${EntityType}>
{
	readonly System.Collections.Generic.List<I${EventListener}> _listenerBuffer;

	public ${Event}EventSystem(Contexts contexts) : base(contexts.${contextName})
	{
		_listenerBuffer = new System.Collections.Generic.List<I${EventListener}>();
	}

	protected override JCMG.EntitasRedux.ICollector<${EntityType}> GetTrigger(JCMG.EntitasRedux.IContext<${EntityType}> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.${GroupEvent}(${MatcherType}.${ComponentName})
		);
	}

	protected override bool Filter(${EntityType} entity)
	{
		return ${filter};
	}

	protected override void Execute(System.Collections.Generic.List<${EntityType}> entities)
	{
		foreach (var e in entities)
		{
			${cachedAccess}
			_listenerBuffer.Clear();
			_listenerBuffer.AddRange(e.${eventListener}.value);
			foreach (var listener in _listenerBuffer)
			{
				listener.On${ComponentName}${EventType}(e${methodArgs});
			}
		}
	}
}
";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ComponentData>()
				.Where(d => d.IsEvent())
				.SelectMany(Generate)
				.ToArray();
		}

		private CodeGenFile[] Generate(ComponentData data)
		{
			return data.GetContextNames()
				.SelectMany(contextName => Generate(contextName, data))
				.ToArray();
		}

		private CodeGenFile[] Generate(string contextName, ComponentData data)
		{
			return data.GetEventData()
				.Select(
					eventData =>
					{
						var methodArgs = data.GetEventMethodArgs(
							eventData,
							", " +
							(data.GetMemberData().Length == 0
								? data.PrefixedComponentName()
								: GetMethodArgs(data.GetMemberData())));

						var cachedAccess = data.GetMemberData().Length == 0
							? string.Empty
							: "var component = e." + data.ComponentNameValidUppercaseFirst() + ";";

						if (eventData.eventType == EventType.Removed)
						{
							methodArgs = string.Empty;
							cachedAccess = string.Empty;
						}

						var template = eventData.eventTarget == EventTarget.Self
							? SELF_TARGET_TEMPLATE
							: ANY_TARGET_TEMPLATE;

						var fileContent = template
							.Replace("${GroupEvent}", eventData.eventType.ToString())
							.Replace("${filter}", GetFilter(data, contextName, eventData))
							.Replace("${cachedAccess}", cachedAccess)
							.Replace("${methodArgs}", methodArgs)
							.Replace(data, contextName, eventData);

						return new CodeGenFile(
							"Events" +
							Path.DirectorySeparatorChar +
							"Systems" +
							Path.DirectorySeparatorChar +
							data.Event(contextName, eventData) +
							"EventSystem.cs",
							fileContent,
							GetType().FullName);
					})
				.ToArray();
		}

		private string GetFilter(ComponentData data, string contextName, EventData eventData)
		{
			var filter = string.Empty;
			if (data.GetMemberData().Length == 0)
			{
				switch (eventData.eventType)
				{
					case EventType.Added:
						filter = "entity." + data.PrefixedComponentName();
						break;
					case EventType.Removed:
						filter = "!entity." + data.PrefixedComponentName();
						break;
				}
			}
			else
			{
				switch (eventData.eventType)
				{
					case EventType.Added:
						filter = "entity.Has" + data.ComponentName();
						break;
					case EventType.Removed:
						filter = "!entity.Has" + data.ComponentName();
						break;
				}
			}

			if (eventData.eventTarget == EventTarget.Self)
			{
				filter += " && entity.Has" + data.EventListener(contextName, eventData);
			}

			return filter;
		}

		private string GetMethodArgs(MemberData[] memberData)
		{
			return string.Join(
				", ",
				memberData
					.Select(info => "component." + info.name)
					.ToArray());
		}
	}
}
