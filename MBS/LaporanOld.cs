using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MBS
{
    public partial class LaporanOld : Form
    {
        public void loadLaporanPenjualan(DataGridView dgv, string tanggal)
        {
            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            DataTable rdr = App.executeReader("SELECT Faktur, KodeBarang, NamaBarang, Jumlah, Harga, Subtotal, Laba FROM penjualan WHERE Tanggal = '" + tanggal + "'");

            double subtotal = 0;
            double labatotal = 0;


            foreach (DataRow row in rdr.Rows)
            {
                dgv.Rows.Add(row[0], row[1], row[2], row[3], App.strtomoney(row[4].ToString()), App.strtomoney(row[5].ToString()), App.strtomoney(row[6].ToString()));
            }

            string lastfaktur = "";
            try
            {
                lastfaktur = dgv[0, 0].Value.ToString();
            }
            catch (Exception)
            {
            }

            for (int i = 1; i < dgv.RowCount; i++)
            {
                subtotal += App.moneytodouble(dgv[5, i].Value.ToString());
                labatotal += App.moneytodouble(dgv[6, i].Value.ToString());

                if (lastfaktur == dgv[0, i].Value.ToString())
                {
                    dgv.Rows[i].Cells[0].Value = "";
                }
                else
                {
                    lastfaktur = dgv.Rows[i].Cells[0].Value.ToString();
                }

            }

            label2.Text = "TOTAL: " + App.strtomoney(subtotal.ToString());
            label5.Text = "LABA: " + App.strtomoney(labatotal.ToString());

            //dataGridView1.Rows.Add("", "", "", "", "", "TOTAL:", App.strtomoney(subtotal.ToString()), "");
            //dataGridView1[5, dataGridView1.RowCount - 1].Style.Font =  new Font("Arial", 12, FontStyle.Bold);
            //dataGridView1[6, dataGridView1.RowCount - 1].Style.Font = new Font("Arial", 12, FontStyle.Bold);
            //dataGridView1[5, dataGridView1.RowCount - 1].Style.ForeColor = Color.Red;
            //dataGridView1[6, dataGridView1.RowCount - 1].Style.ForeColor = Color.Red;

            //dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            //dataGridView1[0, dataGridView1.RowCount - 1].Selected = true;

        }

        public void loadLaporanPembelian(DataGridView dgv, string tanggal)
        {
            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            DataTable rdr = App.executeReader("SELECT Faktur, KodeBarang, NamaBarang, Jumlah, Subtotal FROM pembelian WHERE Tanggal = '" + tanggal + "'");


            foreach (DataRow row in rdr.Rows)
            {
                dgv.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), App.strtomoney(row[4].ToString()));
            }

            for (int i = 1; i < dgv.RowCount; i++)
            {
                if (dgv[0, i - 1].Value.ToString() == dgv[0, i].Value.ToString() || dgv[0, i - 1].Value.ToString() == "")
                {
                    dgv.Rows[i].Cells[1].Value = "";
                }

            }

        }

        public LaporanOld()
        {
            InitializeComponent();
        }

        private void Laporan_Load(object sender, EventArgs e)
        {
            App.DoubleBuffered(dataGridView1, true);
            App.DoubleBuffered(dataGridView2, true);

            App.autoResizeDataGridView(dataGridView1);
            App.autoResizeDataGridView(dataGridView2);
            DateTime tgl = DateTime.Now;
            //App.loadTable(dataGridView1, "SELECT * FROM penjualan WHERE Tanggal = '" + tgl.ToShortDateString() + "'");
            loadLaporanPenjualan(dataGridView1, tgl.ToShortDateString());
            label1.Text = "Tanggal: " + tgl.ToShortDateString();

            App.formatDataGridView(dataGridView3);
            App.DoubleBuffered(dataGridView3, true);
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView3.Columns[0].FillWeight = 30;
            dataGridView3.Columns[1].FillWeight = 70;

            textBox2.Text = DateTime.Now.Year.ToString();

        }



        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dataGridView1.Rows.Clear();
            loadLaporanPenjualan(dataGridView1, monthCalendar1.SelectionRange.Start.ToShortDateString());
            label1.Text = "Tanggal: " + monthCalendar1.SelectionRange.Start.ToShortDateString();
            //            App.loadTable(dataGridView1, "SELECT * FROM penjualan WHERE Tanggal = '"+ monthCalendar1.SelectionRange.Start.ToShortDateString() + "'");
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows.Clear();
            }
            loadLaporanPembelian(dataGridView2, monthCalendar2.SelectionRange.Start.ToShortDateString());
            label2.Text = "Tanggal: " + monthCalendar2.SelectionRange.Start.ToShortDateString();
            //App.loadTable(dataGridView2, "SELECT * FROM pembelian WHERE Tanggal = '" + monthCalendar1.SelectionRange.Start.ToShortDateString() + "'");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataTable dt = new DataTable();
                dt = App.executeReader("SELECT KodeBarang, NamaBarang FROM barang WHERE NamaBarang LIKE '%" + textBox1.Text + "%'");

                foreach (DataRow row in dt.Rows)
                {
                    dataGridView3.Rows.Add(row[0].ToString(), row[1].ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = App.executeReader("SELECT KodeBarang, NamaBarang FROM barang");

            foreach (DataRow row in dt.Rows)
            {
                dataGridView3.Rows.Add(row[0].ToString(), row[1].ToString());
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataTable dt = new DataTable();
                dt = App.executeReader("SELECT Tanggal, Jumlah, Subtotal, Laba FROM penjualan WHERE KodeBarang = '" + dataGridView3[0, dataGridView3.CurrentRow.Index].Value.ToString() + "' AND Tanggal LIKE '%" + textBox2.Text + "'");

                foreach (DataRow row in dt.Rows)
                {
                    this.chart1.Series["Jumlah"].Points.AddXY(row[0].ToString(), row[1].ToString());
                    this.chart1.Series["Subtotal"].Points.AddXY(row[0].ToString(), row[2].ToString());
                    this.chart1.Series["Laba"].Points.AddXY(row[0].ToString(), row[3].ToString());

                }
            }
        }

        private void loadChart()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Text = dataGridView3[1, dataGridView3.CurrentRow.Index].Value.ToString();
            chart1.ChartAreas[0].AxisY.Interval = 1;
            chart1.ChartAreas[0].AxisY.Maximum = 5;
            chart1.ChartAreas[0].AxisX.Interval = 1;

            //chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            //chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            //chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            DataTable dt = new DataTable();
            dt = App.executeReader("SELECT Tanggal, Jumlah, Subtotal, Laba FROM penjualan WHERE KodeBarang = '" + dataGridView3[0, dataGridView3.CurrentRow.Index].Value.ToString() + "' AND Tanggal LIKE '%" + textBox2.Text + "'");

            foreach (DataRow row in dt.Rows)
            {
                chart1.Series["Jumlah"].Points.AddXY(row[0].ToString(), row[1].ToString());
                chart1.Series["Subtotal"].Points.AddXY(row[0].ToString(), App.strtomoney(row[2].ToString()));
                chart1.Series["Laba"].Points.AddXY(row[0].ToString(), App.strtomoney(row[3].ToString()));

            }

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            loadChart();
        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                loadChart();
            }
        }
    }
}
