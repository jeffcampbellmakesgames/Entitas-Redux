using System.IO;
using System.Linq;
using Genesis.Plugin;

namespace EntitasRedux.Core.Plugins
{
	internal sealed class ContextAttributeGenerator : ICodeGenerator
	{
		private const string TEMPLATE =
@"public sealed class ${ContextName}Attribute : JCMG.EntitasRedux.ContextAttribute
{
	public ${ContextName}Attribute() : base(""${ContextName}"")	{ }
}
";

		private CodeGenFile Generate(ContextData data)
		{
			var contextName = data.GetContextName();
			return new CodeGenFile(
				contextName +
				Path.DirectorySeparatorChar +
				contextName +
				"Attribute.cs",
				TEMPLATE.Replace(contextName),
				GetType().FullName);
		}

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Context (Attribute)";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ContextData>()
				.Select(Generate)
				.ToArray();
		}
	}
}
