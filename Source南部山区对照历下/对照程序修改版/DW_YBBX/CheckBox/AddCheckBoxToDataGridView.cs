using System;
using System.Collections.Generic;
using System.Drawing;

namespace CheckBox
{
    /// <summary>  
    /// 给DataGridView添加全选  
    /// </summary>  
    public class AddCheckBoxToDataGridView
    {
        public static System.Windows.Forms.DataGridView dgv;
        public static void AddFullSelect()
        {
            System.Windows.Forms.CheckBox ckBox = new System.Windows.Forms.CheckBox();
            ckBox.Text = "";
            ckBox.Checked = false;
            System.Drawing.Rectangle rect = dgv.GetCellDisplayRectangle(0, -1, true);
            ckBox.Size = new System.Drawing.Size(13, 13);
            ckBox.Location = new Point(rect.Location.X + dgv.Columns[0].Width / 2 - 13 / 2 - 1, rect.Location.Y + 7);
            //ckBox.Location.Offset(-40, rect.Location.Y);  
            ckBox.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
            dgv.Controls.Add(ckBox);
        }
        static void ckBox_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[0].Value = ((System.Windows.Forms.CheckBox)sender).Checked;
            }
            dgv.EndEdit();
        }  
    }  
}
/*
 * 在页面 _Load事件中加入代码即可 
CheckBox.AddCheckBoxToDataGridView.dgv = 你的datagridview的id;  
CheckBox.AddCheckBoxToDataGridView.AddFullSelect(); 
 * 
 */