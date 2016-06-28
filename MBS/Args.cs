using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MBS
{
    class Args
    {
        public static bool admin = Convert.ToBoolean(Environment.GetCommandLineArgs()[1].ToString());
        public static string printer = Environment.GetCommandLineArgs()[2].ToString();

        public static string username, password, host, database;

        public static void getSQLiteSettings(bool local1)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
                conn.Open();

                string id;

                if (local1 == true)
                {
                    id = "1";
                }
                else
                {
                    id = "2";
                }

                string sql = "SELECT * FROM connection WHERE id = '" + id + "'";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    username = reader["user"].ToString();
                    password = reader["password"].ToString();
                    host = reader["host"].ToString();
                    database = reader["database"].ToString();
                }
                conn.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("SQLite Database Not Found. Please create database first.");
                Settings settings = new Settings();
                settings.ShowDialog();
               
            }
        }

        public static bool testConnection()
        {
            try
            {
                getSQLiteSettings(true);
                MySqlConnection conn = new MySqlConnection(App.getConnectionString());
                conn.Open();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                getSQLiteSettings(false);
                return false;
            }

        }


    }
}
