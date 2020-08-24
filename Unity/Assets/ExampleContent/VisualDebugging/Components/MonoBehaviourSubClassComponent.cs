using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class MonoBehaviourSubClassComponent : IComponent
	{
		public MonoBehaviourSubClass monoBehaviour;
	}
}
