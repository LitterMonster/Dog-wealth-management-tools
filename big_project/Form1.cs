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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string tempmoney;
        private int Count = 0;
        private double decDaily = 0;
        private double decStudy = 0;
        private double decPhoneCard = 0;
        private double decFood = 0;
        private double decClothes = 0;
        private double decOthers = 0;
        private double decTotalSum = 0;
        private BinaryReader br;
        private double sum_money = 0;
        private FileStream fs;
        private BinaryWriter bw;
        private double[] array = new double[6];
        private void Form1_Load(object sender, EventArgs e)
        {
            
            if (File.Exists("password.db"))
            {
                LoginForm login = new LoginForm();
                login.ShowDialog();
                if (login.getFlag() != 1)
                {
                    this.Close();
                }
            }
            else
            {
                FillPassword new_item = new FillPassword();
                new_item.ShowDialog();
                if (new_item.getFlag() != 1)
                {
                    this.Close();
                }
            }
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
            catch {
                res = false;
            }
            return res;
        }

        //将textbox置为空
        private void clear_textbox(TextBox textbox)
        {
            textbox.Text = "";
        }

        //将box中的信息写入listview
        private void fill_listview(ListView listview)
        {

            ListViewItem lvt = listview.Items.Add(textBox1.Text);
            lvt.SubItems.Add(comboBox1.Text);
            lvt.SubItems.Add(textBox2.Text);
            sum_money += Convert.ToDouble(textBox2.Text);
            textBox3.Text = listview.Items.Count.ToString();
            textBox4.Text = sum_money.ToString();
            clear_box();
        }
        //判断三个box是否有空值
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("消费项目不能为空!");
                clear_textbox(textBox1);
                textBox1.Focus();
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择消费分类!");
            }
            else if (is_Number(textBox2.Text) == false)
            {
                MessageBox.Show("消费金额必须为数字!");
                clear_textbox(textBox2);
                textBox2.Focus();
            }
            else
            {
                fill_listview(listView1);
            }
        }

        //将textbox和combox都清空
        private void clear_box()
        {
            clear_textbox(textBox1);
            clear_textbox(textBox2);
            comboBox1.Text = null;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clear_box();
        }

        private void 加入账本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void 重写账本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_box();
        }

        //保存文件时，文件已存在
        private void file_exists(string date)
        {
            MessageBox.Show(date + "的账目已经存在，请重新选择日期" + "\n\n如果你想修改" + date
                + "的账目， 可以进入 查看/每日清单 里修改", "保存错误", MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
            dateTimePicker1.Focus();
            dateTimePicker1.Value = DateTime.Now;
        }

        //将所有消费记录写入文件
        private void button3_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Text;
            save_day_record(date);
        }

        //当listview中没有消费记录时，给出警告
        private void empty_listview()
        {
            MessageBox.Show("列表里没有账目记录，请添加记录!", "保存错误", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            textBox1.Focus();
        }

        //保存今日消费记录，并写入文件
        private void save_day_record(string date)
        {
            DateTime now_date = DateTime.Now.Date;
            if (File.Exists(date + ".db"))
            {
                file_exists(date);
            }
            else if (listView1.Items.Count == 0)
            {
                empty_listview();
            }
            else if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                after_today();
            }
            else
            {
                DialogResult responseDialogResult;

                //确认是否要保存帐目
                responseDialogResult = MessageBox.Show("确定要保存帐目吗？", "保存确认", 
                    MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (responseDialogResult == DialogResult.Yes)
                {
                    //把帐目写入二进制文件里
                    fs = new FileStream(date + ".db", FileMode.CreateNew);
                    bw = new BinaryWriter(fs);

                    bw.Write(textBox3.Text);
                    bw.Write(textBox4.Text);

                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = 0;
                    }

                    //把列表中的数据逐一写入二进制文件里
                    for (int intRowIndex = 0; intRowIndex < (this.listView1.Items.Count); intRowIndex++)
                    {
                        switch (listView1.Items[intRowIndex].SubItems[1].Text)
                        {
                            case "日常用品":
                                array[0] += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                                break;
                            case "学习用品":
                                array[1] += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                                break;
                            case "电话费":
                                array[2] += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                                break;
                            case "饮食":
                                array[3] += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                                break;
                            case "衣服":
                                array[4] += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                                break;
                            case "其他":
                                array[5] += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                                break;
                        }
                        for (int intColIndex = 0; intColIndex < (this.listView1.Columns.Count); intColIndex++)
                        {
                            bw.Write(this.listView1.Items[intRowIndex].SubItems[intColIndex].Text);
                        }
                    }
                    fs.Close();
                    bw.Close();

                //显示下一个窗口
                ShowSave showsave = new ShowSave(dateTimePicker1, date, array, listView1.Items.Count);
                showsave.ShowDialog();
                clear_box();
                this.listView1.Items.Clear();
                clear_textbox(textBox3);
                clear_textbox(textBox4);
                }
            }
        }

        //删除listview中被选中的一项，但并不直接操作文件
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem li = this.listView1.SelectedItems[0];
                double sum = Convert.ToDouble(textBox4.Text);
                sum -= Convert.ToDouble(li.SubItems[2].Text);
                textBox4.Text = sum.ToString();
                this.listView1.Items.Remove(li);
                textBox3.Text = listView1.Items.Count.ToString();
                
            }
            catch
            {
                MessageBox.Show("请选择删除记录项", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        //清除listview中的所有消费记录，但并不直接操作文件
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                DialogResult responseDialogResult;

                //确认是否要清除所有帐目
                responseDialogResult = MessageBox.Show("确定要清除所有帐目吗？", "保存确认",
                    MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (responseDialogResult == DialogResult.Yes)
                {
                    sum_money = 0;
                    listView1.Items.Clear();
                    clear_textbox(textBox3);
                    clear_textbox(textBox4);
                }
            }
            else
            {
                MessageBox.Show("当前账目记录已为空！", "提示", MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        //每保存新的一天消费记录，就要重写月消费文件
        private void save_month_record(string date)
        {
            fs = new FileStream(date + ".db", FileMode.CreateNew);
            bw = new BinaryWriter(fs);

            //保存年份，月份和当日无消费
            //binaryWriterObject.Write(consumptionDateTimePicker.Value.Year.ToString());
            //binaryWriterObject.Write(consumptionDateTimePicker.Value.Month.ToString());
            bw.Write("无消费记录！");

            fs.Close();
            bw.Close();

            //判断月份的文件是否存在
            if (File.Exists(dateTimePicker1.Value.Year.ToString() + "年"
                + dateTimePicker1.Value.Month.ToString() + "月" + ".db"))
            {
                //从文件中读取数据
                fs = new FileStream(dateTimePicker1.Value.Year.ToString()
                    + "年" + dateTimePicker1.Value.Month.ToString() + "月" + ".db", FileMode.Open,
                    FileAccess.Read);
                br = new BinaryReader(fs);

                Count = int.Parse(br.ReadString());
                decDaily = double.Parse(br.ReadString());
                decStudy = double.Parse(br.ReadString());
                decPhoneCard = double.Parse(br.ReadString());
                decFood = double.Parse(br.ReadString());
                decClothes = double.Parse(br.ReadString());
                decOthers = double.Parse(br.ReadString());
                decTotalSum = double.Parse(br.ReadString());


                fs.Close();
                br.Close();

                //改变文件中的值
                Count++;

                //删除原来的文件
                File.Delete(dateTimePicker1.Value.Year.ToString() + "年"
                    + dateTimePicker1.Value.Month.ToString() + "月" + ".db");

                //创建月份结算的文件
                fs = new FileStream(dateTimePicker1.Value.Year.ToString() + "年"
                    + dateTimePicker1.Value.Month.ToString()
                    + "月" + ".db", FileMode.CreateNew);
                bw = new BinaryWriter(fs);

                //写入商品总数
                bw.Write(Count.ToString());

                //显示分类消费的情况
                bw.Write(decDaily.ToString());
                bw.Write(decStudy.ToString());
                bw.Write(decPhoneCard.ToString());
                bw.Write(decFood.ToString());
                bw.Write(decClothes.ToString());
                bw.Write(decOthers.ToString());

                bw.Write(decTotalSum.ToString());

                bw.Close();
                bw.Close();
            }
            else
            {
                create_new_month();
            }
        }

        //创建月份结算的文件
        private void create_new_month()
        {
            fs = new FileStream(dateTimePicker1.Value.Year.ToString() + "年" + 
                dateTimePicker1.Value.Month.ToString() 
                + "月" + ".db", FileMode.CreateNew);
            bw = new BinaryWriter(fs);

            bw.Write("1");

            //显示分类消费的情况
            bw.Write("0");
            bw.Write("0");
            bw.Write("0");
            bw.Write("0");
            bw.Write("0");
            bw.Write("0");
            bw.Write("0");

            fs.Close();
            bw.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Text;
            if (File.Exists(date + ".db"))
                file_exists(date);
            else
            {
                DialogResult responseDialogResult;

                //确认今天是否真的没有消费
                responseDialogResult = MessageBox.Show("今天确定没消费吗？", "保存确认", 
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (responseDialogResult == DialogResult.Yes)
                {
                    save_month_record(date);
                    MessageBox.Show(date + "的帐目已保存成功！", "保存成功", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                }
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 保存账目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }

        private void 每日清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DayList day_list= new DayList();
            day_list.ShowDialog();
        }

        private void after_today()
        {

            MessageBox.Show("您选择的日期还没到来，无法操作!", "操作错误", MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
            dateTimePicker1.Focus();
            dateTimePicker1.Value = DateTime.Now;
        }
        //当日历合上时触发的事件
        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            //string input = dateTimePicker1.Text;
            //string now = DateTime.Now.Date.ToLongDateString();
            if (dateTimePicker1.Value.Date > DateTime.Now.Date.Date)
            {
                after_today();
            }
            else if (dateTimePicker1.Value.Date < DateTime.Now.Date)
            {
                if (File.Exists(dateTimePicker1.Text + ".db"))
                {
                    file_exists(dateTimePicker1.Text);
                }
            }
        }

        private void 每月清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonthList monthlist = new MonthList();
            monthlist.ShowDialog();
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifyPassword modifypassword = new ModifyPassword();
            modifypassword.ShowDialog();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void 增加收入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddIncome addincome = new AddIncome();
            addincome.ShowDialog();
        }

    }
}
