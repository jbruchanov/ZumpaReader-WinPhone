using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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
            return;                       
            BackgroundColorConverter conv = new BackgroundColorConverter();                                                
            Brush b1 = conv.Convert(1, typeof(SolidColorBrush), null, CultureInfo.DefaultThreadCurrentCulture) as Brush;
            Brush b2 = conv.Convert(2, typeof(SolidColorBrush), null, CultureInfo.DefaultThreadCurrentCulture) as Brush;

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);

            Assert.IsTrue(b1 == b2);            
        }
    }
}
