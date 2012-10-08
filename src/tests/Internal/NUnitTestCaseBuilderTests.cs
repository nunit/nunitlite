#if NET_4_5
using System.Reflection;
using NUnit.Framework.Api;
using NUnit.Framework.Builders;
using NUnit.TestData;
using NUnit.TestUtilities;

namespace NUnit.Framework.Internal
{
	[TestFixture]
	public class NUnitTestCaseBuilderTests
	{
        private static readonly System.Type fixtureType = typeof(AsyncDummyFixture);

        [Test]
		public void Async_Void_IsRunnable()
		{
            TestAssert.IsRunnable(fixtureType, "AsyncVoid");
		}

		[Test]
		public void Async_Task_IsRunnable()
		{
            TestAssert.IsRunnable(fixtureType, "AsyncTask");
		}

        [Test]
        public void Async_GenericTask_IsNotRunnable()
        {
            TestAssert.IsNotRunnable(fixtureType, "AsyncGenericTask");
        }

        [Test]
        public void NonAsync_Task_IsNotRunnable()
        {
            TestAssert.IsNotRunnable(fixtureType, "NonAsyncTask");
        }

        [Test]
        public void NonAsync_GenericTask_IsNotRunnable()
        {
            TestAssert.IsNotRunnable(fixtureType, "NonAsyncGenericTask");
        }

        [Test]
        public void Async_Void_TestCase_IsRunnable()
        {
            TestAssert.FirstChildIsRunnable(fixtureType, "AsyncVoidTestCase");
        }

        [Test]
        public void Async_Void_TestCaseWithExpectedResult_IsNotRunnable()
        {
            TestAssert.FirstChildIsNotRunnable(fixtureType, "AsyncVoidTestCaseWithExpectedResult");
        }

        [Test]
        public void Async_Task_TestCase_IsRunnable()
        {
            TestAssert.FirstChildIsRunnable(fixtureType, "AsyncTaskTestCase");
        }

        [Test]
        public void Async_Task_TestCaseWithExpectedResult_IsNotRunnable()
        {
            TestAssert.FirstChildIsNotRunnable(fixtureType, "AsyncTaskTestCaseWithExpectedResult");
        }

        [Test]
        public void Async_GenericTask_TestCase_IsNotRunnable()
        {
            TestAssert.FirstChildIsNotRunnable(fixtureType, "AsyncGenericTask_TestCase");
        }

        [Test]
        public void Async_GenericTask_TestCaseWithExpectedResult_IsRunnable()
        {
            TestAssert.FirstChildIsRunnable(fixtureType, "AsyncGenericTask_TestCaseWithExpectedResult");
        }
    }
}
#endif