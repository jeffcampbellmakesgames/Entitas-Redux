using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Example, VisualDebug]
	[Cleanup(CleanupMode.DestroyEntity)]
	public sealed class AnCleanupDestroyEntityComponent : IComponent
	{

	}
}
