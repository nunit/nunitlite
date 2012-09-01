// ***********************************************************************
// Copyright (c) 2012 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

#if (CLR_2_0 || CLR_4_0) && !NETCF
using System.Threading;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.WorkItems;
using NUnit.TestData;
using NUnit.TestUtilities;
using System;

namespace NUnit.Framework.Attributes
{
    public class AsynchronousTests
    {
        int parentThreadId;
        int setupThreadId;

        [TestFixtureSetUp]
        public void GetParentThreadInfo()
        {
			this.parentThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        [SetUp]
        public void GetSetUpThreadInfo()
        {
            this.setupThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        [Test, Asynchronous]
        public void LongRunningAsynchronousTestSucceeds()
        {
            Thread.Sleep(20);
        }

        [Test, Asynchronous]
        public void AsynchronousTestRunsOnSeparateThread()
        {
            Assert.That(Thread.CurrentThread.ManagedThreadId, Is.Not.EqualTo(parentThreadId));
        }

        [Test, Timeout(50)]
        public void AsynchronousTestReturnsBeforeTestIsComplete()
        {
            AsynchronousFixture fixture = new AsynchronousFixture();
            WorkItem wi = TestBuilder.RunTestCaseAsync(fixture, "AsynchronousTest");

            Assert.That(wi.State, Is.Not.EqualTo(WorkItemState.Complete));

            fixture.Quit = true;
            while (wi.State != WorkItemState.Complete)
                Thread.Sleep(2);

            Assert.That(wi.Result.ResultState, Is.EqualTo(ResultState.Success));
        }

        [Test]
        public void AsynchronousTestMayHaveTimeoutSpecified()
        {
            AsynchronousFixture fixture = new AsynchronousFixture();
            ITestResult result = TestBuilder.RunTestCase(fixture, "AsynchronousTestWithTimeout");

            Assert.That(result.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(result.Message, Contains.Substring("5ms"));
        }
    }
}
#endif
