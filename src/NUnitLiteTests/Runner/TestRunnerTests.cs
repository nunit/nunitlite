// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnitLite.Tests;

namespace NUnitLite.Runner.Tests
{
    [TestFixture]
    public class TestRunnerTests : ITestListener
    {
        private int tests;
        private int errors;
        private int failures;
        private int notrun;

        [SetUp]
        public void SetUp()
        {
            tests = errors = failures = notrun = 0;
        }

        [Test]
        public void CanRunTestAndReportResults()
        {
            TestRunner runner = new TestRunner();
            runner.AddListener(this);
            runner.Run(new DummyTestSuite("SNSFSES"));

            Assert.That(tests, Is.EqualTo(7), "tests");
            Assert.That(errors, Is.EqualTo(1), "errors");
            Assert.That(failures, Is.EqualTo(1), "failures");
            Assert.That(notrun, Is.EqualTo(0), "notrun");
        }

        void ITestListener.TestStarted(ITest test)
        {
        }

        void ITestListener.TestFinished(ITestResult result)
        {
            if ( result.Results.Count == 0 )
            {
                ++tests;
                if (result.ResultState == ResultState.Error)
                    ++errors;
                else if (result.ResultState == ResultState.Failure)
                    ++failures;
                else if (result.ResultState.Status == TestStatus.Skipped)
                    ++notrun;
            }
        }
    }
}
