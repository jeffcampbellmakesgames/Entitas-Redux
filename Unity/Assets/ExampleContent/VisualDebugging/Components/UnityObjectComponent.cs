using System;
using JCMG.EntitasRedux;
using Object = UnityEngine.Object;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class UnityObjectComponent : IComponent
	{
		public Object unityObject;
	}
}
