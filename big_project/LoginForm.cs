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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public int getFlag()
        {
            return flag;
        }
        private  int flag = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("密码不能为空!", "密码错误", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                textBox1.Focus();
            }
            else
            {
                FileStream fs = new FileStream("password.db", FileMode.Open, FileAccess.Read);
                BinaryReader bw = new BinaryReader(fs);
                string s = bw.ReadString();
                if (s == textBox1.Text)
                {
                    flag = 1;
                    Close();
                }
                else
                {
                    MessageBox.Show("密码错误!");
                    textBox1.Clear();
                    textBox1.Focus();
                }
                fs.Close();
                bw.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
