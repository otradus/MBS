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

            //string sql = "create table if not exists users(username varchar(10), password varchar(10), admin integer)";
            //SQLiteCommand command = new SQLiteCommand(sql, conn);
            //command.ExecuteNonQuery();

            string sql2 = "CREATE TABLE IF NOT EXISTS connection(id int, user varchar(10), password varchar(30), host varchar(30), database varchar(10))";
            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            command2.ExecuteNonQuery();

            string sql4 = "CREATE TABLE IF NOT EXISTS etc(enableadmin integer, poledisplay integer, jatuhtemporeminder integer, printer varchar(50), printerbarcode varchar(50))";
            SQLiteCommand command4 = new SQLiteCommand(sql4, conn);
            command4.ExecuteNonQuery();

            string sql_email = "CREATE TABLE IF NOT EXISTS email(username varchar(50), password varchar(50), recipient varchar(50))";
            SQLiteCommand command_email = new SQLiteCommand(sql_email, conn);
            command_email.ExecuteNonQuery();

            string sql3 = "SELECT * FROM connection";
            SQLiteCommand command3 = new SQLiteCommand(sql3, conn);
            SQLiteDataReader reader = command3.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    if (reader["id"].ToString() == "1")
                    {
                        textBox1.Text = reader["user"].ToString();
                        textBox2.Text = reader["password"].ToString();
                        textBox3.Text = reader["host"].ToString();
                        textBox4.Text = reader["database"].ToString();
                    }
                    else
                    {
                        textBox7.Text = reader["password"].ToString();
                        textBox6.Text = reader["host"].ToString();
                        textBox5.Text = reader["database"].ToString();
                        textBox8.Text = reader["user"].ToString();

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            string sql5 = "SELECT * FROM etc";
            SQLiteCommand command5 = new SQLiteCommand(sql5, conn);
            SQLiteDataReader readeretc = command5.ExecuteReader();

            try
            {
                readeretc.Read();
                if (readeretc["enableadmin"].ToString() == "0")
                {
                    checkBox1.Checked = false;
                }else
                {
                    checkBox1.Checked = true;
                }

                if (readeretc["poledisplay"].ToString() == "0")
                {
                    checkBox2.Checked = false;
                }
                else
                {
                    checkBox2.Checked = true;
                }

                if (readeretc["jatuhtemporeminder"].ToString() == "0")
                {
                    checkBox3.Checked = false;
                }
                else
                {
                    checkBox3.Checked = true;
                }

                textBox9.Text = readeretc["printer"].ToString();

                textBox10.Text = readeretc["printerbarcode"].ToString();

                readeretc.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            string sql6 = "SELECT * FROM email";
            SQLiteCommand command6 = new SQLiteCommand(sql6, conn);
            SQLiteDataReader readeremail = command6.ExecuteReader();

            try
            {
                readeremail.Read();
 
                textBox11.Text = Encryption.Decrypt(readeremail["username"].ToString(),"123");
                textBox12.Text = Encryption.Decrypt(readeremail["password"].ToString(), "123");
                textBox13.Text = Encryption.Decrypt(readeremail["recipient"].ToString(), "123");

                readeremail.Close();
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

        private void button4_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn;
            conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();
            try
            {
                SQLiteCommand delete = new SQLiteCommand("DELETE FROM etc", conn);
                delete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            string enableadmin;
            if (checkBox1.Checked == true)
            {
                enableadmin = "1";
            }
            else
            {
                enableadmin = "0";
            }

            string poledisplay;
            if (checkBox2.Checked == true)
            {
                poledisplay = "1";
            }
            else
            {
                poledisplay = "0";
            }

            string jatuhtemporeminder;
            if (checkBox3.Checked == true)
            {
                jatuhtemporeminder = "1";
            }
            else
            {
                jatuhtemporeminder = "0";
            }

            string sql = "INSERT INTO etc VALUES ('"+ enableadmin +"', '"+ poledisplay + "', '" + jatuhtemporeminder + "','" + textBox9.Text +"','" + textBox10.Text + "')";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Etc settings saved.");

        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Args.getSQLiteSettings(Args.testConnection());
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SQLiteConnection conn;
            conn = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            conn.Open();
            try
            {
                SQLiteCommand delete = new SQLiteCommand("DELETE FROM email", conn);
                delete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            string username, password, recipient;
            username = Encryption.Encrypt(textBox11.Text, "123");
            password = Encryption.Encrypt(textBox12.Text, "123");
            recipient = Encryption.Encrypt(textBox13.Text, "123");

            string sql = "INSERT INTO email VALUES ('" + username + "', '" + password + "','" + recipient + "')";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Email settings saved.");
        }
    }
}
