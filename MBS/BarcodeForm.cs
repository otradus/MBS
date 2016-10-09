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
    public partial class BarcodeForm : Form
    {
        static string kode;
        static string nama;
        static string harga;
        public BarcodeForm(string kode1, string nama1, string harga1)
        {
            kode = kode1;
            nama = nama1;
            harga = App.strtomoney(harga1);
            InitializeComponent();
        }

        private void BarcodeForm_Load(object sender, EventArgs e)
        {
            label1.Text = kode;
            label2.Text = nama;
            label3.Text = harga;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int queue = (int)Math.Ceiling(Convert.ToDouble(textBox1.Text) / 3);
                App.printBarcode(kode, nama, harga, queue.ToString(), Args.printerbarcode);
                Close();
            }
        }
    }
}
