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
    /// Interaction logic for GamesOptionsWindow.xaml
    /// </summary>
    public partial class GamesOptionsWindow : Window
    {
        GameMethods gm = new GameMethods();//Creates object that is used to call methods from GameMethods
        List<Game> GameNavigation = new List<Game>();

        MainWindow main = new MainWindow();

        public GamesOptionsWindow()
        {
            InitializeComponent();
        }


        private void btnShowGames_Click(object sender, RoutedEventArgs e)
        {
            FillGameList();//When showGames i clicked, a list of the games will be created.


            txtGameId.Text = GameNavigation[0].id.ToString();
            txtGamePrice.Text = GameNavigation[0].price.ToString();
            txtGameStock.Text = GameNavigation[0].stock.ToString();
            txtGameTitle.Text = GameNavigation[0].title.ToString();

        }

        private void btnClearGame_Click(object sender, RoutedEventArgs e)
        {
            //Clears the textbox's
            txtGameId.Text = "";
            txtGamePrice.Text = "";
            txtGameStock.Text = "";
            txtGameTitle.Text = "";
        }

        private void btnNextGame_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            bool notLoaded = false;

            try
            {
                index = Convert.ToInt32(txtGameId.Text) - 1; //index counter is gameId - 1. the lowest gameId is 1 and list's starts at 0 therefor -1 (gameId = 1 it's position in the list is 0)
            }
            catch (Exception)
            {
                MessageBox.Show("To be able to navigate through the games you first have to press the *show games button*");
                notLoaded = true;//if notLoaded is true, it means that the navigationList has not yet been loaded. if the list was loaded txtGameId would have a value, therefor it would be possible to subtract from that number.
            }

            if (!notLoaded)
            {
                if (index + 1 >= GameNavigation.Count)//checks if the end of the list is reached
                {
                    MessageBox.Show("No more games");
                }
                else
                {
                    index++;
                    txtGameId.Text = GameNavigation[index].id.ToString();
                    txtGamePrice.Text = GameNavigation[index].price.ToString();
                    txtGameStock.Text = GameNavigation[index].stock.ToString();
                    txtGameTitle.Text = GameNavigation[index].title.ToString();
                }
            }
        }

        private void btnPreviousGame_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            bool notLoaded = false;

            try
            {
                index = Convert.ToInt32(txtGameId.Text) - 1; //index counter is gameId - 1. the lowest gameId is 1 and list's starts at 0 therefor -1 (gameId = 1 it's position in the list is 0)
            }
            catch (Exception)
            {
                MessageBox.Show("To be able to navigate through the games you first have to press the *show games* button");
                notLoaded = true;//if notLoaded is true, it means that the navigationList has not yet been loaded. if the list was loaded txtGameId would have a value, therefor it would be possible to subtract from that number.
            }

            if (!notLoaded)
            {
                if (index <= 0)//checks if the end of the list is reached
                {
                    MessageBox.Show("No more games");
                }
                else
                {
                    index--;
                    txtGameId.Text = GameNavigation[index].id.ToString();
                    txtGamePrice.Text = GameNavigation[index].price.ToString();
                    txtGameStock.Text = GameNavigation[index].stock.ToString();
                    txtGameTitle.Text = GameNavigation[index].title.ToString();
                }
            }
        }

        #region CreateNewGame
        private void btnCreateNewGame_Click(object sender, RoutedEventArgs e)
        {
            string price = txtGamePrice.Text;
            string stock = txtGameStock.Text;
            string title = txtGameTitle.Text;

            foreach (Game game in GameNavigation)
            {
                if (title.Trim() == game.title.Trim())
                {
                    MessageBox.Show("The game alreadt exists");
                }
            }
            if (string.IsNullOrEmpty(price.ToString()) ||
                string.IsNullOrEmpty(stock.ToString()) ||
                string.IsNullOrEmpty(title.ToString()))//Checks if any of data being added to the database is empty
            {
                MessageBox.Show("Empty fields not allowed");
            }
            else if (!Regex.IsMatch(price.ToString(), "^[0-9]") || !Regex.IsMatch(stock.ToString(), "^[0-9]"))//Check if price and stock contains only contains numbers *only number allowed*
            {
                MessageBox.Show("Only numbers are allowed in price and stock");
            }
            else
            {
                try
                {
                    gm.CreateGame(Convert.ToDouble(price), Convert.ToInt32(stock), title);//Calls the CreateGame method with parameters
                    MessageBox.Show("Success!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not create the game");//If somethin goes wrong while creating the game. a message will show up.
                }
            }

            FillGameList();//Updates the list after creating a new game

            main.FillGameComboBox();//Updates the combobox in mainWindow that contains the games
            main.FillGameListBox();//Updates the combox in mainWindow that contains the games

            //Updates the gui to show the game that was just added, and makes it possible to navigate from it
            int index = GameNavigation.Count() - 1;
            txtGameId.Text = GameNavigation[index].id.ToString();
            txtGamePrice.Text = GameNavigation[index].price.ToString();
            txtGameStock.Text = GameNavigation[index].stock.ToString();
            txtGameTitle.Text = GameNavigation[index].title.ToString();
        }
        #endregion CreateNewGame


        /// <summary>
        /// fills a list with all the games wich will be used to navigate between games
        /// </summary>
        public void FillGameList()
        {
            GameNavigation = gm.FillGameList();//Fills GameNavigation with all the games in the database
        }

        private void btnAddToStock_Click(object sender, RoutedEventArgs e)
        {
            string amountToAdd = txtAddToStock.Text;
            string currentStock = txtGameStock.Text;
            string gameId = txtGameId.Text;

            int amount = 0;

            if (string.IsNullOrEmpty(gameId.ToString()))
            {
                MessageBox.Show("You have not selected which game's stock you wish to increase \n you can do this by pressing *Show games* and navigate with *<* *>* to the desired game");
            }
            else if (string.IsNullOrEmpty(amountToAdd.ToString()))
            {
                MessageBox.Show("You have to select the amount of units, you wish to add to the game's stock");
            }
            else if (int.TryParse(amountToAdd, out amount) && amount < 1)
            {
                MessageBox.Show("The amount of units you wish to add has to be greater than 1");
            }
            else if (!Regex.IsMatch(amountToAdd.ToString(), "^[0-9]"))
            {
                MessageBox.Show("The amount of units you wish to add has to be a number");
            }
            else
            {
                try
                {
                    gm.AddToStock(Convert.ToInt32(amountToAdd), Convert.ToInt32(currentStock), Convert.ToInt32(gameId));
                    MessageBox.Show("Success!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not add the amount to the game's stock \n please make sure you haven't changed any field other than the amount of units you wish to add");
                }
            }

            main.FillGameComboBox();//Updates the combobox in mainWindow that contains the games
            main.FillGameListBox();//Updates the listbox in mainWindow that contains the games

            //Calls the FillGameList method. which will then update the gameNavigation. After that the game's stock will be updated
            FillGameList();
            foreach (Game game in GameNavigation)
            {
                if (Convert.ToInt32(gameId) == game.id)
                {
                    txtGameStock.Text = game.stock.ToString();
                    break;
                }
            }
        }
    }
}
