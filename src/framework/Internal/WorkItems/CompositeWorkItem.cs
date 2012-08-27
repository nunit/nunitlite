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

using System;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal.WorkItems
{
    /// <summary>
    /// A CompositeWorkItem represents a test suite and
    /// encapsulates the execution of the suite as well
    /// as all its child tests.
    /// </summary>
    public class CompositeWorkItem : WorkItem
    {
        private TestSuite _suite;
        private TestSuiteResult _suiteResult;
#if CLR_2_0 || CLR_4_0
        private System.Collections.Generic.List<WorkItem> _children = new System.Collections.Generic.List<WorkItem>();
#else
        private System.Collections.ArrayList _children = new System.Collections.ArrayList();
#endif
        private TestSuiteCommand _suiteCommand;

        /// <summary>
        /// Construct a CompositeWorkItem for executing a test suite
        /// using a filter to select child tests.
        /// </summary>
        /// <param name="suite">The TestSuite to be executed</param>
        /// <param name="childFilter">A filter used to select child tests</param>
        public CompositeWorkItem(TestSuite suite, ITestFilter childFilter)
            : base(suite)
        {
            _suite = suite;
            _suiteResult = Result as TestSuiteResult;
            _suiteCommand = Command as TestSuiteCommand;

            foreach (Test test in _suite.Tests)
                _children.Add(test.CreateWorkItem(childFilter));
        }

        /// <summary>
        /// Method that actually performs the work. Overridden
        /// in CompositeWorkItem to do setup, run all child
        /// items and then do teardown.
        /// </summary>
        protected override void PerformWork()
        {
            PerformOneTimeSetUp();

            if (Result.ResultState.Status == TestStatus.Passed)
                RunChildren();

            PerformOneTimeTearDown();

            WorkItemComplete();
        }

        #region Helper Methods

        private void PerformOneTimeSetUp()
        {
            // Assume success, since the result will be inconclusive
            // if there is no setup method at all to run
            Result.SetResult(ResultState.Success);

            try
            {
                _suiteCommand.DoOneTimeSetUp(Context);

                // SetUp may have changed some things
                Context.Update();
            }
            catch (Exception ex)
            {
                if (ex is NUnitException || ex is System.Reflection.TargetInvocationException)
                    ex = ex.InnerException;

                ((TestSuiteResult)Result).RecordException(ex, FailureSite.SetUp);
            }
        }

        private void RunChildren()
        {
            foreach (WorkItem child in _children)
                Result.AddResult(child.Execute());
        }

        private void PerformOneTimeTearDown()
        {
            _suiteCommand.DoOneTimeTearDown(Context);
        }

        #endregion
    }
}
