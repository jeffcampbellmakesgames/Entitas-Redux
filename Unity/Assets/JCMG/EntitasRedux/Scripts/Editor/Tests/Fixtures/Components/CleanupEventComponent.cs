using JCMG.EntitasRedux;

namespace EntitasRedux.Tests
{
	[Game]
	[Event(EventTarget.Self)]
	[Cleanup(CleanupMode.RemoveComponent)]
	[Cleanup(CleanupMode.DestroyEntity)]
	public class CleanupEventComponent : IComponent
	{

	}
}
