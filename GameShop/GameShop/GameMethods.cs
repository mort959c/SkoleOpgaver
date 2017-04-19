using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace GameShop
{
    class GameMethods
    {
        SqlConnection myConnection = new SqlConnection(@"Data Source = CV-PC-T-41\SQLEXPRESS; Initial Catalog = GameShop; Integrated Security = True");//Connection string for the database
        SqlDataReader myReader = null;//Reads data from the database
        SqlCommand myCommand = null;

        #region FillGameList
        public List<Game> FillGameList()
        {
            List<Game> GameNavigation = new List<Game>();//List will be used to navigate through the games

            string sql = "select g.id, g.price, g.stock, g.title from games g";
            myConnection = new SqlConnection(@"Data Source = CV-PC-T-41\SQLEXPRESS; Initial Catalog = GameShop; Integrated Security = True");
            myCommand = new SqlCommand(sql, myConnection);

            try
            {
                myConnection.Open();//Opens the connection to the database
                myReader = myCommand.ExecuteReader();//Executes the reader

                while (myReader.Read())//creates the list wich will store game objects
                {
                    Game c = new Game();
                    c.id = Convert.ToInt32(myReader["Id"]);
                    c.price = Convert.ToDouble(myReader["Price"]);
                    c.stock = Convert.ToInt32(myReader["Stock"]);
                    c.title = myReader["Title"].ToString();
                  
                    //Adds the game to the gamelist
                    GameNavigation.Add(c);
                }

                myConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            myCommand = null;

            return GameNavigation;
        }
        
        #endregion FillGameList

        #region CreateGame
        public void CreateGame(double price, int stock, string title)
        {
            myCommand = new SqlCommand();
            myCommand.CommandText = "CreateGame";
            myCommand.CommandType = System.Data.CommandType.StoredProcedure;
            myCommand.Connection = myConnection;

            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Stock", stock);
            myCommand.Parameters.AddWithValue("@Title", title);

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
        #endregion CreateGame

        public void AddToStock(int xStock, int currentStock, int gameId)
        {
            myCommand = new SqlCommand();
            myCommand.CommandText = "UpdateGameStock";
            myCommand.CommandType = System.Data.CommandType.StoredProcedure;
            myCommand.Connection = myConnection;

            myCommand.Parameters.AddWithValue("@Stock", currentStock + xStock);
            myCommand.Parameters.AddWithValue("@GameId", gameId);

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
        }
    }
    public class Game
    {
        public int id { get; set; }
        public double price { get; set; }
        public int stock { get; set; }
        public string title { get; set; }

        public override string ToString()
        {
            string str = id + " - " + title + " - " + price + "kr.";
            return str;
        }
    }
}
