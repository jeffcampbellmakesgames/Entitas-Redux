using System;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	public class CustomObject : ICloneable
	{
		public string name;

		public CustomObject(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			return new CustomObject(name);
		}
	}
}
