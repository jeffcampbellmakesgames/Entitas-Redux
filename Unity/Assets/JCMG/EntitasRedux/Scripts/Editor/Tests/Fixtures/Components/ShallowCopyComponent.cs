/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

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
