using Genesis.Shared;

namespace EntitasRedux.Core.Plugins
{
	internal sealed class ContextSettingsConfig : AbstractConfigurableConfig
	{
		public string[] ContextNames
		{
			get => _genesisConfig.GetOrSetValue(CONTEXTS_KEY, DEFAULT_CONTEXTS).ArrayFromCSV();
			set => _genesisConfig.SetValue(CONTEXTS_KEY, value.ToCSV());
		}

		internal string RawContextNames
		{
			get => _genesisConfig.GetOrSetValue(CONTEXTS_KEY, DEFAULT_CONTEXTS);
			set => _genesisConfig.SetValue(CONTEXTS_KEY, value);
		}

		private const string DEFAULT_CONTEXTS = "Game, Input";
		private const string CONTEXTS_KEY = "EntitasRedux.CodeGeneration.Plugins.Contexts";

		/// <inheritdoc />
		public override void Configure(IGenesisConfig genesisConfig)
		{
			base.Configure(genesisConfig);

			_genesisConfig.SetIfNotPresent(CONTEXTS_KEY, DEFAULT_CONTEXTS);
		}
	}
}
