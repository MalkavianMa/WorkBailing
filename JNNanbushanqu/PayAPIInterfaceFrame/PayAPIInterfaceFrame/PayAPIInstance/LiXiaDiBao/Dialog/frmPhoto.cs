using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.LiXiaDiBao.Dialog
{
    public partial class frmPhoto : Form
    {
        bool bz=false;
        Image photo;
        public frmPhoto(Image _photo)
        {
            InitializeComponent();
            photo = _photo;
        }

        private void frmPhoto_Shown(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = photo;
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            this.Width= pictureBox1.Width;
            this.Height = pictureBox1.Height;

            bz = true;
            
        }

        private void frmPhoto_Resize(object sender, EventArgs e)
        {
            if (bz == true)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Width = this.Width;
                pictureBox1.Height = this.Height;
            }
        }
    }
}
