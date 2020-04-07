using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	[Event(EventTarget.Any)]
	public class MyEventComponent : IComponent
	{
		public string value;
	}
}
