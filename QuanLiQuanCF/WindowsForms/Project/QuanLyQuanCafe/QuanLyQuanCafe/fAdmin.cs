using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();


        public Account currentLoginAccount;

        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region Methods

        // Load main 
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadListCategory();
            LoadListTable();
            LoadAccount();
            LoadCategoryIntoComboBox(cbFoodCategory);
           // LoadTableIntoComboBox(cbTableStatus);

            AddFoodBinding();
            AddAccountBinding();
            AddFoodCategoryBinDing();
            AddTableBinDing();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        { }
        private void fAdmin_Load_1(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        // load list account
        void LoadAccountList()
        {
            string query = "EXEC USP_GetAccountByUserName @userName";

            dtgvAccount.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] { "staff" });
            
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        // Add acount 
        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void AddAccount(string userName, string displayName, int Type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, Type))
            {
                MessageBox.Show("Thêm tài khoản thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!\n\nVui lòng thử lại!", "Thông báo");
            }

            LoadAccount();
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            AddAccount(txbUserName.Text, txbDisplayName.Text, (int)nmAccountType.Value);
        }

        // Edit account 
        void EditAccount(string userName, string displayName, int Type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, Type))
            {
                MessageBox.Show("Sửa tài khoản thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại!\n\nVui lòng thử lại!", "Thông báo");
            }

            LoadAccount();
        }
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            EditAccount(txbUserName.Text, txbDisplayName.Text, (int)nmAccountType.Value);
        }

        // Delete account 
        void DeleteAccount(string userName)
        {
            if (currentLoginAccount.UserName != userName)
            {
                if (AccountDAO.Instance.DeleteAccount(userName))
                {
                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại!\n\nVui lòng thử lại!", "Thông báo");
            }

            LoadAccount();
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            DeleteAccount(txbUserName.Text);
        }

        // Reset Account 
        void ResetAccountPassword(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại!\n\nVui lòng thử lại!", "Thông báo");
            }
        }
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            ResetAccountPassword(txbUserName.Text);
        }

        // load statistical datetime 
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

            // event view bill 
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetAmountBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
            {
                lastPage++;
            }

            txbBillPageNumber.Text = lastPage.ToString();
        }

        private void txbBillPageNumber_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbBillPageNumber.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbBillPageNumber.Text);

            if (page > 1)
            {
                page--;
            }

            txbBillPageNumber.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbBillPageNumber.Text);
            int totalPages = (BillDAO.Instance.GetAmountBillByDate(dtpkFromDate.Value, dtpkToDate.Value)) / 8 + 1;

            if (page > 0 && page < totalPages)
            {
                page++;
            }

            txbBillPageNumber.Text = page.ToString();
        }
        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbBillPageNumber.Text = "1";
        }

        // load list food
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
            // Add information in button
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
            // Load combobox in food
        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

            // Add food
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công!", "Thông báo");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn!\n\nVui lòng thử lại!", "Thông báo");
            }
        }
        private event EventHandler insertFood;

        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

            // Edit food
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int idFood = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(name, categoryID, price, idFood))
            {
                MessageBox.Show("Sửa món thành công!", "Thông báo");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món!\n\nVui lòng thử lại!", "Thông báo");
            }
        }
        private event EventHandler updateFood;

        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

            // Delete food 
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show("Xóa món thành công!", "Thông báo");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món!\n\nVui lòng thử lại!", "Thông báo");
            }
        }
        private event EventHandler deleteFood;

        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

         
        
            // Search food
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }

        private void txbSearchFoodName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch
            { }

        }

        // View food 
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        // Load list category
        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        // Add infomation category 
        void AddFoodCategoryBinDing()
        {
            txtDM.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
           
        }

            // Add category 
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtDM.Text;

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm loại thành công!", "Thông báo");
                LoadListCategory();
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm loại!\n\nVui lòng thử lại!", "Thông báo");
            }
        }
        private event EventHandler insertCategory;

        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        // delete category 
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int idCategory = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.DeleteFoodCategory(idCategory))
            {
                MessageBox.Show("Xóa món thành công!", "Thông báo");
                LoadListCategory();
                if (deleteFoodCategory != null)
                {
                    deleteFoodCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món!\n\nVui lòng thử lại!", "Thông báo");
            }
        }
        private event EventHandler deleteFoodCategory;

        public event EventHandler DeleteFoodCategory
        {
            add { deleteFoodCategory += value; }
            remove { deleteFoodCategory -= value; }
        }

        // View category
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        // Load list table 
        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
        }


        // Load combobox table
        void LoadTableIntoComboBox(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Stat";

        }

        // Load infomation table
        void AddTableBinDing()
        {
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtIDTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }

        // add 
        private void btnAddTable_Click(object sender, EventArgs e)
        {
        //    string name = txbTableName.Text;
        //    string status = cbTableStatus.Text; 

        //    if (TableDAO.Instance.InsertTable(name, status))
        //    {
        //        MessageBox.Show("Thêm loại thành công!", "Thông báo");
        //        LoadListTable();
        //        if (insertTable != null)
        //        {
        //            insertTable(this, new EventArgs());
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Có lỗi khi thêm loại!\n\nVui lòng thử lại!", "Thông báo");
        //    }
        //}
        //private event EventHandler insertTable;

        //public event EventHandler InsertTable
        //{
        //    add { insertTable += value; }
        //    remove { insertTable -= value; }
        }


        private void label1_Click(object sender, EventArgs e)
        { }

        private void tpFoodCategory_Click(object sender, EventArgs e)
        { 
        
        }

        #endregion

       
    }
}
