using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class MyFloatComponent : IComponent
	{
		public float myFloat;
	}
}
