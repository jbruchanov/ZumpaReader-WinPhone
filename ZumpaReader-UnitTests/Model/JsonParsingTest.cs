using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader.Model;

namespace ZumpaReader_UnitTests.Model
{
    [TestClass]
    public class JsonParsingTest : WorkItemTest
    {        
        [TestMethod]
        public void TestZumpaItemParsing()
        {
            string json = JsonResources.ZumpaItemJson;
            ZumpaItem zi = JsonConvert.DeserializeObject<ZumpaItem>(json);
            
            Assert.IsNotNull(zi);
            Assert.AreEqual("Tohle si zumpa zaslouzi videt..", zi.Subject);
        }
    }
}
