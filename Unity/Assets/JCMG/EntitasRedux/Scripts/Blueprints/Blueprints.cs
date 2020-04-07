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
	[CreateAssetMenu(menuName = "Entitas/Blueprints", fileName = "Blueprints.asset")]
	public class Blueprints : ScriptableObject
	{
		public BinaryBlueprint[] blueprints;

		private Dictionary<string, BinaryBlueprint> _binaryBlueprintsMap;

		#if (!UNITY_EDITOR)
        Dictionary<string, Blueprint> _blueprintsMap;
		#endif

		private void OnEnable()
		{
			if (blueprints == null)
			{
				blueprints = new BinaryBlueprint[0];
			}

			_binaryBlueprintsMap = new Dictionary<string, BinaryBlueprint>(blueprints.Length);
			#if (!UNITY_EDITOR)
            _blueprintsMap = new Dictionary<string, Blueprint>(blueprints.Length);
			#endif

			for (var i = 0; i < blueprints.Length; i++)
			{
				var blueprint = blueprints[i];
				if (blueprint != null)
				{
					_binaryBlueprintsMap.Add(blueprint.name, blueprint);
				}
			}
		}

		#if (UNITY_EDITOR)
		public Blueprint GetBlueprint(string name)
		{
			BinaryBlueprint binaryBlueprint;
			if (!_binaryBlueprintsMap.TryGetValue(name, out binaryBlueprint))
			{
				throw new BlueprintsNotFoundException(name);
			}

			return binaryBlueprint.Deserialize();
		}

		#else
        public Blueprint GetBlueprint(string name) {
            Blueprint blueprint;
            if (!_blueprintsMap.TryGetValue(name, out blueprint)) {
                BinaryBlueprint binaryBlueprint;
                if (_binaryBlueprintsMap.TryGetValue(name, out binaryBlueprint)) {
                    blueprint = binaryBlueprint.Deserialize();
                    _blueprintsMap.Add(name, blueprint);
                } else {
                    throw new BlueprintsNotFoundException(name);
                }
            }

            return blueprint;
        }

		#endif
	}
}
