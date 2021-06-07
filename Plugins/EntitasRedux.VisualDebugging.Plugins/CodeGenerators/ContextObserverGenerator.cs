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
using EntitasRedux.Core.Plugins;
using Genesis.Plugin;

namespace EntitasRedux.Unity.VisualDebuggingPlugins
{
	internal sealed class ContextObserverGenerator : ICodeGenerator
	{
		private const string CONTEXTS_TEMPLATE =
			@"public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeContextObservers() {
		try {
${contextObservers}
		} catch(System.Exception) {
		}
	}

	public void CreateContextObserver(JCMG.EntitasRedux.IContext context) {
		if (UnityEngine.Application.isPlaying) {
			var observer = new JCMG.EntitasRedux.VisualDebugging.ContextObserver(context);
			UnityEngine.Object.DontDestroyOnLoad(observer.GameObject);
		}
	}

#endif
}
";

		private const string CONTEXT_OBSERVER_TEMPLATE = @"			CreateContextObserver(${contextName});";

		private string GenerateContextsClass(string[] contextNames)
		{
			var contextObservers = string.Join(
				"\n",
				contextNames
					.Select(
						contextName => CONTEXT_OBSERVER_TEMPLATE
							.Replace("${contextName}", contextName))
					.ToArray());

			return CONTEXTS_TEMPLATE
				.Replace("${contextObservers}", contextObservers);
		}

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Context Observer";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var contextNames = data
				.OfType<ContextData>()
				.Select(d => d.GetContextName())
				.OrderBy(contextName => contextName)
				.ToArray();

			return new[]
			{
				new CodeGenFile(
					"Contexts.cs",
					GenerateContextsClass(contextNames),
					GetType().FullName)
			};
		}
	}
}
