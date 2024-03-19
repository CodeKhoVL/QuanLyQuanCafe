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
    public class EmployeeDAO
    {
        private static EmployeeDAO instance;

        public static EmployeeDAO Instance
        {
            get { if (instance == null) instance = new EmployeeDAO(); return EmployeeDAO.instance; }
            private set { EmployeeDAO.instance = value; }
        }

        private EmployeeDAO() { }

        public List<Employee> GetListEmployee()
        {
            List<Employee> list = new List<Employee>();

            string query = "select * from Employee";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Employee Employee = new Employee(item);
                list.Add(Employee);
            }

            return list;
        }

        /*
        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT dbo.Food ( name, idCategory, price )VALUES  ( N'{0}', {1}, {2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }*/


        public bool InsertEmployee(string Name, string Year, string Position)
        {
            string query = string.Format("INSERT dbo.Employee (name, year, position )VALUES  ( N'{0}', N'{1}', N'{2}')", Name, Year, Position);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public bool UpdateEmployee(int id, string name, string year, string position)
        {
            string query = string.Format("UPDATE dbo.Employee SET name = N'{0}', year = N'{1}', position = N'{2}' WHERE id = {3}", name, year, position, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

       

        public bool DeleteEmployee(int id)
        {
            string query = string.Format("Delete Employee where id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
