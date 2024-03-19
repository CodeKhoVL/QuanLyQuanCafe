using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();

        BindingSource employeeList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            LoadData();
        }

        #region methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }

       

        
        void LoadData()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvEmployee.DataSource = employeeList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();

            AddEmployeeBiding();
          

            LoadAccount();
            LoadEmployee();
            //
            //LoadListEmployee();

        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        // addbinding
        void AddEmployeeBiding()
        {
            txbEmployeeId.DataBindings.Add(new Binding("Text", dtgvEmployee.DataSource, "id", true, DataSourceUpdateMode.Never));
            txbEmployeeName.DataBindings.Add(new Binding("Text", dtgvEmployee.DataSource, "name", true, DataSourceUpdateMode.Never));
            txbEmployeeYear.DataBindings.Add(new Binding("Text", dtgvEmployee.DataSource, "year", true, DataSourceUpdateMode.Never));
            txbEmployeePosition.DataBindings.Add(new Binding("Text", dtgvEmployee.DataSource, "position", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        
        void LoadEmployee()
        {
            employeeList.DataSource = EmployeeDAO.Instance.GetListEmployee();
        }



        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        #endregion

        #region events

        
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericUpDown1.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericUpDown1.Value;

            EditAccount(userName, displayName, type);
        }


        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            ResetPass(userName);
        }


        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }


        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        #endregion              

        private void btnFristBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txbPageBill.Text = page.ToString();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'QuanLyQuanCafeDataSet2.USP_GetListBillByDateForReport' table. You can move, or remove it, as needed.
            // important this.USP_GetListBillByDateForReportTableAdapter.Fill(this.QuanLyQuanCafeDataSet2.USP_GetListBillByDateForReport, dtpkFromDate.Value, dtpkToDate.Value);           

            // this.rpViewer.RefreshReport();
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txbUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tpTable_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dtgvFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnShowEmployee_Click(object sender, EventArgs e)
        {
            //show
            LoadEmployee();
        }

        void AddEmployee(string Name, string Year, string Position)
        {
            if (EmployeeDAO.Instance.InsertEmployee(Name, Year, Position))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
            LoadEmployee();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            // add
          
         //   int id = Convert.ToInt32(txbEmployeeId.Text);
            string name = txbEmployeeName.Text;
            string year = txbEmployeeYear.Text;  
            string position = txbEmployeePosition.Text;

            AddEmployee(name, year, position); 
        }



        void DeleteEmployee(int id)
        {
            
            if (EmployeeDAO.Instance.DeleteEmployee(id))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
            LoadEmployee(); 

        }
        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            // delete
            int id  = Convert.ToInt32(txbEmployeeId.Text);

            DeleteEmployee(id);
        }


      
        void EditEmployee(int id, string name, string year, string position)
        {
            if (EmployeeDAO.Instance.UpdateEmployee(id, name, year, position))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }
            LoadEmployee();
        }

        private void btnEditEmployee_Click(object sender, EventArgs e)
        {


            // edit
          //  int id = txbEmployeeId.Text
            int id = Convert.ToInt32(txbEmployeeId.Text);
            string name = txbEmployeeName.Text;
            string year = txbEmployeeYear.Text;
            string position = txbEmployeePosition.Text;
            EditEmployee(id, name, year, position);
        }
    }
}
