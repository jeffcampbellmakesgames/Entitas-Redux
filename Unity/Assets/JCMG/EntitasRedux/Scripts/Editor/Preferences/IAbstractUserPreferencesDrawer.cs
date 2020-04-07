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

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Represents methods for an object that can render user preferences in the editor.
	/// </summary>
	public interface IAbstractUserPreferencesDrawer
	{
		/// <summary>
		/// The ascending order in which this drawer should be shown with regards to other <see cref="IAbstractUserPreferencesDrawer"/>
		/// instances.
		/// </summary>
		int Order { get; }

		/// <summary>
		/// Performs any needed setup or initialization prior to drawing user preferences
		/// </summary>
		void Initialize();

		/// <summary>
		/// Draws user preferences
		/// </summary>
		void DrawContent();
	}
}
