using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_RFID_PROJECT
{
    public partial class UCAttendanceReport : UserControl
    {
        public UCAttendanceReport()
        {
            InitializeComponent();
            loadAttendance();
            searchFilter.Visible = false;

            grade.DropDownStyle = ComboBoxStyle.DropDownList;
            section.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
         
        }

        public void loadAttendance()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql = "SELECT student.id,student.firstname,student.lastname,student.middlename," +
                "attendance.time_in," +
                "attendance.time_out,attendance.date_attendance FROM student INNER JOIN attendance ON student.id = attendance.student_id ";

            try
            {

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                DataTable dtbl = new DataTable();

                OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
                adp.Fill(dtbl);
                dataGridView2.AutoGenerateColumns = false;
                dataGridView2.DataSource = dtbl;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text == "Search Student ID or Name" && dateTimePicker1.CustomFormat == " ")
            {
                searchFilter.Visible = false;

            }
            else
            {
                searchFilter.Visible = true;

            }

            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql =
                 "SELECT student.id,student.firstname,student.lastname,student.middlename," +
                "attendance.time_in," +
                "attendance.time_out,attendance.date_attendance FROM student INNER JOIN attendance " +
                "ON student.id = attendance.student_id where student.id LIKE '" + textBox1.Text + "%' OR student.lastname LIKE '" + textBox1.Text + "%'";

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            DataTable dtbl = new DataTable();
            conn.Open();
            OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
            adp.Fill(dtbl);
            dataGridView2.DataSource = dtbl;
            conn.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text == "Search Student ID or Name" && dateTimePicker1.CustomFormat == " ")
            {
                searchFilter.Visible = false;

            }
            else
            {
                searchFilter.Visible = true;

            }

            dateTimePicker1.CustomFormat = "MM/dd/yyyy";

        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {

           

            if ((e.KeyCode == Keys.Back))
            {
                dateTimePicker1.CustomFormat = " ";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {


            if (textBox1.Text == "")
            {
                textBox1.Text = "Search Student ID or Name";
                textBox1.ForeColor = Color.Gray;
            }
            loadAttendance();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search Student ID or Name")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void grade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text == "Search Student ID or Name" && dateTimePicker1.CustomFormat == " ")
            {
                searchFilter.Visible = false;
            }
            else
            {
                searchFilter.Visible = true;
            }
        }

        private void section_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text == "Search Student ID or Name" && dateTimePicker1.CustomFormat == " ")
            {
                searchFilter.Visible = false;

            }
            else
            {
                searchFilter.Visible = true;

            }
        }

        private void searchFilter_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Search Student ID or Name";
            grade.SelectedIndex = 0;
            section.SelectedIndex = 0;
            dateTimePicker1.CustomFormat = " ";
            loadAttendance();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Search Student ID or Name";
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string sql1 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where grade.grade LIKE '" + grade.SelectedItem.ToString() + "%' AND grade.section LIKE '" + section.SelectedItem.ToString() + "%'";

            string sql2 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where grade.grade = '" + grade.SelectedItem.ToString() + "'";

            string sql3 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where grade.section = '" + section.SelectedItem.ToString() + "'";

            string sql4 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where attendance.date_attendance = '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "'";

            string sql5 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
            "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
            "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
            "INNER JOIN grade ON student.id = grade.student_id) where grade.grade LIKE '" + grade.SelectedItem.ToString() + "%' AND grade.section LIKE '" + section.SelectedItem.ToString() + "%' AND attendance.date_attendance LIKE '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "%'";

            string sql6 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where grade.grade LIKE '" + grade.SelectedItem.ToString() + "%' AND attendance.date_attendance LIKE '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "%'";

            string sql7 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
                "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
                "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
                "INNER JOIN grade ON student.id = grade.student_id) where grade.section LIKE '" + section.SelectedItem.ToString() + "%' AND grade.grade LIKE '" + grade.SelectedItem.ToString() + "%' AND attendance.date_attendance LIKE '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "%' AND student.id LIKE '" + textBox1.Text + "%' OR student.lastname LIKE '" + textBox1.Text + "%'";

            string sql8 = "SELECT student.id,student.firstname,student.lastname,student.middlename,attendance.time_in," +
              "attendance.time_out,attendance.date_attendance,grade.grade,grade.section FROM" +
              "  ((student INNER JOIN attendance ON student.id = attendance.student_id) " +
              "INNER JOIN grade ON student.id = grade.student_id) where  AND attendace.date_attendance LIKE '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "%' AND student.id LIKE '" + textBox1.Text + "%' OR student.lastname LIKE '" + textBox1.Text + "%'";
           
            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && dateTimePicker1.CustomFormat == " ")
            {
                MessageBox.Show("Please Fill Up the filters to search", "Search Filter is required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
         
            else if (grade.SelectedItem.ToString() != "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && dateTimePicker1.CustomFormat == " ")
            {

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql2;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql2, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();

                if (dataGridView2.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }

            else if (grade.SelectedItem.ToString() != "--SELECT--" && dateTimePicker1.CustomFormat != " " && section.SelectedItem.ToString() != "--SELECT--" && textBox1.Text != "Search Student ID or Name") {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql7;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql7, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();

                if (dataGridView2.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }

            else if (grade.SelectedItem.ToString() == "--SELECT--" && dateTimePicker1.CustomFormat != " " && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text != "Search Student ID or Name")
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql8;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql8, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();

                if (dataGridView2.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }

            else if (grade.SelectedItem.ToString() == "--SELECT--" && dateTimePicker1.CustomFormat != " " && section.SelectedItem.ToString() != "--SELECT--")
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql6;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql6, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();

                if (dataGridView2.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }

            else if (grade.SelectedItem.ToString() != "--SELECT--" && section.SelectedItem.ToString() != "--SELECT--" && dateTimePicker1.CustomFormat != " ")
            {

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql5;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql5, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();

                if (dataGridView2.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }
            else if (section.SelectedItem.ToString() != "--SELECT--" && grade.SelectedItem.ToString() == "--SELECT--")
            {
                dateTimePicker1.CustomFormat = " ";
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql3;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql3, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();

                if (dataGridView2.RowCount == 0) {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }
            else if (section.SelectedItem.ToString() == "--SELECT--" && grade.SelectedItem.ToString() == "--SELECT--" && dateTimePicker1.CustomFormat != " ")
            {


                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql4;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql4, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();

                if (dataGridView2.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            else
            {
                dateTimePicker1.CustomFormat = " ";

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql1;
                cmd.CommandType = CommandType.Text;


                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql1, conn);
                adp.Fill(dtbl);
                dataGridView2.DataSource = dtbl;
                dataGridView2.Refresh();
                if (dataGridView2.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }
        }

        private void UCAttendanceReport_Load(object sender, EventArgs e)
        {
            // Define the colors to use in the gradient
            Color[] colors = {
          ColorTranslator.FromHtml("#52a2de"), // start color
            ColorTranslator.FromHtml("#99dbe4"),
            ColorTranslator.FromHtml("#75aee2") // end color
    };

            // Create a new LinearGradientBrush
            LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0), // start point
                new Point(Width, Height), // end point
                colors[0], // start color
                colors[colors.Length - 1] // end color
            );
            brush.InterpolationColors = new ColorBlend
            {
                Colors = colors,
                Positions = new[] { 0f, 0.5f, 1f }
            };

            // Set the UserControl's background color to the LinearGradientBrush
            BackColor = Color.Transparent; // Set the UserControl's BackColor to Transparent
            BackgroundImage = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(BackgroundImage);
            g.FillRectangle(brush, new Rectangle(0, 0, Width, Height));
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            loadAttendance();
            MessageBox.Show("Table has been reloaded successfully", "Table Reload", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       
        private void label8_Click(object sender, EventArgs e)
        {
            attendanceReport ar = new attendanceReport(this);
            ar.Show();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
