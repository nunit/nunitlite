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
using System.Threading;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal.Commands
{
    /// <summary>
    /// CommandRunner class is used to execute test commands.
    /// </summary>
    public class CommandRunner
    {
        /// <summary>
        /// Runs a TestCommand and returns the result.
        /// </summary>
        /// <param name="command">A TestCommand to be executed.</param>
        /// <returns>A TestResult.</returns>
        public static TestResult Execute(TestCommand command)
        {
            TestResult testResult;

            TestExecutionContext.Save();
            TestExecutionContext context = TestExecutionContext.CurrentContext;

            context.CurrentTest = command.Test;
            context.CurrentResult = command.Test.MakeTestResult();

            context.Listener.TestStarted(command.Test);
            long startTime = DateTime.Now.Ticks;

            try
            {
                TestSuiteCommand suiteCommand = command as TestSuiteCommand;
                if (suiteCommand != null)
                    testResult = ExecuteSuiteCommand(suiteCommand, context);
                //{
                //    suiteCommand.DoOneTimeSetup();
                //    foreach (TestCommand childCommand in suiteCommand.Children)
                //        Execute(childCommand, context);
                //    suiteCommand.DoOneTimeTearDown();
                //}
                else
                    testResult = command.Execute(context);

                testResult.AssertCount = context.AssertCount;

                long stopTime = DateTime.Now.Ticks;
                double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;
                testResult.Time = time;

                context.Listener.TestFinished(testResult);
            }
            catch (Exception ex)
            {
#if !NETCF && !SILVERLIGHT
                if (ex is ThreadAbortException)
                    Thread.ResetAbort();
#endif
                context.CurrentResult.RecordException(ex);
                return context.CurrentResult;
            }
            finally
            {
                TestExecutionContext.Restore();
            }

            return testResult;
        }

        private static TestResult ExecuteSuiteCommand(TestSuiteCommand command, TestExecutionContext context)
        {
            TestSuiteResult suiteResult = context.CurrentResult as TestSuiteResult;
            System.Diagnostics.Debug.Assert(suiteResult != null);

            bool oneTimeSetUpComplete = false;
            try
            {
                // Temporary: this should be done by individual commands
                ApplyTestSettingsToExecutionContext(command.Test, context);

                command.DoOneTimeSetUp(context);
                oneTimeSetUpComplete = true;

                // SetUp may have changed some things
                context.Update();

                suiteResult = RunChildCommands(command, context);
            }
            catch (Exception ex)
            {
                if (ex is NUnitException || ex is System.Reflection.TargetInvocationException)
                    ex = ex.InnerException;


                if (oneTimeSetUpComplete)
                    suiteResult.RecordException(ex);
                else
                    suiteResult.RecordException(ex, FailureSite.SetUp);
            }
            finally
            {
                command.DoOneTimeTearDown(context);
            }

            return suiteResult;
        }

        /// <summary>
        /// Runs the child commands for a TestSuiteCommand, using the context provided.
        /// </summary>
        /// <param name="command">The TestSuiteCommand whose child tests are to be run</param>
        /// <param name="context">The context in which to run the tests</param>
        /// <returns></returns>
        public static TestSuiteResult RunChildCommands(TestSuiteCommand command, TestExecutionContext context)
        {
            TestSuiteResult suiteResult = TestExecutionContext.CurrentContext.CurrentResult as TestSuiteResult;
            suiteResult.SetResult(ResultState.Success);

            foreach (TestCommand childCommand in command.Children)
            {
                TestResult childResult = CommandRunner.Execute(childCommand);

                suiteResult.AddResult(childResult);

                if (childResult.ResultState == ResultState.Cancelled)
                    break;

                if (childResult.ResultState.Status == TestStatus.Failed && TestExecutionContext.CurrentContext.StopOnError)
                    break;
            }

            return suiteResult;
        }

        /// <summary>
        /// Applies the culture settings specified on the test
        /// to the TestExecutionContext.
        /// </summary>
        public static void ApplyTestSettingsToExecutionContext(Test test, TestExecutionContext context)
        {
#if !NETCF
            string setCulture = (string)test.Properties.Get(PropertyNames.SetCulture);
            if (setCulture != null)
                context.CurrentCulture = new System.Globalization.CultureInfo(setCulture);

            string setUICulture = (string)test.Properties.Get(PropertyNames.SetUICulture);
            if (setUICulture != null)
                context.CurrentUICulture = new System.Globalization.CultureInfo(setUICulture);
#endif

#if !NUNITLITE
            if (test.Properties.ContainsKey(PropertyNames.Timeout))
                context.TestCaseTimeout = (int)test.Properties.Get(PropertyNames.Timeout);
#endif
        }
    }
}

