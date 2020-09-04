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
using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class EventListenerInterfaceGenerator : AbstractGenerator
	{
		public override string Name => NAME;

		private const string NAME = "Event (Listener Interface)";

		private const string TEMPLATE =
			@"public interface I${EventListener} {
    void On${EventComponentName}${EventType}(${ContextName}Entity entity${methodParameters});
}
";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return data
				.OfType<ComponentData>()
				.Where(d => d.IsEvent())
				.SelectMany(Generate)
				.ToArray();
		}

		private CodeGenFile[] Generate(ComponentData data)
		{
			return data.GetContextNames()
				.SelectMany(contextName => Generate(contextName, data))
				.ToArray();
		}

		private CodeGenFile[] Generate(string contextName, ComponentData data)
		{
			return data.GetEventData()
				.Select(
					eventData =>
					{
						var memberData = data.GetMemberData();
						if (memberData.Length == 0)
						{
							memberData = new[]
							{
								new MemberData(typeof(bool), data.PrefixedComponentName())
							};
						}

						var fileContent = TEMPLATE
							.Replace(
								"${methodParameters}",
								data.GetEventMethodArgs(eventData, ", " + memberData.GetMethodParameters(false)))
							.Replace(data, contextName, eventData);

						return new CodeGenFile(
							"Events" +
							Path.DirectorySeparatorChar +
							"Interfaces" +
							Path.DirectorySeparatorChar +
							"I" +
							data.EventListener(contextName, eventData) +
							".cs",
							fileContent,
							GetType().FullName);
					})
				.ToArray();
		}
	}
}
