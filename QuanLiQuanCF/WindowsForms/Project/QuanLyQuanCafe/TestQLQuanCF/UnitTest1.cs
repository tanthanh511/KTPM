
using TestLib;

namespace TestQLQuanCF
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogin()
        {
            var testDatas = new TestData[]
            {
                new TestData("htt", "admin", true),
                new TestData ("staff", "staff", true),

            };


            TestAll log = new TestAll();
            foreach (var item in testDatas)
            {
                var res = log.Login(item.user, item.password);
                Assert.AreEqual(item.result, res);
            }
        }

        [TestMethod]
        public void TestLoadCategory()
        {
            TestAll t = new TestAll();
            var res = t.LoadCategory();
            Assert.AreEqual(true, res);
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