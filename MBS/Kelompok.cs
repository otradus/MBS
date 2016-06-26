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
    public partial class Kelompok : Form
    {
        public Kelompok()
        {
            InitializeComponent();
        }

        private void Kelompok_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            App.loadTable(dataGridView1, "SELECT * FROM kelompok ORDER BY Nama");
            this.ActiveControl = textBox1;
        }

        public void insertKelompok()
        {
            if (textBox1.Text != "")
            {
                App.executeNonQuery("INSERT INTO kelompok SET Nama = '" + textBox1.Text + "'");
                MessageBox.Show("Item has been saved successfully");
                App.loadTable(dataGridView1, "SELECT * FROM kelompok ORDER BY Nama");
                textBox1.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            insertKelompok();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.CurrentRow.Selected == true)
                {
                    DialogResult result = MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        App.executeNonQuery("DELETE FROM kelompok WHERE nama = '" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "' LIMIT 1");
                        MessageBox.Show("Item has been deleted");
                        App.loadTable(dataGridView1, "SELECT * FROM kelompok ORDER BY Nama");
                    }
                    else
                    {
                        MessageBox.Show("NOT DELETED");
                    }
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                insertKelompok();
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
