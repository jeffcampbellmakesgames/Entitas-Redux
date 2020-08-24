using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	[FlagPrefix("my")]
	public class CustomFlagComponent : IComponent
	{
	}
}
