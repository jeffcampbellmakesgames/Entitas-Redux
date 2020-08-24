using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	[DontDrawComponent]
	public class DontDrawSimpleObjectComponent : IComponent
	{
		public SimpleObject simpleObject;
	}
}
