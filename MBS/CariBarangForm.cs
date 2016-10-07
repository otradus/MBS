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
    public partial class CariBarangForm : Form
    {
        public string valueFromCari;

        public CariBarangForm()
        {
            InitializeComponent();
        }

        private void CariBarangForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.autoSizeDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            //DataTable table = App.executeReader("SELECT Kode, Nama, Stok, Harga FROM barang");
            //foreach (DataRow row in table.Rows)
            //{
            //    dataGridView1.Rows.Add(row[0], row[1], row[2], App.strtomoney(row[3].ToString()));
            //}

            this.ActiveControl = textBox1;
        }


        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            valueFromCari = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            this.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                valueFromCari = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                this.Close();
            }

            if (e.KeyCode == Keys.Up)
            {
                if (dataGridView1.CurrentRow.Index == 0)
                {
                    textBox1.Focus();
                    textBox1.SelectAll();
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text != "")
                {
                    dataGridView1.Rows.Clear();
                    DataTable table = App.executeReader("SELECT KodeBarang, NamaBarang, Jumlah, Satuan, HargaJual FROM barang WHERE NamaBarang LIKE '%" + textBox1.Text + "%'");
                    foreach (DataRow row in table.Rows)
                    {
                        dataGridView1.Rows.Add(row[0], row[1], row[2], row[3], App.strtomoney(row[4].ToString()));
                    }
                    textBox1.Text = "";
                }
            }

            if (e.KeyCode == Keys.Down)
            {
                dataGridView1.Focus();

            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

        }
    }
}
