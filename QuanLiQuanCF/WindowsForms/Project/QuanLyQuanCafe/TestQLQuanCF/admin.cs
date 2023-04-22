
using TestLib;

namespace TestQLQuanCF
{
    [TestClass]
    public class admin
    {
      

        [TestMethod]
        public void TestLoadAccount()
        {
            TestAll t = new TestAll();
            var res = t.LoadAccount();
            Assert.AreEqual(true, res);
        }

        [TestMethod]
        public void TestAddAccount()
        {
            var testDatas = new TestAddAccount[]
            {
                new TestAddAccount("dnt", "dntt",1, true),
                new TestAddAccount ("ntk", "ntkk",0, true),

            };

            TestAll log = new TestAll();
            foreach (var item in testDatas)
            {
                var res = log.AddAccount(item.userName, item.displayName,item.Type);
                Assert.AreEqual(item.result, res);
            }
        }

        [TestMethod]
        public void TestLoadFoodByCategoryID()
        {
            var testDatas = new TestDataFood[]
            {
                new TestDataFood(1, true),
                new TestDataFood (2,true),

            };

            TestAll t = new TestAll();
            foreach (var item in testDatas)
            {
                var res = t.LoadFoodListByCategoryID(item.id);
                Assert.AreEqual(item.result, res);
            }
        }


        [TestMethod]
        public void TestLoadTable()
        {
            TestAll t = new TestAll();
            var res = t.LoadTable();
            Assert.AreEqual(true, res);
        }

        [TestMethod]
        public void ShowBill()
        {
            var testDatas = new TestDataFood[]
            {
                new TestDataFood(1, true),
                new TestDataFood (2,true),

            };

            TestAll t = new TestAll();
            foreach (var item in testDatas)
            {
                var res = t.ShowBill(item.id);
                Assert.AreEqual(item.result, res);
            }
        }

        [TestMethod]
        public void LoadComboBoxTable()
        {
            TestAll t = new TestAll();
            var res = t.LoadComboBoxTable();
            Assert.AreEqual(true, res);
        }

     
    }
}