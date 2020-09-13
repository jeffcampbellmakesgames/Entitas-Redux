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
using UnityEngine;

namespace JCMG.EntitasRedux.Blueprints
{
	public abstract class BlueprintBase : ScriptableObject
	{
		internal List<IComponent> Components => _components;

		[SerializeReference]
		protected List<IComponent> _components;

		protected virtual void OnEnable()
		{
			if (_components == null)
			{
				_components = new List<IComponent>();
			}
		}

		internal void Validate()
		{
			OnValidate();
		}

		protected virtual void OnValidate()
		{
			// Remove components that have become null due to serialization issues or other reason
			if (_components != null)
			{
				for (var i = _components.Count - 1; i >= 0; i--)
				{
					if (_components[i] == null)
					{
						_components.RemoveAt(i);
					}
				}
			}
		}
	}
}
