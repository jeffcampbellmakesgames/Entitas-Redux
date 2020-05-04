using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class DictArrayComponent : IComponent
	{
		public Dictionary<int, string[]> dict;
		public Dictionary<int, string[]>[] dictArray;
	}
}
