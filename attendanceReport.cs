using Microsoft.Reporting.WinForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SE_RFID_PROJECT
{
    public partial class attendanceReport : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        UCAttendanceReport ua;
        public attendanceReport(UCAttendanceReport ua)
        {
            InitializeComponent();
            this.ua = ua;
        }

        private void attendanceReport_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            DataTable dt = new DataTable();

            string sqlAll = "select * from attendance";


            string sql = "SELECT student.id,student.firstname,student.lastname,student.middlename," +
                  "attendance.student_id,attendance.time_in," +
                  "attendance.time_out,attendance.date_attendance FROM student INNER JOIN attendance " +
                   "ON student.id = attendance.student_id where student.id LIKE '" + ua.textBox1.Text + "%' OR student.lastname LIKE '" + ua.textBox1.Text + "%'";

            string sql1 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.student_id,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
            "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
            "INNER JOIN grade ON student.id = grade.student_id) where grade.grade LIKE '" + ua.grade.SelectedItem.ToString() + "%' AND grade.section LIKE '" + ua.section.SelectedItem.ToString() + "%'";

            string sql2 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.student_id,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
            "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where grade.grade = '" + ua.grade.SelectedItem.ToString() + "'";

            string sql3 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.student_id,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where grade.section = '" + ua.section.SelectedItem.ToString() + "'";

            string sql4 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.student_id,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where attendance.date_attendance = '" + ua.dateTimePicker1.CustomFormat + "'";

            string sql5 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.student_id,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
           "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
           "INNER JOIN grade ON student.id = grade.student_id) where grade.grade LIKE '" + ua.grade.SelectedItem.ToString() + "%' AND grade.section LIKE '" + ua.section.SelectedItem.ToString() + "%' AND attendance.date_attendance LIKE '" + ua.dateTimePicker1.Value.ToString("MM/dd/yyyy") + "%'";

            if (ua.grade.SelectedItem.ToString() == "--SELECT--" && ua.section.SelectedItem.ToString() == "--SELECT--" && ua.textBox1.Text != "Search Student ID or Name")
            {
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
                adp.Fill(dt);
              
            }
            else if (ua.grade.SelectedItem.ToString() != "--SELECT--" && ua.section.SelectedItem.ToString() != "--SELECT--" && ua.dateTimePicker1.CustomFormat != " ")
            {

                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql5, conn);
                adp.Fill(dt);
            }

            else if (ua.grade.SelectedItem.ToString() != "--SELECT--" && ua.section.SelectedItem.ToString() == "--SELECT--")
            {
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql2, conn);
                adp.Fill(dt);
            
            }
            else if (ua.section.SelectedItem.ToString() != "--SELECT--" && ua.grade.SelectedItem.ToString() == "--SELECT--")
            {
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql3, conn);
                adp.Fill(dt);
               
            }
            else if (ua.section.SelectedItem.ToString() == "--SELECT--" && ua.grade.SelectedItem.ToString() == "--SELECT--" && ua.dateTimePicker1.CustomFormat != " ")
            {
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql4, conn);
                adp.Fill(dt);
      
            } else if (ua.section.SelectedItem.ToString() != "--SELECT--" && ua.grade.SelectedItem.ToString() != "--SELECT--" && ua.dateTimePicker1.CustomFormat == " ") {
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql1, conn);
                adp.Fill(dt);
   
            }
            else
            {
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sqlAll, conn);
                adp.Fill(dt);
         
            }

       

            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("DataSet1", dt);

            reportViewer1.LocalReport.ReportPath = @"C:\Users\user\source\repos\SE_RFID_PROJECT1\Report2.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);

            if (ua.grade.SelectedItem.ToString() == "--SELECT--" && ua.section.SelectedItem.ToString() == "--SELECT--" && ua.textBox1.Text != "Search Student ID or Name")
            {
             
                ReportParameter keyword = new ReportParameter("keyword", ua.textBox1.Text);
                reportViewer1.LocalReport.SetParameters(keyword);
            }

            else if (ua.grade.SelectedItem.ToString() != "--SELECT--" && ua.section.SelectedItem.ToString() == "--SELECT--")
            {
            
                ReportParameter keyword = new ReportParameter("keyword", ua.grade.SelectedItem.ToString());
                reportViewer1.LocalReport.SetParameters(keyword);
            }
            else if (ua.section.SelectedItem.ToString() != "--SELECT--" && ua.grade.SelectedItem.ToString() == "--SELECT--")
            {
              
                ReportParameter keyword = new ReportParameter("keyword", ua.section.SelectedItem.ToString());
                reportViewer1.LocalReport.SetParameters(keyword);
            }
            else if (ua.section.SelectedItem.ToString() == "--SELECT--" && ua.grade.SelectedItem.ToString() == "--SELECT--" && ua.dateTimePicker1.CustomFormat != " ")
            {
               
                ReportParameter keyword = new ReportParameter("keyword", ua.dateTimePicker1.Value.ToString());
                reportViewer1.LocalReport.SetParameters(keyword);
            }
            else if (ua.section.SelectedItem.ToString() != "--SELECT--" && ua.grade.SelectedItem.ToString() != "--SELECT--" && ua.dateTimePicker1.CustomFormat == " ")
            {
               
                ReportParameter keyword = new ReportParameter("keyword", ua.grade.SelectedItem.ToString() + " - " + ua.section.SelectedItem.ToString());
                reportViewer1.LocalReport.SetParameters(keyword);
            }
            else if (ua.grade.SelectedItem.ToString() != "--SELECT--" && ua.section.SelectedItem.ToString() != "--SELECT--" && ua.dateTimePicker1.CustomFormat != " ")
            {

                ReportParameter keyword = new ReportParameter("keyword", ua.grade.SelectedItem.ToString() + " - " + ua.section.SelectedItem.ToString() + "    " + ua.dateTimePicker1.Value.ToString());
                reportViewer1.LocalReport.SetParameters(keyword);
            }
            else
            {
               
                ReportParameter keyword = new ReportParameter("keyword", "All Student");
                reportViewer1.LocalReport.SetParameters(keyword);
            }


            this.reportViewer1.RefreshReport();
            conn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
