using JCMG.EntitasRedux;
using JCMG.EntitasRedux.VisualDebugging;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	[DontDrawComponent]
	public class MyHiddenIntComponent : IComponent
	{
		public int myInt;
	}
}
