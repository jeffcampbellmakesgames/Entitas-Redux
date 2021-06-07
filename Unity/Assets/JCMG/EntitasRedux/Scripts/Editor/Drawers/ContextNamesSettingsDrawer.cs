/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using JCMG.Genesis.Editor;
using UnityEditor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class ContextNamesSettingsDrawer : AbstractSettingsDrawer
	{
		public override string Title => TITLE;

		public override int Order => 50;

		private readonly ContextSettingsConfig _config;

		private const string TITLE = "EntitasRedux Context Names";
		private const string CONTEXT_NAMES_LABEL = "Context Names";

		public ContextNamesSettingsDrawer()
		{
			_config = new ContextSettingsConfig();
		}

		public override void Initialize(GenesisSettings settings)
		{
			base.Initialize(settings);

			_config.Configure(settings);
		}

		protected override void DrawContentBody(GenesisSettings settings)
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField(CONTEXT_NAMES_LABEL);

				using (var scope = new EditorGUI.ChangeCheckScope())
				{
					var newValue = EditorGUILayout.TextField(_config.RawContextNames);

					if (scope.changed)
					{
						_config.RawContextNames = newValue;

						EditorUtility.SetDirty(settings);
					}
				}
			}
		}
	}
}
