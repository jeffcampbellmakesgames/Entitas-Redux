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

using System.Linq;

namespace JCMG.EntitasRedux
{
	public class ContextStillHasRetainedEntitiesException : EntitasReduxException
	{
		public ContextStillHasRetainedEntitiesException(IContext context, IEntity[] entities)
			: base(
				"'" +
				context +
				"' detected retained entities " +
				"although all entities got destroyed!",
				"Did you release all entities? Try calling systems.DeactivateReactiveSystems()" +
				"before calling context.DestroyAllEntities() to avoid memory leaks." +
				"Do not forget to activate them back when needed.\n" +
				string.Join(
					"\n",
					entities
						.Select(
							e =>
							{
								if (e.AERC is SafeAERC safeAerc)
								{
									return e +
										   " - " +
										   string.Join(
											   ", ",
											   safeAerc.Owners
												   .Select(o => o.ToString())
												   .ToArray());
								}

								return e.ToString();
							})
						.ToArray()))
		{
		}
	}
}
