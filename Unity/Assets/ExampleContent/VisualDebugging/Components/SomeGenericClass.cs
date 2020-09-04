using System;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class SomeGenericClass<T> : ICloneable
	{
		public T value;

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			return new SomeGenericClass<T>
			{
				value = value
			};
		}
	}
}
