using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class ListComponent : IComponent
	{
		public List<string> list;
	}
}
