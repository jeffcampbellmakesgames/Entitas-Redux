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

using System.Collections.Generic;
using System.Linq;

namespace JCMG.EntitasRedux.Migration.Editor
{
	internal sealed class M0410_3 : IMigration
	{
		private MigrationFile[] UpdateNamespace(MigrationFile[] files, string oldNamespace, string newNamespace)
		{
			var filesToMigrate = files.Where(f => f.fileContent.Contains(oldNamespace)).ToArray();
			foreach (var file in filesToMigrate)
			{
				file.fileContent = file.fileContent.Replace(oldNamespace, newNamespace);
			}

			return filesToMigrate;
		}

		public string Version
		{
			get { return "0.41.0-3"; }
		}

		public string WorkingDirectory
		{
			get { return "where custom TypeDrawers are located"; }
		}

		public string Description
		{
			get { return "Updating namespaces"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			var files = MigrationUtils.GetFiles(path);

			var migratedFiles = new List<MigrationFile>();

			migratedFiles.AddRange(
				UpdateNamespace(files, "Entitas.Unity.VisualDebugging", "Entitas.VisualDebugging.Unity.Editor"));

			return migratedFiles.ToArray();
		}
	}
}
