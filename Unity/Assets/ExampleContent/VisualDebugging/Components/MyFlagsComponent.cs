using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class MyFlagsComponent : IComponent
	{
		[Flags]
		public enum MyFlags
		{
			Item1 = 1,
			Item2 = 2,
			Item3 = 4,
			Item4 = 8
		}

		public MyFlags myFlags;
	}
}
