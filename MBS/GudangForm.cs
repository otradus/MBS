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
    public partial class GudangForm : Form
    {
        public GudangForm()
        {
            InitializeComponent();
        }

        private void GudangForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            this.ActiveControl = textBox1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" || textBox2.Text != "0")
            {
                try
                {
                    string namaBarang = App.executeScalar("SELECT NamaBarang FROM barang WHERE KodeBarang = '" + textBox1.Text + "'").ToString();
                    dataGridView1.Rows.Add(textBox1.Text, namaBarang, textBox2.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Kode Barang tidak ada!");
                }
                finally
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Right)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Masukkan barang gudang ini ke toko?", "Masuk Barang", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                int qty = 0;
                DateTime tgl = DateTime.Now;
 
                //PRINT INVOICE
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "Maju Baby Shop");
                sb.AppendLine(Convert.ToChar(27) + "@");
                sb.AppendLine("Gudang KE Toko");
                sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
                sb.AppendLine("");
                sb.AppendLine("========================================");

                
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //MYSQL
                    App.executeNonQuery("UPDATE barang SET Jumlah = Jumlah + '" + dataGridView1[2, i].Value.ToString() + "', Gudang = Gudang - '" + dataGridView1[2, i].Value.ToString() + "' WHERE KodeBarang = '" + dataGridView1[0, i].Value.ToString() + "'");
                    //Print
                    qty += Convert.ToInt32(dataGridView1[2, i].Value.ToString());
                    sb.AppendLine(dataGridView1[1, i].Value.ToString() + " ... " + dataGridView1[2, i].Value.ToString());
                }

                sb.AppendLine("----------------------------------------");
                sb.AppendLine("   Qty: " + qty.ToString());
                sb.AppendLine("");

                sb.AppendLine(Convert.ToChar(29) + "VA0");


                System.IO.File.WriteAllText(@"C:\test\kirimangudang.txt", sb.ToString());

                App.shellCommand("copy c:\\test\\kirimangudang.txt " + Args.printer);


                MessageBox.Show("Barang Gudang berhasil dimasukkan ke toko.");
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cetakLorisanGudang();
        }

        public void cetakLorisanGudang()
        {
            int qty = 0;
            DateTime tgl = DateTime.Now;

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "Maju Baby Shop");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("LORISAN GUDANG");
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("");
            sb.AppendLine("========================================");

            DataTable dt = new DataTable();
            dt = App.executeReader("SELECT NamaBarang, Jumlah, Gudang, PengambilanGudang, BatasGudang FROM barang");

            foreach (DataRow row in dt.Rows)
            {
                if (row["PengambilanGudang"] != DBNull.Value && row["BatasGudang"] != DBNull.Value)
                {
                    if (Convert.ToInt32(row["Jumlah"].ToString()) <= Convert.ToInt32(row["BatasGudang"].ToString()))
                    {
                        if (Convert.ToInt32(row["Gudang"].ToString()) >= Convert.ToInt32(row["PengambilanGudang"].ToString()))
                        {
                            qty += Convert.ToInt32(row["PengambilanGudang"].ToString());
                            sb.AppendLine(row["NamaBarang"].ToString() + " ... " + Convert.ToInt32(row["PengambilanGudang"].ToString()));
                        }
                        else
                        {
                            if (Convert.ToInt32(row["Gudang"].ToString()) != 0)
                            {
                                qty += Convert.ToInt32(row["Gudang"].ToString());
                                sb.AppendLine(row["NamaBarang"].ToString() + " ... " + Convert.ToInt32(row["Gudang"].ToString()));
                            }
                        }
                    }

                }
            }


            sb.AppendLine("----------------------------------------");
            sb.AppendLine("   Qty: " + qty.ToString());
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\lorisangudang.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\lorisangudang.txt " + Args.printer);

            MessageBox.Show("Lorisan Gudang berhasil dicetak.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (CariBarangForm cari = new CariBarangForm())
            {
                cari.ShowDialog();

                textBox1.Text = cari.valueFromCari;
                textBox2.Focus();
                // do what ever with result...
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(App.executeScalar("SELECT COUNT(*) FROM barang WHERE KodeBarang = '" + textBox1.Text + "'").ToString()) > 0)
                {
                    label3.Text = App.executeScalar("SELECT NamaBarang FROM barang WHERE KodeBarang = '" + textBox1.Text + "'").ToString();
                }
                else
                {
                    label3.Text = "";
                }

            }
            catch (Exception)
            {

            }

        }
    }
}
