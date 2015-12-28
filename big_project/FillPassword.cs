using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace big_project
{
    public partial class FillPassword : Form
    {
        private int flag = 0;
        public FillPassword()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public int getFlag()
        {
            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (verify_password(textBox1) == true)
            {
                if (verify_password(textBox2) == true)
                {
                    if (textBox1.Text == textBox2.Text)
                    {
                        try
                        {
                            FileStream fs = new FileStream("password.db", FileMode.CreateNew);
                            BinaryWriter bw = new BinaryWriter(fs);

                            bw.Write(textBox1.Text);
                            fs.Close();
                            bw.Close();
                            flag = 1;
                            MessageBox.Show("新密码注册成功", "注册成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("不能注册新的密码，可能新的密码已经存在", "注册失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        label4.Text = "两次密码不一样，请重新输入!";
                        textBox1.Text = textBox2.Text = "";
                    }
                }
            }
        }

        private bool is_chinese(string text)
        {
            bool ret = false;
            if (Regex.IsMatch(text, @"[\u4e00-\u9fbb]+$"))
                ret = true;
            return ret;
        }

        private bool verify_password(TextBox textbox1)
        {
            label4.Text = "(密码不能为中文，且长度大于6位！)";
            if (textbox1.Text == "")
            {
                MessageBox.Show("密码不能为空!", "填写错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textbox1.Focus();
            }
            else
            {
                if (is_chinese(textbox1.Text) == false)
                {
                    if (textbox1.Text.Length < 6)
                    {
                        label4.Text = "密码长度不能小于6！";
                        textbox1.Focus();
                        return false;
                    }
                    return true;
                }
                else
                {
                    label4.Text = "密码不能包括中文!";
                    textbox1.Focus();
                }
            }
            return false;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }



        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (verify_password(textBox1) == true)
            {
 
            }
        }
    }
}
