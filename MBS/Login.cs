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
    public partial class Login : Form
    {
        public int tries = 0;
        public bool validate = false;

        public Login()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.ToUpper() == "MAREMA168" || textBox1.Text.ToUpper() == "314159")
                {
                    validate = true;
                    this.Close();
                }
                else if (textBox1.Text.ToUpper() == "BB")
                {
                    validate = true;
                    Args.admin = false;
                    this.Close();
                }
                else if (textBox1.Text.ToUpper() == "SETTINGS")
                {
                    textBox1.Text = "";
                    Settings settings = new Settings();
                    settings.ShowDialog();
                    this.Close();
                }
                else
                {
                    tries += 1;
                    textBox1.Text = "";
                    if (tries > 2)
                    {
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

       

        private void Login_Load(object sender, EventArgs e)
        {
            if (Args.admin == false)
            {
                validate = true;
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (validate == false)
            {
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}
