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
    public partial class PembelianForm : Form
    {
        public PembelianForm()
        {
            InitializeComponent();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                groupBox1.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
                button6.Enabled = false;
            }
        }

        private void PembelianForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            App.autoResizeDataGridView(dataGridView1);
            label10.Text = DateTime.Now.ToShortDateString() + " -";
        }

        private string getNo()
        {
            int num = dataGridView1.Rows.Count + 1;
            return num.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string jumlah, ecer, gudang;

            if (textBox5.Text == "")
            {
                jumlah = "0";
            }
            else
            {
                jumlah = textBox5.Text;
            }

            if (textBox9.Text == "")
            {
                ecer = "0";
            }
            else
            {
                ecer = textBox9.Text;
            }

            if (textBox10.Text == "")
            {
                gudang = "0";
            }
            else
            {
                gudang = textBox10.Text;
            }

            if (jumlah != "0")
            {
                dataGridView1.Rows.Add(getNo(),
                                textBox2.Text,
                                textBox3.Text,
                                textBox6.Text,
                                textBox4.Text,
                                jumlah,
                                App.strtomoney(App.stripMoney(textBox7.Text)),
                                App.strtomoney(App.stripMoney(textBox8.Text)),
                                App.strtomoney((Convert.ToInt32(jumlah) * App.moneytodecimal(textBox7.Text)).ToString()),
                                ecer,
                                gudang
                                );

                calculateTotal();

                clearBox();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.CharacterCasing = CharacterCasing.Upper;

            DataTable rdr = App.executeReader("SELECT NamaBarang, Kelompok, Satuan , HargaJual, HargaBeli, Jumlah, Gudang From barang WHERE KodeBarang = '" + textBox2.Text + "'");

            if (rdr.Rows.Count != 0)
            {
                foreach (DataRow row in rdr.Rows)
                {
                    textBox3.Text = row[0].ToString();
                    textBox4.Text = row[2].ToString();
                    textBox6.Text = row[1].ToString();
                    textBox7.Text = App.strtomoney(row[4].ToString());
                    textBox8.Text = App.strtomoney(row[3].ToString());
                    label17.Text = "Toko:   " + row[5].ToString();
                    label18.Text = "Gudang: " + row[6].ToString();
                }
                textBox13.Text = calculateLaba();

            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                label17.Text = "";
                label18.Text = "";
            }

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox5.Text != "")
                {
                    if (textBox10.Text != "")
                    {

                        textBox9.Text = (Convert.ToInt32(textBox5.Text) - Convert.ToInt32(textBox10.Text)).ToString();
                    }
                    else
                    {
                        textBox9.Text = textBox5.Text;
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox9.Text = textBox5.Text;
                textBox10.Text = "0";
            }
            else
            {
                textBox10.Text = textBox5.Text;
                textBox9.Text = "0";
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox5.Text != "")
                {
                    if (textBox9.Text != "")
                    {

                        textBox10.Text = (Convert.ToInt32(textBox5.Text) - Convert.ToInt32(textBox9.Text)).ToString();
                    }
                    else
                    {
                        textBox10.Text = textBox5.Text;
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Text = textBox5.Text;
            textBox10.Text = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox10.Text = textBox5.Text;
            textBox9.Text = "0";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                //INSERT INTO pembelian
                string sql = string.Format("INSERT INTO pembelian VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToShortDateString() + "-" + textBox1.Text,
                    comboBox1.Text,
                    dataGridView1[1, i].Value.ToString(),
                    dataGridView1[2, i].Value.ToString(),
                    dataGridView1[5, i].Value.ToString(),
                    App.stripMoney(dataGridView1[6, i].Value.ToString()),
                    App.stripMoney(dataGridView1[8, i].Value.ToString())
                    );

                App.executeNonQuery(sql);


                //UPDATE barang
                string sqlupdate = "UPDATE barang SET HargaBeli = '" + App.stripMoney(dataGridView1[6, i].Value.ToString()) + "', HargaJual = '" + App.stripMoney(dataGridView1[7, i].Value.ToString()) + "',Jumlah = Jumlah + '" + dataGridView1[9, i].Value.ToString() + "', Gudang = Gudang + '" + dataGridView1[10, i].Value.ToString() + "' WHERE KodeBarang = '" + dataGridView1[1, i].Value.ToString() + "'";

                App.executeNonQuery(sqlupdate);

                //MessageBox.Show(sql);
                //MessageBox.Show(sqlupdate);
            }

            //INSERT INTO pembeliancompact
            string sqlcompact = string.Format("INSERT INTO pembeliancompact VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToShortDateString() + "-" + textBox1.Text,
                    comboBox1.Text,
                    App.stripMoney(label2.Text),
                    "",
                    "",
                    "0"
                    );

            App.executeNonQuery(sqlcompact);

            MessageBox.Show("Pembelian berhasil dimasukkan.");

            //App.printPembelian(DateTime.Now.ToShortDateString() + "-" + textBox1.Text, false);
            if (printMe("Toko") == true)
            {
                printPembelianToko();
            }

            if (printMe("Gudang") == true)
            {
                printPembelianGudang();
            }
            Close();
        }

        private void calculateTotal()
        {
            decimal total = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                total += Convert.ToDecimal(App.stripMoney(dataGridView1[8, i].Value.ToString()));
            }

            label2.Text = App.decimaltomoney(total);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                textBox5.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                textBox5.Focus();
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                textBox2.Focus();
            }

            if (e.KeyCode == Keys.Right)
            {
                textBox7.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (CariBarangForm cari = new CariBarangForm())
            {
                cari.ShowDialog();

                textBox2.Text = cari.valueFromCari;
                textBox2.Focus();
                // do what ever with result...
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                textBox5.Focus();
            }

            if (e.KeyCode == Keys.Right)
            {
                textBox9.Focus();
            }

            if (e.KeyCode == Keys.Down)
            {
                textBox8.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                textBox7.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clearBox();
        }

        private void clearBox()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox2.Focus();
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                textBox7.Focus();
            }

            if (e.KeyCode == Keys.Right)
            {
                textBox10.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                textBox9.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Barang barang = new Barang();
            barang.ShowDialog();
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (comboBox1.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    textBox2.Focus();
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Enter)
                {
                    comboBox1.Focus();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (groupBox2.Enabled == false)
            {
                groupBox2.Enabled = true;
            }
            else
            {
                groupBox2.Enabled = false;
            }
        }

        private void clearKalkulator()
        {
            textBox11.Text = "0";
            textBox12.Text = "0";
            textBox13.Text = "0";
            textBox14.Text = "0";
            label14.Text = "0";
        }



        private void hitungHargaBeliJual()
        {
            try
            {
                if (textBox12.Text != "" || textBox12.Text != "0")
                {
                    label14.Text = App.strtomoney((App.moneytodouble(textBox11.Text) / Convert.ToDouble(textBox12.Text)).ToString());
                    //textBox14.Text = App.strtomoney((App.moneytodouble(label14.Text) + App.moneytodouble(textBox13.Text)).ToString());
                }
            }
            catch (Exception)
            {

            }
        }


        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox14.Text = App.doubletomoney(App.moneytodouble(textBox13.Text) + App.moneytodouble(label14.Text));
                textBox14.Focus();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                calculateTotal();
            }
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox12.Focus();
            }
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button9.PerformClick();
            }
        }

        private void textBox7_TextChanged_1(object sender, EventArgs e)
        {
            textBox11.Text = textBox7.Text;
            textBox12.Text = "1";
            label14.Text = textBox7.Text;

        }

        private string calculateLaba()
        {
            decimal hargabeli, hargajual;
            hargabeli = App.moneytodecimal(textBox7.Text);
            hargajual = App.moneytodecimal(textBox8.Text);
            return App.strtomoney((hargajual - hargabeli).ToString());
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBox14.Text = textBox8.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            hitungHargaBeliJual();
            textBox13.Focus();
        }


        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox8.Text = App.strtomoney(App.stripMoney(textBox14.Text));
                textBox7.Text = App.strtomoney(App.stripMoney(label14.Text));
                textBox8.Focus();
            }
        }

        private void printPembelianToko()
        {
            DateTime tgl = DateTime.Now;

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "PEMBELIAN [BABY]");
            sb.AppendLine("TOKO");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Faktur: " + label10.Text + " " + textBox1.Text);
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("");
            sb.AppendLine("========================================");

            int qty = 0;
            int jumlahtoko;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                jumlahtoko = Convert.ToInt32(dataGridView1[9, i].Value.ToString());
                if (jumlahtoko != 0)
                {
                    sb.AppendLine(dataGridView1[2, i].Value.ToString() + " ... " + jumlahtoko.ToString());
                    qty += jumlahtoko;
                }

            }

            sb.AppendLine("-----------------------------------------");
            sb.AppendLine("Qty: " + qty.ToString());
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\invoicepembelianketoko.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\invoicepembelianketoko.txt " + Args.printer);

        }

        private void printPembelianGudang()
        {
            DateTime tgl = DateTime.Now;

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "PEMBELIAN [BABY]");
            sb.AppendLine("Gudang");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Faktur: " + label10.Text + " " + textBox1.Text);
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("");
            sb.AppendLine("========================================");

            int qty = 0;
            int jumlahgudang;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                jumlahgudang = Convert.ToInt32(dataGridView1[10, i].Value.ToString());
                if (jumlahgudang != 0)
                {
                    sb.AppendLine(dataGridView1[2, i].Value.ToString() + " ... " + jumlahgudang.ToString());
                    qty += jumlahgudang;
                }

            }

            sb.AppendLine("-----------------------------------------");
            sb.AppendLine("Qty: " + qty.ToString());
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\invoicepembeliankegudang.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\invoicepembeliankegudang.txt " + Args.printer);

        }

        private bool printMe(string tipe)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (tipe == "Toko")
                {
                    if (dataGridView1[9, i].Value.ToString() != "0")
                    {
                        return true;
                    }
                }
                else
                {
                    if (dataGridView1[10, i].Value.ToString() != "0")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            App.shellCommand("copy c:\\test\\invoicepembelianketoko.txt " + Args.printer);
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            App.shellCommand("copy c:\\test\\invoicepembeliankegudang.txt " + Args.printer);

        }
    }
}
