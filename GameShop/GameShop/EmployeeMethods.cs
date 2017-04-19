using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace GameShop
{
    class EmployeeMethods
    {
        SqlConnection myConnection = new SqlConnection(@"Data Source = CV-PC-T-41\SQLEXPRESS; Initial Catalog = GameShop; Integrated Security = True");//Connection string for the database
        SqlDataReader myReader = null;//Reads data from the database
        SqlCommand myCommand = new SqlCommand();

        public List<Employee> FillEmployeeList()
        {
            List<Employee> employeeNavigation = new List<Employee>();//List will be used to navigate through the games

            string sql = "select e.id, e.firstName, e.lastName, e.salary, e.bonus from employee e";
            myCommand = new SqlCommand(sql, myConnection);

            try
            {
                myConnection.Open();//Opens the connection to the database
            }
            catch (Exception e)
            {
                throw e;
            }

            myReader = myCommand.ExecuteReader();//Executes the reader

            while (myReader.Read())//creates the content list which will store game objects
            {
                Employee e = new Employee();

                e.bonus = Convert.ToDouble(myReader["Bonus"]);
                e.firstName = myReader["FirstName"].ToString();
                e.id = Convert.ToInt32(myReader["Id"].ToString());
                e.lastName = myReader["LastName"].ToString();
                e.salary = Convert.ToDouble(myReader["Salary"].ToString());
                e.salaryWithBonus = Convert.ToDouble(myReader["Salary"].ToString()) + Convert.ToDouble(myReader["Bonus"].ToString());

                //Adds the game to the gamelist
                employeeNavigation.Add(e);
            }

            myConnection.Close();

            return employeeNavigation;
        }

        

        public void CreateEmployee(string firstName, string lastName, double salary, double bonus)
        {
            myCommand.CommandText = "CreateEmployee";//Name of the stored procedure wished to use
            myCommand.CommandType = System.Data.CommandType.StoredProcedure;//Determines that we want to use a stored procedure
            myCommand.Connection = myConnection;//Gives my command a connection

            myCommand.Parameters.AddWithValue("@firstName", firstName);
            myCommand.Parameters.AddWithValue("@lastName", lastName);
            myCommand.Parameters.AddWithValue("@salary", salary);
            myCommand.Parameters.AddWithValue("@bonus", bonus);

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            myCommand = null;
        }
    }
    public class Employee
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public double salary { get; set; }
        public double bonus { get; set; }
        public double salaryWithBonus { get; set; }

        public override string ToString()
        {
            string str = id + " - " + firstName + " - " + lastName;
            return str;
        }
    }
}
