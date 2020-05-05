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

namespace JCMG.EntitasRedux.Migration.Editor
{
	internal sealed class M0260 : IMigration
	{
		private const string POOL_PATTERN_1 = @"var poolObserver = new Entitas.Unity.VisualDebugging.PoolObserver(";
		private const string POOL_PATTERN_2 = @"UnityEngine.Object.DontDestroyOnLoad(poolObserver.entitiesContainer);";

		private const string COMPONENT_PATTERN = @"throw new SingleEntityException(";

		private const string REPLACEMENT = @"//";

		public string Version
		{
			get { return "0.26.0"; }
		}

		public string WorkingDirectory
		{
			get { return "where generated files are located"; }
		}

		public string Description
		{
			get { return "Deactivates code to prevent compile erros"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			var files = MigrationUtils.GetFiles(path)
				.Where(file => file.fileContent.Contains(POOL_PATTERN_1) || file.fileContent.Contains(COMPONENT_PATTERN))
				.ToArray();

			for (var i = 0; i < files.Length; i++)
			{
				var file = files[i];
				file.fileContent = file.fileContent.Replace(POOL_PATTERN_1, REPLACEMENT + POOL_PATTERN_1);
				file.fileContent = file.fileContent.Replace(POOL_PATTERN_2, REPLACEMENT + POOL_PATTERN_2);
				file.fileContent = file.fileContent.Replace(COMPONENT_PATTERN, REPLACEMENT + COMPONENT_PATTERN);
			}

			return files;
		}
	}
}
