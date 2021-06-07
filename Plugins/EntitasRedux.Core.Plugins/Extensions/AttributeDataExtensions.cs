using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace EntitasRedux.Core.Plugins
{
	/// <summary>
	/// Helper methods for <see cref="AttributeData"/>.
	/// </summary>
	public static class AttributeDataExtensions
	{
		/// <summary>
		/// Returns true if this <see cref="AttributeData"/> represents the type <typeparamref name="T"/>.
		/// </summary>
		public static bool IsAttribute<T>(this AttributeData attributeData)
		{
			//Debug.Assert(typeof(T).IsAssignableFrom(typeof(Attribute)));

			return attributeData.AttributeClass != null &&
			       attributeData.AttributeClass.Name == nameof(T);
		}
	}
}
