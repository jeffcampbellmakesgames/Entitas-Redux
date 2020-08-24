using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	[Event(EventTarget.Any)]
	public class MyEventClass
	{
	}
}
