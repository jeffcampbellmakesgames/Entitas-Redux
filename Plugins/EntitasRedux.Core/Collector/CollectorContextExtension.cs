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

namespace JCMG.EntitasRedux
{
	public static class CollectorContextExtension
	{
		/// <summary>
		/// Creates a Collector.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="context"></param>
		/// <param name="matcher"></param>
		/// <returns></returns>
		public static ICollector<TEntity> CreateCollector<TEntity>(
			this IContext<TEntity> context, IMatcher<TEntity> matcher)
			where TEntity : class, IEntity
		{
			return context.CreateCollector(new TriggerOnEvent<TEntity>(matcher, GroupEvent.Added));
		}

		/// <summary>
		/// Creates a Collector.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="context"></param>
		/// <param name="triggers"></param>
		/// <returns></returns>
		public static ICollector<TEntity> CreateCollector<TEntity>(
			this IContext<TEntity> context, params TriggerOnEvent<TEntity>[] triggers)
			where TEntity : class, IEntity
		{
			var groups = new IGroup<TEntity>[triggers.Length];
			var groupEvents = new GroupEvent[triggers.Length];

			for (var i = 0; i < triggers.Length; i++)
			{
				groups[i] = context.GetGroup(triggers[i].matcher);
				groupEvents[i] = triggers[i].groupEvent;
			}

			return new Collector<TEntity>(groups, groupEvents);
		}
	}
}
