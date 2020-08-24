using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class MyDoubleComponent : IComponent
	{
		public double myDouble;
	}
}
