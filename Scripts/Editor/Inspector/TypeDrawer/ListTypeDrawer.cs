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
using System.Linq;
using JCMG.Genesis.Editor;
using UnityEditor;

namespace JCMG.EntitasRedux.Editor
{
	public class ListTypeDrawer : ITypeDrawer
	{
		private static Func<IList> DrawEditActions(IList list, Type elementType, int index)
		{
			if (EditorGUILayoutTools.MiniButtonLeft("↑"))
			{
				if (index > 0)
				{
					return () =>
					{
						var otherIndex = index - 1;
						var other = list[otherIndex];
						list[otherIndex] = list[index];
						list[index] = other;
						return list;
					};
				}
			}

			if (EditorGUILayoutTools.MiniButtonMid("↓"))
			{
				if (index < list.Count - 1)
				{
					return () =>
					{
						var otherIndex = index + 1;
						var other = list[otherIndex];
						list[otherIndex] = list[index];
						list[index] = other;
						return list;
					};
				}
			}

			if (EditorGUILayoutTools.MiniButtonMid("+"))
			{
				if (EntityDrawer.CreateDefault(elementType, out var defaultValue))
				{
					var insertAt = index + 1;
					return () =>
					{
						list.Insert(insertAt, defaultValue);
						return list;
					};
				}
			}

			if (EditorGUILayoutTools.MiniButtonRight("-"))
			{
				var removeAt = index;
				return () =>
				{
					list.RemoveAt(removeAt);
					return list;
				};
			}

			return null;
		}

		private IList DrawAddElement(IList list, string memberName, Type elementType)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(memberName, "empty");
				if (EditorGUILayoutTools.MiniButton("add " + elementType.ToCompilableString().ShortTypeName()))
				{
					if (EntityDrawer.CreateDefault(elementType, out var defaultValue))
					{
						list.Add(defaultValue);
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			return list;
		}

		public bool HandlesType(Type type)
		{
			return type.GetInterfaces().Contains(typeof(IList));
		}

		public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
		{
			var list = (IList)value;
			var elementType = memberType.GetGenericArguments()[0];
			if (list.Count == 0)
			{
				list = DrawAddElement(list, memberName, elementType);
			}
			else
			{
				EditorGUILayout.LabelField(memberName);
			}

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = indent + 1;
			Func<IList> editAction = null;
			for (var i = 0; i < list.Count; i++)
			{
				var localIndex = i;
				EditorGUILayout.BeginHorizontal();
				{
					EntityDrawer.DrawObjectMember(
						elementType,
						memberName + "[" + localIndex + "]",
						list[localIndex],
						target,
						(newComponent, newValue) => list[localIndex] = newValue);

					var action = DrawEditActions(list, elementType, localIndex);
					if (action != null)
					{
						editAction = action;
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			if (editAction != null)
			{
				list = editAction();
			}

			EditorGUI.indentLevel = indent;

			return list;
		}
	}
}
