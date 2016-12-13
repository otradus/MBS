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
using System.Windows.Forms.DataVisualization.Charting;

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

            //Cashflow
            App.formatDataGridView(dataGridView6);
            App.DoubleBuffered(dataGridView6, true);
            App.autoResizeDataGridView(dataGridView6);

            App.formatDataGridView(dataGridView7);
            App.DoubleBuffered(dataGridView7, true);
            App.autoResizeDataGridView(dataGridView7);

            comboBox3.Text = DateTime.Now.ToShortDateString().Substring(3, 2);
            textBox4.Text = DateTime.Now.Year.ToString();

            //Asset
            App.formatDataGridView(dataGridView8);
            App.DoubleBuffered(dataGridView8, true);
            App.autoResizeDataGridView(dataGridView8);

            //Grafik Tabel
            comboBox4.Text = DateTime.Now.ToShortDateString().Substring(3, 2);
            textBox5.Text = DateTime.Now.Year.ToString();

            //Users
            App.formatDataGridView(dataGridView9);
            App.DoubleBuffered(dataGridView9, true);
            App.autoResizeDataGridView(dataGridView9);

            //ServisCacad
            App.formatDataGridView(dataGridView10);
            App.DoubleBuffered(dataGridView10, true);
            App.autoResizeDataGridView(dataGridView10);

            comboBox5.Text = DateTime.Now.ToShortDateString().Substring(3, 2);
            textBox8.Text = DateTime.Now.Year.ToString();
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

            string tanggal = dataGridView4[4, dataGridView4.CurrentCell.RowIndex].Value.ToString();

            if (tanggal != "Tunai")
            {
                //dateTimePicker2.Value = new DateTime(int.Parse(tanggal.Substring(6, 4)), int.Parse(tanggal.Substring(3, 2)), int.Parse(tanggal.Substring(0, 2)));
                dateTimePicker2.Value = DateTime.Parse(tanggal);
            }
            textBox3.Text = dataGridView4[5, dataGridView4.CurrentCell.RowIndex].Value.ToString();

        }

        private void dataGridView4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult result = MessageBox.Show("Ganti status pembayaran faktur " + dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "?", "Lunas", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    if (dataGridView4[6, dataGridView4.CurrentCell.RowIndex].Value.ToString() == "Belum Lunas")
                    {
                        App.executeNonQuery("UPDATE pembeliancompact SET Lunas = 'LUNAS' WHERE Faktur = '" + dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "'");
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

        private void button3_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Ganti tanggal jatuh tempo dan keterangan?", "Ganti Tanggal", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {

                if (radioButton1.Checked == true)
                {
                    App.executeNonQuery("UPDATE pembeliancompact SET JatuhTempo = 'Tunai', Keterangan = '" + textBox3.Text + "', Lunas = 'LUNAS' WHERE Faktur = '" + dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "' AND Tanggal = '" + dataGridView4[0, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "'");
                    dataGridView4[4, dataGridView4.CurrentCell.RowIndex].Value = "Tunai";
                    dataGridView4[6, dataGridView4.CurrentCell.RowIndex].Value = "LUNAS";
                }
                else
                {
                    App.executeNonQuery("UPDATE pembeliancompact SET JatuhTempo = '" + dateTimePicker2.Value.ToShortDateString() + "', Keterangan = '" + textBox3.Text + "' WHERE Faktur = '" + dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "' AND Tanggal = '" + dataGridView4[0, dataGridView4.CurrentCell.RowIndex].Value.ToString() + "'");
                    dataGridView4[4, dataGridView4.CurrentCell.RowIndex].Value = dateTimePicker2.Value.ToShortDateString();
                }

                dataGridView4[5, dataGridView4.CurrentCell.RowIndex].Value = textBox3.Text;

            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            double penjualantotal = 0, pembeliantotal = 0, pembelianlunas = 0;

            //penjualan
            dataGridView6.Rows.Clear();
            DataTable dt = App.executeReader("SELECT Tanggal, Faktur, Total FROM penjualancompact WHERE Tanggal LIKE '%" + comboBox3.Text + "/" + textBox4.Text + "'");
            foreach (DataRow row in dt.Rows)
            {
                dataGridView6.Rows.Add(row[0], row[1], App.strtomoney(row[2].ToString()));
                penjualantotal += Convert.ToDouble(row[2].ToString());
            }


            //pembelian
            dataGridView7.Rows.Clear();
            DataTable dt2 = App.executeReader("SELECT Tanggal, Faktur, Total, JatuhTempo, Lunas FROM pembeliancompact WHERE Tanggal LIKE '%" + comboBox3.Text + "/" + textBox4.Text + "' AND JatuhTempo = 'Tunai'");
            foreach (DataRow row in dt2.Rows)
            {
                dataGridView7.Rows.Add(row[0], row[1], App.strtomoney(row[2].ToString()), row[3], row[4]);
                pembeliantotal += Convert.ToDouble(row[2].ToString());
                if (row[4].ToString() == "LUNAS")
                {
                    pembelianlunas += Convert.ToDouble(row[2].ToString());
                }
            }

            DataTable dt3 = App.executeReader("SELECT Tanggal, Faktur, Total, JatuhTempo, Lunas FROM pembeliancompact WHERE JatuhTempo LIKE '%" + comboBox3.Text + "/" + textBox4.Text + "'");
            foreach (DataRow row in dt3.Rows)
            {
                dataGridView7.Rows.Add(row[0], row[1], App.strtomoney(row[2].ToString()), row[3], row[4]);
                pembeliantotal += Convert.ToDouble(row[2].ToString());
                if (row[4].ToString() == "LUNAS")
                {
                    pembelianlunas += Convert.ToDouble(row[2].ToString());
                }
            }


            dataGridView6.Rows.Add("", "Total", App.doubletomoney(penjualantotal));
            dataGridView7.Rows.Add("", "Total", App.doubletomoney(pembeliantotal), "Lunas", App.doubletomoney(pembelianlunas));

            dataGridView6[1, dataGridView6.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView6[2, dataGridView6.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView7[1, dataGridView7.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView7[3, dataGridView7.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView7[4, dataGridView7.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);

            dataGridView6[2, dataGridView6.RowCount - 1].Style.ForeColor = Color.Blue;
            dataGridView7[4, dataGridView7.RowCount - 1].Style.ForeColor = Color.Red;

            dataGridView6.FirstDisplayedScrollingRowIndex = dataGridView6.RowCount - 1;
            dataGridView7.FirstDisplayedScrollingRowIndex = dataGridView7.RowCount - 1;

            label3.Text = "Casflow bulan " + comboBox3.Text + " tahun " + textBox4.Text + " adalah " + App.doubletomoney(penjualantotal) + " - " + App.doubletomoney(pembelianlunas) + " = " + App.doubletomoney(penjualantotal - pembelianlunas);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int jumlah = 0;
            decimal subtotal = 0;
            decimal laba = 0;

            dataGridView8.Rows.Clear();
            DataTable dt = App.executeReader("SELECT Nama FROM Kelompok ORDER BY Nama");
            foreach (DataRow row in dt.Rows)
            {
                DataTable dt2 = App.executeReader("SELECT SUM(Jumlah), SUM(Jumlah*HargaBeli), SUM(Jumlah*(HargaJual-HargaBeli)) FROM barang WHERE Kelompok = '" + row[0] + "'");
                foreach (DataRow row2 in dt2.Rows)
                {
                    try
                    {
                        dataGridView8.Rows.Add(row[0].ToString(), row2[0].ToString(), App.strtomoneyasset(row2[1].ToString()), App.strtomoneyasset(row2[2].ToString()), (Convert.ToDecimal(row2[2]) / Convert.ToDecimal(row2[1]) * 100).ToString("#.##"));
                        jumlah += Convert.ToInt32(row2[0].ToString());
                        subtotal += Convert.ToDecimal(row2[1].ToString());
                        laba += Convert.ToDecimal(row2[2].ToString());
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            dataGridView8.Rows.Add("Total", jumlah.ToString(), App.strtomoneyasset(subtotal.ToString()), App.strtomoneyasset(laba.ToString()));

            dataGridView8[0, dataGridView8.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView8[1, dataGridView8.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView8[2, dataGridView8.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);
            dataGridView8[3, dataGridView8.RowCount - 1].Style.Font = new Font("Consolas", 10, FontStyle.Bold);

            dataGridView8[1, dataGridView8.RowCount - 1].Style.ForeColor = Color.Blue;
            dataGridView8[2, dataGridView8.RowCount - 1].Style.ForeColor = Color.Red;
            dataGridView8[3, dataGridView8.RowCount - 1].Style.ForeColor = Color.Green;

            dataGridView8.FirstDisplayedScrollingRowIndex = dataGridView8.RowCount - 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTime tgl = DateTime.Now;

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "Kontra Bon BABY");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Faktur: " + dataGridView4[1, dataGridView4.CurrentCell.RowIndex].Value.ToString());
            sb.AppendLine("Tanggal: " + dataGridView4[0, dataGridView4.CurrentCell.RowIndex].Value.ToString());
            sb.AppendLine("");
            sb.AppendLine("========================================");
            sb.AppendLine("");
            if (radioButton1.Checked == false)
            {
                sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "Jatuh Tempo:");
                sb.AppendLine(dateTimePicker2.Value.ToShortDateString());
                sb.AppendLine(Convert.ToChar(27) + "@");

            }
            else
            {
                sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "LUNAS");
                sb.AppendLine(Convert.ToChar(27) + "@");
            }
            sb.AppendLine("");
            sb.AppendLine("Keterangan:");
            sb.AppendLine(textBox3.Text);

            sb.AppendLine("-----------------------------------------");
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\invoicepembeliankontrabon.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\invoicepembeliankontrabon.txt " + Args.printer);


        }

        private void button7_Click(object sender, EventArgs e)
        {
            //string[] seriesArray = { "Cats", "Dogs" };
            //int[] pointsArray = { 1, 2 };

            //// Set palette.
            //this.chart1.Palette = ChartColorPalette.SeaGreen;

            //// Set title.
            //this.chart1.Titles.Add("Penjualan");

            //// Add series.
            //for (int i = 0; i < seriesArray.Length; i++)
            //{
            //    // Add series.
            //    Series series = this.chart1.Series.Add(seriesArray[i]);

            //    // Add point.
            //    series.Points.Add(pointsArray[i]);
            //}

            //foreach (var Series in chart1.Series)
            //{
            //    Series.Points.Clear();
            //}
            chart1.Series.Clear();
            chart1.Series.Add("Penjualan");
            chart1.Series.Add("Pembelian");
            chart1.Series["Penjualan"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series["Pembelian"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series["Penjualan"].IsValueShownAsLabel = true;
            chart1.Series["Pembelian"].IsValueShownAsLabel = true;



            DataTable dtTanggal = App.executeReader("SELECT DISTINCT Tanggal FROM penjualancompact WHERE Tanggal LIKE '%" + comboBox4.Text + "/" + textBox5.Text + "'");
            foreach (DataRow rowtanggal in dtTanggal.Rows)
            {


                double dtPenjualan = Convert.ToDouble(App.executeScalar("SELECT SUM(Total) FROM penjualancompact WHERE Tanggal = '" + rowtanggal[0].ToString() + "'").ToString());
                chart1.Series["Penjualan"].Points.AddXY(rowtanggal[0].ToString(), dtPenjualan);
                chart1.Series["Penjualan"].Label = "#VALY{###,###,###}";

                try
                {
                    double dtPembelian = Convert.ToDouble(App.executeScalar("SELECT SUM(Total) FROM pembeliancompact WHERE Tanggal = '" + rowtanggal[0].ToString() + "'").ToString());
                    chart1.Series["Pembelian"].Points.AddXY(rowtanggal[0].ToString(), dtPembelian);
                    chart1.Series["Pembelian"].Label = "#VALY{###,###,###}";
                }
                catch (Exception)
                {
                }

            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.Series.Add("Penjualan");
            chart1.Series["Penjualan"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series["Penjualan"].IsValueShownAsLabel = true;
            chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0:C0}";

            DataTable dtTanggal = App.executeReader("SELECT DISTINCT Name FROM Users");
            foreach (DataRow rowuser in dtTanggal.Rows)
            {
                try
                {
                    double dtPenjualan = Convert.ToDouble(App.executeScalar("SELECT SUM(Total) FROM penjualancompact WHERE User = '" + rowuser[0].ToString() + "' AND Tanggal LIKE '%" + comboBox4.Text + "/" + textBox5.Text + "'").ToString());
                    chart1.Series["Penjualan"].Points.AddXY(rowuser[0].ToString(), dtPenjualan);
                    chart1.Series["Penjualan"].Label = "#VALY{Rp###,###,###}";
                }
                catch (Exception)
                {
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            loadUsers();
        }

        private void loadUsers()
        {
            dataGridView9.Rows.Clear();
            DataTable dt = App.executeReader("SELECT ID, Name FROM users ORDER BY ID");
            foreach (DataRow row in dt.Rows)
            {
                dataGridView9.Rows.Add(row[0].ToString(), row[1].ToString());
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView9.CurrentCell.RowIndex != -1)
            {
                DialogResult result = MessageBox.Show("Hapus user ini? " + dataGridView9[1, dataGridView9.CurrentCell.RowIndex].Value.ToString(), "Hapus", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    App.executeNonQuery("DELETE FROM Users WHERE Name = '" + dataGridView9[1, dataGridView9.CurrentCell.RowIndex].Value.ToString() + "'");
                    loadUsers();
                    MessageBox.Show("User berhasil dihapus");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (groupBox1.Enabled == false)
            {
                groupBox1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "")
            {
                DialogResult result = MessageBox.Show("Simpan user ini?", "Simpan", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    App.executeNonQuery("INSERT INTO users SET ID = '" + textBox6.Text + "', Name ='" + textBox7.Text + "'");
                    loadUsers();
                    MessageBox.Show("User berhasil disimpan");
                }
            }
        }

        private void loadServisCacad(bool semuayangbelumselesai)
        {
            dataGridView10.Rows.Clear();

            DataTable dt;
            if (semuayangbelumselesai == true)
            {
                dt = App.executeReader("SELECT * FROM retur WHERE Status = 'Belum Selesai'");
            }
            else
            {
                dt = App.executeReader("SELECT * FROM retur WHERE Tanggal LIKE '%" + comboBox5.Text + "/" + textBox8.Text + "'");
            }

            foreach (DataRow row in dt.Rows)
            {
                dataGridView10.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), App.strtomoney(row[5].ToString()), App.strtomoney(row[6].ToString()), row[7].ToString(), row[8].ToString(), row[9].ToString());
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            loadServisCacad(true);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            loadServisCacad(false);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = "SELESAI";
                checkBox1.ForeColor = Color.Green;
            }
            else
            {
                checkBox1.Text = "Belum Selesai";
                checkBox1.ForeColor = Color.Red;
            }
        }

        private void dataGridView10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            groupBox2.Enabled = true;

            textBox9.Text = dataGridView10[0, dataGridView10.CurrentCell.RowIndex].Value.ToString();
            textBox10.Text = dataGridView10[1, dataGridView10.CurrentCell.RowIndex].Value.ToString();
            textBox11.Text = dataGridView10[2, dataGridView10.CurrentCell.RowIndex].Value.ToString();
            textBox12.Text = dataGridView10[3, dataGridView10.CurrentCell.RowIndex].Value.ToString();
            textBox13.Text = dataGridView10[4, dataGridView10.CurrentCell.RowIndex].Value.ToString();
            textBox14.Text = App.strtomoneyasset(dataGridView10[5, dataGridView10.CurrentCell.RowIndex].Value.ToString());
            textBox15.Text = App.strtomoneyasset(dataGridView10[6, dataGridView10.CurrentCell.RowIndex].Value.ToString());
            textBox16.Text = dataGridView10[7, dataGridView10.CurrentCell.RowIndex].Value.ToString();
            textBox17.Text = dataGridView10[8, dataGridView10.CurrentCell.RowIndex].Value.ToString();

            if (dataGridView10[9,dataGridView10.CurrentCell.RowIndex].Value.ToString() == "SELESAI")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }

            textBox17.Focus();

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (textBox17.Text != "")
            {
                string selesai;
                if (checkBox1.Checked == true)
                {
                    selesai = "SELESAI";
                }
                else
                {
                    selesai = "Belum Selesai";
                }

                DialogResult result = MessageBox.Show("Ubah keterangan atau status barang ini?", "Keterangan", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    App.executeNonQuery("UPDATE retur SET Alasan = '" + textBox17.Text + "', Status = '" + selesai + "' WHERE Tanggal = '"+ textBox9.Text +"' AND Faktur = '"+ textBox10.Text +"' AND KodeBarang = '"+ textBox11.Text +"' AND User = '"+ textBox16.Text +"', AND Alasan = '"+ dataGridView10[8,dataGridView10.CurrentCell.RowIndex].Value.ToString() +"'");
                    dataGridView10[8, dataGridView10.CurrentCell.RowIndex].Value = textBox17.Text;
                    dataGridView10[9, dataGridView10.CurrentCell.RowIndex].Value = selesai;
                    MessageBox.Show("Barang servis / cacad berhasil di update");

                    groupBox2.Enabled = false;
                }
            }
        }

        private void Laporan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
