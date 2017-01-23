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
                App.loadTable(dataGridView1, "SELECT KodeBarang, NamaBarang, Kelompok, Satuan, HargaJual, Jumlah, Gudang, Opname FROM barang WHERE Kelompok = 'PIGEON'");
                App.fillColumn(dataGridView1, 1);
                colorJumlah();
                //dataGridView1.Columns[1].FillWeight = 60;

                this.ActiveControl = textBox1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (Args.admin == false)
            {
                button3.Enabled = false;
                button4.Enabled = false;
            }

            App.poleDisplay("Maju Baby Shop", DateTime.Now.ToShortDateString());

            if (Args.jatuhtemporeminder == true)
            {
                timer1.Enabled = true;
            }

            this.WindowState = FormWindowState.Maximized;
        }

        private void colorJumlah()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                if (dataGridView1[5, i].Value.ToString() == "0")
                {
                    dataGridView1[5, i].Style.ForeColor = Color.LightGray;
                }

                if (dataGridView1[6, i].Value.ToString() == "0")
                {
                    dataGridView1[6, i].Style.ForeColor = Color.LightGray;
                }

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int barcodecount = Convert.ToInt32(App.executeScalar("SELECT COUNT(*) FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
                if (barcodecount != 0)
                {
                    App.loadTable(dataGridView1, "SELECT KodeBarang, NamaBarang, Kelompok, Satuan, HargaJual, Jumlah, Gudang, Opname FROM barang WHERE KodeBarang = '" + textBox1.Text + "'");
                }
                else
                {
                    App.loadTable(dataGridView1, "SELECT KodeBarang, NamaBarang, Kelompok, Satuan, HargaJual, Jumlah, Gudang, Opname FROM barang WHERE NamaBarang LIKE '%" + textBox1.Text + "%'");
                }

                App.fillColumn(dataGridView1, 1);
                colorJumlah();
                textBox1.Text = "";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Barang barang = new Barang();
            barang.Show();
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

            string sql5 = "CREATE TABLE IF NOT EXISTS email(username varchar(50), password varchar(50), recipient varchar(50))";
            SQLiteCommand command5 = new SQLiteCommand(sql5, conn);
            command5.ExecuteNonQuery();

            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PembelianForm pembelian = new PembelianForm();
            pembelian.Show();
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

        private void button6_Click(object sender, EventArgs e)
        {
            OpnameForm opname = new OpnameForm();
            opname.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tampilkan semua barang? PERHATIAN: Harap tunggu beberapa waktu!", "Tampilkan semua barang", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                App.loadTable(dataGridView1, "SELECT KodeBarang, NamaBarang, Kelompok, Satuan, HargaJual, Jumlah, Gudang, Opname FROM barang");
                App.fillColumn(dataGridView1, 1);
                colorJumlah();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (Args.admin == true)
                {
                    DialogResult result = MessageBox.Show("Hapus barang ini? " + dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString(), "PERHATIAN", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        App.executeNonQuery("DELETE FROM barang WHERE KodeBarang = '" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "'");
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                        MessageBox.Show("Barang berhasil dihapus");
                    }
                }
            }

            if (e.KeyCode == Keys.F9)
            {
                string kode, nama, harga;
                kode = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                nama = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                harga = dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();

                BarcodeForm barcode = new BarcodeForm(kode, nama, harga);
                barcode.ShowDialog();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LorisanForm lorisan = new LorisanForm();
            lorisan.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Kode Barang;Nama Barang;Kelompok;Satuan;Harga Jual;Toko;Gudang;Opname;");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sb.AppendLine(dataGridView1[0, i].Value.ToString() + ";" + dataGridView1[1, i].Value.ToString() + ";" + dataGridView1[2, i].Value.ToString() + ";" + dataGridView1[3, i].Value.ToString() + ";" + dataGridView1[4, i].Value.ToString() + ";" + dataGridView1[5, i].Value.ToString() + ";" + dataGridView1[6, i].Value.ToString() + ";" + dataGridView1[7, i].Value.ToString() + ";");
            }

            System.IO.File.WriteAllText(@"C:\test\csvbaby.csv", sb.ToString());

            App.shellCommand("excel c:\\test\\csvbaby.csv");


        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("Keluar dari Aplikasi?", "Keluar", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    Close();
                }
            }

            if (e.KeyCode == Keys.F1)
            {
                button1.PerformClick();
            }

            if (e.KeyCode == Keys.F2)
            {
                button2.PerformClick();
            }

            if (e.KeyCode == Keys.F3)
            {
                button3.PerformClick();
            }

            if (e.KeyCode == Keys.F4)
            {
                button4.PerformClick();
            }

            if (e.KeyCode == Keys.F5)
            {
                button5.PerformClick();
            }

            if (e.KeyCode == Keys.F6)
            {
                button6.PerformClick();
            }

            if (e.KeyCode == Keys.F7)
            {
                button7.PerformClick();
            }

            if (e.KeyCode == Keys.F8)
            {
                if (Args.admin == true)
                {
                    Laporan laporan = new Laporan();
                    laporan.Show();
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == 9 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                string mailbody = "";
                int count = 0;
                count = Convert.ToInt32(App.executeScalar("SELECT COUNT(*) FROM pembeliancompact WHERE JatuhTempo = '" + DateTime.Now.ToShortDateString() + "' AND Lunas = 'Belum Lunas'"));
                if (count > 0)
                {
                    DataTable dt = App.executeReader("SELECT * FROM pembeliancompact WHERE JatuhTempo = '" + DateTime.Now.ToShortDateString() + "' AND Lunas = 'Belum Lunas'");
                    foreach (DataRow row in dt.Rows)
                    {
                        mailbody += row[0] + " " + row[1] + " " + row[2] + " " + App.strtomoney(row[3].ToString()) + " " + row[4] + " " + row[5] + " " + row[6] + " " + "\n";
                    }
                    App.sendEmail("Utang Jatuh Tempo", mailbody);
                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }
    }
}
