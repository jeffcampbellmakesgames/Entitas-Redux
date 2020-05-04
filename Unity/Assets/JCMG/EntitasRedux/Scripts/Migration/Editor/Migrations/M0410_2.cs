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

using System.IO;

namespace JCMG.EntitasRedux.Migration.Editor
{
	internal sealed class M0410_2 : IMigration
	{
		public string Version
		{
			get { return "0.41.0-2"; }
		}

		public string WorkingDirectory
		{
			get { return "where generated files are located"; }
		}

		public string Description
		{
			get { return "Adding temporary Feature class"; }
		}

		public MigrationFile[] Migrate(string path)
		{
			const string featureClass =
				@"namespace Entitas {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

    public class Feature : Entitas.VisualDebugging.Unity.DebugSystems {

        public Feature(string name) : base(name) {
        }

        public Feature() : base(true) {
            var typeName = Entitas.Utils.TypeSerializationExtension.ToCompilableString(GetType());
            var shortType = Entitas.Utils.TypeSerializationExtension.ShortTypeName(typeName);
            initialize(toSpacedCamelCase(shortType));
        }

        static string toSpacedCamelCase(string text) {
            var sb = new System.Text.StringBuilder(text.Length * 2);
            sb.Append(char.ToUpper(text[0]));
            for (int i = 1; i < text.Length; i++) {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ') {
                    sb.Append(' ');
                }
                sb.Append(text[i]);
            }

            return sb.ToString();
        }
    }

#else

    public class Feature : Entitas.Systems {

        public Feature(string name) {
        }

        public Feature() {
        }
    }

#endif

}";

			return new[]
			{
				new MigrationFile(path + Path.DirectorySeparatorChar + "Feature.cs", featureClass)
			};
		}
	}
}
