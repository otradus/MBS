using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
            //cekSqlite();
            Args.getSQLiteSettings(Args.testConnection());
            this.Hide();
            Login login = new Login();
            login.ShowDialog();

            try
            {
                this.Show();
                App.formatDataGridView(dataGridView1);
                App.DoubleBuffered(dataGridView1, true);
                App.loadTable(dataGridView1, "SELECT KodeBarang, NamaBarang, Kelompok, Satuan, HargaJual, Jumlah, Gudang FROM barang WHERE Kelompok = 'PIGEON'");

                this.ActiveControl = textBox1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                App.loadTable(dataGridView1, "SELECT KodeBarang, NamaBarang, Kelompok, Satuan, HargaJual, Jumlah, Gudang FROM barang WHERE NamaBarang LIKE '%" + textBox1.Text + "%'");
                textBox1.Text = "";
            }

            if (e.KeyCode == Keys.F8)
            {
                if (Args.admin == true)
                {
                    Laporan laporan = new Laporan();
                    laporan.ShowDialog();
                }
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

        private void button7_Click(object sender, EventArgs e)
        {
            CetakUlangForm cetakulang = new CetakUlangForm();
            cetakulang.ShowDialog();
        }
        
        public void cekSqlite()
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

            string sql4 = "CREATE TABLE IF NOT EXISTS etc(enableadmin integer, poledisplay integer, printer varchar(50), printerbarcode varchar(50))";
            SQLiteCommand command4 = new SQLiteCommand(sql4, conn);
            command4.ExecuteNonQuery();

            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PembelianForm pembelian = new PembelianForm();
            pembelian.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GudangForm gudang = new GudangForm();
            gudang.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RevisiForm revisi = new RevisiForm();
            revisi.ShowDialog();
        }
    }
}
