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

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Extension methods for <see cref="PublicMemberInfo"/>
	/// </summary>
	public static class PublicMemberInfoExtensions
	{
		public static object PublicMemberClone(this object obj)
		{
			var instance = Activator.CreateInstance(obj.GetType());
			obj.CopyPublicMemberValues(instance);
			return instance;
		}

		public static T PublicMemberClone<T>(this object obj)
			where T : new()
		{
			var obj1 = new T();
			obj.CopyPublicMemberValues(obj1);
			return obj1;
		}

		public static void CopyPublicMemberValues(this object source, object target)
		{
			var publicMemberInfos = source.GetType().GetPublicMemberInfos();
			for (var index = 0; index < publicMemberInfos.Count; ++index)
			{
				var publicMemberInfo = publicMemberInfos[index];
				publicMemberInfo.SetValue(target, publicMemberInfo.GetValue(source));
			}
		}
	}
}
