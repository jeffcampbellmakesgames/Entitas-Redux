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
	internal sealed class M0180 : IMigration
	{
		private const string METHOD_END_PATTERN = @"(\s|.)*?\}";
		private const string TRIGGER_PATTERN = @"public\s*IMatcher\s*GetTriggeringMatcher\s*\(\s*\)\s*\{\s*";
		private const string TRIGGER_END_PATTERN = TRIGGER_PATTERN + METHOD_END_PATTERN;
		private const string TRIGGER_REPLACEMENT = "public IMatcher trigger { get { ";

		private const string EVENT_TYPE_PATTERN = @"public\s*GroupEventType\s*GetEventType\s*\(\s*\)\s*\{\s*";
		private const string EVENT_TYPE_PATTERN_END = EVENT_TYPE_PATTERN + METHOD_END_PATTERN;
		private const string EVENT_TYPE_REPLACEMENT = "public GroupEventType eventType { get { ";

		public string Version
		{
			get { return "0.18.0"; }
		}

		public string WorkingDirectory
		{
			get { return "where all systems are located"; }
		}

		public string Description
		{
			get { return "Migrates IReactiveSystem GetXyz methods to getters"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			var files = MigrationUtils.GetFiles(path)
				.Where(
					file => Regex.IsMatch(file.fileContent, TRIGGER_PATTERN) ||
					        Regex.IsMatch(file.fileContent, EVENT_TYPE_PATTERN))
				.ToArray();

			for (var i = 0; i < files.Length; i++)
			{
				var file = files[i];
				file.fileContent = Regex.Replace(
					file.fileContent,
					TRIGGER_END_PATTERN,
					match => match.Value + " }",
					RegexOptions.Multiline);
				file.fileContent = Regex.Replace(
					file.fileContent,
					EVENT_TYPE_PATTERN_END,
					match => match.Value + " }",
					RegexOptions.Multiline);
				file.fileContent = Regex.Replace(
					file.fileContent,
					TRIGGER_PATTERN,
					TRIGGER_REPLACEMENT,
					RegexOptions.Multiline);
				file.fileContent = Regex.Replace(
					file.fileContent,
					EVENT_TYPE_PATTERN,
					EVENT_TYPE_REPLACEMENT,
					RegexOptions.Multiline);
			}

			return files;
		}
	}
}
