using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQLQuanCF
{
    public class TestData
    {
        public string user;
        public string password;
        public bool result;

        public TestData(string user, string password, bool res)
        {
            this.user = user;
            this.password = password;
            this.result = res;
        }

    }
}
