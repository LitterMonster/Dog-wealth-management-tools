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
    public partial class ModifyPassword : Form
    {
        public ModifyPassword()
        {
            InitializeComponent();
        }

        private bool is_chinese(string text)
        {
            bool ret = false;
            if (Regex.IsMatch(text, @"[\u4e00-\u9fbb]+$"))
                ret = true;
            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        try
                        {
                            File.Delete("password.db");
                            FileStream fs = new FileStream("password.db", FileMode.CreateNew);
                            BinaryWriter bw = new BinaryWriter(fs);

                            bw.Write(textBox2.Text);
                            fs.Close();
                            bw.Close();
                            MessageBox.Show("密码修改成功！", "密码修改", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch
                        {
                            label5.Text = "密码修改失败!";
                        }
                    }
                    else
                    {
                        label5.Text = "两次密码不一致！";
                        textBox2.Text = "";
                        textBox3.Text = "";
                    }
                }
                else
                {
                    label5.Text = "有空密码！请填充完整!";
                }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            
        }

        private bool verify_password(TextBox textbox)
        {
            label5.Text = "";
            if (textbox.Text == "")
            {
                label5.Text = "密码不能为空!";
            }
            else
            {
                if (is_chinese(textbox.Text) == false)
                {
                    if (textbox.Text.Length < 6)
                    {
                        label5.Text = "密码长度不能小于6！";
                        return false;
                    }
                    return true;
                }
                else
                {
                    label5.Text = "密码不能包括中文!";
                }
            }
            return false;
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                label5.Text = "新密码不能为空!";
                textBox2.Focus();
            }
            if (verify_password(textBox2) == true)
            {
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                label5.Text = "旧密码不能为空!";
                textBox1.Focus();
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream("password.db", FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);

                    string password = br.ReadString();
                    if (password == textBox1.Text)
                    {
                        label5.Text = "旧密码正确!请输入新密码!";
                        textBox3.Enabled = true;
                    }
                    else
                    {
                        label5.Text = "旧密码错误！请输入正确的旧密码!";
                        textBox1.Focus();
                    }
                    fs.Close();
                    br.Close();
                }
                catch
                {
                    MessageBox.Show("密码文件打开失败!", "打开失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ModifyPassword_Load(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
            button1.Enabled = false;
        }
    }
}
