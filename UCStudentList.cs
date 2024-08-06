using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace SE_RFID_PROJECT
{
    public partial class UCStudentList : UserControl
    {

        public UCStudentList()
        {
            InitializeComponent();
            loadData();
            searchFilter.Visible = false;
            grade.DropDownStyle = ComboBoxStyle.DropDownList;
            section.DropDownStyle = ComboBoxStyle.DropDownList;



        }

        private void UCStudentList_Load(object sender, EventArgs e)
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

        public void loadData()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql = "SELECT student.id,student.lrn,student.firstname,student.lastname,student.middlename" +
                ",student.gender,student.guardian,student.contact,student.address,grade.section,grade.grade" +
                " FROM student INNER JOIN grade ON student.id = grade.student_id";

            try
            {

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                DataTable dtbl = new DataTable();

                OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
                adp.Fill(dtbl);
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dtbl;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            AddStudent student = new AddStudent(this);
            student.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string colname = dataGridView1.Columns[e.ColumnIndex].Name;
            string sql = "SELECT student.image, student.* ,grade.section,grade.grade FROM student INNER JOIN grade ON student.id = grade.student_id where student.id like'" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";

            if (colname == "colEdit")
            {
                UpdateStudent updateStudent = new UpdateStudent(this);
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    updateStudent.id.Text = dr.GetString(1);
                    updateStudent.rfid.Text = dr.GetString(2);
                    updateStudent.lrn.Text = dr.GetString(3);
                    updateStudent.firstname.Text = dr.GetString(4);
                    updateStudent.lastname.Text = dr.GetString(5);
                    updateStudent.middlename.Text = dr.GetString(6);
                    updateStudent.birthdate.Value = Convert.ToDateTime(dr.GetString(7));
                    updateStudent.gender.Text = dr.GetString(8);
                    updateStudent.guardian.Text = dr.GetString(9);
                    updateStudent.contact.Text = dr.GetString(10);
                    updateStudent.address.Text = dr.GetString(11);
                    updateStudent.grade.Text = dr.GetString(14);
                    updateStudent.section.Text = dr.GetString(13);

                    long len = dr.GetBytes(0, 0, null, 0, 0);
                    byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                    dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len) + 1);

                    MemoryStream ms = new MemoryStream(array);
                    Bitmap bitmap = new Bitmap(ms);
                    updateStudent.studentPicture.BackgroundImage = bitmap;

                    updateStudent.ShowDialog();
                }

                dr.Close();
                conn.Close();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Search Student ID or Name";
                textBox1.ForeColor = Color.Gray;
            }
            loadData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text == "Search Student ID or Name")
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
                "SELECT student.id,student.lrn,student.firstname,student.lastname,student.middlename,student.gender,student.guardian,student.contact,student.address, grade.section, grade.grade" +
                " from student inner join grade on student.id = grade.student_id " +
                "where student.id LIKE '" + textBox1.Text + "%' OR student.lastname LIKE '" + textBox1.Text + "%'";

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            DataTable dtbl = new DataTable();
            conn.Open();
            OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
            adp.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            conn.Close();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search Student ID or Name")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Search Student ID or Name";
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string sql1 =
               "SELECT student.id,student.lrn,student.firstname,student.lastname,student.middlename,student.gender,student.guardian,student.contact,student.address, grade.section, grade.grade" +
            " from student inner join grade on student.id = grade.student_id " +
            " where grade.grade = '" + grade.SelectedItem.ToString() + "' and grade.section = '" + section.SelectedItem.ToString() + "'";
            string sql2 =
                 "SELECT student.id,student.lrn,student.firstname,student.lastname,student.middlename,student.gender,student.guardian,student.contact,student.address, grade.section, grade.grade" +
            " from student inner join grade on student.id = grade.student_id " +
                  " where grade.grade = '" + grade.SelectedItem.ToString() + "'";

            string sql3 =
                "SELECT student.id,student.lrn,student.firstname,student.lastname,student.middlename,student.gender,student.guardian,student.contact,student.address, grade.section, grade.grade" +
               " from student inner join grade on student.id = grade.student_id " +
            " where grade.section = '" + section.SelectedItem.ToString() + "'";

            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--")
            {
                MessageBox.Show("Search filter is empty", "Please Fill up search filter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (grade.SelectedItem.ToString() != "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--")
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql2;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql2, conn);
                adp.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                dataGridView1.Refresh();
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }
            else if (section.SelectedItem.ToString() != "--SELECT--" && grade.SelectedItem.ToString() == "--SELECT--")
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql1;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql3, conn);
                adp.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                dataGridView1.Refresh();
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }
            else
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql1;
                cmd.CommandType = CommandType.Text;

                DataTable dtbl = new DataTable();
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql1, conn);
                adp.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                dataGridView1.Refresh();
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("No record found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
            }
        }

            private void grade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text == "Search Student ID or Name")
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
            if (grade.SelectedItem.ToString() == "--SELECT--" && section.SelectedItem.ToString() == "--SELECT--" && textBox1.Text == "Search Student ID or Name")
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
            loadData();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
