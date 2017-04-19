using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace GameShop
{
    /// <summary>
    /// Interaction logic for EmployeeOptionsWindow.xaml
    /// </summary>
    public partial class EmployeeOptionsWindow : Window
    {
        EmployeeMethods em = new EmployeeMethods();
        List<Employee> employeeNavigation = new List<Employee>();

        MainWindow main = new MainWindow();

        public EmployeeOptionsWindow()
        {
            InitializeComponent();
        }

        private void btnShowEmployees_Click(object sender, RoutedEventArgs e)
        {
            FillEmployeeList();
            txtEmployeeNumber.Text = employeeNavigation[0].id.ToString();
            txtEmployeeFirstName.Text = employeeNavigation[0].firstName;
            txtEmployeeLastName.Text = employeeNavigation[0].lastName;
            txtEmployeeSalary.Text = employeeNavigation[0].salary.ToString();
            txtEmployeeBonus.Text = employeeNavigation[0].bonus.ToString();
            txtEmployeeSalaryWithBonus.Text = employeeNavigation[0].salaryWithBonus.ToString();
        }

        private void btnNextEmployee_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            bool notLoaded = false;

            try
            {
                index = Convert.ToInt32(txtEmployeeNumber.Text) - 1; //index counter is gameId - 1. the lowest gameId is 1 and list's starts at 0 therefor -1 (employeeId = 1 it's position in the list is 0)
            }
            catch (Exception)
            {
                MessageBox.Show("To be able to navigate through the employees you first have to press the *show employees* button");
                notLoaded = true;//if notLoaded is true, it means that the navigationList has not yet been loaded. if the list was loaded txtGameId would have a value, therefor it would be possible to subtract from that number.
            }

            if (!notLoaded)
            {
                if (index + 1 >= employeeNavigation.Count)//checks if the end of the list is reached
                {
                    MessageBox.Show("No more employees");
                }
                else
                {
                    index++;
                    txtEmployeeNumber.Text = employeeNavigation[index].id.ToString();
                    txtEmployeeFirstName.Text = employeeNavigation[index].firstName;
                    txtEmployeeLastName.Text = employeeNavigation[index].lastName;
                    txtEmployeeSalary.Text = employeeNavigation[index].salary.ToString();
                    txtEmployeeBonus.Text = employeeNavigation[index].bonus.ToString();
                    txtEmployeeSalaryWithBonus.Text = employeeNavigation[index].salaryWithBonus.ToString();
                }
            }
        }

        private void btnPreviousEmployee_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            bool notLoaded = false;

            try
            {
                index = Convert.ToInt32(txtEmployeeNumber.Text) - 1;  //index counter is employeeNumber - 1. the lowest employeeNumber is 1 and list's starts at 0 therefor -1 (employeeNumber = 1 it's position in the list is 0)
            }
            catch (Exception)
            {
                MessageBox.Show("To be able to navigate through the employees you first have to press the *show employees* button");
                notLoaded = true;//if notLoaded is true, it means that the navigationList has not yet been loaded. if the list was loaded txtGameId would have a value, therefor it would be possible to subtract from that number.
            }
            
            if (!notLoaded)//Checks if there is any elements in the list. if there isn't the user hasn't pressed the "show employee" button--
            {
                if (index <= 0)//checks if the end of the list is reached
                {
                    MessageBox.Show("No more employees");
                }
                else
                {
                    index--;
                    txtEmployeeNumber.Text = employeeNavigation[index].id.ToString();
                    txtEmployeeFirstName.Text = employeeNavigation[index].firstName;
                    txtEmployeeLastName.Text = employeeNavigation[index].lastName;
                    txtEmployeeSalary.Text = employeeNavigation[index].salary.ToString();
                    txtEmployeeBonus.Text = employeeNavigation[index].bonus.ToString();
                    txtEmployeeSalaryWithBonus.Text = employeeNavigation[index].salaryWithBonus.ToString();
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtEmployeeBonus.Text = "";
            txtEmployeeFirstName.Text = "";
            txtEmployeeLastName.Text = "";
            txtEmployeeNumber.Text = "";
            txtEmployeeSalary.Text = "";
            txtEmployeeSalaryWithBonus.Text = "";
        }

        private void btnCreateNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtEmployeeFirstName.Text;
            string lastName = txtEmployeeLastName.Text;
            string salary = txtEmployeeSalary.Text;//Used as a double
            string bonus = txtEmployeeBonus.Text;//Used as a double

            if (string.IsNullOrEmpty(firstName) || //Checks for empty fields. Empty fields is not allowed
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(salary) ||
                string.IsNullOrEmpty(bonus))
            {
                MessageBox.Show("Empty fields is not allowed");
            }
            else if (!Regex.IsMatch(firstName, "^[a-å A-Å]") ||
                     !Regex.IsMatch(lastName, "^[a-å A-Å]")) //Checks if firstName and lastName contains anything beside letters
            {
                MessageBox.Show("First name, and last name can only contain letters");
            }
            else if (!Regex.IsMatch(salary, "^[0-9]") ||
                     !Regex.IsMatch(bonus, "^[0-9]")) //Checks if salary, and bonus contains anything beside numbers
            {
                MessageBox.Show("Salary, and bonus can only contain numbers");
            }
            else
            {
                try
                {
                    em.CreateEmployee(firstName, lastName, Convert.ToDouble(salary), Convert.ToDouble(bonus));
                    MessageBox.Show("Success!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not create the new employee");
                }
            }

            FillEmployeeList();//Updates the employeeNavigation so it contains the employee just added

            main.FillEmployeeComboBox();//Updates the combobox in the mainwindow that contains the employees

            //Updates the gui to show the employee that was just added, and makes it possible to navigate from it
            int index = employeeNavigation.Count() - 1;
            txtEmployeeNumber.Text = employeeNavigation[index].id.ToString();
            txtEmployeeFirstName.Text = employeeNavigation[index].firstName;
            txtEmployeeLastName.Text = employeeNavigation[index].lastName;
            txtEmployeeSalary.Text = employeeNavigation[index].salary.ToString();
            txtEmployeeBonus.Text = employeeNavigation[index].bonus.ToString();
            txtEmployeeSalaryWithBonus.Text = employeeNavigation[index].salaryWithBonus.ToString();
        }

        private void FillEmployeeList()
        {
            employeeNavigation = em.FillEmployeeList();//Fills the list with all the employees in the database
        }
    }
}
