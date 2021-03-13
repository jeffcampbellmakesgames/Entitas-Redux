using System.Collections.Generic;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Represents information about an attribute
	/// </summary>
	public class AttributeInfo
	{
		public readonly object attribute;
		public readonly List<PublicMemberInfo> memberInfos;

		public AttributeInfo(object attribute, List<PublicMemberInfo> memberInfos)
		{
			this.attribute = attribute;
			this.memberInfos = memberInfos;
		}
	}
}
