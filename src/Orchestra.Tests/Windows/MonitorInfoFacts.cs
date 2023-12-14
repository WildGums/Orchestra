namespace Orchestra.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using Catel.Windows;
    using NUnit.Framework;
    using Orchestra.Windows;

    [TestFixture]
    public class MonitorInfoFacts
    {
        [Explicit]
        [TestCase(false)]
        [TestCase(true)]
        public void GetMonitorInfoManifestHandled(bool manifestCheck)
        {
            if (manifestCheck)
            {
                Assert.Throws<NotSupportedException>(() => MonitorInfo.GetAllMonitors(manifestCheck));
            }
            else
            {
                Assert.DoesNotThrow(() => MonitorInfo.GetAllMonitors(manifestCheck), "Unexpected exception thrown");
            }
        }

        [TestCase]
        public void MonitorInfoWorkingAreaAndResolutionScalesOnDpiCorrectly()
        {
            var testMonitorInfo = new MonitorInfo()
            {
                MonitorArea = new System.Windows.Int32Rect(0, 0, 1920, 1080),
                WorkingArea = new System.Windows.Int32Rect(0, 0, 1900, 1000),
                DpiScale = new Windows.DpiScale() { X = 1.25, Y = 1.25 }
            };

            var scaledResolution = testMonitorInfo.GetDpiAwareResolution();
            var scaledWorkingArea = testMonitorInfo.GetDpiAwareWorkingArea();

            Assert.That(new Rect(0, 0, 2400, 1350), Is.EqualTo(scaledResolution));
            Assert.That(new Rect(0, 0, 2375, 1250), Is.EqualTo(scaledWorkingArea));
        }
    }
}
