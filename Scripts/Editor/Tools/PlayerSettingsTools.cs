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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Helper methods for <see cref="PlayerSettings"/>.
	/// </summary>
	public static class PlayerSettingsTools
	{
		private static readonly StringBuilder SB;

		private const char SCRIPTING_SYMBOL_DELIMITER = ';';

		static PlayerSettingsTools()
		{
			const int STARTING_SIZE = 250;
			SB = new StringBuilder(STARTING_SIZE);
		}

		/// <summary>
		/// Aggregates all <paramref name="scriptingSymbols"/> into a single string.
		/// </summary>
		/// <param name="scriptingSymbols"></param>
		/// <returns></returns>
		public static string AggregateScriptingSymbols(ICollection<string> scriptingSymbols)
		{
			SB.Clear();
			foreach (var scriptingSymbol in scriptingSymbols)
			{
				SB.Append(scriptingSymbol);
				SB.Append(SCRIPTING_SYMBOL_DELIMITER);
			}

			return SB.ToString();
		}

		/// <summary>
		/// Splits all scripting symbols in <paramref name="rawScriptingSymbols"/> into a <see cref="ICollection{T}"/>
		/// of string symbols.
		/// </summary>
		/// <param name="rawScriptingSymbols"></param>
		/// <returns></returns>
		public static ICollection<string> SplitScriptingSymbols(string rawScriptingSymbols)
		{
			return rawScriptingSymbols.Split(SCRIPTING_SYMBOL_DELIMITER).ToList();
		}

		/// <summary>
		/// Returns true if <paramref name="symbol"/> is defined, otherwise false.
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns></returns>
		public static bool IsScriptingSymbolDefined(string symbol)
		{
			var scriptingSymbols = GetCurrentScriptingSymbols();
			return scriptingSymbols.Contains(symbol);
		}

		/// <summary>
		/// Adds the passed string as a ScriptingSymbol if it is not present already.
		/// </summary>
		/// <param name="symbol"></param>
		public static void AddScriptingSymbol(string symbol)
		{
			if (IsScriptingSymbolDefined(symbol))
			{
				return;
			}

			var currentBuildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			var scriptingSymbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentBuildGroup);

			// Add the new symbol to the list of symbols, always ensuring there is a semi-colon separating all symbols
			if (string.IsNullOrEmpty(scriptingSymbolStr))
			{
				scriptingSymbolStr = string.Format("{0}", symbol);
			}
			else if (scriptingSymbolStr[scriptingSymbolStr.Length - 1] == SCRIPTING_SYMBOL_DELIMITER)
			{
				scriptingSymbolStr += string.Format("{0}", symbol);
			}
			else
			{
				scriptingSymbolStr += string.Format(";{0}", symbol);
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(currentBuildGroup, scriptingSymbolStr);
		}

		/// <summary>
		/// Removes the passed string as a ScriptingSymbol if it is present.
		/// </summary>
		/// <param name="symbol"></param>
		public static void RemoveScriptingSymbol(string symbol)
		{
			if (!IsScriptingSymbolDefined(symbol))
			{
				return;
			}

			var scriptingSymbols = GetCurrentScriptingSymbols();
			scriptingSymbols.Remove(symbol);

			var newScriptingSymbols = AggregateScriptingSymbols(scriptingSymbols);
			var currentBuildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			PlayerSettings.SetScriptingDefineSymbolsForGroup(currentBuildGroup, newScriptingSymbols);
		}

		public static ICollection<string> GetCurrentScriptingSymbols()
		{
			var currentBuildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			var scriptingSymbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentBuildGroup);
			return SplitScriptingSymbols(scriptingSymbolStr);
		}
	}
}
