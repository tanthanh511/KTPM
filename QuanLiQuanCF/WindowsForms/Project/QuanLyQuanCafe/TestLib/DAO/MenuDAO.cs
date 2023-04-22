using TestLib.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLib.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuDAO();
                }
                return instance;
            }
            private set => instance = value;
        }

        private MenuDAO()
        { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT C.Name, B.Count, C.Price, B.Count * C.Price AS [Thành tiền] FROM dbo.Bill A, dbo.BillInfor B, dbo.Food C WHERE A.ID = B.IDBill AND B.IDFood = C.ID AND A.Stat = 0 AND A.IDTable = " + id);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
