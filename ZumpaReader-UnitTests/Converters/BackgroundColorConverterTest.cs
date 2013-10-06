using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ZumpaReader;
using ZumpaReader.Converters;

namespace ZumpaReader_UnitTests.Converters
{
    [TestClass]
    public class BackgroundColorConverterTest : TestCase
    {
        [TestMethod]
        public void TestConverterReturnsSameColorWithNoIndexer()
        {
            Brush b1 = null;
            Brush b2 = null;
            RunInMainThread(() =>
            {
                BackgroundColorConverter conv = new BackgroundColorConverter();
                b1 = conv.Convert(1, typeof(SolidColorBrush), null, CultureInfo.DefaultThreadCurrentCulture) as Brush;
                b2 = conv.Convert(2, typeof(SolidColorBrush), null, CultureInfo.DefaultThreadCurrentCulture) as Brush;
                FinishWaiting();
            });
            TestWait();

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);

            Assert.IsTrue(b1 == b2);
        }

        [TestMethod]
        public void TestConverterReturnsDifferentColorWithIndexer()
        {

            Mock<BackgroundColorConverter.IGetIndexEvaluator> eval = new Mock<BackgroundColorConverter.IGetIndexEvaluator>();
            int toReturn = 0;
            eval.Setup<int>((e) => e.GetIndex(It.IsAny<int>())).Returns<int>((src) => toReturn++);

            Brush b1 = null;
            Brush b2 = null;
            RunInMainThread(() =>
            {
                BackgroundColorConverter conv = new BackgroundColorConverter();
                b1 = conv.Convert(1, typeof(SolidColorBrush), eval.Object, CultureInfo.DefaultThreadCurrentCulture) as Brush;
                b2 = conv.Convert(2, typeof(SolidColorBrush), eval.Object, CultureInfo.DefaultThreadCurrentCulture) as Brush;
                FinishWaiting();
            });
            TestWait();

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);

            Assert.IsFalse(b1 == b2);
            eval.Verify(e => e.GetIndex(It.IsAny<int>()), Times.Exactly(2));
        }
    }
}
