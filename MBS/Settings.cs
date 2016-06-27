using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

            string sql2 = "CREATE TABLE IF NOT EXISTS connection(id int, user varchar(10), password varchar(30), host varchar(30), database varchar(10))";
            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            command2.ExecuteNonQuery();

            string sql3 = "SELECT * FROM connection";
            SQLiteCommand command3 = new SQLiteCommand(sql3, conn);
            SQLiteDataReader reader = command3.ExecuteReader();

            try
            {
                reader.Read();
                textBox1.Text = reader["user"].ToString();
                textBox2.Text = reader["password"].ToString();
                textBox3.Text = reader["host"].ToString();
                textBox4.Text = reader["database"].ToString();

                reader.Read();
                textBox8.Text = reader["user"].ToString();
                textBox7.Text = reader["password"].ToString();
                textBox6.Text = reader["host"].ToString();
                textBox5.Text = reader["database"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn;
            conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();

            try
            {
                SQLiteCommand delete = new SQLiteCommand("DELETE FROM connection WHERE id = '1'", conn);
                delete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            string sql = "INSERT INTO connection VALUES ('1', '"+textBox1.Text+"','"+textBox2.Text+"','"+textBox3.Text+"','"+textBox4.Text+"')";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Connection settings saved.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn;
            conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();
            try
            {
                SQLiteCommand delete = new SQLiteCommand("DELETE FROM connection where id = '2'", conn);
                delete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

            string sql = "INSERT INTO connection VALUES ('2', '" + textBox8.Text + "','" + textBox7.Text + "','" + textBox6.Text + "','" + textBox5.Text + "')";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Connection2 settings saved.");

        }


        public string sendit(string ReciverMail)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("email@gmail.com");
            msg.To.Add(ReciverMail);
            msg.Subject = "Hello world! " + DateTime.Now.ToString();
            msg.Body = "hi to you ... :)";
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("email@gmail.com", "password");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return "Mail has been successfully sent!";
            }
            catch (Exception ex)
            {
                return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendit("dsudarto@gmail.com");
    }
    }
}
