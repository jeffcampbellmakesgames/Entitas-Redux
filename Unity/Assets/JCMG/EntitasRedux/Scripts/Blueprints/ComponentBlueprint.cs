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
using System.Collections.Generic;
using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.Blueprints
{
	[Serializable]
	public class ComponentBlueprint
	{
		private Dictionary<string, PublicMemberInfo> _componentMembers;

		private Type _type;
		public string fullTypeName;

		public int index;
		public SerializableMember[] members;

		public ComponentBlueprint()
		{
		}

		public ComponentBlueprint(int index, IComponent component)
		{
			_type = component.GetType();
			_componentMembers = null;

			this.index = index;
			fullTypeName = _type.FullName;

			var memberInfos = _type.GetPublicMemberInfos();
			members = new SerializableMember[memberInfos.Count];
			for (var i = 0; i < memberInfos.Count; i++)
			{
				var info = memberInfos[i];
				members[i] = new SerializableMember(info.name, info.GetValue(component));
			}
		}

		public IComponent CreateComponent(IEntity entity)
		{
			if (_type == null)
			{
				_type = Type.GetType(fullTypeName);

				if (_type == null)
				{
					throw new ComponentBlueprintException(
						"Type '" +
						fullTypeName +
						"' doesn't exist in any assembly!",
						"Please check the full type name.");
				}

				if (!_type.ImplementsInterface<IComponent>())
				{
					throw new ComponentBlueprintException(
						"Type '" +
						fullTypeName +
						"' doesn't implement IComponent!",
						typeof(ComponentBlueprint).Name +
						" only supports IComponent.");
				}
			}

			var component = entity.CreateComponent(index, _type);

			if (_componentMembers == null)
			{
				var memberInfos = _type.GetPublicMemberInfos();
				_componentMembers = new Dictionary<string, PublicMemberInfo>(memberInfos.Count);
				for (var i = 0; i < memberInfos.Count; i++)
				{
					var info = memberInfos[i];
					_componentMembers.Add(info.name, info);
				}
			}

			for (var i = 0; i < members.Length; i++)
			{
				var member = members[i];

				PublicMemberInfo memberInfo;
				if (_componentMembers.TryGetValue(member.name, out memberInfo))
				{
					memberInfo.SetValue(component, member.value);
				}
				else
				{
					Console.WriteLine(
						"Could not find member '" +
						member.name +
						"' in type '" +
						_type.FullName +
						"'!\n" +
						"Only non-static public members are supported.");
				}
			}

			return component;
		}
	}
}
