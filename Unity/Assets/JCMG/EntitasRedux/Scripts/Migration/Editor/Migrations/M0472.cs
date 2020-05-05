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

namespace JCMG.EntitasRedux.Migration.Editor
{
	internal sealed class M0472 : IMigration
	{
		public string Version
		{
			get { return "0.47.2"; }
		}

		public string WorkingDirectory
		{
			get { return "project root"; }
		}

		public string Description
		{
			get { return "Updates properties to use renamed keys"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			var properties = MigrationUtils.GetFiles(path, "*.properties");

			for (var i = 0; i < properties.Length; i++)
			{
				var file = properties[i];

				file.fileContent = file.fileContent.Replace("CodeGenerator.SearchPaths", "Jenny.SearchPaths");

				file.fileContent = file.fileContent.Replace("CodeGenerator.Plugins", "Jenny.Plugins");

				file.fileContent = file.fileContent.Replace("CodeGenerator.PreProcessors", "Jenny.PreProcessors");
				file.fileContent = file.fileContent.Replace("CodeGenerator.DataProviders", "Jenny.DataProviders");
				file.fileContent = file.fileContent.Replace("CodeGenerator.CodeGenerators", "Jenny.CodeGenerators");
				file.fileContent = file.fileContent.Replace("CodeGenerator.PostProcessors", "Jenny.PostProcessors");

				file.fileContent = file.fileContent.Replace("CodeGenerator.CLI.Ignore.UnusedKeys", "Jenny.Ignore.Keys");
				file.fileContent = file.fileContent.Replace("Ignore.Keys", "Jenny.Ignore.Keys");

				file.fileContent = file.fileContent.Replace("CodeGenerator.Server.Port", "Jenny.Server.Port");
				file.fileContent = file.fileContent.Replace("CodeGenerator.Client.Host", "Jenny.Client.Host");
			}

			return properties;
		}
	}
}
