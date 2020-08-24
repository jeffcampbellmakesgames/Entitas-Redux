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

namespace JCMG.EntitasRedux.Editor
{
	public class DictionaryTypeDrawer : ITypeDrawer
	{
		private static Dictionary<Type, string> _keySearchTexts = new Dictionary<Type, string>();

		public bool HandlesType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		}

		public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
		{
			var dictionary = (IDictionary)value;
			var keyType = memberType.GetGenericArguments()[0];
			var valueType = memberType.GetGenericArguments()[1];
			var targetType = target.GetType();
			if (!_keySearchTexts.ContainsKey(targetType))
			{
				_keySearchTexts.Add(targetType, string.Empty);
			}

			EditorGUILayout.BeginHorizontal();
			{
				if (dictionary.Count == 0)
				{
					EditorGUILayout.LabelField(memberName, "empty");
					_keySearchTexts[targetType] = string.Empty;
				}
				else
				{
					EditorGUILayout.LabelField(memberName);
				}

				var keyTypeName = keyType.ToCompilableString().ShortTypeName();
				var valueTypeName = valueType.ToCompilableString().ShortTypeName();
				if (EditorGUILayoutTools.MiniButton("new <" + keyTypeName + ", " + valueTypeName + ">"))
				{
					if (EntityDrawer.CreateDefault(keyType, out var defaultKey))
					{
						if (EntityDrawer.CreateDefault(valueType, out var defaultValue))
						{
							dictionary[defaultKey] = defaultValue;
						}
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			if (dictionary.Count > 0)
			{
				var indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = indent + 1;

				if (dictionary.Count > 5)
				{
					EditorGUILayout.Space();
					_keySearchTexts[targetType] = EditorGUILayoutTools.SearchTextField(_keySearchTexts[targetType]);
				}

				EditorGUILayout.Space();

				var keys = new ArrayList(dictionary.Keys);
				for (var i = 0; i < keys.Count; i++)
				{
					var key = keys[i];
					if (EditorGUILayoutTools.MatchesSearchString(key.ToString().ToLower(), _keySearchTexts[targetType].ToLower()))
					{
						EntityDrawer.DrawObjectMember(
							keyType,
							"key",
							key,
							target,
							(newComponent, newValue) =>
							{
								var tmpValue = dictionary[key];
								dictionary.Remove(key);
								if (newValue != null)
								{
									dictionary[newValue] = tmpValue;
								}
							});

						EntityDrawer.DrawObjectMember(
							valueType,
							"value",
							dictionary[key],
							target,
							(newComponent, newValue) => dictionary[key] = newValue);

						EditorGUILayout.Space();
					}
				}

				EditorGUI.indentLevel = indent;
			}

			return dictionary;
		}
	}
}
