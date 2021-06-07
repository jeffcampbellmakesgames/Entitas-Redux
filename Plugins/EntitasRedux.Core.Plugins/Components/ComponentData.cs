using System;
using System.Collections.Generic;
using Genesis.Plugin;

namespace EntitasRedux.Core.Plugins
{
	public class ComponentData : CodeGeneratorData
	{
		public ComponentData()
		{
		}

		public ComponentData(CodeGeneratorData data) : base(data)
		{
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.GetTypeName();
		}
	}

	public class ComponentDataEqualityComparer : IEqualityComparer<ComponentData>
	{
		public bool Equals(ComponentData x, ComponentData y)
		{
			return x.GetTypeName().Equals(y.GetTypeName());
		}

		public int GetHashCode(ComponentData obj)
		{
			return obj.GetTypeName().GetHashCode();
		}
	}
}
