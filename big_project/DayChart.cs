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
    public partial class DayChart : Form
    {
        double[] yValues = new double[31];
        string[] xValues = new string[31];
        int[] sum = new int[32];

        public DayChart()
        {
            InitializeComponent();
        }

        private void show_data()
        {
            int month = int.Parse(numericUpDown2.Value.ToString());
            int year = int.Parse(numericUpDown1.Value.ToString());

            string str1;
            int money = 0;
            for (int day = 1; day <= 31; day++)
            {
                money = 0;
                string filename = year.ToString() + "年" + month.ToString() + "月" + day.ToString() + "日" + ".db";
                if (File.Exists(filename))
                {
                    FileStream fileStreamObject = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fileStreamObject);

                    try
                    {
                        while (true)
                        {
                            str1 = br.ReadString();
                            money += int.Parse(br.ReadString());
                        }
                    }
                    catch{}

                    sum[day] = money;
                }
                else
                    sum[day] = 0;

                yValues[day - 1] = sum[day];
                xValues[day - 1] = day.ToString();
            }

            chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
 
        }

        private void Chart_show_Load(object sender, EventArgs e)
        {
            show_data();
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            show_data();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            show_data();
        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {
            show_data();
        }
    }
}
