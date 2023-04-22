using TestLib.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLib.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new TableDAO();
                return TableDAO.instance;
            }
            private set => TableDAO.instance = value;
        }

        public static int TableWidth = 100;
        public static int TableHeight = 100;

        private TableDAO()
        { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        // show table in combobox 
        public List<Table> GetListTable()
        {
            List<Table> tableList = new List<Table>();
            string query = "SELECT * FROM dbo.TableFood";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }

        // add 
        public bool InsertTable(string name, string status)
        {
            string query = string.Format("INSERT INTO dbo.TableFood(Name, Stat) VALUES(N'{0}', {1})", name, status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }


        public void SwitchTable(int id1, int id2)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTabel1 , @idTable2", new object[] { id1, id2 });
        }
    }
}
