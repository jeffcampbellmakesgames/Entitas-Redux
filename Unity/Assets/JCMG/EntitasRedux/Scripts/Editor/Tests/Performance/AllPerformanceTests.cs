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
using System.Diagnostics;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace EntitasRedux.Tests.Performance
{
	[TestFixture]
	internal class AllPerformanceTests
	{
		private static class PerformanceTestRunner
		{
			private static readonly Stopwatch STOPWATCH;

			static PerformanceTestRunner()
			{
				STOPWATCH = new Stopwatch();
			}

			public static long Run(IPerformanceTest test)
			{
				test.Before();
				STOPWATCH.Reset();
				STOPWATCH.Start();
				test.Run();
				STOPWATCH.Stop();
				return STOPWATCH.ElapsedMilliseconds;
			}
		}

		private static readonly StringBuilder SB = new StringBuilder(100);

		[NUnit.Framework.Test]
		[Explicit("This test should only be manually run as it takes a significant amount of time to " +
		                        "complete and is not a simple pass/fail.")]
		public void RunAllTests()
		{
			SB.Clear();

			Run<ContextCreateEntity>();
			Run<ContextDestroyEntity>();
			Run<ContextDestroyAllEntities>();
			Run<ContextGetGroup>();
			Run<ContextGetEntities>();
			Run<ContextHasEntity>();
			Run<ContextOnEntityReplaced>();
			Run<EmptyTest>();

			Run<EntityAddComponent>();
			Run<EntityGetComponent>();
			Run<EntityGetComponents>();
			Run<EntityHasComponent>();
			Run<EntityRemoveAddComponent>();
			Run<EntityReplaceComponent>();
			Run<EmptyTest>();

			Run<MatcherEquals>();
			Run<MatcherGetHashCode>();

			Run<NewInstanceT>();
			Run<NewInstanceActivator>();

			Run<EntityIndexGetEntity>();
			Run<EmptyTest>();

			Run<ObjectGetProperty>();
			Run<EmptyTest>();
			Run<CollectorIterateCollectedEntities>();
			Run<CollectorActivate>();

			Run<Casting>();

			UnityEngine.Debug.Log(SB.ToString());
		}

		//Running performance tests...
		//ContextCreateEntity:                     30 ms
		//ContextDestroyEntity:                    29 ms
		//ContextDestroyAllEntities:               25 ms
		//ContextGetGroup:                          5 ms
		//ContextGetEntities:                       2 ms
		//ContextHasEntity:                        10 ms
		//ContextOnEntityReplaced:                  6 ms

		//EntityAddComponent:                     257 ms
		//EntityGetComponent:                      44 ms
		//EntityGetComponents:                      4 ms
		//EntityHasComponent:                       2 ms
		//EntityRemoveAddComponent:               289 ms
		//EntityReplaceComponent:                  20 ms

		//MatcherEquals:                          171 ms
		//MatcherGetHashCode:                      17 ms

		//ContextCreateBlueprint:                 256 ms

		//NewInstanceT:                           393 ms
		//NewInstanceActivator:                   542 ms
		//EntityIndexGetEntity:                    59 ms

		//IterateHashetToArray:                   456 ms
		//IterateHashSet:                         774 ms

		//ObjectGetProperty:                        6 ms

		//CollectorIterateCollectedEntities:      957 ms
		//CollectorActivate:                        1 ms
		//PropertiesCreate:                       251 ms

		private static void Run<T>()
			where T : class, IPerformanceTest, new()
		{
			Thread.Sleep(100);
			if (typeof(T) == typeof(EmptyTest))
			{
				Console.WriteLine(string.Empty);
				return;
			}

			SB.Append((typeof(T) + ": ").PadRight(40));
			// For more reliable results, run before
			PerformanceTestRunner.Run(new T());
			var ms = PerformanceTestRunner.Run(new T());
			SB.Append(ms.ToString() + " ms");
			SB.AppendLine();
		}
	}
}
