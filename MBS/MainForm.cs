using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Args.testConnection();
            this.Hide();
            Login login = new Login();
            login.ShowDialog();

            try
            {
                this.Show();
                App.formatDataGridView(dataGridView1);
                App.DoubleBuffered(dataGridView1, true);
                App.loadTable(dataGridView1, "SELECT * FROM barang WHERE Kelompok = 'PIGEON'");

                this.ActiveControl = textBox1;

            }
            catch (Exception)
            {
                MessageBox.Show("Password Salah");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                App.loadTable(dataGridView1, "SELECT * FROM barang WHERE NamaBarang LIKE '%" + textBox1.Text + "%'");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Barang barang = new Barang();
            barang.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserLoginForm userlogin = new UserLoginForm("Penjualan");
            userlogin.Show();
        }
    }
}
