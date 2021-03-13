using Genesis.Plugin;

namespace EntitasRedux.Core.Plugins
{
	public class ContextData : CodeGeneratorData
	{
		public const string CONTEXT_NAME = "Context.Name";

		public string GetContextName()
		{
			return (string)this[CONTEXT_NAME];
		}

		public void SetContextName(string contextName)
		{
			this[CONTEXT_NAME] = contextName;
		}
	}
}
