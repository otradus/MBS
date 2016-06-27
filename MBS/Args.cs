using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace MBS
{
    class Args
    {
        public static bool admin = Convert.ToBoolean(Environment.GetCommandLineArgs()[1].ToString());
        public static string printer = Environment.GetCommandLineArgs()[2].ToString();
        public static bool local = true;

        public static string[] getSQLiteSettings(bool local1)
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

            string[] setting = new string[4];

            while (reader.Read())
            {
                setting[0] = reader["user"].ToString();
                setting[1] = reader["password"].ToString();
                setting[2] = reader["host"].ToString();
                setting[3] = reader["database"].ToString();
            }
            conn.Close();

            return setting;
        }

        public static void testConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(App.getConnectionString());
                conn.Open();
                conn.Close();
            }
            catch (Exception)
            {

                Args.local = false;

            }

        }


    }
}
