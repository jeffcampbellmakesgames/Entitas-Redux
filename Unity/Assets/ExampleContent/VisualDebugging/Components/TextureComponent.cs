using System;
using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class TextureComponent : IComponent
	{
		public Texture texture;
	}
}
