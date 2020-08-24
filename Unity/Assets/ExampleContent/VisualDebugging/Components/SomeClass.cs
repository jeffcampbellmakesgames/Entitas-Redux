using System;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class SomeClass
	{
		public string name;

		public SomeClass(string name)
		{
			this.name = name;
		}
	}
}
