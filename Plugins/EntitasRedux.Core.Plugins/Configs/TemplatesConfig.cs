using Genesis.Shared;

namespace EntitasRedux.Core.Plugins
{
	public sealed class TemplatesConfig : AbstractConfigurableConfig
	{
		public string[] Templates
		{
			get => _genesisConfig.GetOrSetValue(TEMPLATES_KEY, DEFAULT_VALUE).ArrayFromCSV();
			set => _genesisConfig.SetValue(TEMPLATES_KEY, value.ToCSV());
		}

		private const string TEMPLATES_KEY = "EntitasRedux.CodeGeneration.Plugins.Templates";
		private const string DEFAULT_VALUE = "Plugins/EntitasRedux/Templates";

		public override void Configure(IGenesisConfig genesisConfig)
		{
			base.Configure(genesisConfig);

			genesisConfig.SetIfNotPresent(TEMPLATES_KEY, DEFAULT_VALUE);
		}
	}
}
