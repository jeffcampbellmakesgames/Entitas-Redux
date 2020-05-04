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
	internal sealed class M0220 : IMigration
	{
		private const string TRIGGER_PATTERN =
			@"public\s*IMatcher\s*trigger\s*\{\s*get\s*\{\s*return\s*(?<matcher>.*?)\s*;\s*\}\s*\}";

		private const string EVENT_TYPE_PATTERN =
			@"^\s*public\s*GroupEventType\s*eventType\s*\{\s*get\s*\{\s*return\s*GroupEventType\.(?<eventType>\w*)\s*;\s*\}\s*\}";

		private const string TRIGGER_REPLACEMENT_FORMAT = @"public TriggerOnEvent trigger {{ get {{ return {0}.{1}(); }} }}";

		public string Version
		{
			get { return "0.22.0"; }
		}

		public string WorkingDirectory
		{
			get { return "where all systems are located"; }
		}

		public string Description
		{
			get { return "Migrates IReactiveSystem to combine trigger and eventTypes to TriggerOnEvent"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			var files = MigrationUtils.GetFiles(path)
				.Where(file => Regex.IsMatch(file.fileContent, TRIGGER_PATTERN))
				.ToArray();

			for (var i = 0; i < files.Length; i++)
			{
				var file = files[i];

				var eventTypeMatch = Regex.Match(file.fileContent, EVENT_TYPE_PATTERN, RegexOptions.Multiline);
				var eventType = eventTypeMatch.Groups["eventType"].Value;
				file.fileContent = Regex.Replace(
					file.fileContent,
					EVENT_TYPE_PATTERN,
					string.Empty,
					RegexOptions.Multiline);

				file.fileContent = Regex.Replace(
					file.fileContent,
					TRIGGER_PATTERN,
					match => string.Format(TRIGGER_REPLACEMENT_FORMAT, match.Groups["matcher"].Value, eventType),
					RegexOptions.Multiline);
			}

			return files;
		}
	}
}
