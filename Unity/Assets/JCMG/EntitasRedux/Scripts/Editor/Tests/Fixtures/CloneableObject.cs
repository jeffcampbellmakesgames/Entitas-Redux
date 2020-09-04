using System;

namespace EntitasRedux.Tests
{
	public sealed class CloneableObject : ICloneable
	{
		public int value;

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			return new CloneableObject { value = value };
		}
	}
}
