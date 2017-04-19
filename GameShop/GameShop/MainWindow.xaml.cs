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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace GameShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeeMethods em = new EmployeeMethods();
        List<Employee> employeeList = new List<Employee>();

        GameMethods gm = new GameMethods();
        List<Game> gameList = new List<Game>();

        Game selectedGame = null;
        public MainWindow()
        {
            InitializeComponent();

            FillEmployeeComboBox();//Fills the employee combobox with all the employees in the database

            FillGameComboBox();//Fills the game combobox with all the games in the database

            FillGameListBox();//Fills the game listbox with all the games in the database

            FindCheapestGame();//Finds the cheapest game and displays it in a label

            FindBestEmployee();//Finds the employee with the highest bonus and displays them
        }



        public void FillGameComboBox()
        {
            gameList = gm.FillGameList();//Fills gameList using a method in GameMethods.cs
            cboSelectGame.ItemsSource = gameList;//Fills the combobox with the list that contains the game objects

            if (selectedGame != null) { cboSelectGame.SelectedItem = selectedGame; }
            
        }

        public void FillEmployeeComboBox()
        {
            employeeList = em.FillEmployeeList();//Fills the employeeList using a method in EmployeeMethods.cs
            cboEmployee.ItemsSource = employeeList;//Fills the combobox with the list that contains the employee objects
        }

        public void FillGameListBox()
        {
            lboGames.Items.Clear();
            foreach (Game game in gameList)
            {
                lboGames.Items.Add(game.id + " - " + game.title + " - " + game.price + "kr. - " + game.stock);
            }
        }

        private void FindBestEmployee()
        {
            FillEmployeeComboBox();
            Employee bestEmployee = new Employee();
            double highestBonus = 0;
            foreach (Employee em in employeeList)
            {
                if (em.bonus > highestBonus)
                {
                    bestEmployee = em;
                    highestBonus = em.bonus;
                }
            }

            lblBestEmployee.Content = bestEmployee.id + " - " + bestEmployee.firstName + " - " + bestEmployee.bonus;
        }

        private void FindCheapestGame()
        {
            double price;
            price = double.MaxValue;

            Game cheapestGame = new Game();

            foreach (Game game in gameList)
            {
                if (game.price < price)
                {
                    cheapestGame = game;
                    price = game.price;
                }
            }

            lblCheapestGame.Content = cheapestGame.title + " - " + cheapestGame.price + "kr.";
        }

        private void btnGamesOptions_Click(object sender, RoutedEventArgs e)
        {
            GamesOptionsWindow gameWindow = new GamesOptionsWindow();
            gameWindow.Show();
        }

        private void btnEmployeeOptions_Click(object sender, RoutedEventArgs e)
        {
            EmployeeOptionsWindow employeeWindow = new EmployeeOptionsWindow();
            employeeWindow.Show();
        }

        private void cboEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee emp = (Employee)cboEmployee.SelectedItem;

        }

        private void cboSelectGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGame = (Game)cboSelectGame.SelectedItem;
            if (selectedGame != null)
            {
                lblAmountInStock.Content = selectedGame.stock;
            }
        }


        private void btnShowGames_Click(object sender, RoutedEventArgs e)
        {
            FillGameListBox();//Fills the game listbox with all the games in the database
        }

        private void btnShowOutOfStockGames_Click(object sender, RoutedEventArgs e)
        {
            FindGamesOutOfStock();
        }

        private void FindGamesOutOfStock()
        {
            lboGames.Items.Clear();
            foreach (Game game in gameList)
            {
                if (game.stock == 0)
                {
                    lboGames.Items.Add(game);
                }
            }
        }

        private void btnRestock_Click(object sender, RoutedEventArgs e)
        {
            RestockSoldOutGames();
        }

        private void RestockSoldOutGames()
        {
            SqlCommand myCommand = new SqlCommand();
            SqlConnection myConnection = new SqlConnection(@"Data Source = CV-PC-T-41\SQLEXPRESS; Initial Catalog = GameShop; Integrated Security = True");//Connection string for the database

            string sql = "UPDATE games SET stock = 5 WHERE stock = 0";
            myCommand = new SqlCommand(sql, myConnection);

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                MessageBox.Show("Success!");
            }
            catch (Exception)
            {
                MessageBox.Show("Could not restock");
            }


            lboGames.Items.Clear();

            gameList = gm.FillGameList();
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            Sell();
        }

        private void Sell()
        {
            SqlCommand gameCommand = new SqlCommand();
            SqlCommand employeeCommand = new SqlCommand();
            SqlConnection myConnection = new SqlConnection(@"Data Source = CV-PC-T-41\SQLEXPRESS; Initial Catalog = GameShop; Integrated Security = True");//Connection string for the database

            string gameSql = "UPDATE games SET stock = @stock WHERE id = @id";// sql string, that subtracts 1 from the selected games stock, when sold
            gameCommand = new SqlCommand(gameSql, myConnection);

            string employeeSql = "UPDATE employee SET bonus = @bonus WHERE id = @id";// sql string, that adds a bonus of 5% of the selected game's price, to the selected employee
            employeeCommand = new SqlCommand(employeeSql, myConnection);

            int gameStock = 0; //assisting variable to help keep track of the new stock value, after a game has been sold
            double employeeBonus = 0; //assisting variable to help keep track of the new bonus value, after a game has been sold

            Employee selectedEmployee = (Employee)cboEmployee.SelectedItem; //Sets selectedEmployee to the selected employee in the employee combobox
            Game selectedGame = (Game)cboSelectGame.SelectedItem; //Sets selectedGame to the selected Game in the Game combobox

            if (cboEmployee.SelectedItem == null)// If no employee is selected, show error message.
            {
                MessageBox.Show("You have to select an employee from the drop down menu");
            }
            else if (cboSelectGame.SelectedItem == null)// if no game is selected, show error message
            {
                MessageBox.Show("You have to select a game form the drop down menu");
            }
            else if (selectedGame.stock == 0)// if the selcted game's stock is 0 (sold out) show error message
            {
                MessageBox.Show("The game is sold out");
            }
            else
            {
                gameStock = selectedGame.stock - 1;
                employeeBonus = selectedEmployee.bonus + 0.05 * selectedGame.price;

                gameCommand.Parameters.AddWithValue("@stock", gameStock);
                gameCommand.Parameters.AddWithValue("@id", selectedGame.id);

                employeeCommand.Parameters.AddWithValue("@bonus", employeeBonus);
                employeeCommand.Parameters.AddWithValue("@id", selectedEmployee.id);

                try
                {
                    myConnection.Open();
                    gameCommand.ExecuteNonQuery();
                    employeeCommand.ExecuteNonQuery();
                    myConnection.Close();
                    MessageBox.Show("Sold!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Something went wrong, could not sell the game");
                }

                gameList = gm.FillGameList(); //Updates the game list
                FillGameListBox(); //Updates the list of all the games with it's info

                FindBestEmployee();

                lblAmountInStock.Content = "";

                FillGameComboBox();
            }
        }
    }
}
