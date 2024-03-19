using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace QuanLyQuanCafe.DTO
{
    public class Employee
    {
        public Employee(int id, string name, string year, string position)
        {
            this.ID = id;
            this.Name = name;
            this.Year = year;
            this.Position = position;
        }
        public Employee(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Year = row["year"].ToString();
            this.Position = row["position"].ToString();
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }// can return
         // 
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string year;
        public string Year
        {
            get { return year;}
            set { year = value; }
        }

        private string position;
        public string Position
        {
           get { return position;} 
           set { position = value; }
        }

        public static object Instance { get; internal set; }
    }
}
