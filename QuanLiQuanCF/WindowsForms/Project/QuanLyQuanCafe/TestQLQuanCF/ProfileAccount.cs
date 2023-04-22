
using TestLib;

namespace TestQLQuanCF
{
    [TestClass]
    public class ProfileAccount
    {
        [TestMethod]
        public void TestAccount()
        {
            var testData = new TestDataAccount[]
            {
                new TestDataAccount("dnt", "thang","thangtn","thangtn", true),
               // new TestDataAccount ("ntk", "kiet","kietnt","kietnt", true),

            };

            TestAll log = new TestAll();
            foreach (var item in testData)
            {
                var res = log.UpdateAccount(item.userName, item.displayName, item.password, item.newPassword);
                Assert.AreEqual(item.result, res);
            }
        }

       
    }
}