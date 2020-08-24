using System;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public struct SomeStruct
	{
		public string name;

		public SomeStruct(string name)
		{
			this.name = name;
		}
	}
}
