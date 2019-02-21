using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    public partial class Formsfyybk : Form
    {
        public Formsfyybk()
        {
            InitializeComponent();
        }
        public  bool sfyybk = false;
        private void button1_Click(object sender, EventArgs e)
        {
               sfyybk = true;
               this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sfyybk = false;
            this.Hide();
        }

       
    }
}
