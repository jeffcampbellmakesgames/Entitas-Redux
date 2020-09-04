using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class CustomObjectDictionaryComponent : IComponent
	{
		public Dictionary<string, CustomObject> value;
	}
}
