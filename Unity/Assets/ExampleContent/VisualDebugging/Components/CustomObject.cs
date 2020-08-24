using System;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	public class CustomObject
	{
		public string name;

		public CustomObject(string name)
		{
			this.name = name;
		}
	}
}
