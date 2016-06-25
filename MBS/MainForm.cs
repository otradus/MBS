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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();

            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            App.loadTable(dataGridView1, "SELECT * FROM barang WHERE Kelompok = 'PIGEON'");
            this.ActiveControl = textBox1;
        }
    }
}
