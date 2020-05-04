using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class DictionaryComponent : IComponent
	{
		public Dictionary<string, string> dict;
	}
}
