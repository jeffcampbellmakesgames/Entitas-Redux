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
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class ContextsGenerator : ICodeGenerator
	{
		private const string TEMPLATE =
@"public partial class Contexts : JCMG.EntitasRedux.IContexts
{

	#if UNITY_EDITOR

	static Contexts()
	{
		UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}

	/// <summary>
	/// Invoked when the Unity Editor has a <see cref=""UnityEditor.PlayModeStateChange""/> change.
	/// </summary>
	private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange playModeStateChange)
	{
		// When entering edit-mode, reset all static state so that it does not interfere with the
		// next play-mode session.
		if (playModeStateChange == UnityEditor.PlayModeStateChange.EnteredEditMode)
		{
			_sharedInstance = null;
		}
	}

	public static Contexts SharedInstance
	{
		get
		{
			if (_sharedInstance == null)
			{
				_sharedInstance = new Contexts();
			}

			return _sharedInstance;
		}
		set	{ _sharedInstance = value; }
	}

	#endif

	static Contexts _sharedInstance;

${contextPropertiesList}

	public JCMG.EntitasRedux.IContext[] AllContexts { get { return new JCMG.EntitasRedux.IContext [] { ${contextList} }; } }

	public Contexts()
{
${contextAssignmentsList}

		var postConstructors = System.Linq.Enumerable.Where(
			GetType().GetMethods(),
			method => System.Attribute.IsDefined(method, typeof(JCMG.EntitasRedux.PostConstructorAttribute))
		);

		foreach (var postConstructor in postConstructors)
		{
			postConstructor.Invoke(this, null);
		}
	}

	public void Reset()
	{
		var contexts = AllContexts;
		for (int i = 0; i < contexts.Length; i++)
		{
			contexts[i].Reset();
		}
	}
}
";

		private const string CONTEXT_PROPERTY_TEMPLATE = @"	public ${ContextType} ${contextName} { get; set; }";
		private const string CONTEXT_LIST_TEMPLATE = @"${contextName}";
		private const string CONTEXT_ASSIGNMENT_TEMPLATE = @"		${contextName} = new ${ContextType}();";

		private string Generate(string[] contextNames)
		{
			var contextPropertiesList = string.Join(
				"\n",
				contextNames
					.Select(contextName => CONTEXT_PROPERTY_TEMPLATE.Replace(contextName))
					.ToArray());

			var contextList = string.Join(
				", ",
				contextNames
					.Select(contextName => CONTEXT_LIST_TEMPLATE.Replace(contextName))
					.ToArray());

			var contextAssignmentsList = string.Join(
				"\n",
				contextNames
					.Select(contextName => CONTEXT_ASSIGNMENT_TEMPLATE.Replace(contextName))
					.ToArray());

			return TEMPLATE
				.Replace("${contextPropertiesList}", contextPropertiesList)
				.Replace("${contextList}", contextList)
				.Replace("${contextAssignmentsList}", contextAssignmentsList);
		}

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Contexts";

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
					Generate(contextNames),
					GetType().FullName)
			};
		}
	}
}
