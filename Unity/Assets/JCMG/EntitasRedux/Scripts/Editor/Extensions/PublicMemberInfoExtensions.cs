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
