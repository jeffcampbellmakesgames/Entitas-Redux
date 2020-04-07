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

using System.Linq;
using System.Text.RegularExpressions;

namespace JCMG.EntitasRedux.Migration.Editor
{
	internal sealed class M0320 : IMigration
	{
		public string Version
		{
			get { return "0.32.0"; }
		}

		public string WorkingDirectory
		{
			get { return "project root"; }
		}

		public string Description
		{
			get { return "Updates Entitas.properties to use renamed keys and updates calls to pool.CreateSystem<T>()"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			var properties = MigrationUtils.GetFiles(path, "Entitas.properties");

			for (var i = 0; i < properties.Length; i++)
			{
				var file = properties[i];

				//Entitas.Unity.VisualDebugging.DefaultInstanceCreatorFolderPath = Assets/Editor/DefaultInstanceCreator/
				//Entitas.Unity.VisualDebugging.TypeDrawerFolderPath = Assets/Editor/TypeDrawer/

				file.fileContent = file.fileContent.Replace(
					"Entitas.Unity.CodeGenerator.GeneratedFolderPath",
					"Entitas.CodeGenerator.GeneratedFolderPath");
				file.fileContent = file.fileContent.Replace("Entitas.Unity.CodeGenerator.Pools", "Entitas.CodeGenerator.Pools");
				file.fileContent = file.fileContent.Replace(
					"Entitas.Unity.CodeGenerator.EnabledCodeGenerators",
					"Entitas.CodeGenerator.EnabledCodeGenerators");
			}

			const string pattern = @".CreateSystem<(?<system>\w*)>\(\s*\)";

			var sources = MigrationUtils.GetFiles(path)
				.Where(file => Regex.IsMatch(file.fileContent, pattern))
				.ToArray();

			for (var i = 0; i < sources.Length; i++)
			{
				var file = sources[i];

				file.fileContent = Regex.Replace(
					file.fileContent,
					pattern,
					match => ".CreateSystem(new " + match.Groups["system"].Value + "())");
			}

			return properties.Concat(sources).ToArray();
		}
	}
}
