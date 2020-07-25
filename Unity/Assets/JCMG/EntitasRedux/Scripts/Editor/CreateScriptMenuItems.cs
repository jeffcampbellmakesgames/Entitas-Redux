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

using UnityEditor;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Menu items for creating C# scripts for EntitasRedux
	/// </summary>
	internal static class CreateScriptMenuItems
	{
		private const string SCRIPT_TEMPLATE_FOLDER_GUID = "5a15a764f3e073b4cbdd6195a11a98ec";

		public const string COMPONENT_FILE_NAME = "Component.txt";
		public const string INITIALIZE_SYSTEM_FILE_NAME = "InitializeSystem.txt";
		public const string FIXED_UPDATE_SYSTEM_FILE_NAME = "FixedUpdateSystem.txt";
		public const string UPDATE_SYSTEM_FILE_NAME = "UpdateSystem.txt";
		public const string LATE_UPDATE_SYSTEM_FILE_NAME = "LateUpdateSystem.txt";
		public const string REACTIVE_SYSTEM_FILE_NAME = "ReactiveSystem.txt";
		public const string CLEANUP_SYSTEM_FILE_NAME = "CleanupSystem.txt";

		private const string MENU_ITEM_PREFIX = "Assets/Create/EntitasRedux/";

		[MenuItem(MENU_ITEM_PREFIX + "Component")]
		private static void CreateEntitasComponent()
		{
			ScriptTools.CreateScriptAsset(SCRIPT_TEMPLATE_FOLDER_GUID, COMPONENT_FILE_NAME);
		}

		[MenuItem(MENU_ITEM_PREFIX + "InitializeSystem")]
		private static void CreateEntitasInitializeSystem()
		{
			ScriptTools.CreateScriptAsset(SCRIPT_TEMPLATE_FOLDER_GUID, INITIALIZE_SYSTEM_FILE_NAME);
		}

		[MenuItem(MENU_ITEM_PREFIX + "FixedUpdateSystem")]
		private static void CreateEntitasFixedUpdateSystem()
		{
			ScriptTools.CreateScriptAsset(SCRIPT_TEMPLATE_FOLDER_GUID, FIXED_UPDATE_SYSTEM_FILE_NAME);
		}

		[MenuItem(MENU_ITEM_PREFIX + "UpdateSystem")]
		private static void CreateEntitasUpdateSystem()
		{
			ScriptTools.CreateScriptAsset(SCRIPT_TEMPLATE_FOLDER_GUID, UPDATE_SYSTEM_FILE_NAME);
		}

		[MenuItem(MENU_ITEM_PREFIX + "LateUpdateSystem")]
		private static void CreateEntitasLateUpdateSystem()
		{
			ScriptTools.CreateScriptAsset(SCRIPT_TEMPLATE_FOLDER_GUID, LATE_UPDATE_SYSTEM_FILE_NAME);
		}

		[MenuItem(MENU_ITEM_PREFIX + "ReactiveSystem")]
		private static void CreateEntitasReactiveSystem()
		{
			ScriptTools.CreateScriptAsset(SCRIPT_TEMPLATE_FOLDER_GUID, REACTIVE_SYSTEM_FILE_NAME);
		}

		[MenuItem(MENU_ITEM_PREFIX + "CleanupSystem")]
		private static void CreateEntitasCleanupSystem()
		{
			ScriptTools.CreateScriptAsset(SCRIPT_TEMPLATE_FOLDER_GUID, CLEANUP_SYSTEM_FILE_NAME);
		}
	}
}
