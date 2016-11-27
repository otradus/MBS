using MySql.Data.MySqlClient;
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
    public partial class Penjualan : Form
    {
        public static string user;
        public static bool retur = false;
        public bool selesai = false;

        public Penjualan(string user1)
        {
            user = user1;
            InitializeComponent();
        }

        private void Penjualan_Load(object sender, EventArgs e)
        {
            label1.Text = App.getFaktur(DateTime.Now);
            label2.Text = user;

            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            ActiveControl = textBox1;
        }

        public void inputPenjualan()
        {
            int jumlah = 0;

            if (label6.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {

                if (textBox2.Text == "0" || textBox2.Text == "")
                {
                    jumlah = 1;
                }
                else
                {
                    jumlah = Convert.ToInt32(textBox2.Text);
                }

                //RETUR
                if (retur == true)
                {
                    jumlah = jumlah * -1;

                }

                bool newitem = true;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (textBox1.Text == dataGridView1[0, i].Value.ToString())
                    {
                        newitem = false;
                        dataGridView1[3, i].Value = Convert.ToString(Convert.ToInt32(dataGridView1[3, i].Value.ToString()) + jumlah);
                        dataGridView1[5, i].Value = App.strtomoney(Convert.ToString(Convert.ToInt32(dataGridView1[3, i].Value.ToString()) * App.moneytodouble(dataGridView1[4, i].Value.ToString())));
                    }

                }

                if (newitem == true)
                {
                    dataGridView1.Rows.Add(textBox1.Text, label6.Text, satuan, jumlah.ToString(), textBox3.Text, App.strtomoney((Convert.ToDouble(jumlah) * App.moneytodouble(textBox3.Text)).ToString()));
                }

                calculateTotalQty();

                if (Args.poledisplay == true)
                {
                    string string1 = dataGridView1[1, dataGridView1.RowCount - 1].Value.ToString();
                    string string2 = dataGridView1[3, dataGridView1.RowCount - 1].Value.ToString() + " X " + dataGridView1[4, dataGridView1.RowCount - 1].Value.ToString();
                    App.poleDisplay(string1, string2);
                }


                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";


                dataGridView1.ClearSelection();

                textBox1.Focus();
            }
        }
        public void calculateTotalQty()
        {
            int qty = 0;
            double total = 0;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                qty += App.cInt(dataGridView1[3, i].Value.ToString());
                total += App.moneytodouble(dataGridView1[5, i].Value.ToString());
            }
            label4.Text = App.strtomoney(total.ToString());
            label3.Text = "Qty: " + qty.ToString();
        }

        public string satuan;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable rdr = App.executeReader("SELECT NamaBarang, HargaJual, Satuan From barang WHERE KodeBarang = '" + textBox1.Text + "'");

            if (rdr.Rows.Count != 0)
            {
                foreach (DataRow row in rdr.Rows)
                {
                    label6.Text = Convert.ToString(row[0]);
                    textBox2.Text = "1";
                    textBox3.Text = App.strtomoney(row[1].ToString());
                    satuan = row[2].ToString();
                }

            }
            else
            {
                label6.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                textBox2.Focus();
                textBox2.SelectAll();
            }

            if (e.KeyCode == Keys.Enter)
            {
                //textBox2.Focus();
                //textBox2.SelectAll();
                inputPenjualan();
            }

            if (e.KeyCode == Keys.F7)
            {
                button3.PerformClick();
            }

            if (e.KeyCode == Keys.Up)
            {
                addup();
            }

            if (e.KeyCode == Keys.Down)
            {
                subtractdown();
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                textBox3.Focus();
                textBox3.SelectAll();
            }

            if (e.KeyCode == Keys.Left)
            {
                textBox1.Focus();
                textBox1.SelectAll();
            }

            if (e.KeyCode == Keys.Enter)
            {
                inputPenjualan();
            }

            if (e.KeyCode == Keys.F7)
            {
                button2.PerformClick();
            }

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                textBox2.Focus();
                textBox2.SelectAll();
            }

            if (e.KeyCode == Keys.Enter)
            {
                inputPenjualan();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    int x = Convert.ToInt32(textBox2.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Masukkan jumlah angka saja jangan huruf!");
                textBox2.Text = "";
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                calculateTotalQty();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (CariBarangForm cari = new CariBarangForm())
            {
                cari.ShowDialog();

                textBox1.Text = cari.valueFromCari;
                textBox2.Focus();
                // do what ever with result...
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selesai == false)
            {
                if (dataGridView1.RowCount != 0)
                {
                    DateTime tgl = DateTime.Now;
                    MySqlConnection conn = new MySqlConnection(App.getConnectionString());
                    MySqlCommand cmd = new MySqlCommand();

                    string lastfaktur = App.getFaktur(tgl);

                    try
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        string kode;
                        string nama;
                        string jumlah;
                        string harga;
                        string subtotal;
                        double total = 0;
                        double laba = 0;
                        double labatotal = 0;

                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            kode = dataGridView1[0, i].Value.ToString();
                            nama = dataGridView1[1, i].Value.ToString();
                            jumlah = dataGridView1[3, i].Value.ToString();
                            harga = App.stripMoney(dataGridView1[4, i].Value.ToString());
                            subtotal = App.stripMoney(dataGridView1[5, i].Value.ToString());

                            laba = (Convert.ToDouble(harga) - Convert.ToDouble(App.executeScalar("SELECT HargaBeli FROM barang WHERE KodeBarang = '" + kode + "'").ToString())) * Convert.ToDouble(jumlah);


                            labatotal += laba;

                            cmd.CommandText = "INSERT INTO penjualan SET Tanggal='" + tgl.ToShortDateString() + "', Faktur='" + lastfaktur + "',KodeBarang='" + kode + "',NamaBarang='" + nama + "',Jumlah='" + jumlah + "',Harga='" + harga + "',Subtotal='" + subtotal + "',Laba='" + laba + "'";
                            cmd.ExecuteNonQuery();

                            //check if jumlah = 0
                            int cekjumlah = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM barang WHERE KodeBarang = '" + kode + "'"));
                            if (cekjumlah - Convert.ToInt32(jumlah) >= 0)
                            {
                                cmd.CommandText = "UPDATE barang SET Jumlah = Jumlah - '" + jumlah + "' WHERE KodeBarang = '" + kode + "'";
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                MessageBox.Show("PERHATIAN: Jumlah barang " + nama + " sudah 0 di komputer");
                                cmd.CommandText = "UPDATE barang SET Jumlah = '0' WHERE KodeBarang = '" + kode + "'";
                                cmd.ExecuteNonQuery();
                            }


                            addLorisan(kode, nama, jumlah);

                            total += App.cDouble(App.stripMoney(dataGridView1[5, i].Value.ToString()));
                        }

                        cmd.CommandText = "INSERT INTO penjualancompact SET Tanggal='" + tgl.ToShortDateString() + "', Faktur='" + lastfaktur + "', User='" + user + "',Total='" + total + "',Laba='" + labatotal + "', Bayar='0'";
                        cmd.ExecuteNonQuery();


                        conn.Close();

                        selesai = true;

                        App.poleDisplay("Maju Baby Shop", DateTime.Now.ToShortDateString());
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    App.sendEmail("Penjualan MBS " + DateTime.Now.ToShortDateString(), mailBody());
                    App.printPenjualan(lastfaktur, user);


                    this.Close();

                }
                else
                {
                    MessageBox.Show("Penjualan masih kosong!");
                }

            }
            else
            {
                MessageBox.Show("Penjualan sudah selesai!");
                Close();
            }
        }

        private string mailBody()
        {
            string msg;
            msg = "Tanggal: " + DateTime.Now.ToShortDateString() + " Jam: " + DateTime.Now.ToShortTimeString() + "\n";
            msg += "Faktur: " + label1.Text + " User: " + label2.Text + "\n\n";

            msg += "Penjualan: " + label4.Text + "\n\n";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                msg += dataGridView1[1, i].Value.ToString() + "\n";
                msg += dataGridView1[3, i].Value.ToString() + " x " + dataGridView1[4, i].Value.ToString() + " = " + dataGridView1[5, i].Value.ToString() + "\n";
            }

            msg += "\nTotal: " + App.strtomoney(App.executeScalar("SELECT SUM(Total) FROM penjualancompact WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "'").ToString());

            return msg;
        }

        public void addLorisan(string kode, string nama, string jumlah)
        {
            DateTime tgl = DateTime.Now;
            if (Convert.ToInt32(App.executeScalar("SELECT COUNT(*) FROM lorisan WHERE KodeBarang = '" + kode + "'")) == 0)
            {
                App.executeNonQuery("INSERT INTO lorisan SET Tanggal = '" + tgl.ToShortDateString() + "', KodeBarang = '" + kode + "', NamaBarang = '" + nama + "', Jumlah = '" + jumlah + "'");
            }
            else
            {
                App.executeNonQuery("UPDATE lorisan SET Jumlah = Jumlah + '" + jumlah + "' WHERE KodeBarang = '" + kode + "'");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (retur == false)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    retur = true;
                    this.BackColor = Color.Maroon;
                    button1.Text = "!RETUR!";
                    textBox1.Focus();
                }
                else
                {
                    MessageBox.Show("Daftar penjualan masih kosong");
                }
            }
            else
            {
                retur = false;
                this.BackColor = default(Color);
                button1.Text = "RETUR";
                textBox1.Focus();
            }
        }

        private void addup()
        {
            int num = Convert.ToInt32(dataGridView1[3, dataGridView1.Rows.Count - 1].Value.ToString());
            dataGridView1[3, dataGridView1.Rows.Count - 1].Value = (num + 1).ToString();

            decimal harga = Convert.ToDecimal(App.stripMoney(dataGridView1[4, dataGridView1.Rows.Count - 1].Value.ToString()));
            dataGridView1[5, dataGridView1.Rows.Count - 1].Value = App.decimaltomoney(harga * (num + 1));
            calculateTotalQty();
        }

        private void subtractdown()
        {
            if (retur != true)
            {

                int num = Convert.ToInt32(dataGridView1[3, dataGridView1.Rows.Count - 1].Value.ToString());

                if (num > 1)
                {
                    dataGridView1[3, dataGridView1.Rows.Count - 1].Value = (num - 1).ToString();


                    decimal harga = Convert.ToDecimal(App.stripMoney(dataGridView1[4, dataGridView1.Rows.Count - 1].Value.ToString()));
                    dataGridView1[5, dataGridView1.Rows.Count - 1].Value = App.decimaltomoney(harga * (num - 1));
                    calculateTotalQty();
                }
            }
            else
            {
                int num = Convert.ToInt32(dataGridView1[3, dataGridView1.Rows.Count - 1].Value.ToString());

                dataGridView1[3, dataGridView1.Rows.Count - 1].Value = (num - 1).ToString();


                decimal harga = Convert.ToDecimal(App.stripMoney(dataGridView1[4, dataGridView1.Rows.Count - 1].Value.ToString()));
                dataGridView1[5, dataGridView1.Rows.Count - 1].Value = App.decimaltomoney(harga * (num - 1));
                calculateTotalQty();
            }
        }

        private void Penjualan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("Keluar dari Penjualan?", "Keluar", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    Close();
                }
            }

            if (e.KeyCode == Keys.F7)
            {
                button3.PerformClick();
            }

        }
    }
}
