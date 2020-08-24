using System;
using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class Vector4Component : IComponent
	{
		public Vector4 vector4;
	}
}
