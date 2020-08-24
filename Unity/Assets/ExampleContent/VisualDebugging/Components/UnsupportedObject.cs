using System;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	public class UnsupportedObject
	{
		public string name;

		public UnsupportedObject(string name)
		{
			this.name = name;
		}
	}
}
