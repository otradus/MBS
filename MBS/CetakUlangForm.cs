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
    public partial class CetakUlangForm : Form
    {
        public CetakUlangForm()
        {
            InitializeComponent();
        }

        public void loadCetakTable(String tanggal)
        {
            //dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            //string tanggal = monthCalendar1.SelectionRange.Start.ToShortDateString();

            if (radioButton1.Checked == true)
            {
                DataTable dt = App.executeReader("SELECT Faktur FROM penjualancompact WHERE Tanggal = '" + tanggal + "'");

                foreach (DataRow row in dt.Rows)
                {
                    dataGridView2.Rows.Add(row[0]);
                }
            }
            else if (radioButton2.Checked == true)
            {
                DataTable dt = App.executeReader("SELECT Faktur FROM pembeliancompact WHERE Tanggal = '" + tanggal + "'");

                foreach (DataRow row in dt.Rows)
                {
                    dataGridView2.Rows.Add(row[0]);
                }
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            loadCetakTable(monthCalendar1.SelectionRange.Start.ToShortDateString());
        }

        private void CetakUlangForm_Load(object sender, EventArgs e)
        {
            App.DoubleBuffered(dataGridView1, true);
            App.DoubleBuffered(dataGridView2, true);
            App.formatDataGridView(dataGridView1);
            App.formatDataGridView(dataGridView2);

            loadCetakTable(DateTime.Now.ToShortDateString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string faktur = dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString();
            if (radioButton1.Checked == true)
            {
                App.printPenjualan(faktur, "COPY");
            }
            else if (radioButton2.Checked == true)
            {
                App.printPembelian(faktur, true);
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            string faktur = dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString();
            if (radioButton1.Checked == true)
            {
                App.loadTable(dataGridView1, "SELECT * FROM penjualan WHERE Faktur = '" + faktur + "'");
            }
            else if (radioButton2.Checked == true || radioButton3.Checked == true)
            {
                App.loadTable(dataGridView1, "SELECT * FROM pembelian WHERE Faktur = '" + faktur + "'");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            monthCalendar1.SetDate(DateTime.Now);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            monthCalendar1.SetDate(DateTime.Now);
        }

        private void CetakUlangForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

        }
    }
}
