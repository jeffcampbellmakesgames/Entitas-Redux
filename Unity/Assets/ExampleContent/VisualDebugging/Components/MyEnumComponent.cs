using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
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
