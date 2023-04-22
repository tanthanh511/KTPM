using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
            private set => instance = value;
        }

        private AccountDAO()
        { }

        public bool Login(string userName, string password)
        {
            // SQL Injection
            // ' OR 1 = 1 --
            //string query = "SELECT * FROM dbo.Account WHERE UserName = N'" + userName + "' AND Password = N'" + password + "'";

            /*byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }*/

            // Lật ngược chuỗi
            //var list = hasData.ToString();
            //list.Reverse();

            string query = "EXEC USP_Login @userName , @password"; // Có dấu cách giữa các tham số

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {userName, password});

            if (result.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Account WHERE UserName = '" + userName + "'");
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public bool UpdateAccount(string userName, string displayName, string password, string newPassword)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @UserName , @DisplayName , @Password , @newPassword", new object[] { userName, displayName, password, newPassword });
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT UserName, DisplayName, Type FROM dbo.Account");
        }

        public bool InsertAccount(string userName, string displayName, int Type)
        {
            string query = $"INSERT INTO dbo.Account (UserName, DisplayName, Type, Password) VALUES (N'{userName}',	N'{displayName}', {Type}, '20720532132149213101239102231223249249135100218')";
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateAccount(string userName, string displayName, int Type)
        {
            string query = $"UPDATE dbo.Account SET UserName = N'{userName}', DisplayName = N'{displayName}', Type = {Type} WHERE UserName = N'{userName}'";

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string userName)
        {
            Account user = GetAccountByUserName(userName);

            string query = $"DELETE dbo.Account WHERE UserName = N'{userName}' AND DisplayName = N'{user.DisplayName}'";

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string userName)
        {
            return DataProvider.Instance.ExecuteNonQuery($"UPDATE dbo.Account SET Password = N'20720532132149213101239102231223249249135100218' WHERE UserName = N'{userName}'") > 0;
        }
    }

    internal class quanlicf
    {
    }
}
