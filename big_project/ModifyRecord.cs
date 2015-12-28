using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace big_project
{
    public partial class ModifyRecord : Form
    {
        private string item;
        private string type;
        private string money;
        private int state = 0;
        public ModifyRecord(string item, string type, string money)
        {
            InitializeComponent();
            textBox1.Text = item;
            comboBox1.Text = type;
            textBox2.Text = money;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //判断某串文本是否是数字
        private bool is_Number(string text)
        {
            bool res = false;
            try
            {
                Convert.ToDouble(text);
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("消费项目不能为空!");
                textBox1.Text = "";
                textBox1.Focus();
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择消费分类!");
            }
            else if (is_Number(textBox2.Text) == false)
            {
                MessageBox.Show("消费金额必须为数字!");
                textBox2.Text = "";
                textBox2.Focus();
            }
            else
            {
                state = 1;
                item = textBox1.Text;
                type = comboBox1.Text;
                money = textBox2.Text;
                this.Close();
            }
        }

        public int getState()
        {
            return state;
        }

        public string getItem()
        {
            return item;
        }

        public string getType()
        {
            return type;
        }

        public string getMoney()
        {
            return money;
        }
    }
}
