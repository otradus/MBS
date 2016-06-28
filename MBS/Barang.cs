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
    public partial class Barang : Form
    {
        public static bool add = false;
        public static bool edit = false;

        public Barang()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            comboBox1.Text = "";
            groupBox1.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            edit = false;
            add = false;
        }

        private void Barang_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            //App.loadTable(dataGridView1, "SELECT * FROM Barang");
            App.loadComboBox(comboBox1, "SELECT * FROM kelompok");

            this.ActiveControl = textBox1;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                App.loadTable(dataGridView1, "SELECT * FROM BARANG WHERE NamaBarang LIKE '%" + textBox1.Text + "%'");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            edit = true;
            add = false;

            try
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    groupBox1.Enabled = true;

                    int col = dataGridView1.CurrentRow.Index;

                    textBox2.Text = dataGridView1[0, col].Value.ToString();
                    textBox3.Text = dataGridView1[1, col].Value.ToString();
                    comboBox1.Text = dataGridView1[2, col].Value.ToString();
                    textBox4.Text = dataGridView1[3, col].Value.ToString();
                    textBox5.Text = dataGridView1[4, col].Value.ToString();
                    textBox6.Text = dataGridView1[5, col].Value.ToString();
                    textBox7.Text = dataGridView1[6, col].Value.ToString();
                }
                else
                {
                    MessageBox.Show("Silahkan pilih barang yang akan di ubah.");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Silahkan pilih barang yang akan di ubah.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
            add = true;
            edit = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kelompok kelompok = new Kelompok();
            kelompok.ShowDialog();
        }
    }
}
