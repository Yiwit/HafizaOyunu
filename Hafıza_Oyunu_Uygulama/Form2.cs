using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hafıza_Oyunu_Uygulama
{
    public partial class Form2 : Form
    {



        public Form2()
        {
            InitializeComponent();

            lblPuan.Text = "1000";
            lblkalanSure.Text = "30";
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lblkalanSure.Text == "0")
            {

                timer1.Stop();
                MessageBox.Show("ZAMAN DOLDU! ");
                Application.Exit();

            }
            else
            {
                lblkalanSure.Text = (int.Parse(lblkalanSure.Text) - 1).ToString();
                lblPuan.Text = (int.Parse(lblPuan.Text) - 20).ToString();
            }
        }

    }
}

