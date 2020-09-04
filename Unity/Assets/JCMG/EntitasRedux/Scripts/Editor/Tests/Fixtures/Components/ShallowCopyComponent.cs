using System.Collections.Generic;
using JCMG.EntitasRedux;
using UnityEngine;

namespace EntitasRedux.Tests
{
	/// <summary>
	/// All of these fields should be shallow copied as they are either value types , <see cref="UnityEngine.Object"/>
	/// types, or invalid collection types. All of the collections should be distinct, but the contents shallow.
	/// </summary>
	[MyTest]
	public sealed class ShallowCopyComponent : IComponent
	{
		public int intValue;
		public string strValue;
		public Vector2 vector2Value;
		public TestScriptableObject testScriptableObject;
		public Dictionary<int, TestScriptableObject> dictValue;
		public List<TestScriptableObject> listValue;
		public IList<TestScriptableObject> iListValue;
	}
}
