using System;
using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class Vector2Component : IComponent
	{
		public Vector2 vector2;
	}
}
