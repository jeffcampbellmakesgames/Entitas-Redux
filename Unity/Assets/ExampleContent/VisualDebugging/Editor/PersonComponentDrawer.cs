using System;
using JCMG.EntitasRedux;
using JCMG.EntitasRedux.VisualDebugging.Editor;
using UnityEditor;

namespace ExampleContent.VisualDebugging.Editor
{
	public class PersonComponentDrawer : IComponentDrawer
	{
		private enum PersonGender
		{
			Male,
			Female
		}

		public bool HandlesType(Type type)
		{
			return type == typeof(PersonComponent);
		}

		public IComponent DrawComponent(IComponent component)
		{
			var person = (PersonComponent)component;

			person.name = EditorGUILayout.TextField("Name", person.name);

			if (person.gender == null)
			{
				person.gender = PersonGender.Male.ToString();
			}

			var gender = (PersonGender)Enum.Parse(typeof(PersonGender), person.gender);
			gender = (PersonGender)EditorGUILayout.EnumPopup("Gender", gender);
			person.gender = gender.ToString();

			return person;
		}
	}
}
