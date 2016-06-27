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
            this.ActiveControl = textBox1;
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
                textBox2.Focus();
                textBox2.SelectAll();
            }

            if (e.KeyCode == Keys.F7)
            {
                button3.PerformClick();
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
    }
}
