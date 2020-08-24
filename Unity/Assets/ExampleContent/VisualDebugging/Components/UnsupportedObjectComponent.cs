using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class UnsupportedObjectComponent : IComponent
	{
		public UnsupportedObject unsupportedObject;
	}
}
