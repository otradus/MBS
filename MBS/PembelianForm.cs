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
    public partial class PembelianForm : Form
    {
        public PembelianForm()
        {
            InitializeComponent();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                groupBox1.Enabled = true;
                button6.Enabled = true;
            }else
            {
                groupBox1.Enabled = false;
                button6.Enabled = false;
            }
        }

        private void PembelianForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            textBox1.Text = DateTime.Now.ToShortDateString() + "-";
        }

        private string getNo()
        {
            int num = dataGridView1.Rows.Count + 1;
            return num.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(
                getNo(),
                textBox2.Text,
                textBox3.Text,
                textBox4.Text,
                textBox5.Text,
                textBox6.Text,
                textBox7.Text,
                textBox8.Text,
                textBox9.Text,
                textBox10.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.CharacterCasing = CharacterCasing.Upper;

            DataTable rdr = App.executeReader("SELECT NamaBarang, Kelompok, Satuan , HargaJual, HargaBeli From barang WHERE KodeBarang = '" + textBox2.Text + "'");

            if (rdr.Rows.Count != 0)
            {
                foreach (DataRow row in rdr.Rows)
                {
                    textBox3.Text = row[0].ToString();
                    textBox4.Text = row[1].ToString();
                    textBox6.Text = row[2].ToString();
                    textBox7.Text = App.strtomoney(row[3].ToString());
                    textBox8.Text = App.strtomoney(row[4].ToString());
                }

            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox5.Text != "")
                {
                    if (textBox10.Text != "")
                    {

                        textBox9.Text = (Convert.ToInt32(textBox5.Text) - Convert.ToInt32(textBox10.Text)).ToString();
                    }
                    else
                    {
                        textBox9.Text = textBox5.Text;
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox9.Text = textBox5.Text;
                textBox10.Text = "0";
            }
            else
            {
                textBox10.Text = textBox5.Text;
                textBox9.Text = "0";
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox5.Text != "")
                {
                    if (textBox9.Text != "")
                    {

                        textBox10.Text = (Convert.ToInt32(textBox5.Text) - Convert.ToInt32(textBox9.Text)).ToString();
                    }
                    else
                    {
                        textBox10.Text = textBox5.Text;
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Text = textBox5.Text;
            textBox10.Text = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox10.Text = textBox5.Text;
            textBox9.Text = "0";
        }
    }
}
