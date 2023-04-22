using TestLib.DAO;

namespace TestLib
{
    public class TestAll
    {
        public bool Login(string userName, string password)
        {
            return AccountDAO.Instance.Login(userName, password);
        }

        public bool LoadCategory()
        {
            return CategoryDAO.Instance.GetListCategory().Count() > 0;
        }

        public bool LoadFoodListByCategoryID(int id)
        {
            return FoodDAO.Instance.GetFoodByCategoryID(id).Count() > 0;
        }
        public bool LoadTable()
        {
            return TableDAO.Instance.LoadTableList().Count() > 0;
        }


        public bool ShowBill(int id)
        {
            return MenuDAO.Instance.GetListMenuByTable(id).Count() > 0;
        }

        public bool LoadComboBoxTable()
        {
            return TableDAO.Instance.LoadTableList().Count() > 0;
        }
    }
}