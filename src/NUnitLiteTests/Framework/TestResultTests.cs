// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;

namespace NUnitLite.Tests
{
    [TestFixture]
    public class TestResultTests
    {
        private static readonly string MESSAGE = "my message";
#if !NETCF_1_0
        private static readonly string STACKTRACE = "stack trace";
#endif

        private TestResult result;

        [SetUp]
        public void SetUp()
        {
            result = new TestResult(null);
        }

        void VerifyResultState(ResultState expectedState, bool executed, string message )
        {
            Assert.That( result.ResultState , Is.EqualTo( expectedState ) );
            Assert.That( result.Executed, Is.EqualTo( executed ) );
            //if ( expectedState == ResultState.Error )
            //    Assert.That(result.Message, Is.EqualTo("System.Exception : " + message));
            //else
                Assert.That(result.Message, Is.EqualTo(message));
        }

        [Test]
        public void DefaultStateIsNotRun()
        {
            VerifyResultState(ResultState.NotRun, false, null);
        }

        [Test]
        public void CanMarkAsSuccess()
        {
            result.SetResult(ResultState.Success);
            VerifyResultState(ResultState.Success, true, null);
        }

        [Test]
        public void CanMarkAsFailure()
        {
#if NETCF_1_0
            result.SetResult(ResultState.Failure, MESSAGE);
            VerifyResultState(ResultState.Failure, true, false, true, false, MESSAGE);
#else
            result.SetResult(ResultState.Failure, MESSAGE, STACKTRACE);
            VerifyResultState(ResultState.Failure, true, MESSAGE);
            Assert.That( result.StackTrace, Is.EqualTo( STACKTRACE ) );
#endif
        }

        [Test]
        public void CanMarkAsError()
        {
            Exception caught;
            try
            {
                throw new Exception(MESSAGE);
            }
            catch(Exception ex)
            {
                caught = ex;          
            }

#if !NETCF_1_0
            result.SetResult(ResultState.Error, caught.Message, caught.StackTrace);
#else
            result.SetResult(ResultState.Error, caught.Message);
#endif
            VerifyResultState(ResultState.Error, true, MESSAGE);
#if !NETCF_1_0
            Assert.That( result.StackTrace, Is.EqualTo( caught.StackTrace ) );
#endif
        }

        [Test]
        public void CanMarkAsNotRun()
        {
            result.SetResult(ResultState.NotRun, MESSAGE);
            VerifyResultState(ResultState.NotRun, false, MESSAGE);
        }
    }
}
