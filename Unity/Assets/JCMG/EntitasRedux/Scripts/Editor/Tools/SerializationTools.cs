using System;
using System.Collections.Generic;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Helper methods for Unity serialization.
	/// </summary>
	public static class SerializationTools
	{
		private static readonly Dictionary<string, string> BUILT_IN_TYPES_TO_STRING = new Dictionary<string, string>
		{
			{ "System.Boolean", "bool" },
			{ "System.Byte", "byte" },
			{ "System.SByte", "sbyte" },
			{ "System.Char", "char" },
			{ "System.Decimal", "decimal" },
			{ "System.Double", "double" },
			{ "System.Single", "float" },
			{ "System.Int32", "int" },
			{ "System.UInt32", "uint" },
			{ "System.Int64", "long" },
			{ "System.UInt64", "ulong" },
			{ "System.Object", "object" },
			{ "System.Int16", "short" },
			{ "System.UInt16", "ushort" },
			{ "System.String", "string" },
			{ "System.Void", "void" }
		};

		private static readonly Dictionary<string, string> BUILT_IN_TYPE_STRINGS = new Dictionary<string, string>
		{
			{ "bool", "System.Boolean" },
			{ "byte", "System.Byte" },
			{ "sbyte", "System.SByte" },
			{ "char", "System.Char" },
			{ "decimal", "System.Decimal" },
			{ "double", "System.Double" },
			{ "float", "System.Single" },
			{ "int", "System.Int32" },
			{ "uint", "System.UInt32" },
			{ "long", "System.Int64" },
			{ "ulong", "System.UInt64" },
			{ "object", "System.Object" },
			{ "short", "System.Int16" },
			{ "ushort", "System.UInt16" },
			{ "string", "System.String" },
			{ "void", "System.Void" }
		};

		public static bool TryGetBuiltInTypeToString(string typeString, out string name)
		{
			return BUILT_IN_TYPES_TO_STRING.TryGetValue(typeString, out name);
		}

		public static bool TryGetBuiltInTypeToString(Type type, out string name)
		{
			return BUILT_IN_TYPES_TO_STRING.TryGetValue(type.FullName, out name);
		}

		public static bool TryGetBuiltInTypeString(string typeString, out string name)
		{
			return BUILT_IN_TYPE_STRINGS.TryGetValue(typeString, out name);
		}
	}
}
