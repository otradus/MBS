using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MBS
{
    public partial class UserLoginForm : Form
    {
        public static string destination;
        public UserLoginForm(string destination1)
        {
            destination = destination1;
            InitializeComponent();
        }

        private void UserLoginForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (e.KeyChar == (char)13 || e.KeyChar == (char)3)
            //{
            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Name FROM users WHERE ID = '" + textBox1.Text + "'", conn);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    label1.Text = Convert.ToString(result);

                    if (destination == "Penjualan")
                    {
                        Penjualan penjualan = new Penjualan(Convert.ToString(result));
                        penjualan.ShowDialog();
                        this.Close();
                    }
                    //else if (destination == "Pembelian")
                    //{
                    //    PembelianForm pembelian = new PembelianForm(Convert.ToString(result));
                    //    pembelian.ShowDialog();
                    //    this.Close();
                    //}
                }
                else
                {
                    label1.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //}
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
