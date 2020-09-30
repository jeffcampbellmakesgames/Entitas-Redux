using System;
using JCMG.EntitasRedux.Editor;
using JCMG.EntitasRedux.VisualDebugging.Editor;
using UnityEditor;

namespace ExampleContent.VisualDebugging.Editor
{
	public class CustomObjectDrawer : ITypeDrawer,
									  IDefaultInstanceCreator
	{
		public object CreateDefault(Type type)
		{
			return new CustomObject("Default");
		}

		public bool HandlesType(Type type)
		{
			return type == typeof(CustomObject);
		}

		public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
		{
			var myObject = (CustomObject)value;
			myObject.name = EditorGUILayout.TextField(memberName, myObject.name);
			return myObject;
		}
	}
}
