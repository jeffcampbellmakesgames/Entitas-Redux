using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	[Unique]
	public class UniqueComponent : IComponent
	{
		public string value;
	}
}
