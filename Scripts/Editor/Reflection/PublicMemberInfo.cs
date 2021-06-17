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
using System.Reflection;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Represents information about a public member of an object.
	/// </summary>
	public class PublicMemberInfo
	{
		public readonly AttributeInfo[] attributes;
		public readonly string name;
		public readonly Type type;

		private readonly FieldInfo _fieldInfo;
		private readonly PropertyInfo _propertyInfo;

		public PublicMemberInfo(FieldInfo info)
		{
			_fieldInfo = info;
			type = _fieldInfo.FieldType;
			name = _fieldInfo.Name;
			attributes = GetAttributes(_fieldInfo.GetCustomAttributes(false));
		}

		public PublicMemberInfo(PropertyInfo info)
		{
			_propertyInfo = info;
			type = _propertyInfo.PropertyType;
			name = _propertyInfo.Name;
			attributes = GetAttributes(_propertyInfo.GetCustomAttributes(false));
		}

		public PublicMemberInfo(Type type, string name, AttributeInfo[] attributes = null)
		{
			this.type = type;
			this.name = name;
			this.attributes = attributes;
		}

		public object GetValue(object obj)
		{
			return _fieldInfo == null ? _propertyInfo.GetValue(obj, null) : _fieldInfo.GetValue(obj);
		}

		public void SetValue(object obj, object value)
		{
			if (_fieldInfo != null)
			{
				_fieldInfo.SetValue(obj, value);
			}
			else
			{
				_propertyInfo.SetValue(obj, value, null);
			}
		}

		private static AttributeInfo[] GetAttributes(object[] attributes)
		{
			var attributeInfoArray = new AttributeInfo[attributes.Length];
			for (var index = 0; index < attributes.Length; ++index)
			{
				var attribute = attributes[index];
				attributeInfoArray[index] = new AttributeInfo(attribute, attribute.GetType().GetPublicMemberInfos());
			}

			return attributeInfoArray;
		}
	}
}
