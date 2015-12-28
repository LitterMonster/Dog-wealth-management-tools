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
    public partial class ShowSave : Form
    {
        private DateTimePicker dateTimePicker;
        private FileStream fs;
        private double[]array = new double[6];
        private int Count = 0;
        private double decDaily = 0;
        private double decStudy = 0;
        private double decPhoneCard = 0;
        private double decFood = 0;
        private double decClothes = 0;
        private double decOthers = 0;
        private double decTotalSum = 0;
       
        public ShowSave(DateTimePicker dateTimePicker, string date, double[] array, int count)
        {
            InitializeComponent();
            this.dateTimePicker = dateTimePicker;
            label1.Text = date;
            this.array = array;
            label15.Text = "  共" + count.ToString() + "条记录!";
            textBox1.Text = array[0].ToString();
            textBox2.Text = array[1].ToString();
            textBox3.Text = array[2].ToString();
            textBox4.Text = array[3].ToString();
            textBox5.Text = array[4].ToString();
            textBox6.Text = array[5].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowSave_Load(object sender, EventArgs e)
        {
            int intYear = dateTimePicker.Value.Year;
            int intMonth = dateTimePicker.Value.Month;
            if (File.Exists(intYear.ToString() + "年" + intMonth.ToString() + "月" + ".db"))
            {
                //从文件中读取数据
                fs = new FileStream(intYear.ToString() + "年" + intMonth.ToString() + "月" + ".db", FileMode.Open, FileAccess.Read);
                BinaryReader binaryReaderObject = new BinaryReader(fs);

                Count = int.Parse(binaryReaderObject.ReadString());
                decDaily = double.Parse(binaryReaderObject.ReadString());
                decStudy = double.Parse(binaryReaderObject.ReadString());
                decPhoneCard = double.Parse(binaryReaderObject.ReadString());
                decFood = double.Parse(binaryReaderObject.ReadString());
                decClothes = double.Parse(binaryReaderObject.ReadString());
                decOthers = double.Parse(binaryReaderObject.ReadString());
                decTotalSum = double.Parse(binaryReaderObject.ReadString());

                fs.Close();
                binaryReaderObject.Close();

                //改变文件中的值
                Count++;
                decDaily += array[0];
                decStudy += array[1];
                decPhoneCard += array[2];
                decFood += array[3];
                decClothes += array[4];
                decOthers += array[5];
                decTotalSum += array[0] + array[1] + array[2] + array[3] + array[4] + array[5];

                //删除原来的文件
                File.Delete(intYear.ToString() + "年" + intMonth.ToString() + "月" + ".db");

                //创建月份结算的文件
                fs = new FileStream(intYear.ToString() + "年" + intMonth.ToString() + "月" + ".db", FileMode.CreateNew);
                BinaryWriter binaryWriterObject = new BinaryWriter(fs);

                binaryWriterObject.Write(Count.ToString());

                //显示分类消费的情况
                binaryWriterObject.Write(decDaily.ToString());
                binaryWriterObject.Write(decStudy.ToString());
                binaryWriterObject.Write(decPhoneCard.ToString());
                binaryWriterObject.Write(decFood.ToString());
                binaryWriterObject.Write(decClothes.ToString());
                binaryWriterObject.Write(decOthers.ToString());

                binaryWriterObject.Write(decTotalSum.ToString());

                fs.Close();
                binaryWriterObject.Close();
            }
            else
            {
                //创建月份结算的文件
                FileStream fileStreamObject = new FileStream(intYear.ToString() + "年" + intMonth.ToString() + "月" + ".db", FileMode.CreateNew);
                BinaryWriter binaryWriterObject = new BinaryWriter(fileStreamObject);

                binaryWriterObject.Write("1");

                //显示分类消费的情况
                double sum = array[0] + array[1] + array[2] + array[3] + array[4] + array[5];
                binaryWriterObject.Write(array[0].ToString());
                binaryWriterObject.Write(array[1].ToString());
                binaryWriterObject.Write(array[2].ToString());
                binaryWriterObject.Write(array[3].ToString());
                binaryWriterObject.Write(array[4].ToString());
                binaryWriterObject.Write(array[5].ToString());
                binaryWriterObject.Write(sum.ToString());

                fileStreamObject.Close();
                binaryWriterObject.Close();
            }
        }
    }
}
