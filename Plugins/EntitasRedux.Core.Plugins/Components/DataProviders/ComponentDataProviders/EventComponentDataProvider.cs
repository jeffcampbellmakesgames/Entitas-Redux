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
using Genesis.Plugin;
using JCMG.EntitasRedux;
using Microsoft.CodeAnalysis;

namespace EntitasRedux.Core.Plugins
{
	internal sealed class EventComponentDataProvider : IComponentDataProvider
	{
		public void Provide(NamedTypeSymbolInfo namedTypeSymbolInfo, ComponentData data)
		{
			var attrs = namedTypeSymbolInfo.GetAttributes(nameof(EventAttribute)).ToArray();
			if (attrs.Any())
			{
				data.IsEvent(true);
				var eventData = attrs
					.Select(attr =>
						{
							var args = attr.ConstructorArguments;
							return new EventData(
								(EventTarget) args[0].Value,
								(EventType) args[1].Value,
								(int) args[2].Value);
					})
					.ToArray();

				data.SetEventData(eventData);
			}
			else
			{
				data.IsEvent(false);
			}
		}
	}
}
