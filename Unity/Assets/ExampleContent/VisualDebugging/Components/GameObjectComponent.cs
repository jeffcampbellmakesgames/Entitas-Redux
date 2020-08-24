using System;
using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class GameObjectComponent : IComponent
	{
		public GameObject gameObject;
	}
}
