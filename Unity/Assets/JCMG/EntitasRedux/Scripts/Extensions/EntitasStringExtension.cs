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

namespace JCMG.EntitasRedux
{
	public static class EntitasStringExtension
	{
		public const string CONTEXT_SUFFIX = "Context";
		public const string ENTITY_SUFFIX = "Entity";
		public const string COMPONENT_SUFFIX = "Component";
		public const string SYSTEM_SUFFIX = "System";
		public const string MATCHER_SUFFIX = "Matcher";
		public const string LISTENER_SUFFIX = "Listener";

		public static string AddContextSuffix(this string str)
		{
			return AddSuffix(str, CONTEXT_SUFFIX);
		}

		public static string RemoveContextSuffix(this string str)
		{
			return RemoveSuffix(str, CONTEXT_SUFFIX);
		}

		public static bool HasContextSuffix(this string str)
		{
			return HasSuffix(str, CONTEXT_SUFFIX);
		}

		public static string AddEntitySuffix(this string str)
		{
			return AddSuffix(str, ENTITY_SUFFIX);
		}

		public static string RemoveEntitySuffix(this string str)
		{
			return RemoveSuffix(str, ENTITY_SUFFIX);
		}

		public static bool HasEntitySuffix(this string str)
		{
			return HasSuffix(str, ENTITY_SUFFIX);
		}

		public static string AddComponentSuffix(this string str)
		{
			return AddSuffix(str, COMPONENT_SUFFIX);
		}

		public static string RemoveComponentSuffix(this string str)
		{
			return RemoveSuffix(str, COMPONENT_SUFFIX);
		}

		public static bool HasComponentSuffix(this string str)
		{
			return HasSuffix(str, COMPONENT_SUFFIX);
		}

		public static string AddSystemSuffix(this string str)
		{
			return AddSuffix(str, SYSTEM_SUFFIX);
		}

		public static string RemoveSystemSuffix(this string str)
		{
			return RemoveSuffix(str, SYSTEM_SUFFIX);
		}

		public static bool HasSystemSuffix(this string str)
		{
			return HasSuffix(str, SYSTEM_SUFFIX);
		}

		public static string AddMatcherSuffix(this string str)
		{
			return AddSuffix(str, MATCHER_SUFFIX);
		}

		public static string RemoveMatcherSuffix(this string str)
		{
			return RemoveSuffix(str, MATCHER_SUFFIX);
		}

		public static bool HasMatcherSuffix(this string str)
		{
			return HasSuffix(str, MATCHER_SUFFIX);
		}

		public static string AddListenerSuffix(this string str)
		{
			return AddSuffix(str, LISTENER_SUFFIX);
		}

		public static string RemoveListenerSuffix(this string str)
		{
			return RemoveSuffix(str, LISTENER_SUFFIX);
		}

		public static bool HasListenerSuffix(this string str)
		{
			return HasSuffix(str, LISTENER_SUFFIX);
		}

		private static string AddSuffix(string str, string suffix)
		{
			return HasSuffix(str, suffix) ? str : str + suffix;
		}

		private static string RemoveSuffix(string str, string suffix)
		{
			return HasSuffix(str, suffix) ? str.Substring(0, str.Length - suffix.Length) : str;
		}

		private static bool HasSuffix(string str, string suffix)
		{
			return str.EndsWith(suffix, StringComparison.Ordinal);
		}
	}
}
