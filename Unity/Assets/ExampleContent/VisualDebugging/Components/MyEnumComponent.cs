using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class MyEnumComponent : IComponent
	{
		public enum MyEnum
		{
			Item1,
			Item2,
			Item3
		}

		public MyEnum myEnum;
	}
}
