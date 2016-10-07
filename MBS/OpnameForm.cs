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
    public partial class OpnameForm : Form
    {
        public OpnameForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string namabarang = Convert.ToString(App.executeScalar("SELECT NamaBarang FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
            label3.Text = namabarang;
        }

        private void OpnameForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            ActiveControl = textBox1;

            if (Args.admin == false)
            {
                button2.Enabled = false;
            }

            loadOpname();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool toko = true;
            if (radioButton1.Checked == true)
            {
                toko = true;
            }
            else
            {
                toko = false;
            }

            int jumlah = 0;

            if (textBox1.Text != "" && label3.Text != "")
            {

                if (textBox2.Text == "0" || textBox2.Text == "")
                {
                    jumlah = 1;
                }
                else
                {
                    jumlah = Convert.ToInt32(textBox2.Text);
                }


                bool newitem = true;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (textBox1.Text == dataGridView1[0, i].Value.ToString())
                    {
                        newitem = false;

                        if (toko == true)
                        {
                            dataGridView1[2, i].Value = Convert.ToString(Convert.ToInt32(dataGridView1[2, i].Value) + jumlah);

                            int jumlahbarang;
                            jumlahbarang = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
                            int selisih = Convert.ToInt32(dataGridView1[2, i].Value) + jumlah - jumlahbarang;

                            dataGridView1[3, i].Value = Convert.ToString(selisih);
                        }
                        else
                        {
                            dataGridView1[4, i].Value = Convert.ToString(Convert.ToInt32(dataGridView1[4, i].Value) + jumlah);

                            int jumlahgudang;
                            jumlahgudang = Convert.ToInt32(App.executeScalar("SELECT Gudang FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
                            int selisih = Convert.ToInt32(dataGridView1[4, i].Value) + jumlah - jumlahgudang;

                            dataGridView1[5, i].Value = Convert.ToString(selisih);

                        }
                    }

                }

                if (newitem == true)
                {
                    if (toko == true)
                    {
                        int jumlahbarang;
                        jumlahbarang = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
                        dataGridView1.Rows.Add(textBox1.Text, label3.Text, jumlah.ToString(), Convert.ToString((jumlahbarang - 1) * -1), "0", "0");
                    }
                    else
                    {
                        int jumlahgudang;
                        jumlahgudang = Convert.ToInt32(App.executeScalar("SELECT Gudang FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
                        dataGridView1.Rows.Add(textBox1.Text, label3.Text, "0", "0", jumlah.ToString(), Convert.ToString((jumlahgudang - 1) * -1));

                    }
                }


                textBox1.Text = "";
                textBox2.Text = "";

                dataGridView1.ClearSelection();

                textBox1.Focus();

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }

            if (e.KeyCode == Keys.Down)
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

            if (e.KeyCode == Keys.Up)
            {
                textBox1.Focus();
            }
        }

        private void loadOpname()
        {
            dataGridView1.Rows.Clear();
            DataTable dt = new DataTable();
            dt = App.executeReader("SELECT * FROM opname");
            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            App.executeNonQuery("DELETE FROM opname");

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                App.executeNonQuery("INSERT INTO opname SET KodeBarang = '" + dataGridView1[0, i].Value.ToString() + "', NamaBarang = '" + dataGridView1[1, i].Value.ToString() + "', Jumlah = '" + dataGridView1[2, i].Value.ToString() + "', SelisihJumlah = '" + dataGridView1[3, i].Value.ToString() + "', Gudang = '" + dataGridView1[4, i].Value.ToString() + "', SelisihGudang = '" + dataGridView1[5, i].Value.ToString() + "'");
            }
            MessageBox.Show("Data Opname berhasil disimpan");
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tanggal = DateTime.Now.ToShortDateString();
            int jumlah, gudang;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                jumlah = Convert.ToInt32(dataGridView1[2, i].Value);
                gudang = Convert.ToInt32(dataGridView1[4, i].Value);

                if (jumlah > 0 && gudang == 0)
                {
                    App.executeNonQuery("UPDATE barang SET Jumlah = '" + jumlah + "', Opname = '" + tanggal + "' WHERE Kodebarang = '" + dataGridView1[0, i].Value.ToString() + "'");

                }
                else if (gudang > 0 && jumlah == 0)
                {
                    App.executeNonQuery("UPDATE barang SET Gudang = '" + jumlah + "', Opname = '" + tanggal + "' WHERE Kodebarang = '" + dataGridView1[0, i].Value.ToString() + "'");

                }
                else if (jumlah > 0 && gudang > 0)
                {
                    App.executeNonQuery("UPDATE barang SET Jumlah = '" + jumlah + "', Gudang = '"+ gudang +"', Opname = '" + tanggal + "' WHERE Kodebarang = '" + dataGridView1[0, i].Value.ToString() + "'");

                }
            }
            App.executeNonQuery("DELETE FROM opname");

            MessageBox.Show("Opname berhasil dimasukkan");
        }
    }
}
