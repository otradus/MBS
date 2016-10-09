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
    public partial class LorisanForm : Form
    {
        public LorisanForm()
        {
            InitializeComponent();
        }

        private void LorisanForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.DoubleBuffered(dataGridView1, true);
            App.loadTable(dataGridView1, "SELECT * FROM lorisan");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printLorisan();
            App.executeNonQuery("DELETE FROM lorisan");
            Close();
        }

        private void printLorisan()
        {
            DateTime tgl = DateTime.Now;

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "LORISAN");
            sb.AppendLine("Baby");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("========================================");

            int jumlah, lorisan;
            bool loris;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                loris = Convert.ToBoolean(App.executeScalar("SELECT Lorisan FROM barang WHERE KodeBarang = '" + dataGridView1[1, i].Value.ToString() + "'"));

                if (loris == true)
                {
                    jumlah = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM barang WHERE KodeBarang = '" + dataGridView1[1, i].Value.ToString() + "'"));
                    lorisan = Convert.ToInt32(dataGridView1[3, i].Value.ToString());

                    if (jumlah > 0)
                    {
                        if (jumlah >= lorisan)
                        {
                            sb.AppendLine(dataGridView1[2, i].Value.ToString() + " ... " + lorisan.ToString());
                        }
                        else
                        {
                            sb.AppendLine(dataGridView1[2, i].Value.ToString() + " ... " + jumlah.ToString());
                        }
                    }
                }

                
            }

            sb.AppendLine("----------------------------------------");
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\lorisanbaby.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\lorisanbaby.txt " + Args.printer);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            App.shellCommand("copy c:\\test\\lorisanbaby.txt " + Args.printer);
        }
    }
}
