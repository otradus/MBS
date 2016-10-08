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
    public partial class RevisiForm : Form
    {
        public RevisiForm()
        {
            InitializeComponent();
        }

        private void RevisiForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.formatDataGridView(dataGridView2);
            App.DoubleBuffered(dataGridView1, true);
            App.DoubleBuffered(dataGridView2, true);

            //            DataTable dt = App.executeReader("SELECT Faktur FROM penjualan WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "'");
            DataTable dt = App.executeReader("SELECT Faktur FROM penjualancompact WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "' AND Bayar = '0'");

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row[0]);

            }
        }


        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult result = MessageBox.Show("Batalkan barang ini?", "REVISI", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    App.executeNonQuery("UPDATE penjualan SET Jumlah = '0' , Subtotal = '0', Laba = '0' WHERE Faktur = '" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "' AND KodeBarang = '" + dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() + "'");

                    //Return jumlah to barang
                    App.executeNonQuery("UPDATE barang SET Jumlah = Jumlah + '"+ dataGridView2[2, dataGridView2.CurrentRow.Index].Value.ToString() +"' WHERE KodeBarang = '" + dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() + "'");

                    MessageBox.Show("Barang sudah dibatalkan dari penjualan");

                    double lababarang = Convert.ToDouble(App.executeScalar("SELECT HargaJual-HargaBeli FROM barang WHERE KodeBarang = '" + dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() + "'"));
                    double lababarangjumlah = lababarang * Convert.ToInt32(dataGridView2[2, dataGridView2.CurrentRow.Index].Value.ToString());


                    dataGridView2.Rows.Clear();

                    string faktur = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                    DataTable dt = App.executeReader("SELECT KodeBarang, NamaBarang, Jumlah, Harga, Subtotal FROM penjualan WHERE Faktur = '" + faktur + "'");

                    double total = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView2.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
                        total += Convert.ToDouble(row[4].ToString());
                    }

                    
                    App.executeNonQuery("UPDATE penjualancompact SET Total = '" + total.ToString() + "', Laba = Laba - '"+ lababarangjumlah.ToString() +"' WHERE Faktur = '" + faktur + "'");

                    label1.Text = "Faktur: " + faktur;
                    label2.Text = "Total: " + App.strtomoney(total.ToString());

                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            string faktur = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            DataTable dt = App.executeReader("SELECT KodeBarang, NamaBarang, Jumlah, Harga, Subtotal FROM penjualan WHERE Faktur = '" + faktur + "'");

            double total = 0;
            foreach (DataRow row in dt.Rows)
            {
                dataGridView2.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
                total += Convert.ToDouble(row[4].ToString());
            }

            label1.Text = "Faktur: " + faktur;
            label2.Text = "Total: " + App.strtomoney(total.ToString());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            App.printPenjualan(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString(), "REVISI");
            this.Close();
        }

        private void RevisiForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
