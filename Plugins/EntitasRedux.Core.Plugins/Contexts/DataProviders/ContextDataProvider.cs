using System.Linq;
using Genesis.Plugin;
using Genesis.Shared;

namespace EntitasRedux.Core.Plugins
{
	/// <summary>
	/// A <see cref="IDataProvider"/> that creates a collection of <see cref="ContextData"/> per user-defined context.
	/// </summary>
	internal sealed class ContextDataProvider : IDataProvider,
	                                            IConfigurable
	{
		private readonly ContextSettingsConfig _contextSettingsConfig;

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Context";

		public ContextDataProvider()
		{
			_contextSettingsConfig = new ContextSettingsConfig();
		}

		public CodeGeneratorData[] GetData()
		{
			return _contextSettingsConfig.ContextNames
				.Select(CreateContextData)
				.Cast<CodeGeneratorData>()
				.ToArray();
		}

		/// <summary>
		/// Creates an instance of <see cref="ContextData"/> for <paramref name="contextName"/>.
		/// </summary>
		private ContextData CreateContextData(string contextName)
		{
			var data = new ContextData();
			data.SetContextName(contextName);
			return data;
		}

		/// <inheritdoc />
		public void Configure(IGenesisConfig genesisConfig)
		{
			_contextSettingsConfig.Configure(genesisConfig);
		}
	}
}
