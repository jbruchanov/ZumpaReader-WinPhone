using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader.Model;
using ZRWS = ZumpaReader.WebService;

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

        [TestMethod]
        public void TestZumpaItemResultParsing()
        {
            string json = JsonResources.ZumpaItemsResult;
            ZRWS.WebService.ContextResult<ZumpaItemsResult> result = ZRWS.WebService.Parse<ZumpaItemsResult>(json);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasError);
            
            ZumpaItemsResult zi = result.Context;

            Assert.IsNotNull(zi);
            Assert.AreEqual("www.prev.com", zi.PreviousPage);
            Assert.AreEqual("www.next.com", zi.NextPage);
            Assert.AreEqual(1, zi.Items.Count);
            Assert.IsNotNull(zi.Items[0]);
        }

        [TestMethod]
        public void TestZumpaSubItemParsing()
        {
            string json = JsonResources.ZumpaSubItemWithUrls;
            ZRWS.WebService.ContextResult<ZumpaSubItem> result = ZRWS.WebService.Parse<ZumpaSubItem>(json);
            Assert.IsNotNull(result);            

            ZumpaSubItem zi = result.Context;

            Assert.IsNotNull(zi);
            Assert.IsNull(zi.Survey);
            Assert.AreEqual("old.a", zi.AuthorReal);
            Assert.AreEqual("http://www.epubbud.com/book.php?g=N8YACER3", zi.Body);
            Assert.AreEqual(1381270016000L, zi.Time);
            Assert.AreEqual(true, zi.HasRespondForYou);
            Assert.AreEqual(true, zi.HasInsideUris);
            Assert.IsNotNull(zi.InsideUris);
            Assert.AreEqual(1, zi.InsideUris.Count);
            Assert.AreEqual("http://www.epubbud.com/book.php?g=N8YACER3", zi.InsideUris[0]);
        }

        [TestMethod]
        public void TestZumpaSubItemParsingWithSurvey()
        {
            string json = JsonResources.ZumpaSubItemWithSurvey;
            ZRWS.WebService.ContextResult<ZumpaSubItem> result = ZRWS.WebService.Parse<ZumpaSubItem>(json);
            Assert.IsNotNull(result);            

            ZumpaSubItem zi = result.Context;

            Assert.IsNotNull(zi);            
            Assert.IsNotNull(zi.Survey);

            Survey s = zi.Survey;            
            Assert.AreEqual("Mno", s.Question);
            Assert.AreEqual(14, s.Responds);
            Assert.AreEqual(3939, s.ID);
            Assert.AreEqual(4, s.Answers.Count);
            Assert.AreEqual("WP < 7.5", s.Answers[0]);
            Assert.AreEqual("WP 7.5", s.Answers[1]);
            Assert.AreEqual("WP 8", s.Answers[2]);
            Assert.AreEqual("chystam se koupit do konce roku...", s.Answers[3]);
            Assert.AreEqual(4, s.Percents.Count);
            Assert.AreEqual(7, s.Percents[0]);
            Assert.AreEqual(7, s.Percents[1]);
            Assert.AreEqual(7, s.Percents[2]);
            Assert.AreEqual(79, s.Percents[3]);
            Assert.AreEqual(-1, s.VotedItem);
        }
    }
}
