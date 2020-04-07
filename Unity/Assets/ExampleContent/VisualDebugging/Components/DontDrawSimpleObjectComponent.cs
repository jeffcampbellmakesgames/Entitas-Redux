using JCMG.EntitasRedux;
using JCMG.EntitasRedux.VisualDebugging;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	[DontDrawComponent]
	public class DontDrawSimpleObjectComponent : IComponent
	{
		public SimpleObject simpleObject;
	}
}
