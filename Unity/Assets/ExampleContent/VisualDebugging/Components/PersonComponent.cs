using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class PersonComponent : IComponent
	{
		public string gender;
		public string name;
	}
}
