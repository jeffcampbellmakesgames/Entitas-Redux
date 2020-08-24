using System;
using JCMG.EntitasRedux;
using JCMG.EntitasRedux.Editor;
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

			if (!Enum.TryParse<PersonGender>(person.gender, out var personGender))
			{
				personGender = PersonGender.Male;
			}

			personGender = (PersonGender)EditorGUILayout.EnumPopup("Gender", personGender);
			person.gender = personGender.ToString();

			return person;
		}
	}
}
