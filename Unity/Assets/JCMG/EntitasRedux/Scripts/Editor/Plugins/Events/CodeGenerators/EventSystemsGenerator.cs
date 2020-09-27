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
using System.IO;
using System.Linq;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Editor.Plugins
{
	internal sealed class EventSystemsGenerator : AbstractGenerator
	{
		private struct DataTuple
		{
			public ComponentData componentData;
			public EventData eventData;
		}

		public override string Name => NAME;

		private const string NAME = "Event (Systems)";

		private const string TEMPLATE =
@"public sealed class ${ContextName}EventSystems : Feature
{
	public ${ContextName}EventSystems(Contexts contexts)
	{
${systemsList}
	}
}
";

		private const string SYSTEM_ADD_TEMPLATE = @"		Add(new ${Event}EventSystem(contexts)); // priority: ${priority}";

		public override CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return Generate(
				data
					.OfType<ComponentData>()
					.Where(d => d.IsEvent())
					.ToArray());
		}

		private CodeGenFile[] Generate(ComponentData[] data)
		{
			var contextNameToComponentData = data
				.Aggregate(
					new Dictionary<string, List<ComponentData>>(),
					(dict, d) =>
					{
						var contextNames = d.GetContextNames();
						foreach (var contextName in contextNames)
						{
							if (!dict.ContainsKey(contextName))
							{
								dict.Add(contextName, new List<ComponentData>());
							}

							dict[contextName].Add(d);
						}

						return dict;
					});

			var contextNameToDataTuple = new Dictionary<string, List<DataTuple>>();
			foreach (var key in contextNameToComponentData.Keys.ToArray())
			{
				var orderedEventData = contextNameToComponentData[key]
					.SelectMany(
						d => d.GetEventData()
							.Select(
								eventData => new DataTuple
								{
									componentData = d, eventData = eventData
								})
							.ToArray())
					.OrderBy(tuple => tuple.eventData.priority)
					.ThenBy(tuple => tuple.componentData.ComponentName())
					.ToList();

				contextNameToDataTuple.Add(key, orderedEventData);
			}

			return Generate(contextNameToDataTuple);
		}

		private CodeGenFile[] Generate(Dictionary<string, List<DataTuple>> contextNameToDataTuple)
		{
			return contextNameToDataTuple
				.Select(kv => GenerateSystem(kv.Key, kv.Value.ToArray()))
				.ToArray();
		}

		private CodeGenFile GenerateSystem(string contextName, DataTuple[] data)
		{
			var fileContent = TEMPLATE
				.Replace("${systemsList}", GenerateSystemList(contextName, data))
				.Replace(contextName);

			return new CodeGenFile(
				"Events" +
				Path.DirectorySeparatorChar +
				contextName +
				"EventSystems.cs",
				fileContent,
				GetType().FullName);
		}

		private string GenerateSystemList(string contextName, DataTuple[] data)
		{
			return string.Join(
				"\n",
				data
					.SelectMany(tuple => GenerateSystemListForData(contextName, tuple))
					.ToArray());
		}

		private string[] GenerateSystemListForData(string contextName, DataTuple data)
		{
			return data.componentData.GetContextNames()
				.Where(ctxName => ctxName == contextName)
				.Select(ctxName => GenerateAddSystem(ctxName, data))
				.ToArray();
		}

		private string GenerateAddSystem(string contextName, DataTuple data)
		{
			return SYSTEM_ADD_TEMPLATE
				.Replace(data.componentData, contextName, data.eventData)
				.Replace("${priority}", data.eventData.priority.ToString())
				.Replace("${Event}", data.componentData.Event(contextName, data.eventData));
		}
	}
}
