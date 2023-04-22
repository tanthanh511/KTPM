using TestLib.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLib.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new FoodDAO();
                return instance;
            }
            private set => instance = value;
        }

        private FoodDAO()
        { }

        public List<Food> GetFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food>();
            string query = "SELECT * FROM dbo.Food WHERE IDCategory = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }

        //public DataTable GetListFood()
        //{
        //    return DataProvider.Instance.ExecuteQuery("SELECT ID AS [Mã số món], Name AS [Tên món], IDCategory AS [Mã số loại], Price AS [Giá tiền] FROM dbo.Food");
        //}

        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            string query = "SELECT * FROM dbo.Food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT INTO dbo.Food(Name, IDCategory, Price) VALUES(N'{0}', {1}, {2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateFood(string name, int id, float price, int idFood)
        {
            string query = string.Format("UPDATE dbo.Food SET Name = N'{0}', IDCategory = {1}, Price = {2} WHERE ID = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillInforDAO.Instance.DeleteBillInforByFoodID(idFood);

            string query = $"DELETE dbo.Food WHERE ID = {idFood}";
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public void DeleteFoodByCategoryID(int id)
        {
            DataProvider.Instance.ExecuteNonQuery($"DELETE dbo.FoodCategory WHERE IDCategory = {id}");
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();
            string query = $"SELECT * FROM dbo.Food WHERE dbo.USF_ConvertToUnsign(Name) LIKE '%' + dbo.USF_ConvertToUnsign(N'{name}') + '%'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
    }
}
