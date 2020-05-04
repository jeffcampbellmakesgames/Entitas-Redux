using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class MyStringComponent : IComponent
	{
		public string myString;

		public override string ToString()
		{
			return "MyString(" + myString + ")";
		}
	}
}
