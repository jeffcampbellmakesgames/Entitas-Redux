using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class MyBoolComponent : IComponent
	{
		public bool myBool;
	}
}
