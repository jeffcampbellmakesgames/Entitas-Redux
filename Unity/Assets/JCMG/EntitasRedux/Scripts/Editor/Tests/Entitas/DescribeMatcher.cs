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

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeMatcher
	{
		private MyTestEntity _eA;
		private MyTestEntity _eB;
		private MyTestEntity _eC;
		private MyTestEntity _eAb;
		private MyTestEntity _eAbc;

		private IAllOfMatcher<MyTestEntity> _allOfMatcher;
		private IAnyOfMatcher<MyTestEntity> _anyOfMatcher;
		private ICompoundMatcher<MyTestEntity> _compoundMatcher;

		[SetUp]
		public void Setup()
		{
			_eA = TestTools.CreateEntity();
			_eA.AddComponentA();

			_eB = TestTools.CreateEntity();
			_eB.AddComponentB();

			_eC = TestTools.CreateEntity();
			_eC.AddComponentC();

			_eAb = TestTools.CreateEntity();
			_eAb.AddComponentA();
			_eAb.AddComponentB();

			_eAbc = TestTools.CreateEntity();
			_eAbc.AddComponentA();
			_eAbc.AddComponentB();
			_eAbc.AddComponentC();
		}

		#region AllOf

		[NUnit.Framework.Test]
		public void HasCorrectIndicesForAllOf()
		{
			SetupAllOfMatcher();

			AssertIndicesContain(_allOfMatcher.Indices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(_allOfMatcher.AllOfIndices, CID.ComponentA, CID.ComponentB);
		}

		[NUnit.Framework.Test]
		public void HasCorrectIndicesForAllOfWithoutDuplicates()
		{
			_allOfMatcher = Matcher<MyTestEntity>.AllOf(
				CID.ComponentA,
				CID.ComponentA,
				CID.ComponentB,
				CID.ComponentB);
			AssertIndicesContain(_allOfMatcher.Indices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(_allOfMatcher.AllOfIndices, CID.ComponentA, CID.ComponentB);
		}

		[NUnit.Framework.Test]
		public void AllOfIndicesAreCached()
		{
			SetupAllOfMatcher();

			Assert.AreEqual(_allOfMatcher.Indices, _allOfMatcher.Indices);
		}

		[NUnit.Framework.Test]
		public void AllOfMatches()
		{
			SetupAllOfMatcher();

			Assert.IsTrue(_allOfMatcher.Matches(_eAb));
			Assert.IsTrue(_allOfMatcher.Matches(_eAbc));
		}

		[NUnit.Framework.Test]
		public void AllOfMergedMatchersHaveCorrectIndices()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AllOf(CID.ComponentB);
			var m3 = Matcher<TestEntity>.AllOf(CID.ComponentC);
			var mergedMatcher = Matcher<TestEntity>.AllOf(m1, m2, m3);

			AssertIndicesContain(mergedMatcher.Indices, CID.ComponentA, CID.ComponentB, CID.ComponentC);
			AssertIndicesContain(mergedMatcher.AllOfIndices, CID.ComponentA, CID.ComponentB, CID.ComponentC);
		}

		[NUnit.Framework.Test]
		public void AllOfMergedMatchersHaveZeroDuplicateIndices()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
            var m2 = Matcher<TestEntity>.AllOf(CID.ComponentA);
            var m3 = Matcher<TestEntity>.AllOf(CID.ComponentB);
            var mergedMatcher = Matcher<TestEntity>.AllOf(m1, m2, m3);

			AssertIndicesContain(mergedMatcher.Indices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(mergedMatcher.AllOfIndices, CID.ComponentA, CID.ComponentB);
		}

		[NUnit.Framework.Test]
		public void ThrowsWhenMergingMatcherWithMoreThanOneIndex()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA, CID.ComponentB);

			Assert.Throws<MatcherException>(() => Matcher<TestEntity>.AllOf(m1));
		}

		[NUnit.Framework.Test]
		public void ValidateAllOfToString()
		{
			SetupAllOfMatcher();

			Assert.AreEqual("AllOf(1, 2)", _allOfMatcher.ToString());
		}

		[NUnit.Framework.Test]
		public void ValidateAllOfToStringWhenComponentNamesAreSet()
		{
			SetupAllOfMatcher();

			var matcher = (Matcher<MyTestEntity>)_allOfMatcher;
			matcher.ComponentNames = new[] { "one", "two", "three" };

			Assert.AreEqual("AllOf(two, three)", matcher.ToString());
		}

		[NUnit.Framework.Test]
		public void ValidateMergedAllOfMatcherToStringUsesComponentNames()
		{
			var m1 = (Matcher<MyTestEntity>)Matcher<MyTestEntity>.AllOf(CID.ComponentA);
			var m2 = (Matcher<MyTestEntity>)Matcher<MyTestEntity>.AllOf(CID.ComponentB);
			var m3 = (Matcher<MyTestEntity>)Matcher<MyTestEntity>.AllOf(CID.ComponentC);

			m2.ComponentNames = new[] { "m_0", "m_1", "m_2", "m_3" };

			var mergedMatcher = Matcher<MyTestEntity>.AllOf(m1, m2, m3);

			Assert.AreEqual("AllOf(m_1, m_2, m_3)", mergedMatcher.ToString());
		}

		#endregion

		#region AnyOf

		[NUnit.Framework.Test]
		public void AnyOfMatcherHasCorrectIndices()
		{
			SetupAnyOfMatcher();

			AssertIndicesContain(_anyOfMatcher.Indices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(_anyOfMatcher.AnyOfIndices, CID.ComponentA, CID.ComponentB);
		}

		[NUnit.Framework.Test]
		public void AnyOfMatcherHasAllIndicesWithoutDuplicates()
		{
			_anyOfMatcher = Matcher<MyTestEntity>.AnyOf(
				CID.ComponentA,
				CID.ComponentA,
				CID.ComponentB,
				CID.ComponentB);

			AssertIndicesContain(_anyOfMatcher.Indices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(_anyOfMatcher.AnyOfIndices, CID.ComponentA, CID.ComponentB);
		}

		[NUnit.Framework.Test]
		public void AnyOfMatcherCachesIndices()
		{
			SetupAnyOfMatcher();

			Assert.AreEqual(_anyOfMatcher.Indices, _anyOfMatcher.Indices);
		}

		[NUnit.Framework.Test]
		public void AnyOfMatcherDoesNotMatch()
		{
			SetupAnyOfMatcher();

			Assert.IsFalse(_anyOfMatcher.Matches(_eC));
		}

		[NUnit.Framework.Test]
		public void AnyOfMatcherMatches()
		{
			SetupAnyOfMatcher();

			Assert.IsTrue(_anyOfMatcher.Matches(_eA));
			Assert.IsTrue(_anyOfMatcher.Matches(_eB));
			Assert.IsTrue(_anyOfMatcher.Matches(_eAbc));
		}

		[NUnit.Framework.Test]
		public void AnyOfMergesMatchersToNewMatcher()
		{
			var m1 = Matcher<TestEntity>.AnyOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AnyOf(CID.ComponentB);
			var m3 = Matcher<TestEntity>.AnyOf(CID.ComponentC);
			var mergedMatcher = Matcher<TestEntity>.AnyOf(m1, m2, m3);

			AssertIndicesContain(mergedMatcher.Indices, CID.ComponentA, CID.ComponentB, CID.ComponentC);
			AssertIndicesContain(mergedMatcher.AnyOfIndices, CID.ComponentA, CID.ComponentB, CID.ComponentC);
		}

		[NUnit.Framework.Test]
		public void AnyOfMergesMatchersToNewMatcherWithoutDuplicates()
		{
			var m1 = Matcher<TestEntity>.AnyOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AnyOf(CID.ComponentB);
			var m3 = Matcher<TestEntity>.AnyOf(CID.ComponentC);
			var mergedMatcher = Matcher<TestEntity>.AnyOf(m1, m2, m3);

			AssertIndicesContain(mergedMatcher.Indices, CID.ComponentA, CID.ComponentB, CID.ComponentC);
			AssertIndicesContain(mergedMatcher.AnyOfIndices, CID.ComponentA, CID.ComponentB, CID.ComponentC);
		}

		[NUnit.Framework.Test]
		public void AnyOfThrowsWhenMergingMatcherWithMoreThanOneIndex()
		{
			var m1 = Matcher<TestEntity>.AnyOf(CID.ComponentA, CID.ComponentB);

			Assert.Throws<MatcherException>(() => Matcher<TestEntity>.AnyOf(m1));
		}

		[NUnit.Framework.Test]
		public void ValidateAnyOfToString()
		{
			SetupAnyOfMatcher();

			Assert.AreEqual(_anyOfMatcher.ToString(), "AnyOf(1, 2)");
		}

		#endregion

		#region AllOf.NoneOf

		[NUnit.Framework.Test]
		public void CompoundMatcherHasCorrectIndices()
		{
			SetupCompoundMatcher();

			AssertIndicesContain(_compoundMatcher.Indices, CID.ComponentA, CID.ComponentB, CID.ComponentC, CID.ComponentD);
			AssertIndicesContain(_compoundMatcher.AllOfIndices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(_compoundMatcher.NoneOfIndices, CID.ComponentC, CID.ComponentD);
		}

		[NUnit.Framework.Test]
		public void CompoundMatcherHasCorrectIndicesWithoutDuplicates()
		{
			_compoundMatcher = Matcher<MyTestEntity>
				.AllOf(
					CID.ComponentA, CID.ComponentA,
					CID.ComponentB)
				.NoneOf(CID.ComponentB, CID.ComponentC, CID.ComponentC);

			AssertIndicesContain(_compoundMatcher.Indices, CID.ComponentA, CID.ComponentB, CID.ComponentC);
			AssertIndicesContain(_compoundMatcher.AllOfIndices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(_compoundMatcher.NoneOfIndices, CID.ComponentB, CID.ComponentC);
		}

		[NUnit.Framework.Test]
		public void CompoundMatcherCachesIndices()
		{
			SetupCompoundMatcher();

			Assert.AreEqual(_compoundMatcher.Indices, _compoundMatcher.Indices);
		}

		[NUnit.Framework.Test]
		public void CompoundMatcherDoesNotMatch()
		{
			SetupCompoundMatcher();

			Assert.IsFalse(_compoundMatcher.Matches(_eAbc));
		}

		[NUnit.Framework.Test]
		public void CompoundMatcherDoesMatch()
		{
			SetupCompoundMatcher();

			Assert.IsTrue(_compoundMatcher.Matches(_eAb));
		}

		[NUnit.Framework.Test]
		public void CompoundMatcherMutatesExistingMatcher()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = m1.NoneOf(CID.ComponentB);

			Assert.AreEqual(m2, m1);

			AssertIndicesContain(m1.Indices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(m1.AllOfIndices, CID.ComponentA);
			AssertIndicesContain(m1.NoneOfIndices, CID.ComponentB);
		}

		[NUnit.Framework.Test]
		public void CompoundMatcherMutatesExistingMergedMatcher()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AllOf(CID.ComponentB);
			var m3 = Matcher<TestEntity>.AllOf(m1);
			var m4 = m3.NoneOf(m2);

			Assert.AreEqual(m4, m3);

			AssertIndicesContain(m3.Indices, CID.ComponentA, CID.ComponentB);
			AssertIndicesContain(m3.AllOfIndices, CID.ComponentA);
			AssertIndicesContain(m3.NoneOfIndices, CID.ComponentB);
		}

		[NUnit.Framework.Test]
		public void ValidateCompoundToString()
		{
			SetupCompoundMatcher();

			Assert.AreEqual(_compoundMatcher.ToString(), "AllOf(1, 2).NoneOf(3, 4)");
		}

		[NUnit.Framework.Test]
		public void ValidateCompoundToStringWhenComponentNamesAreSet()
		{
			SetupCompoundMatcher();

			var matcher = (Matcher<MyTestEntity>)_compoundMatcher;
			matcher.ComponentNames = new[] { "one", "two", "three", "four", "five" };

			Assert.AreEqual(matcher.ToString(), "AllOf(two, three).NoneOf(four, five)");
		}

		#endregion

		#region Indices Cache

		[NUnit.Framework.Test]
		public void MatcherIndicesCacheInvalidatedWhenCallingAnyOf()
		{
			var m = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var cache = m.Indices;
			m.AnyOf(CID.ComponentB);

			Assert.AreNotEqual(cache, m.Indices);
		}


		[NUnit.Framework.Test]
		public void MatcherIndicesCacheInvalidatedWhenCallingNoneOf()
		{
			var m = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var cache = m.Indices;
			m.NoneOf(CID.ComponentB);

			Assert.AreNotEqual(cache, m.Indices);
		}

		#endregion

		#region Equals

		[NUnit.Framework.Test]
		public void HashIsUpdatedWhenChangedWithAnyOf()
		{
			var m1 = AllOfAb();
			var hash = m1.GetHashCode();

			Assert.AreNotEqual(hash, m1.AnyOf(42).GetHashCode());
		}

		[NUnit.Framework.Test]
		public void HashIsUpdatedWhenChangedWithNoneOf()
		{
			var m1 = AllOfAb();
			var hash = m1.GetHashCode();

			Assert.AreNotEqual(hash, m1.NoneOf(42).GetHashCode());
		}

		[NUnit.Framework.Test]
		public void AllOfMatcherEquals()
		{
			var m1 = AllOfAb();
			var m2 = AllOfAb();

			Assert.IsFalse(ReferenceEquals(m2, m1));
			Assert.IsTrue(m2.Equals(m1));
			Assert.AreEqual(m2.GetHashCode(), m1.GetHashCode());
		}

		[NUnit.Framework.Test]
		public void AllOfMatcherEqualsRegardlessOfIndicesOrder()
		{
			var m1 = AllOfAb();
			var m2 = AllOfBa();

			Assert.AreEqual(m2, m1);
			Assert.AreEqual(m2.GetHashCode(), m1.GetHashCode());
		}

		[NUnit.Framework.Test]
		public void MergedMatcherEquals()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AllOf(CID.ComponentB);
			var m3 = AllOfBa();

			var mergedMatcher = Matcher<TestEntity>.AllOf(m1, m2);

			Assert.AreEqual(m3, mergedMatcher);
			Assert.AreEqual(m3.GetHashCode(), mergedMatcher.GetHashCode());
		}

		[NUnit.Framework.Test]
		public void DifferentAllMatcherDoesNotEqual()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = AllOfAb();

			Assert.AreNotEqual(m2, m1);
			Assert.AreNotEqual(m2.GetHashCode(), m1.GetHashCode());
		}

		[NUnit.Framework.Test]
		public void AllOfDoesNotEqualAnyOfWithSameIndices()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AnyOf(CID.ComponentA);

			Assert.AreNotEqual(m2, m1);
			Assert.AreNotEqual(m2.GetHashCode(), m1.GetHashCode());
		}

		[NUnit.Framework.Test]
		public void DifferentTypeMatchersWithSameIndicesAreNotEqual()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AllOf(CID.ComponentB);

			var m3 = Matcher<TestEntity>.AllOf(m1, m2);
			var m4 = Matcher<TestEntity>.AnyOf(m1, m2);

			Assert.AreNotEqual(m4, m3);
			Assert.AreNotEqual(m4.GetHashCode(), m3.GetHashCode());
		}

		[NUnit.Framework.Test]
		public void CompoundMatchersCanEqual()
		{
			var m1 = Matcher<TestEntity>.AllOf(CID.ComponentA);
			var m2 = Matcher<TestEntity>.AnyOf(CID.ComponentB);
			var m3 = Matcher<TestEntity>.AnyOf(CID.ComponentC);
			var m4 = Matcher<TestEntity>.AnyOf(CID.ComponentD);

			var mX = Matcher<TestEntity>.AllOf(m1, m2).AnyOf(m3, m4);
			var mY = Matcher<TestEntity>.AllOf(m1, m2).AnyOf(m3, m4);

			Assert.AreEqual(mY, mX);
			Assert.AreEqual(mY.GetHashCode(), mX.GetHashCode());
		}

		#endregion

		#region Helpers

		private void SetupAllOfMatcher()
		{
			_allOfMatcher = Matcher<MyTestEntity>.AllOf(CID.ComponentA, CID.ComponentB);
		}

		private void SetupAnyOfMatcher()
		{
			_anyOfMatcher = Matcher<MyTestEntity>.AnyOf(CID.ComponentA, CID.ComponentB);
		}

		private void SetupCompoundMatcher()
		{
			_compoundMatcher = Matcher<MyTestEntity>
				.AllOf(CID.ComponentA, CID.ComponentB)
				.NoneOf(CID.ComponentC, CID.ComponentD);
		}

		private static void AssertIndicesContain(int[] indices, params int[] expectedIndices)
		{
			Assert.AreEqual(expectedIndices.Length, indices.Length);

			for (int i = 0; i < expectedIndices.Length; i++)
			{
				Assert.AreEqual(expectedIndices[i], indices[i]);
			}
		}

		private static IAllOfMatcher<TestEntity> AllOfAb()
		{
			return Matcher<TestEntity>.AllOf(CID.ComponentA, CID.ComponentB);
		}

		private static IAllOfMatcher<TestEntity> AllOfBa()
		{
			return Matcher<TestEntity>.AllOf(CID.ComponentB, CID.ComponentA);
		}

		#endregion
	}
}
