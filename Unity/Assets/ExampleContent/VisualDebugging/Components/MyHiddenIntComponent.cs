using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	[DontDrawComponent]
	public class MyHiddenIntComponent : IComponent
	{
		public int myInt;
	}
}
