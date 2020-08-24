using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class CustomObjectComponent : IComponent
	{
		public CustomObject customObject;
	}
}
