using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnitLite.Runner;
using System.ComponentModel;
using System.Collections;

namespace NUnitLite.Runner.Silverlight
{
    public partial class TestPage : UserControl
    {
        private Assembly callingAssembly;
        private ITestAssemblyRunner runner;
        private TextWriter writer;
        private int reportCount = 0;

        public TestPage()
        {
            InitializeComponent();

            this.runner = new NUnitLiteTestAssemblyRunner(new NUnitLiteTestAssemblyBuilder());
            this.callingAssembly = Assembly.GetCallingAssembly();
            this.writer = new TextBlockWriter(this.ScratchArea);
        }

        //private TestRunStatus Summary;

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    TextWriter writer = new TextBlockWriter(this.ScratchArea);
        //    new TextUI(writer, this.Summary).Execute(new string[0]);
        //}

        //private void bntClicker_Click(object sender, RoutedEventArgs e)
        //{
        //    //Execute();
        //    new TextUI(new TextBlockWriter(this.ScratchArea)).Execute(new string[0]);
        //}

#if false
        #region Nested TestRunStatus Class

        public class TestRunStatus : ITestListener, INotifyPropertyChanged
        {
            #region Properties

            private int total = 0;
            public int Total
            {
                get { return total; }
                set
                {
                    total = value;
                    NotifyPropertyChanged("Total");
                }
            }

            private int passed = 0;
            public int Passed
            {
                get { return passed; }
                set
                {
                    passed = value;
                    NotifyPropertyChanged("Passed");
                }
            }

            private int failures = 0;
            public int Failures
            {
                get { return failures; }
                set
                {
                    failures = value;
                    NotifyPropertyChanged("Failures");
                }
            }

            private string currentFixture;
            public string CurrentFixture
            {
                get { return currentFixture; }
                set 
                { 
                    currentFixture = value;
                    NotifyPropertyChanged("CurrentFixture");
                }
            }

            private string currentTest;
            public string CurrentTest 
            {
                get { return currentTest; }
                set 
                { 
                    currentTest = value;
                    NotifyPropertyChanged("CurrentTest");
                }
            }

            private string scratchArea;
            public string ScratchArea
            {
                get { return scratchArea; }
                set
                {
                    scratchArea = value;
                    NotifyPropertyChanged("ScratchArea");
                }
            }

            #endregion

            #region ITestListener Members

            public void TestStarted(ITest test)
            {
                if (test.FixtureType != null)
                    CurrentFixture = test.FixtureType.Name;
                CurrentTest = test.FullName;
            }

            public void TestFinished(ITestResult result)
            {
                Total++;
                Passed++;
            }

            public void TestOutput(TestOutput testOutput)
            {
                ScratchArea += testOutput.Text;
            }

            #endregion

            #region INotifyPropertyChanged Interface

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            #region Helper Methods

            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }

        #endregion
#endif

        private void Execute()
        {
            WriteCopyright();

            IDictionary loadOptions = new System.Collections.Generic.Dictionary<string, string>();

            if (!runner.Load(callingAssembly, loadOptions))
            {
                AssemblyName assemblyName = new AssemblyName(callingAssembly.FullName);
                writer.WriteLine("No tests found in assembly {0}", assemblyName.Name);
                return;
            }

            ITestResult result = runner.Run(TestListener.NULL, TestFilter.Empty);
            ResultReporter reporter = new ResultReporter(result, writer);

            reporter.ReportResults();

            ResultSummary summary = reporter.Summary;

            this.Total.Text = summary.TestCount.ToString();
            this.Failures.Text = summary.FailureCount.ToString();
            this.Errors.Text = summary.ErrorCount.ToString();
            this.NotRun.Text = summary.NotRunCount.ToString();
            this.Passed.Text = summary.PassCount.ToString();
            this.Inconclusive.Text = summary.InconclusiveCount.ToString();

            this.Notice.Visibility = Visibility.Collapsed;
        }

        private void WriteCopyright()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
#if NUNITLITE
            string title = "NUnitLite";
#else
            string title = "NUNit Framework";
#endif
            AssemblyName assemblyName = new AssemblyName(executingAssembly.FullName);
            System.Version version = assemblyName.Version;
            string copyright = "Copyright (C) 2012, Charlie Poole";
            string build = "";

#if !NETCF_1_0
            object[] attrs = executingAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attrs.Length > 0)
            {
                AssemblyTitleAttribute titleAttr = (AssemblyTitleAttribute)attrs[0];
                title = titleAttr.Title;
            }

            attrs = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attrs.Length > 0)
            {
                AssemblyCopyrightAttribute copyrightAttr = (AssemblyCopyrightAttribute)attrs[0];
                copyright = copyrightAttr.Copyright;
            }

            attrs = executingAssembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);
            if (attrs.Length > 0)
            {
                AssemblyConfigurationAttribute configAttr = (AssemblyConfigurationAttribute)attrs[0];
                if (configAttr.Configuration.Length > 0)
                    build = string.Format("({0})", configAttr.Configuration);
            }
#endif

            writer.WriteLine(String.Format("{0} {1} {2}", title, version.ToString(3), build));
            writer.WriteLine(copyright);
            writer.WriteLine();

            string clrPlatform = Type.GetType("Mono.Runtime", false) == null ? ".NET" : "Mono";
            writer.WriteLine("Runtime Environment -");
            writer.WriteLine("    OS Version: {0}", Environment.OSVersion);
            writer.WriteLine("  {0} Version: {1}", clrPlatform, Environment.Version);
            writer.WriteLine();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke( () => Execute() );
        }
    }
}
