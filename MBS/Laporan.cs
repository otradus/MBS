using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;

namespace MBS
{
    public partial class Laporan : Form
    {
        public Laporan()
        {
            InitializeComponent();
        }

        private void Laporan_Load(object sender, EventArgs e)
        {
            //Penjualan Harian
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            App.autoResizeDataGridView(dataGridView1);
            showPenjualanHarian(dataGridView1);

            //Penjualan Bulanan
            App.formatDataGridView(dataGridView2);
            App.DoubleBuffered(dataGridView2, true);
            App.autoResizeDataGridView(dataGridView2);

            App.formatDataGridView(dataGridView3);
            App.DoubleBuffered(dataGridView3, true);
            App.autoResizeDataGridView(dataGridView3);

            comboBox1.Text = DateTime.Now.ToShortDateString().Substring(3, 2);
            textBox1.Text = DateTime.Now.Year.ToString();

            //Pembelian
            App.formatDataGridView(dataGridView4);
            App.DoubleBuffered(dataGridView4, true);
            App.autoResizeDataGridView(dataGridView4);

            App.formatDataGridView(dataGridView5);
            App.DoubleBuffered(dataGridView5, true);
            App.autoResizeDataGridView(dataGridView5);

            comboBox2.Text = DateTime.Now.ToShortDateString().Substring(3, 2);
            textBox2.Text = DateTime.Now.Year.ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            showPenjualanHarian(dataGridView1);
        }

        private void showPenjualanHarian(DataGridView dgv, string faktur = null)
        {
            dgv.Rows.Clear();

            DataTable rdr;

            if (faktur == null)
            {
                rdr = App.executeReader("SELECT * FROM penjualan WHERE Tanggal = '" + dateTimePicker1.Value.ToShortDateString() + "'");
            }
            else
            {
                rdr = App.executeReader("SELECT * FROM penjualan WHERE Faktur = '" + faktur + "'");
            }

            double subtotal = 0;
            double labatotal = 0;


            foreach (DataRow row in rdr.Rows)
            {
                dgv.Rows.Add(row[1], row[2], row[3], App.strtomoney(row[5].ToString()), row[4], App.strtomoney(row[6].ToString()), App.strtomoney(row[7].ToString()));
            }

            string lastfaktur = "";
            try
            {
                lastfaktur = dgv[0, 0].Value.ToString();
            }
            catch (Exception)
            {
            }

            for (int i = 0; i < dgv.RowCount; i++)
            {
                subtotal += App.moneytodouble(dgv[5, i].Value.ToString());
                labatotal += App.moneytodouble(dgv[6, i].Value.ToString());
            }

            for (int i = 1; i < dgv.RowCount; i++)
            {
                if (lastfaktur == dgv[0, i].Value.ToString())
                {
                    dgv.Rows[i].Cells[0].Value = "";
                }
                else
                {
                    lastfaktur = dgv.Rows[i].Cells[0].Value.ToString();
                }

            }

            dgv.Rows.Add("", "", "", "", "TOTAL:", App.strtomoney(subtotal.ToString()), App.strtomoney(labatotal.ToString()));

            dgv[4, dgv.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dgv[5, dgv.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dgv[6, dgv.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);

            dgv[4, dgv.RowCount - 1].Style.ForeColor = Color.Blue;
            dgv[5, dgv.RowCount - 1].Style.ForeColor = Color.Red;
            dgv[6, dgv.RowCount - 1].Style.ForeColor = Color.Green;

            dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
        }

        private void showPembelianHarian(DataGridView dgv, string faktur = null)
        {
            dgv.Rows.Clear();

            DataTable rdr;

            if (faktur == null)
            {
                rdr = App.executeReader("SELECT * FROM pembelian WHERE Tanggal = '" + dateTimePicker1.Value.ToShortDateString() + "'");
            }
            else
            {
                rdr = App.executeReader("SELECT * FROM pembelian WHERE Faktur = '" + faktur + "'");
            }

            double subtotal = 0;
            int jumlah = 0;

            foreach (DataRow row in rdr.Rows)
            {
                dgv.Rows.Add(row[1], row[2], row[3], row[4], row[5].ToString(), App.strtomoney(row[6].ToString()), App.strtomoney(row[7].ToString()));
            }

            string lastfaktur = "";
            try
            {
                lastfaktur = dgv[0, 0].Value.ToString();
            }
            catch (Exception)
            {
            }

            for (int i = 0; i < dgv.RowCount; i++)
            {
                subtotal += App.moneytodouble(dgv[6, i].Value.ToString());
                jumlah += Convert.ToInt32(dgv[4, i].Value.ToString());
            }

            for (int i = 1; i < dgv.RowCount; i++)
            {
                if (lastfaktur == dgv[0, i].Value.ToString())
                {
                    dgv.Rows[i].Cells[0].Value = "";
                }
                else
                {
                    lastfaktur = dgv.Rows[i].Cells[0].Value.ToString();
                }

            }

            dgv.Rows.Add("", "", "", "TOTAL:", jumlah.ToString(), "", App.strtomoney(subtotal.ToString()));

            dgv[3, dgv.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dgv[4, dgv.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dgv[6, dgv.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);

            dgv[3, dgv.RowCount - 1].Style.ForeColor = Color.Blue;
            dgv[6, dgv.RowCount - 1].Style.ForeColor = Color.Red;
            dgv[4, dgv.RowCount - 1].Style.ForeColor = Color.Green;

            dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            DataTable reader = App.executeReader("SELECT Tanggal, Faktur, Total, Laba, User FROM penjualancompact WHERE Tanggal LIKE '%" + comboBox1.Text + "/" + textBox1.Text + "'");

            decimal subtotal = 0;
            decimal laba = 0;

            foreach (DataRow row in reader.Rows)
            {
                dataGridView2.Rows.Add(row[0], row[1], App.strtomoney(row[2].ToString()), App.strtomoney(row[3].ToString()), row[4]);
                subtotal += Convert.ToDecimal(row[2]);
                laba += Convert.ToDecimal(row[3]);
            }
            dataGridView2.Rows.Add("", "TOTAL:", App.strtomoney(subtotal.ToString()), App.strtomoney(laba.ToString()));
            dataGridView2[1, dataGridView2.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView2[2, dataGridView2.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView2[3, dataGridView2.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);

            dataGridView2[1, dataGridView2.RowCount - 1].Style.ForeColor = Color.Blue;
            dataGridView2[2, dataGridView2.RowCount - 1].Style.ForeColor = Color.Red;
            dataGridView2[3, dataGridView2.RowCount - 1].Style.ForeColor = Color.Green;

            dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;


        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            showPenjualanHarian(dataGridView3, dataGridView2[1, dataGridView2.CurrentCell.RowIndex].Value.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            DataTable reader = App.executeReader("SELECT * FROM pembeliancompact WHERE Tanggal LIKE '%" + comboBox2.Text + "/" + textBox2.Text + "'");

            decimal subtotal = 0;

            foreach (DataRow row in reader.Rows)
            {
                dataGridView4.Rows.Add(row[0], row[1], row[2], App.strtomoney(row[3].ToString()), row[4], row[5], row[6]);
                subtotal += Convert.ToDecimal(row[3]);
            }

            dataGridView4.Rows.Add("", "", "TOTAL:", App.strtomoney(subtotal.ToString()));
            dataGridView4[2, dataGridView4.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView4[3, dataGridView4.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);

            dataGridView4[2, dataGridView4.RowCount - 1].Style.ForeColor = Color.Blue;
            dataGridView4[3, dataGridView4.RowCount - 1].Style.ForeColor = Color.Red;

            dataGridView4.FirstDisplayedScrollingRowIndex = dataGridView4.RowCount - 1;
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            showPembelianHarian(dataGridView5, dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString());
        }

        private void dataGridView4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult result = MessageBox.Show("Ganti status pembayaran faktur " + dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "?", "Lunas", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    if (dataGridView4[6,dataGridView4.CurrentCell.RowIndex].Value.ToString() == "Belum Lunas")
                    {
                        App.executeNonQuery("UPDATE pembeliancompact SET Lunas = 'LUNAS' WHERE Faktur = '"+ dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "'");
                        dataGridView4[6, dataGridView4.CurrentCell.RowIndex].Value = "LUNAS";
                    }
                    else if (dataGridView4[6, dataGridView4.CurrentCell.RowIndex].Value.ToString() == "LUNAS")
                    {
                        App.executeNonQuery("UPDATE pembeliancompact SET Lunas = 'Belum Lunas' WHERE Faktur = '" + dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "'");
                        dataGridView4[6, dataGridView4.CurrentCell.RowIndex].Value = "Belum Lunas";
                    }
                }
            }
        }
    }
}
