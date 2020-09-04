using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace EntitasRedux.Tests
{
	/// <summary>
	/// All of these fields should be deep-copied aside from the dictionary keys.
	/// </summary>
	[MyTest]
	public class DeepCopyComponent : IComponent
	{
		public CloneableObject value;
		public List<CloneableObject> list;
		public Dictionary<CloneableObject, CloneableObject> dict;
	}
}
