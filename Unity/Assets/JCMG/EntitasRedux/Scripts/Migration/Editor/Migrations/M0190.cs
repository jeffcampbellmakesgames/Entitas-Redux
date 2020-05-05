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
	internal sealed class M0190 : IMigration
	{
		private const string EXECUTE_PATTERN = @"public\s*void\s*Execute\s*\(\s*Entity\s*\[\s*\]\s*entities\s*\)";
		private const string EXECUTE_REPLACEMENT = "public void Execute(System.Collections.Generic.List<Entity> entities)";

		public string Version
		{
			get { return "0.19.0"; }
		}

		public string WorkingDirectory
		{
			get { return "where all systems are located"; }
		}

		public string Description
		{
			get { return "Migrates IReactiveSystem.Execute to accept List<Entity>"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			var files = MigrationUtils.GetFiles(path)
				.Where(file => Regex.IsMatch(file.fileContent, EXECUTE_PATTERN))
				.ToArray();

			for (var i = 0; i < files.Length; i++)
			{
				files[i].fileContent = Regex.Replace(
					files[i].fileContent,
					EXECUTE_PATTERN,
					EXECUTE_REPLACEMENT,
					RegexOptions.Multiline);
			}

			return files;
		}
	}
}
