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
    public partial class DayList : Form
    {
        public DayList()
        {
            InitializeComponent();
        }

        private string date;
        private int count = 0;
        private double daily = 0;
        private double study = 0;
        private double phone_card = 0;
        private double food = 0;
        private double clothes = 0;
        private double others = 0;
        private double sum = 0;

        public double decSumTotals = 0;
        public double decDailyTotals = 0;
        public double decStudyTotals = 0;
        public double decPhoneCardTotals = 0;
        public double decFoodTotals = 0;
        public double decClothesTotals = 0;
        public double decOthersTotals = 0;

        private string record;
        private FileStream fs;
        private BinaryWriter bw;
        private BinaryReader br;

        private void button1_Click(object sender, EventArgs e)
        {
            AddRecord newrecord = new AddRecord();
            newrecord.ShowDialog();

            if (newrecord.getState() == 1)
            {
                if (listView1.Items[0].SubItems[0].Text == "无消费记录！")
                {
                    listView1.Items.Clear();
                }

                ListViewItem lvt = listView1.Items.Add(newrecord.getItem());
                lvt.SubItems.Add(newrecord.getType());
                lvt.SubItems.Add(newrecord.getMoney());

                //更新每个textBox的值
                set_zero();
                count = listView1.Items.Count;

                double tempsum = 0;
                if (textBox2.Text != "")
                    tempsum = Convert.ToDouble(textBox2.Text);
                sum =  tempsum + Convert.ToDouble(newrecord.getMoney());
                type_add();
                fill_textbox();
            }
            button5.Enabled = true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }
       
        //当日历合上时触发的事件
        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            set_zero();
            date = dateTimePicker1.Text;
            day_list(date);
            
        }

        //记录每个textbox的值
        private void save_textbox()
        {
            if (textBox2.Text != "")
            {
                decSumTotals = System.Convert.ToDouble(textBox2.Text);
            }
            if (textBox3.Text != "")
            {
                decDailyTotals = System.Convert.ToDouble(textBox3.Text);
            }
            if (textBox4.Text != "")
            {
                decStudyTotals = System.Convert.ToDouble(textBox4.Text);
            }
            if (textBox5.Text != "")
            {
                decPhoneCardTotals = System.Convert.ToDouble(textBox5.Text);
            }
            if (textBox6.Text != "")
            {
                decFoodTotals = System.Convert.ToDouble(textBox6.Text);
            }
            if (textBox7.Text != "")
            {
                decClothesTotals = System.Convert.ToDouble(textBox7.Text);
            }
            if (textBox8.Text != "")
            {
                decOthersTotals = System.Convert.ToDouble(textBox8.Text);
            }
        }

        private void set_zero()
        {
            daily = 0;
            study = 0;
            phone_card = 0;
            food = 0;
            clothes = 0;
            others = 0;
            sum = 0;
        }

        //使增删改的删除记录的按钮全部激活
        private void btn_enable()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        /*
         * 该天无消费记录
         * record 为"无当天信息"等消息。
         * */
        private void day_no_record(string record, string text="")
        {
            listView1.Items.Add(record);
            textBox1.Text = text;
            textBox2.Text = text;
            textBox3.Text = text;
            textBox4.Text = text;
            textBox5.Text = text;
            textBox6.Text = text;
            textBox7.Text = text;
            textBox8.Text = text;
        }

        //将各个类别的钱相加
        private void type_add()
        {
            for (int intRowIndex = 0; intRowIndex < listView1.Items.Count; intRowIndex++)
            {
                //sum += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                switch (listView1.Items[intRowIndex].SubItems[1].Text)
                {
                    case "日常用品":
                        daily += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                        break;
                    case "学习用品":
                        study += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                        break;
                    case "电话费":
                        phone_card += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                        break;
                    case "饮食":
                        food += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                        break;
                    case "衣服":
                        clothes += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                        break;
                    case "其他":
                        others += Convert.ToDouble(listView1.Items[intRowIndex].SubItems[2].Text);
                        break;
                }
            }
        }

        /*
         * 填充listview，用从文件中读出的数据
         * record 为文件的第一行的第一个值,为记录总数
         * */
        private void fill_listview(string record)
        {
            count = Convert.ToInt32(record);
            sum = Convert.ToDouble(br.ReadString());

            for (int i = 0; i < count; i++)
            {
                ListViewItem lvt = listView1.Items.Add(br.ReadString());
                lvt.SubItems.Add(br.ReadString());
                lvt.SubItems.Add(br.ReadString());
            }

            type_add();
            fill_textbox();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        //给textbox填充数据
        private void fill_textbox()
        {
            textBox1.Text = count.ToString();
            textBox2.Text = sum.ToString();
            textBox3.Text = daily.ToString();
            textBox4.Text = study.ToString();
            textBox5.Text = phone_card.ToString();
            textBox6.Text = food.ToString();
            textBox7.Text = clothes.ToString();
            textBox8.Text = others.ToString();
        }

        

        //操作每日清单
        private void day_list(string date)
        {
            btn_enable();
            listView1.Items.Clear();
            if (File.Exists(date + ".db"))
            {
                fs = new FileStream(date + ".db", FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                //BinaryWriter binaryWriterObject = new BinaryWriter(fileStreamObject);               
                record = br.ReadString();
                if (record == "无消费记录！")
                    day_no_record(record);
                else
                    fill_listview(record);
                br.Close();

            }
            else if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                day_no_record("该日还未到，无消费记录!");
                btn_disable();
            }
            else if (dateTimePicker1.Value.Date < DateTime.Now.Date)
            {
                day_no_record("该日无消费记录!");
                btn_disable();
            }
            else
            {
                day_no_record("该日无消费记录!");
                btn_disable();
            }
        }

        //使增删该及删除文件的按钮全部disable
        private void btn_disable()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void daily_list_Load(object sender, EventArgs e)
        {
            date = dateTimePicker1.Text;
            day_list(date);
            save_textbox();
            button5.Enabled = false;
        }


        /*
         * 从月文件中读出数据
         * */
        private void read_month_file(string filename)
        {
            fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);

            count = int.Parse(br.ReadString());
            daily = double.Parse(br.ReadString());
            study = double.Parse(br.ReadString());
            phone_card = double.Parse(br.ReadString());
            food = double.Parse(br.ReadString());
            clothes = double.Parse(br.ReadString());
            others = double.Parse(br.ReadString());
            sum = double.Parse(br.ReadString());

            fs.Close();
            br.Close();
        }

        //修改删除文件后的数据
        private void opt_first_read()
        {
            count--;
            daily -= System.Convert.ToDouble(textBox3.Text);
            study -= System.Convert.ToDouble(textBox4.Text);
            phone_card -= System.Convert.ToDouble(textBox5.Text);
            food -= System.Convert.ToDouble(textBox6.Text);
            clothes -= System.Convert.ToDouble(textBox7.Text);
            others -= System.Convert.ToDouble(textBox8.Text);
            sum -= System.Convert.ToDouble(textBox2.Text);
        }

        //将修改后的数据写入新的月文件
        private void create_new_month_file(string month_file)
        {
            //创建月份结算的文件
            fs = new FileStream(month_file, FileMode.CreateNew);
            bw = new BinaryWriter(fs);

            //显示分类消费的情况
            bw.Write(count.ToString());
            bw.Write(daily.ToString());
            bw.Write(study.ToString());
            bw.Write(phone_card.ToString());
            bw.Write(food.ToString());
            bw.Write(clothes.ToString());
            bw.Write(others.ToString());

            bw.Write(sum.ToString());

            fs.Close();
            bw.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult responseDialogResult;
            //确认是否要清除全部
            responseDialogResult = MessageBox.Show("确定要删除" + date + "的帐目吗？", "删除确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (responseDialogResult == DialogResult.Yes)
            {
                try
                {
                    //从每日消费记录文件中读取数据
                    fs = new FileStream(date + ".db", FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);

                    record = br.ReadString();

                    fs.Close();
                    br.Close();
                    string month_file = dateTimePicker1.Value.Year.ToString() + "年" 
                        + dateTimePicker1.Value.Month.ToString() + "月" + ".db";
                    if (record != "无消费记录！")
                    {
                        //从文件中读数据,及操作数据
                        read_month_file(month_file);

                        //操作数据
                        opt_first_read();

                        //删除原来的文件
                        File.Delete(month_file);

                        //写入新的月文件
                        create_new_month_file(month_file);
                    }
                    else
                    {
                        //从文件中读数据,及操作数据
                        read_month_file(month_file);

                        //改变文件中的值
                        count--;
                        if (count == 0)
                        {
                            //当月没有记录时删除的文件
                            File.Delete(month_file);
                        }
                        else
                        {
                            //删除原来的文件
                            File.Delete(month_file);

                            //创建新的月文件
                            create_new_month_file(month_file);
                        }
                    }
                    //删除指定文件
                    File.Delete(date + ".db");
                    dateTimePicker1_CloseUp(sender, e);
                    //button5.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("账目删除失败!", "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                button4.Enabled = false;
            }
            button5.Enabled = false;
        }

        //删除listview中的消费记录，但并没有直接写入文件
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                set_zero();
                ListViewItem li = this.listView1.SelectedItems[0];
                sum = Convert.ToDouble(textBox2.Text);
                sum -= Convert.ToDouble(li.SubItems[2].Text);
                this.listView1.Items.Remove(li);

                type_add();
                count = listView1.Items.Count;
                fill_textbox();
                button2.Enabled = false;
                button3.Enabled = false;
            }
            catch
            {
                MessageBox.Show("请选择删除记录项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            button5.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string month_file = dateTimePicker1.Value.Year.ToString() + "年"
                + dateTimePicker1.Value.Month.ToString() + "月" + ".db";
            if (textBox1.Text == "0")
            {
                DialogResult responseDialogResult;

                //确认今天是否真的没有消费
                responseDialogResult = MessageBox.Show("确定没有消费吗？", "保存确认", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (responseDialogResult == DialogResult.Yes)
                {
                    //从文件中读取数据
                    fs = new FileStream(month_file, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);

                    count = int.Parse(br.ReadString());
                    daily = double.Parse(br.ReadString());
                    study = double.Parse(br.ReadString());
                    phone_card = double.Parse(br.ReadString());
                    food = double.Parse(br.ReadString());
                    clothes = double.Parse(br.ReadString());
                    others = double.Parse(br.ReadString());
                    sum = double.Parse(br.ReadString());

                    fs.Close();
                    br.Close();

                    //改变文件中的值
                    daily -= decDailyTotals;
                    study -= decStudyTotals;
                    phone_card -= decPhoneCardTotals;
                    food -= decFoodTotals;
                    clothes -= decClothesTotals;
                    others -= decOthersTotals;
                    sum -= decSumTotals;

                    //删除原来的文件
                    File.Delete(month_file);

                    //创建月份结算的文件
                    fs = new FileStream(month_file, FileMode.CreateNew);
                    bw = new BinaryWriter(fs);

                    bw.Write(count.ToString());

                    //显示分类消费的情况
                    bw.Write(daily.ToString());
                    bw.Write(study.ToString());
                    bw.Write(phone_card.ToString());
                    bw.Write(food.ToString());
                    bw.Write(clothes.ToString());
                    bw.Write(others.ToString());

                    bw.Write(sum.ToString());

                    fs.Close();
                    bw.Close();

                    //删除原来的文件
                    File.Delete(date + ".db");

                    //把帐目写入二进制文件里
                    fs = new FileStream(date + ".db", FileMode.CreateNew);
                    bw = new BinaryWriter(fs);
                    bw.Write("无消费记录！");

                    fs.Close();
                    bw.Close();

                    date = dateTimePicker1.Text;
                    day_list(date);
                    MessageBox.Show("保存成功！");
                }
            }
            else
            {
                //从文件中读取数据
                FileStream fileStreamObject = new FileStream(date + ".db", FileMode.Open, FileAccess.Read);
                BinaryReader binaryReaderObject = new BinaryReader(fileStreamObject);

                record = binaryReaderObject.ReadString();

                fileStreamObject.Close();
                binaryReaderObject.Close();

                if (record != "无消费记录！")
                {
                    //从文件中读取数据
                    fs = new FileStream(month_file, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);

                    count = int.Parse(br.ReadString());
                    daily = double.Parse(br.ReadString());
                    study = double.Parse(br.ReadString());
                    phone_card = double.Parse(br.ReadString());
                    food = double.Parse(br.ReadString());
                    clothes = double.Parse(br.ReadString());
                    others = double.Parse(br.ReadString());
                    sum = double.Parse(br.ReadString());

                    fs.Close();
                    br.Close();

                    //改变文件中的值
                    //intCount --;
                    daily += (double.Parse(textBox3.Text) - decDailyTotals);
                    study += (double.Parse(textBox4.Text) - decStudyTotals);
                    phone_card += (double.Parse(textBox5.Text) - decPhoneCardTotals);
                    food += (double.Parse(textBox6.Text) - decFoodTotals);
                    clothes += (double.Parse(textBox7.Text) - decClothesTotals);
                    others += (double.Parse(textBox8.Text) - decOthersTotals);
                    sum += (double.Parse(textBox2.Text) - decSumTotals);

                    //删除原来的文件
                    File.Delete(month_file);

                    //创建月份结算的文件
                    fs = new FileStream(month_file, FileMode.CreateNew);
                    bw = new BinaryWriter(fs);

                    bw.Write(count.ToString());

                    //显示分类消费的情况
                    bw.Write(daily.ToString());
                    bw.Write(study.ToString());
                    bw.Write(phone_card.ToString());
                    bw.Write(food.ToString());
                    bw.Write(clothes.ToString());
                    bw.Write(others.ToString());

                    bw.Write(sum.ToString());

                    fs.Close();
                    bw.Close();
                }
                else
                {
                    //从文件中读取数据
                    fs = new FileStream(month_file, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);

                    count = int.Parse(br.ReadString());
                    daily = double.Parse(br.ReadString());
                    study = double.Parse(br.ReadString());
                    phone_card = double.Parse(br.ReadString());
                    food = double.Parse(br.ReadString());
                    clothes = double.Parse(br.ReadString());
                    others = double.Parse(br.ReadString());
                    sum = double.Parse(br.ReadString());

                    fs.Close();
                    br.Close();

                    //改变文件中的值
                    daily += double.Parse(textBox3.Text);
                    study += double.Parse(textBox4.Text);
                    phone_card += double.Parse(textBox5.Text);
                    food += double.Parse(textBox6.Text);
                    clothes += double.Parse(textBox7.Text);
                    others += double.Parse(textBox8.Text);
                    sum += double.Parse(textBox2.Text);

                    //删除原来的文件
                    File.Delete(month_file);

                    //创建月份结算的文件
                    fs = new FileStream(month_file, FileMode.CreateNew);
                    bw = new BinaryWriter(fs);

                    bw.Write(count.ToString());

                    //显示分类消费的情况
                    bw.Write(daily.ToString());
                    bw.Write(study.ToString());
                    bw.Write(phone_card.ToString());
                    bw.Write(food.ToString());
                    bw.Write(clothes.ToString());
                    bw.Write(others.ToString());

                    bw.Write(sum.ToString());

                    fs.Close();
                    bw.Close();
                }

                //删除原来的文件
                File.Delete(date + ".db");

                //把帐目写入二进制文件里
                fs = new FileStream(date + ".db", FileMode.CreateNew);
                bw = new BinaryWriter(fs);

                bw.Write(textBox1.Text);
                bw.Write(textBox2.Text);

                //把列表中的数据逐一写入二进制文件里
                for (int intRowIndex = 0; intRowIndex < (this.listView1.Items.Count); intRowIndex++)
                {
                    for (int intColIndex = 0; intColIndex < (this.listView1.Columns.Count); intColIndex++)
                    {
                        bw.Write(this.listView1.Items[intRowIndex].SubItems[intColIndex].Text);
                    }
                }

                fs.Close();
                bw.Close();
                save_textbox();
                button5.Enabled = false;
                MessageBox.Show("保存成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem li = this.listView1.SelectedItems[0];
                string item = li.SubItems[0].Text;
                string type = li.SubItems[1].Text;
                string money = li.SubItems[2].Text;

                double tempsum = Convert.ToDouble(textBox2.Text) - Convert.ToDouble(money);
 
                ModifyRecord modifyrecord = new ModifyRecord(item, type, money);
                modifyrecord.ShowDialog();

                if (modifyrecord.getState() == 1)
                {
                    if (textBox1.Text == "该日无消费记录!")
                    {
                        listView1.Items.Clear();
                    }

                    li.SubItems[0].Text = modifyrecord.getItem();
                    li.SubItems[1].Text = modifyrecord.getType();
                    li.SubItems[2].Text = modifyrecord.getMoney();

                    //更新每个textBox的值
                    set_zero();
                    count = listView1.Items.Count;
                    sum = tempsum + Convert.ToDouble(modifyrecord.getMoney());
                    type_add();
                    fill_textbox();
                }
                button2.Enabled = false;
                button3.Enabled = false;
            }
            catch
            {
                MessageBox.Show("请选择删除记录项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            button5.Enabled = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DayChart charshow = new DayChart();
            charshow.ShowDialog();
        }
    }
}
