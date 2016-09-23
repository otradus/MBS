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

            DataTable rdr = App.executeReader("SELECT NamaBarang, Kelompok, Satuan , HargaJual, HargaBeli From barang WHERE KodeBarang = '" + textBox2.Text + "'");

            if (rdr.Rows.Count != 0)
            {
                foreach (DataRow row in rdr.Rows)
                {
                    textBox3.Text = row[0].ToString();
                    textBox4.Text = row[2].ToString();
                    textBox6.Text = row[1].ToString();
                    textBox7.Text = App.strtomoney(row[3].ToString());
                    textBox8.Text = App.strtomoney(row[4].ToString());
                }

            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
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
                string sqlupdate = "UPDATE barang SET Jumlah = Jumlah + '"+ dataGridView1[9, i].Value.ToString() +"', Gudang = Gudang + '"+ dataGridView1[10, i].Value.ToString() + "' WHERE KodeBarang = '"+ dataGridView1[1, i].Value.ToString() + "'";

                App.executeNonQuery(sqlupdate);

                MessageBox.Show(sql);
                MessageBox.Show(sqlupdate);
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

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            hitungHargaBeliJual();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            hitungHargaBeliJual();
        }

        private void hitungHargaBeliJual()
        {
            try
            {
                    label14.Text = App.strtomoney((App.moneytodouble(textBox11.Text) / Convert.ToInt32(textBox12.Text)).ToString());
                    textBox14.Text = App.strtomoney((App.moneytodouble(label14.Text) + App.moneytodouble(textBox13.Text)).ToString());
            }
            catch (Exception)
            {

            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            hitungHargaBeliJual();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            textBox11.Text = textBox7.Text;
            textBox12.Text = "1";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox8.Text = textBox14.Text;
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button8.PerformClick();
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
    }
}
