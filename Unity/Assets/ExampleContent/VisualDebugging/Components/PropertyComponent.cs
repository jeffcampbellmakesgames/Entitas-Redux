using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class PropertyComponent : IComponent
	{
		public string value { get; set; }
	}
}
