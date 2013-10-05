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
            Assert.AreEqual(1222776, zi.ID);
            Assert.AreEqual("Conquest", zi.Author);
            Assert.AreEqual(1380990901000L, zi.Time);
            Assert.AreEqual(6, zi.Responses);
            Assert.AreEqual(true, zi.HasResponseForYou);
            Assert.AreEqual("http://portal2.dkm.cz/phorum/read.php?f=2&i=1222776&t=1222776", zi.ItemsUrl);
            Assert.AreEqual(true, zi.IsFavourite);
            Assert.AreEqual(true, zi.HasBeenRead);
            Assert.AreEqual(true, zi.IsNewOne);
        }
    }
}
