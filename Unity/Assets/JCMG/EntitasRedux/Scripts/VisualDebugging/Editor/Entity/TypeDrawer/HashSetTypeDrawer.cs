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

using System;
using System.Collections;
using System.Collections.Generic;
using JCMG.Genesis.Editor;
using UnityEditor;
using EditorGUILayoutTools = JCMG.EntitasRedux.Editor.EditorGUILayoutTools;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	public class HashSetTypeDrawer : ITypeDrawer
	{
		public bool HandlesType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>);
		}

		public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
		{
			var elementType = memberType.GetGenericArguments()[0];
			var itemsToRemove = new ArrayList();
			var itemsToAdd = new ArrayList();
			var isEmpty = !((IEnumerable)value).GetEnumerator().MoveNext();

			EditorGUILayout.BeginHorizontal();
			{
				if (isEmpty)
				{
					EditorGUILayout.LabelField(memberName, "empty");
				}
				else
				{
					EditorGUILayout.LabelField(memberName);
				}

				if (EditorGUILayoutTools.MiniButton("new " + elementType.ToCompilableString().ShortTypeName()))
				{
					if (EntityDrawer.CreateDefault(elementType, out var defaultValue))
					{
						itemsToAdd.Add(defaultValue);
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			if (!isEmpty)
			{
				EditorGUILayout.Space();
				var indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = indent + 1;
				foreach (var item in (IEnumerable)value)
				{
					EditorGUILayout.BeginHorizontal();
					{
						EntityDrawer.DrawObjectMember(
							elementType,
							string.Empty,
							item,
							target,
							(newComponent, newValue) =>
							{
								itemsToRemove.Add(item);
								itemsToAdd.Add(newValue);
							});

						if (EditorGUILayoutTools.MiniButton("-"))
						{
							itemsToRemove.Add(item);
						}
					}
					EditorGUILayout.EndHorizontal();
				}

				EditorGUI.indentLevel = indent;
			}

			foreach (var item in itemsToRemove)
			{
				memberType.GetMethod("Remove")
					.Invoke(
						value,
						new[]
						{
							item
						});
			}

			foreach (var item in itemsToAdd)
			{
				memberType.GetMethod("Add")
					.Invoke(
						value,
						new[]
						{
							item
						});
			}

			return value;
		}
	}
}
