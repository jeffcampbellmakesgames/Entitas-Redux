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
using JCMG.EntitasRedux.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	[CustomEditor(typeof(EntityLink))]
	internal sealed class EntityLinkInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var link = (EntityLink)target;

			if (link.Entity != null)
			{
				if (GUILayout.Button("Unlink"))
				{
					link.Unlink();
				}
			}

			if (link.Entity != null)
			{
				EditorGUILayout.Space();

				EditorGUILayout.LabelField(link.Entity.ToString());

				if (GUILayout.Button("Show entity"))
				{
					Selection.activeGameObject = FindObjectsOfType<EntityBehaviour>()
						.Single(e => e.Entity == link.Entity)
						.gameObject;
				}

				EditorGUILayout.Space();

				EntityDrawer.DrawEntity(link.Entity);
			}
			else
			{
				EditorGUILayout.LabelField("Not linked to an entity");
			}
		}
	}
}
