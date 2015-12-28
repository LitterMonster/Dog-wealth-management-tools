using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace big_project
{
    public partial class MonthList : Form
    {
        public MonthList()
        {
            InitializeComponent();
        }
        private FileStream fs;

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //清空所有的textbox
        private void clear_textbox()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            label17.Text = "";
            clear_textbox();
            string month_file = numericUpDown1.Value + "年" + comboBox1.Text + "月" + ".db";
            if (File.Exists(month_file))
            {
                //从文件中读取数据
                fs = new FileStream(month_file, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                string temp = br.ReadString();
                textBox1.Text = br.ReadString();
                textBox2.Text = br.ReadString();
                textBox3.Text = br.ReadString();
                textBox4.Text = br.ReadString();
                textBox5.Text = br.ReadString();
                textBox6.Text = br.ReadString();
                textBox7.Text = br.ReadString();

                fs.Close();
                br.Close();
            }
            else
            {
                label17.Text = "该月没有消费记录！";
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                comboBox1_DropDownClosed(sender, e);
            }
            else
            {
                label17.Text = "请选择月份!";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MonthChart mc = new MonthChart();
            mc.ShowDialog();
        }
    }
}
