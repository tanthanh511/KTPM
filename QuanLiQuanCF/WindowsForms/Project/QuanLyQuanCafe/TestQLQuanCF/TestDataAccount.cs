using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQLQuanCF
{
    public class TestDataAccount
    {
        public string userName;
        public string displayName;
        public string password;
        public string newPassword;
        public bool result;

        public TestDataAccount(string userName, string displayName, string password, string newPassword, bool result)
        {
            this.userName = userName;
            this.displayName = displayName;
            this.password = password;
            this.newPassword = newPassword;
            this.result = result;
        }
    }

    public class TestAddAccount
    {
        public string userName;
        public string displayName;
        public int Type;
        public bool result;

        public TestAddAccount(string userName, string displayName, int type, bool result)
        {
            this.userName = userName;
            this.displayName = displayName;
            Type = type;
            this.result = result;
        }
    }
}
