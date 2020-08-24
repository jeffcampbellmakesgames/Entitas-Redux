using System;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class SomeOtherClass
	{
		public string name;

		public SomeOtherClass(string name)
		{
			this.name = name;
		}
	}
}
