using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace NUnit.TestData
{
    public class AsynchronousFixture
    {
        public bool Quit;

        [Test, Asynchronous]
        public void AsynchronousTest()
        {
            while (!Quit)
                Thread.Sleep(5);
        }

        [Test, Asynchronous, Timeout(5)]
        public void AsynchronousTestWithTimeout()
        {
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
