// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
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
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal
{
    public class TestResult : ITestResult
    {
        private ITest test;

        private ResultState resultState = ResultState.NotRun;

        private string message;
#if !NETCF_1_0
        private string stackTrace;
#endif

        private ArrayList results;

        public TestResult(ITest test)
        {
            this.test = test;
        }

        public ITest Test
        {
            get { return test; }
        }

        public ResultState ResultState
        {
            get { return resultState; }
        }

        public IList Results
        {
            get 
            {
                if (results == null)
                    results = new ArrayList();

                return results;
            }
        }

        public bool Executed
        {
            get { return resultState != ResultState.NotRun; }
        }

        public string Message
        {
            get { return message; }
        }

#if !NETCF_1_0
        public string StackTrace
        {
            get { return stackTrace; }
        }
#endif

        public void AddResult(ITestResult result)
        {
            if (results == null)
                results = new ArrayList();

            results.Add(result);

            switch (result.ResultState)
            {
                case ResultState.Error:
                case ResultState.Failure:
                    this.SetResult(ResultState.Failure, "Component test failure");
                    break;
                default:
                    break;
            }
        }

//        public void Error(Exception ex)
//        {
//#if !NETCF_1_0
//            SetResult(ResultState.Error, ex.GetType().ToString() + " : " + ex.Message, ex.StackTrace);
//#else
//            SetResult(ResultState.Error, ex.GetType().ToString() + " : " + ex.Message);
//#endif
////            this.resultState = ResultState.Error;
////            this.message = ex.GetType().ToString() + " : " + ex.Message;
////#if !NETCF_1_0
////            this.stackTrace = ex.StackTrace;
////#endif
//        }

        /// <summary>
        /// Set the result of the test
        /// </summary>
        /// <param name="resultState">The ResultState to use in the result</param>
        public void SetResult(ResultState resultState)
        {
            SetResult(resultState, null, null);
        }

        /// <summary>
        /// Set the result of the test
        /// </summary>
        /// <param name="resultState">The ResultState to use in the result</param>
        /// <param name="message">A message associated with the result state</param>
        public void SetResult(ResultState resultState, string message)
        {
            SetResult(resultState, message, null);
        }

        /// <summary>
        /// Set the result of the test
        /// </summary>
        /// <param name="resultState">The ResultState to use in the result</param>
        /// <param name="message">A message associated with the result state</param>
        /// <param name="stackTrace">Stack trace giving the location of the command</param>
        public void SetResult(ResultState resultState, string message, string stackTrace)
        {
            this.resultState = resultState;
            this.message = message;
            this.stackTrace = stackTrace;
        }

        public void RecordException(Exception ex)
        {
            if (ex is NUnitLiteException)
                ex = ex.InnerException;

#if !NETCF_1_0
            if (ex is AssertionException)
                this.SetResult(ResultState.Failure, ex.Message, StackFilter.Filter(ex.StackTrace));
            else if (ex is SuccessException)
                this.SetResult(ResultState.Success, ex.Message);
            else if (ex is IgnoreException)
                this.SetResult(ResultState.NotRun, ex.Message, StackFilter.Filter(ex.StackTrace));
            else if (ex is InconclusiveException)
                this.SetResult(ResultState.NotRun, ex.Message, StackFilter.Filter(ex.StackTrace));
            else
                this.SetResult(ResultState.Error, ex.GetType().ToString() + " : " + ex.Message, ex.StackTrace);
#else
            if (ex is AssertionException)
		        this.SetResult(ResultState.Failure, ex.Message);
            else if (ex is SuccessException)
                this.SetResult(ResultState.Success, ex.Message);
            else if (ex is IgnoreException)
                this.SetResult(ResultState.NotRun, ex.Message);
            else if (ex is InconclusiveException)
                this.SetResult(ResultState.NotRun, ex.Message);
            else
                this.SetResult(ResultState.Error, ex.GetType().ToString() + " : " + ex.Message);
#endif
        }
    }
}
