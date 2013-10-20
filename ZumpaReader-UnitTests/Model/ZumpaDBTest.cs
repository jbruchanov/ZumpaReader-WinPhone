using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZumpaReader;
using ZumpaReader.Model;

namespace ZumpaReader_UnitTests.Model
{
    [TestClass]
    public class ZumpaDBTest : TestCase
    {
        [TestMethod]
        public void TestUpdate()
        {
            ImageRecord ir;
            using (ZumpaDB db = new ZumpaDB())
            {
                ir = new ImageRecord { File = "File", Link = "Link", Size = 12345, IsValid = true };
                db.Images.InsertOnSubmit(ir);
                db.SubmitChanges();
            }

            using (ZumpaDB db = new ZumpaDB())
            {
                ir = db.Images.Where(e => "File".Equals(e.File)).SingleOrDefault();
                Assert.IsNotNull(ir);
                ir.IsValid = false;                
                db.SubmitChanges();
            }

            using (ZumpaDB db = new ZumpaDB())
            {
                ir = db.Images.Where(e => "File".Equals(e.File)).FirstOrDefault();
                Assert.IsFalse(ir.IsValid);
                db.Images.DeleteOnSubmit(ir);
                db.SubmitChanges();
            }
        }
    }
}
