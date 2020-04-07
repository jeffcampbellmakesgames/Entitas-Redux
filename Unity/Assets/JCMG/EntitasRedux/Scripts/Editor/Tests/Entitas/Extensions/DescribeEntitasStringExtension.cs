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

using JCMG.EntitasRedux;
using NUnit.Framework;

namespace EntitasRedux.Tests.Extensions
{
	[TestFixture]
	internal sealed class DescribeEntitasStringExtension
	{
		const string WORD = "Word";

		[NUnit.Framework.Test]
		public void SuffixIsNotAddedToWordEndingWithSuffix()
		{
			AssertSameWord(WORD + EntitasStringExtension.CONTEXT_SUFFIX, WORD.AddContextSuffix());
			AssertSameWord(WORD + EntitasStringExtension.ENTITY_SUFFIX, WORD.AddEntitySuffix());
			AssertSameWord(WORD + EntitasStringExtension.COMPONENT_SUFFIX, WORD.AddComponentSuffix());
			AssertSameWord(WORD + EntitasStringExtension.SYSTEM_SUFFIX, WORD.AddSystemSuffix());
			AssertSameWord(WORD + EntitasStringExtension.MATCHER_SUFFIX, WORD.AddMatcherSuffix());
			AssertSameWord(WORD + EntitasStringExtension.LISTENER_SUFFIX, WORD.AddListenerSuffix());
		}

		[NUnit.Framework.Test]
		public void SuffixIsAddedToWordEndingWithSuffix()
		{
			AssertSameWord(WORD + EntitasStringExtension.CONTEXT_SUFFIX, WORD.AddContextSuffix());
			AssertSameWord(WORD + EntitasStringExtension.ENTITY_SUFFIX, WORD.AddEntitySuffix());
			AssertSameWord(WORD + EntitasStringExtension.COMPONENT_SUFFIX, WORD.AddComponentSuffix());
			AssertSameWord(WORD + EntitasStringExtension.SYSTEM_SUFFIX, WORD.AddSystemSuffix());
			AssertSameWord(WORD + EntitasStringExtension.MATCHER_SUFFIX, WORD.AddMatcherSuffix());
			AssertSameWord(WORD + EntitasStringExtension.LISTENER_SUFFIX, WORD.AddListenerSuffix());
		}

		[NUnit.Framework.Test]
		public void SuffixIsNotRemovedWhenNotEndingWithSuffix()
		{
			AssertSameWord(WORD, WORD.RemoveContextSuffix());
			AssertSameWord(WORD, WORD.RemoveEntitySuffix());
			AssertSameWord(WORD, WORD.RemoveComponentSuffix());
			AssertSameWord(WORD, WORD.RemoveSystemSuffix());
			AssertSameWord(WORD, WORD.RemoveMatcherSuffix());
			AssertSameWord(WORD, WORD.RemoveListenerSuffix());
		}

		[NUnit.Framework.Test]
		public void SuffixIsRemovedFromWordWhenEndingWithSuffix()
		{
			AssertSameWord(WORD, (WORD + EntitasStringExtension.CONTEXT_SUFFIX).RemoveContextSuffix());
			AssertSameWord(WORD, (WORD + EntitasStringExtension.ENTITY_SUFFIX).RemoveEntitySuffix());
			AssertSameWord(WORD, (WORD + EntitasStringExtension.COMPONENT_SUFFIX).RemoveComponentSuffix());
			AssertSameWord(WORD, (WORD + EntitasStringExtension.SYSTEM_SUFFIX).RemoveSystemSuffix());
			AssertSameWord(WORD, (WORD + EntitasStringExtension.MATCHER_SUFFIX).RemoveMatcherSuffix());
			AssertSameWord(WORD, (WORD + EntitasStringExtension.LISTENER_SUFFIX).RemoveListenerSuffix());
		}

		[NUnit.Framework.Test]
		public void SuffixIsNotFoundOnWordsWithoutSuffix()
		{
			Assert.IsFalse(WORD.HasContextSuffix());
			Assert.IsFalse(WORD.HasEntitySuffix());
			Assert.IsFalse(WORD.HasComponentSuffix());
			Assert.IsFalse(WORD.HasSystemSuffix());
			Assert.IsFalse(WORD.HasMatcherSuffix());
			Assert.IsFalse(WORD.HasListenerSuffix());
		}

		[NUnit.Framework.Test]
		public void SuffixIsFoundOnWordsWithSuffix()
		{
			Assert.IsTrue((WORD + EntitasStringExtension.CONTEXT_SUFFIX).HasContextSuffix());
			Assert.IsTrue((WORD + EntitasStringExtension.ENTITY_SUFFIX).HasEntitySuffix());
			Assert.IsTrue((WORD + EntitasStringExtension.COMPONENT_SUFFIX).HasComponentSuffix());
			Assert.IsTrue((WORD + EntitasStringExtension.SYSTEM_SUFFIX).HasSystemSuffix());
			Assert.IsTrue((WORD + EntitasStringExtension.MATCHER_SUFFIX).HasMatcherSuffix());
			Assert.IsTrue((WORD + EntitasStringExtension.LISTENER_SUFFIX).HasListenerSuffix());
		}

		#region Helpers

		private static void AssertSameWord(string word1, string word2)
		{
			Assert.AreEqual(word2, word1);
		}

		#endregion
	}
}
