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
        Random rnd = new Random();


        public Barang()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string jumlah, gudang, hargabeli, hargajual, pengambilangudang, batasgudang, opname, loris;
            jumlah = textBox5.Text != "" ? textBox5.Text : "0";
            gudang = textBox8.Text != "" ? textBox8.Text : "0";
            hargabeli = textBox6.Text != "" ? textBox6.Text : "0";
            hargajual = textBox7.Text != "" ? textBox7.Text : "0";
            pengambilangudang = textBox9.Text != "" ? textBox9.Text : "0";
            batasgudang = textBox10.Text != "" ? textBox10.Text : "0";
            opname = DateTime.Now.ToShortDateString();
            loris = checkBox1.Checked ? "1" : "0";

            if (add == true && edit == false)
            {
                if (findDuplicate(textBox2.Text) == false)
                {
                    try
                    {
                        App.executeNonQuery(string.Format("INSERT INTO barang VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                           textBox2.Text,
                           textBox3.Text,
                           comboBox1.Text,
                           textBox4.Text.ToUpper(),
                           jumlah,
                           App.stripMoney(hargabeli),
                           App.stripMoney(hargajual),
                           gudang,
                           pengambilangudang,
                           batasgudang,
                           opname,
                           loris
                           ));
                        MessageBox.Show("Barang berhasil dimasukkan");
                        loadBarang();
                        clearAll();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Kode Barang sudah ada!");
                }
            }
            else if (add == false && edit == true)
            {
                if (findDuplicate(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()) == true)
                {
                    try
                    {
                        App.executeNonQuery(string.Format("UPDATE barang SET KodeBarang = '{0}', NamaBarang = '{1}', Kelompok = '{2}', Satuan = '{3}', Jumlah = '{4}', HargaBeli = '{5}', HargaJual = '{6}', Gudang = '{7}', PengambilanGudang = '{8}', BatasGudang = '{9}', Opname = '{10}', Lorisan = '{11}' WHERE KodeBarang = '" + dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString() + "'",
                           textBox2.Text,
                           textBox3.Text,
                           comboBox1.Text,
                           textBox4.Text.ToUpper(),
                           jumlah,
                           App.stripMoney(hargabeli),
                           App.stripMoney(hargajual),
                           gudang,
                           pengambilangudang,
                           batasgudang,
                           opname,
                           loris
                           ));

                        MessageBox.Show("Barang berhasil dirubah");
                        loadBarang();
                        clearAll();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("KodeBarang yang akan diedit belum ada!");
                }
            }
        }

        private bool findDuplicate(string kode)
        {
            string count = App.executeScalar("SELECT COUNT(*) FROM barang WHERE KodeBarang = '" + kode + "'").ToString();
            if (count == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void clearAll()
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

            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox4.CharacterCasing = CharacterCasing.Upper;

            this.ActiveControl = textBox1;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadBarang();
            }
        }

        private void loadBarang()
        {
            if (textBox1.Text != "")
            {
                App.loadTable(dataGridView1, "SELECT * FROM BARANG WHERE NamaBarang LIKE '%" + textBox1.Text + "%'");
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("HAPUS barang ini? " + dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString(), "DELETE", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                try
                {
                    App.executeNonQuery("DELETE FROM barang WHERE KodeBarang = '" + dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString() + "' LIMIT 1");
                    MessageBox.Show("Barang berhasil dihapus.");
                    loadBarang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void datagridToBox()
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                int col = dataGridView1.CurrentRow.Index;

                textBox2.Text = dataGridView1[0, col].Value.ToString();
                textBox3.Text = dataGridView1[1, col].Value.ToString();
                comboBox1.Text = dataGridView1[2, col].Value.ToString();
                textBox4.Text = dataGridView1[3, col].Value.ToString();
                textBox5.Text = dataGridView1[4, col].Value.ToString();
                textBox6.Text = dataGridView1[5, col].Value.ToString();
                textBox7.Text = dataGridView1[6, col].Value.ToString();
                textBox8.Text = dataGridView1[7, col].Value.ToString();
                textBox9.Text = dataGridView1[8, col].Value.ToString();
                textBox10.Text = dataGridView1[9, col].Value.ToString();

                if (dataGridView1[11, col].Value.ToString() == "1")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    edit = true;
                    add = false;
                    button4.Enabled = false;
                    button6.Enabled = false;
                    groupBox1.Enabled = true;
                    datagridToBox();
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
            textBox2.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kelompok kelompok = new Kelompok();
            kelompok.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (add == false && edit == true)
            {
                datagridToBox();
            }
        }


        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (edit == true && add == false)
            {
                try
                {
                    int jumlahawal = Convert.ToInt32(dataGridView1[4, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                    int result;
                    if (textBox5.Text != "")
                    {
                        result = Convert.ToInt32(textBox5.Text) - jumlahawal;
                        label11.Text = result.ToString();
                    }
                    else
                    {
                        label11.Text = "";
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            }
        }

        //TODO: Check for double kode
        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.Text = rnd.Next(999, 9999).ToString();
            textBox3.Text = textBox2.Text + " ";
            textBox3.Focus();
            textBox3.SelectionStart = textBox3.Text.Length;
        }
    }
}
