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
using System.Linq;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class EventComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			var attrs = Attribute.GetCustomAttributes(type)
				.OfType<EventAttribute>()
				.ToArray();

			if (attrs.Length > 0)
			{
				data.IsEvent(true);
				var eventData = attrs
					.Select(attr => new EventData(attr.eventTarget, attr.eventType, attr.priority))
					.ToArray();

				data.SetEventData(eventData);
			}
			else
			{
				data.IsEvent(false);
			}
		}
	}

	internal static class EventComponentDataExtension
	{
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
	}
}
