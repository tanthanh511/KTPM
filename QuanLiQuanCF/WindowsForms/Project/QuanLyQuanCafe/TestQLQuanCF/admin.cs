
using TestLib;

namespace TestQLQuanCF
{
    [TestClass]
    public class admin
    {
      
        // hàm add lỗi bởi vì đã thêm vào csdl rồi test lại sẽ bị trùng dữ liệu
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
        public void TestUpdateAccount()
        {
            var testDatas = new TestAddAccount[]
            {
                new TestAddAccount("dnt", "dntx",1, true),
                new TestAddAccount ("ntk", "ntkkk",0, true),

            };

            TestAll log = new TestAll();
            foreach (var item in testDatas)
            {
                var res = log.UpdateAccount(item.userName, item.displayName, item.Type);
                Assert.AreEqual(item.result, res);
            }
        }


        [TestMethod]
        public void TestDeleteAccount()
        {
            var testDatas = new TestDeleteAccount[]
            {
                new TestDeleteAccount("dnt", true),
                

            };

            TestAll log = new TestAll();
            foreach (var item in testDatas)
            {
                var res = log.DeleteAccount(item.userName);
                Assert.AreEqual(item.result, res);
            }
        }


    }
}