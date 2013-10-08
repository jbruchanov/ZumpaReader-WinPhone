using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ZumpaReader
{
    public class TestCase : WorkItemTest
    {
        private object oLock = new object();

        private bool _finished = false;

        protected void TestWait()
        {
            _finished = false;
            lock (oLock) { Monitor.Wait(oLock); }
        }

        protected void TestWait(int wait)
        {
            _finished = false;
            lock (oLock)
            {
                Monitor.Wait(oLock, wait);
                if (!_finished)
                {
                    Assert.Fail("Timeout wait exceeded: " + wait + "ms");
                }
            }

        }

        protected void RunInMainThread(Action a)
        {
            Deployment.Current.Dispatcher.BeginInvoke(a);
        }

        protected void FinishWaiting()
        {
            lock (oLock) 
            {
                _finished = true;
                Monitor.PulseAll(oLock); 
            }            
        }
    }
}
