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
            colorTable();
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
                        int jumlahbarang;
                        int jumlahgudang;
                        int selisih;

                        if (toko == true)
                        {
                            jumlahbarang = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));

                            if (jumlah == 1)
                            {
                                dataGridView1[2, i].Value = Convert.ToString(Convert.ToInt32(dataGridView1[2, i].Value) + jumlah);
                                selisih = Convert.ToInt32(dataGridView1[2, i].Value) - jumlahbarang;
                            }
                            else
                            {
                                dataGridView1[2, i].Value = jumlah.ToString();
                                selisih = jumlah - jumlahbarang;
                            }

                            if (selisih > 0)
                            {
                                dataGridView1[3, i].Value = "+" + Convert.ToString(selisih);
                            }
                            else
                            {
                                dataGridView1[3, i].Value = Convert.ToString(selisih);
                            }
                        }
                        else
                        {
                            jumlahgudang = Convert.ToInt32(App.executeScalar("SELECT Gudang FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));

                            if (jumlah == 1)
                            {
                                dataGridView1[4, i].Value = Convert.ToString(Convert.ToInt32(dataGridView1[4, i].Value) + jumlah);
                                selisih = Convert.ToInt32(dataGridView1[4, i].Value) - jumlahgudang;

                            }
                            else
                            {
                                dataGridView1[4, i].Value = jumlah.ToString();
                                selisih = jumlah - jumlahgudang;

                            }

                            if (selisih > 0)
                            {
                                dataGridView1[5, i].Value = "+" + Convert.ToString(selisih);
                            }
                            else
                            {
                                dataGridView1[5, i].Value = Convert.ToString(selisih);
                            }

                        }
                    }

                }

                if (newitem == true)
                {
                    if (toko == true)
                    {
                        int jumlahbarang;
                        jumlahbarang = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
                        if (jumlah == 1)
                        {
                            dataGridView1.Rows.Add(textBox1.Text, label3.Text, jumlah.ToString(), Convert.ToString((jumlahbarang - 1) * -1), "0", "0");
                        }
                        else
                        {
                            dataGridView1.Rows.Add(textBox1.Text, label3.Text, jumlah.ToString(), Convert.ToString(jumlah - jumlahbarang), "0", "0");
                        }
                    }
                    else
                    {
                        int jumlahgudang;
                        jumlahgudang = Convert.ToInt32(App.executeScalar("SELECT Gudang FROM barang WHERE KodeBarang = '" + textBox1.Text + "'"));
                        if (jumlah == 1)
                        {
                            dataGridView1.Rows.Add(textBox1.Text, label3.Text, "0", "0", jumlah.ToString(), Convert.ToString((jumlahgudang - 1) * -1));
                        }
                        else
                        {
                            dataGridView1.Rows.Add(textBox1.Text, label3.Text, "0", "0", jumlah.ToString(), Convert.ToString(jumlah - jumlahgudang));
                        }
                    }
                }


                dataGridView1.ClearSelection();
                selectLastInput(textBox1.Text);


                textBox1.Text = "";
                textBox2.Text = "";

                colorTable();

                textBox1.Focus();

            }
        }

        private void colorTable()
        {
            string jumlah, gudang;
            int selisihjumlah, selisihgudang;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                jumlah = dataGridView1[2, i].Value.ToString();
                gudang = dataGridView1[4, i].Value.ToString();
                selisihjumlah = Convert.ToInt32(dataGridView1[3, i].Value.ToString());
                selisihgudang = Convert.ToInt32(dataGridView1[5, i].Value.ToString());

                if (jumlah == "0" && selisihjumlah == 0)
                {

                    dataGridView1[2, i].Style.ForeColor = Color.GhostWhite;
                    dataGridView1[3, i].Style.ForeColor = Color.GhostWhite;
                }

                if (gudang == "0" && selisihgudang == 0)
                {

                    dataGridView1[4, i].Style.ForeColor = Color.GhostWhite;
                    dataGridView1[5, i].Style.ForeColor = Color.GhostWhite;
                }


                if (selisihjumlah == 0)
                {
                    dataGridView1[3, i].Style.BackColor = Color.LightGreen;
                }
                else if (selisihjumlah > 0)
                {
                    dataGridView1[3, i].Style.BackColor = Color.LightBlue;
                }
                else
                {
                    dataGridView1[3, i].Style.BackColor = Color.Pink;
                }
            }
        }

        private void selectLastInput(string kode)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1[0, i].Value.ToString() == kode)
                {
                    dataGridView1.Rows[i].Selected = true;
                }
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

            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Text = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                textBox2.Focus();
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
                    App.executeNonQuery("UPDATE barang SET Jumlah = '" + jumlah + "', Gudang = '" + gudang + "', Opname = '" + tanggal + "' WHERE Kodebarang = '" + dataGridView1[0, i].Value.ToString() + "'");

                }
            }
            App.executeNonQuery("DELETE FROM opname");

            printOpname();

            MessageBox.Show("Opname berhasil dimasukkan");
            Close();
        }

        private void printOpname()
        {
            DateTime tgl = DateTime.Now;

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "OPNAME");
            sb.AppendLine("Baby");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("========================================");


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sb.AppendLine(dataGridView1[0, i].Value.ToString() + " " + dataGridView1[1, i].Value.ToString());
                if (dataGridView1[2, i].Value.ToString() != "0" && dataGridView1[3, i].Value.ToString() != "0")
                {
                    sb.AppendLine("     Toko: " + dataGridView1[2, i].Value.ToString() + " | Selisih: " + dataGridView1[3, i].Value.ToString());
                }
                if (dataGridView1[4, i].Value.ToString() != "0" && dataGridView1[5, i].Value.ToString() != "0")
                {
                    sb.AppendLine("   Gudang: " + dataGridView1[4, i].Value.ToString() + " | Selisih: " + dataGridView1[5, i].Value.ToString());
                }

            }

            sb.AppendLine("----------------------------------------");
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\opnamebaby.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\opnamebaby.txt " + Args.printer);


        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBox1.Text = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            //textBox2.Focus();
        }
    }
}
