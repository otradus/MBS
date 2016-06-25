using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBS
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists("settings.sqlite"))
            {
                SQLiteConnection.CreateFile("settings.sqlite");
            }
            SQLiteConnection conn;
            conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            string sql = "create table if not exists users(username varchar(10), password varchar(10), admin integer)";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            string sql2 = "CREATE TABLE IF NOT EXISTS connection(user varchar(10), password varchar(30), host varchar(30), database varchar(10))";
            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            command2.ExecuteNonQuery();

            string sql3 = "SELECT * FROM connection";
            SQLiteCommand command3 = new SQLiteCommand(sql3, conn);
            SQLiteDataReader reader = command3.ExecuteReader();

            while (reader.Read())
            {
                textBox1.Text = reader["user"].ToString();
                textBox2.Text = reader["password"].ToString();
                textBox3.Text = reader["host"].ToString();
                textBox4.Text = reader["database"].ToString();
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn;
            conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            SQLiteCommand delete = new SQLiteCommand("DELETE FROM connection", conn);
            delete.ExecuteNonQuery();
            
            string sql = "INSERT INTO connection VALUES ('"+textBox1.Text+"','"+textBox2.Text+"','"+textBox3.Text+"','"+textBox4.Text+"')";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Connection settings saved.");
        }
    }
}
