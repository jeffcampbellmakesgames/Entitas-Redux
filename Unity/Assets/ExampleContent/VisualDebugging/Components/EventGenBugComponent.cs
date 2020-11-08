using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Example]
	[Event(EventTarget.Self)]
	[Cleanup(CleanupMode.DestroyEntity)]
	public sealed class EventGenBugComponent : IComponent
	{

	}
}
