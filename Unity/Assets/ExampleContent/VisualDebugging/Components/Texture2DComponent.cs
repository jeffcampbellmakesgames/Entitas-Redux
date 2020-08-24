using System;
using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class Texture2DComponent : IComponent
	{
		public Texture2D texture2D;
	}
}
