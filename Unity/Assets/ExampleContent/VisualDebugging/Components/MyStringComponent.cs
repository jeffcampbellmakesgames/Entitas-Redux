using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
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
