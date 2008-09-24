// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Reflection;
using System.Collections;
using NUnit.Framework;

namespace NUnitLite
{
    public class TestCase : ITest
    {
        #region Instance Variables
        private string name;
        private string fullName;

        private object fixture;
        private MethodInfo method;

        private MethodInfo setup;
        private MethodInfo teardown;

        private RunState runState = RunState.Runnable;
        private string ignoreReason;

        private IDictionary properties;
        #endregion

        #region Constructors
        public TestCase(string name)
        {
            this.name = this.fullName = name;
        }

        public TestCase(MethodInfo method)
        {
            Initialize(method, null);
        }

        public TestCase(string name, object fixture)
        {
            Initialize(fixture.GetType().GetMethod(name), fixture);
        }

        private void Initialize(MethodInfo method, object fixture)
        {
            this.name = method.Name;
            this.method = method;
            this.fullName = method.ReflectedType.FullName + "." + name;
            this.fixture = fixture;
            if ( fixture == null )
                this.fixture = Reflect.Construct(method.ReflectedType, null);

            if (!HasValidSignature(method))
            {
                this.runState = RunState.NotRunnable;
                this.ignoreReason = "Test methods must have signature void MethodName()";
            }
            else
            {
                IgnoreAttribute ignore = (IgnoreAttribute)Reflect.GetAttribute(this.method, typeof(IgnoreAttribute));
                if (ignore != null)
                {
                    this.runState = RunState.Ignored;
                    this.ignoreReason = ignore.Reason;
                }
            }

            foreach (MethodInfo m in method.ReflectedType.GetMethods())
            {
                if (Reflect.HasAttribute(m, typeof(SetUpAttribute)))
                    this.setup = m;

                if (Reflect.HasAttribute(m, typeof(TearDownAttribute)))
                    this.teardown = m;
            }
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public string FullName
        {
            get { return fullName; }
        }

        public RunState RunState
        {
            get { return runState; }
        }

        public string IgnoreReason
        {
            get { return ignoreReason; }
        }

        public System.Collections.IDictionary Properties
        {
            get 
            {
                if (properties == null)
                {
                    properties = new Hashtable();

                    object[] attrs = this.method.GetCustomAttributes(typeof(PropertyAttribute), true);
                    foreach (PropertyAttribute attr in attrs)
                        this.Properties[attr.Name] = attr.Value;
                }

                return properties; 
            }
        }

        public int TestCaseCount
        {
            get { return 1; }
        }
        #endregion

        #region Public Methods
        public static bool IsTestMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, typeof(TestAttribute));
        }

        public TestResult Run()
        {
            return Run( new NullListener() );
        }

        public TestResult Run(TestListener listener)
        {
            listener.TestStarted(this);

            TestResult result = new TestResult(this);
            Run(result, listener);

            listener.TestFinished(result);

            return result;
        }
        #endregion

        #region Protected Methods
        protected virtual void SetUp() 
        {
            if (setup != null)
            {
                Assert.That(HasValidSetUpTearDownSignature(setup), "Invalid SetUp method: must return void and have no arguments");
                InvokeMethod(setup);
            }
        }

        protected virtual void TearDown() 
        {
            if (teardown != null)
            {
                Assert.That(HasValidSetUpTearDownSignature(teardown), "Invalid TearDown method: must return void and have no arguments");
                InvokeMethod(teardown);
            }
        }

        protected virtual void Run(TestResult result, TestListener listener)
        {
            IgnoreAttribute ignore = (IgnoreAttribute)Reflect.GetAttribute(method, typeof(IgnoreAttribute));
            if (this.RunState == RunState.NotRunnable)
                result.Failure(this.ignoreReason);
            else if ( ignore != null )
                result.NotRun(ignore.Reason);
            else
            {
                try
                {
                    RunBare();
                    result.Success();
                }
                catch (NUnitLiteException nex)
                {
                    result.RecordException(nex.InnerException);
                }
#if !NETCF_1_0
                catch (System.Threading.ThreadAbortException)
                {
                    throw;
                }
#endif
                catch (Exception ex)
                {
                    result.RecordException(ex);
                }
            }
        }

        protected void RunBare()
        {
            SetUp();
            try
            {
                RunTest();
            }
            finally
            {
                TearDown();
            }
        }

        protected virtual void RunTest()
        {
            try
            {
                InvokeMethod( this.method );
                ProcessNoException(this.method);
            }
            catch (NUnitLiteException ex)
            {
                ProcessException(this.method, ex.InnerException);
            }
        }

        protected void InvokeMethod(MethodInfo method, params object[] args)
        {
            Reflect.InvokeMethod(method, this.fixture, args);
        }
        #endregion

        #region Private Methods       
        public static bool HasValidSignature(MethodInfo method)
        {
            return method != null
                && method.ReturnType == typeof(void)
                && method.GetParameters().Length == 0; ;
        }

        private static bool HasValidSetUpTearDownSignature(MethodInfo method)
        {
            return method.ReturnType == typeof(void)
                && method.GetParameters().Length == 0; ;
        }

        private static void ProcessNoException(MethodInfo method)
        {
            ExpectedExceptionAttribute exceptionAttribute =
                (ExpectedExceptionAttribute)Reflect.GetAttribute(method, typeof(ExpectedExceptionAttribute));

            if (exceptionAttribute != null)
                Assert.Fail("Expected Exception of type <{0}>, but none was thrown", exceptionAttribute.ExceptionType);
        }

        private void ProcessException(MethodInfo method, Exception caughtException)
        {
            ExpectedExceptionAttribute exceptionAttribute =
                (ExpectedExceptionAttribute)Reflect.GetAttribute(method, typeof(ExpectedExceptionAttribute));

            if (exceptionAttribute == null)
                throw new NUnitLiteException("", caughtException);

            Type expectedType = exceptionAttribute.ExceptionType;
            if ( expectedType != null && expectedType != caughtException.GetType() )
                Assert.Fail("Expected Exception of type <{0}>, but was <{1}>", exceptionAttribute.ExceptionType, caughtException.GetType());

            MethodInfo handler = GetExceptionHandler(method.ReflectedType, exceptionAttribute.Handler);

            if (handler != null)
                InvokeMethod( handler, caughtException );
        }

        private MethodInfo GetExceptionHandler(Type type, string handlerName)
        {
            if (handlerName == null && Reflect.HasInterface( type, typeof(IExpectException) ) )
                handlerName = "HandleException";

            if (handlerName == null)
                return null;

            MethodInfo handler = Reflect.GetMethod( type, handlerName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                new Type[] { typeof(Exception) });

            if (handler == null)
                Assert.Fail("The specified exception handler {0} was not found", handlerName);

            return handler;
        }
        #endregion
    }
}
