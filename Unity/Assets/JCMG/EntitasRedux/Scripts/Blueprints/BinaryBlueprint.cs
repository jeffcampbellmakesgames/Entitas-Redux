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
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace JCMG.EntitasRedux.Blueprints
{
	[CreateAssetMenu(menuName = "Entitas/Blueprint", fileName = "New Blueprint.asset")]
	public class BinaryBlueprint : ScriptableObject
	{
		private readonly BinaryFormatter _serializer = new BinaryFormatter();

		public byte[] blueprintData;

		public Blueprint Deserialize()
		{
			Blueprint blueprint;
			if (blueprintData == null || blueprintData.Length == 0)
			{
				blueprint = new Blueprint(string.Empty, "New Blueprint", null);
			}
			else
			{
				using (var stream = new MemoryStream(blueprintData))
				{
					blueprint = (Blueprint)_serializer.Deserialize(stream);
				}
			}

			name = blueprint.name;
			return blueprint;
		}

		public void Serialize(IEntity entity)
		{
			var blueprint = new Blueprint(entity.ContextInfo.name, name, entity);
			Serialize(blueprint);
		}

		public void Serialize(Blueprint blueprint)
		{
			using (var stream = new MemoryStream())
			{
				_serializer.Serialize(stream, blueprint);
				blueprintData = stream.ToArray();
			}
		}
	}
}
