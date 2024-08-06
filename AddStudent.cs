using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_RFID_PROJECT
{
    public partial class AddStudent : Form
    {

        UCStudentList studentList;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);
        public AddStudent(UCStudentList studentList)
        {
            InitializeComponent();
            autoId();
            this.studentList = studentList;
            rfid.Focus();

            gender.DropDownStyle = ComboBoxStyle.DropDownList;
            grade.DropDownStyle = ComboBoxStyle.DropDownList;
            section.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void autoId(){
            try
            {
                string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(conStr);
                string sql = "select MAX(id) from student";

                var date = DateTime.Now.ToString("yy");
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                con.Open();

                var maxid = cmd.ExecuteScalar() as string;

                if (maxid == null)
                {
                    id.Text = DateTime.Now.ToString("yy") + "0001";
                }
                else
                {
                    int intval = int.Parse(maxid.Substring(2, 4));
                    intval++;
                    id.Text = String.Format(DateTime.Now.ToString("yy") + "{0:0000}", intval);

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void label18_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you wanted to clear all the fields?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Clear()
        {
            rfid.Focus();

            rfid.Text = null;
            lrn.Text = null;
            firstname.Text = null;
            lastname.Text = null;
            middlename.Text = null;
            gender.SelectedIndex = 0;
            guardian.Text = null;
            contact.Text = null;
            address.Text = null;
            grade.SelectedIndex = 0;
            section.SelectedIndex = 0;
            studentPicture.BackgroundImage = Image.FromFile(Application.StartupPath + @"\image\stud.png");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    openFileDialog1.Filter = "Image Files (*.png)|*.png|(*.jpg)|*.jpg";
                    studentPicture.BackgroundImage = Image.FromFile(openFileDialog1.FileName);

                }
                else
                {
                    openFileDialog1.Reset();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "INSERT INTO student(id,rfid,lrn,firstname,lastname,middlename,birthdate,gender,guardian,contact,address,image)VALUES(:id,:rfid,:lrn,:firstname,:lastname,:middlename,:birthdate,:gender,:guardian,:contact,:address,:image)";


            if (
                lrn.Text == "" ||
                firstname.Text == "" ||
                lastname.Text == "" ||
                guardian.Text == "" ||
                contact.Text == "" ||
                address.Text == ""
                )
            {
                MessageBox.Show("Please make sure not to leave any empty field in student information", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (grade.SelectedIndex == 0 || section.SelectedIndex == 0 || gender.SelectedIndex == 0 || grade.Text == "--SELECT--" || section.Text == "--SELECT--" || gender.Text == "--SELECT--")
            {
                MessageBox.Show("Please make sure not to leave any empty field in student information", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } else if (rfid.Text == "" && middlename.Text != "") {
                MemoryStream ms = new MemoryStream();
                studentPicture.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arrImage = ms.GetBuffer();

                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("id", id.Text);
                cmd.Parameters.Add("rfid", 1);
                cmd.Parameters.Add("lrn", lrn.Text);
                cmd.Parameters.Add("firstname", firstname.Text);
                cmd.Parameters.Add("lastname", lastname.Text);
                cmd.Parameters.Add("middlename", middlename.Text);
                cmd.Parameters.Add("birthdate", birthdate.Value.ToString("MM-dd-yyyy"));
                cmd.Parameters.Add("gender", gender.SelectedItem.ToString());
                cmd.Parameters.Add("guardian", guardian.Text);
                cmd.Parameters.Add("contact", contact.Text);
                cmd.Parameters.Add("address", address.Text);
                cmd.Parameters.Add("image", arrImage);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Added Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "insert into grade(student_id,section,grade,enrolled)values(:student_id,:section,:grade,'" + DateTime.Now + "')";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("student_id", id.Text);
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
                autoId();
                Clear();
            }
            else if (middlename.Text == "" && rfid.Text != "")
            {
                MemoryStream ms = new MemoryStream();
                studentPicture.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arrImage = ms.GetBuffer();

                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("id", id.Text);
                cmd.Parameters.Add("rfid", rfid.Text);
                cmd.Parameters.Add("lrn", lrn.Text);
                cmd.Parameters.Add("firstname", firstname.Text);
                cmd.Parameters.Add("lastname", lastname.Text);
                cmd.Parameters.Add("middlename", "N/A");
                cmd.Parameters.Add("birthdate", birthdate.Value.ToString("MM-dd-yyyy"));
                cmd.Parameters.Add("gender", gender.SelectedItem.ToString());
                cmd.Parameters.Add("guardian", guardian.Text);
                cmd.Parameters.Add("contact", contact.Text);
                cmd.Parameters.Add("address", address.Text);
                cmd.Parameters.Add("image", arrImage);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Added Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "insert into grade(student_id,section,grade,enrolled)values(:student_id,:section,:grade,'" + DateTime.Now + "')";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("student_id", id.Text);
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
                autoId();
                Clear();
            }
            else if (middlename.Text == "" && rfid.Text == "")
            {
                MemoryStream ms = new MemoryStream();
                studentPicture.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arrImage = ms.GetBuffer();

                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("id", id.Text);
                cmd.Parameters.Add("rfid", 1);
                cmd.Parameters.Add("lrn", lrn.Text);
                cmd.Parameters.Add("firstname", firstname.Text);
                cmd.Parameters.Add("lastname", lastname.Text);
                cmd.Parameters.Add("middlename", "N/A");
                cmd.Parameters.Add("birthdate", birthdate.Value.ToString("MM-dd-yyyy"));
                cmd.Parameters.Add("gender", gender.SelectedItem.ToString());
                cmd.Parameters.Add("guardian", guardian.Text);
                cmd.Parameters.Add("contact", contact.Text);
                cmd.Parameters.Add("address", address.Text);
                cmd.Parameters.Add("image", arrImage);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Added Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "insert into grade(student_id,section,grade,enrolled)values(:student_id,:section,:grade,'" + DateTime.Now + "')";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("student_id", id.Text);
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
                autoId();
                Clear();
            }

            else
            {
                MemoryStream ms = new MemoryStream();
                studentPicture.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arrImage = ms.GetBuffer();

                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("id", id.Text);
                cmd.Parameters.Add("rfid", rfid.Text);
                cmd.Parameters.Add("lrn", lrn.Text);
                cmd.Parameters.Add("firstname", firstname.Text);
                cmd.Parameters.Add("lastname", lastname.Text);
                cmd.Parameters.Add("middlename", middlename.Text);
                cmd.Parameters.Add("birthdate", birthdate.Value.ToString("MM-dd-yyyy"));
                cmd.Parameters.Add("gender", gender.SelectedItem.ToString());
                cmd.Parameters.Add("guardian", guardian.Text);
                cmd.Parameters.Add("contact", contact.Text);
                cmd.Parameters.Add("address", address.Text);
                cmd.Parameters.Add("image", arrImage);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Added Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "insert into grade(student_id,section,grade,enrolled)values(:student_id,:section,:grade,'" + DateTime.Now + "')";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("student_id", id.Text);
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
                autoId();
                Clear();


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are sure you wanted to cancel ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }
    }
}
