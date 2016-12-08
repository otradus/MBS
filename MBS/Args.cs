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
        public static bool admin;
        public static bool poledisplay;
        public static string printer;
        public static string printerbarcode;
        public static bool jatuhtemporeminder;

        public static string emailusername, emailpassword, emailrecipient;

        public static string username, password, host, database;

        public static void getSQLiteSettings(bool local1)
        {
            try
            {
                admin = getEnableAdmin();
                poledisplay = getPoleDisplay();
                printer = getPrinter();
                printerbarcode = getPrinterBarcode();
                jatuhtemporeminder = getJatuhTempoReminder();
                getEmailSettings();

                getMySQLConnection(local1);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show("SQLite Database Not Found. Please create database first.");
                Settings settings = new Settings();
                settings.ShowDialog();
               
            }
        }

        public static void getMySQLConnection(bool local1)
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
            reader.Close();
            conn.Close();

        }

        public static bool getEnableAdmin()
        {
            bool result = false;

            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
                conn.Open();

                string sql = "SELECT enableadmin FROM etc";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["enableadmin"].ToString() == "1")
                    {
                        result = true;
                    }
                }

                reader.Close();
                conn.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return result;

        }

        public static bool getPoleDisplay()
        {
            bool result = false;
            SQLiteConnection conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            string sql = "SELECT poledisplay FROM etc";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["poledisplay"].ToString() == "1")
                {
                    result = true;
                }
            }

            reader.Close();
            conn.Close();

            return result;
        }

        public static bool getJatuhTempoReminder()
        {
            bool result = false;
            SQLiteConnection conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            string sql = "SELECT jatuhtemporeminder FROM etc";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["jatuhtemporeminder"].ToString() == "1")
                {
                    result = true;
                }
            }

            reader.Close();
            conn.Close();

            return result;
        }

        public static string getPrinter()
        {
            string result = "";
            SQLiteConnection conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            string sql = "SELECT printer FROM etc";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                result = reader["printer"].ToString();
            }

            reader.Close();
            conn.Close();

            return result;
        }

        public static string getPrinterBarcode()
        {
            string result = "";
            SQLiteConnection conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            string sql = "SELECT printerbarcode FROM etc";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                result = reader["printerbarcode"].ToString();
            }

            reader.Close();
            conn.Close();

            return result;
        }

        public static void getEmailSettings()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            string sql = "SELECT * FROM email";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                emailusername = Encryption.Decrypt(reader["username"].ToString(),"123");
                emailpassword = Encryption.Decrypt(reader["password"].ToString(),"123");
                emailrecipient = Encryption.Decrypt(reader["recipient"].ToString(),"123");
            }

            reader.Close();
            conn.Close();

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Using External Connection String");
                getSQLiteSettings(false);
                return false;
            }

        }


    }
}
