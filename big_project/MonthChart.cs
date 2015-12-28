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
    public partial class MonthChart : Form
    {
        FileStream fileStreamObject;
        BinaryReader br;
        int[] sum = new int[13];
        public MonthChart()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            show_data();
        }

        private void MonthChart_Load(object sender, EventArgs e)
        {
            show_data();
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Date");

            dt.Columns.Add("Volume1");

            DataRow dr;
            dr = dt.NewRow();
            dr["Date"] = "Jan";
            dr["Volume1"] = sum[1];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Feb";
            dr["Volume1"] = sum[2];
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["Date"] = "Mar";
            dr["Volume1"] = sum[3];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Apr";
            dr["Volume1"] = sum[4];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "May";
            dr["Volume1"] = sum[5];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Jun";
            dr["Volume1"] = sum[6];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "July";
            dr["Volume1"] = sum[7];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Aug";
            dr["Volume1"] = sum[8];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Sep";
            dr["Volume1"] = sum[9];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Oct";
            dr["Volume1"] = sum[10];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Nov";
            dr["Volume1"] = sum[11];
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Date"] = "Dec";
            dr["Volume1"] = sum[12];
            dt.Rows.Add(dr);

            return dt;

        }

        private void show_data()
        {
            int year = int.Parse(numericUpDown1.Value.ToString());
            int str1 = 0, str2 = 0, str3 = 0, str4 = 0, str5 = 0, str6 = 0, str7 = 0, str8 = 0;
            for (int month = 1; month <= 12; month++)
            {
                if (File.Exists(year.ToString() + "年" + month.ToString() + "月" + ".db"))
                {
                    fileStreamObject = new FileStream(year.ToString() + "年" + month.ToString() + "月" + ".db", FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fileStreamObject);

                    try
                    {
                        while (true)
                        {

                            str1 = Convert.ToInt32(br.ReadString());
                            str2 = Convert.ToInt32(br.ReadString());
                            str3 = Convert.ToInt32(br.ReadString());
                            str4 = Convert.ToInt32(br.ReadString());
                            str5 = Convert.ToInt32(br.ReadString());
                            str6 = Convert.ToInt32(br.ReadString());
                            str7 = Convert.ToInt32(br.ReadString());
                            str8 = Convert.ToInt32(br.ReadString());

                        }
                    }
                    catch { }
                    sum[month] = str8;
                    fileStreamObject.Close();
                    br.Close();
                }
                else
                {
                    sum[month] = 0;
                }
            }


            DataTable dt2;// = default(DataTable);

            dt2 = CreateDataTable();
            chart1.DataSource = dt2;
            chart1.Series[0].YValueMembers = "Volume1";
            chart1.Series[0].XValueMember = "Date";
            chart1.DataBind();
        }

       
    }
}
