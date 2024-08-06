using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace SE_RFID_PROJECT
{
    public partial class UpdateStudent : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        UCStudentList studentList;

        public UpdateStudent(UCStudentList studentList)
        {
            InitializeComponent();
            this.studentList = studentList;

        }

        private void UpdateStudent_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "update student set rfid = :rfid, lrn = :lrn,firstname = :firstname,lastname = :lastname,middlename = :middlename,birthdate = :birthdate,gender = :gender,guardian = :guardian,contact = :contact,address = :address,image = :image where id = :id";


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
            else if (rfid.Text == "" && middlename.Text != "")
            {
                MemoryStream ms = new MemoryStream();
                studentPicture.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arrImage = ms.GetBuffer();

                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("rfid", "N/A");
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
                cmd.Parameters.Add("id", id.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Updated Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "update grade set section =:section, grade = :grade where student_id = :id";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.Parameters.Add("id", id.Text);
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
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
                cmd.Parameters.Add("id", id.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Updated Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "update grade set section =:section, grade = :grade where student_id = :id";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.Parameters.Add("id", id.Text);
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
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
                cmd.Parameters.Add("id", id.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Updated Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "update grade set section =:section, grade = :grade where student_id = :id";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.Parameters.Add("id", id.Text);
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
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
                cmd.Parameters.Add("id", id.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Updated Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sql1 = "update grade set section =:section, grade = :grade where student_id = :id";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("section", section.SelectedItem.ToString());
                cmd1.Parameters.Add("grade", grade.SelectedItem.ToString());
                cmd1.Parameters.Add("id", id.Text);
                cmd1.ExecuteNonQuery();
                con.Close();
                studentList.loadData();
            }
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
                    // User cancelled the operation
                    openFileDialog1.Reset(); // Clear the value of the OpenFileDialog
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }
    }
}
