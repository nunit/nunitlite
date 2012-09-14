using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;

namespace NUnitLite.Runner.Silverlight
{
    /// <summary>
    /// TestPage is the display page for the test results
    /// </summary>
    public partial class TestPage : UserControl
    {
        private Assembly callingAssembly;
        private ITestAssemblyRunner runner;
        private TextWriter writer;

        public TestPage()
        {
            InitializeComponent();

            this.runner = new NUnitLiteTestAssemblyRunner(new NUnitLiteTestAssemblyBuilder());
            this.callingAssembly = Assembly.GetCallingAssembly();
            this.writer = new TextBlockWriter(this.ScratchArea);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayApplicationHeader();

            if (!LoadTestAssembly())
                writer.WriteLine("No tests found in assembly {0}", GetAssemblyName(callingAssembly));
            else
                Dispatcher.BeginInvoke(() => ExecuteTests());
        }

        #region Helper Methods

        private bool LoadTestAssembly()
        {
            return runner.Load(callingAssembly, new Dictionary<string, string>());
        }

        private string GetAssemblyName(Assembly assembly)
        {
            return new AssemblyName(assembly.FullName).Name;
        }

        private void ExecuteTests()
        {
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

        private void DisplayApplicationHeader()
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

        #endregion
    }
}
