using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SE_RFID_PROJECT
{
    public partial class UpdateAdmin : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        UCAddAdmin ua;
        public UpdateAdmin(UCAddAdmin ua)
        {
            InitializeComponent();
            this.ua = ua;
            password.PasswordChar = '*';
            confirm_password.PasswordChar = '*';

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "update admin set firstname = :firstname, lastname = :lastname, email = :email, password =:password " +
                "where id = :id";


            if (
                firstname.Text == "" ||
                lastname.Text == "" ||
                email.Text == "" ||
                password.Text == ""
                )
            {
                MessageBox.Show("Please make sure not to leave any empty field in student information", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } else if (confirm_password.Text != password.Text) {
                MessageBox.Show("Please Confirm Your Password", "Confirm Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (confirm_password.Text != confirm_password.Text)
            {
                MessageBox.Show("Your Password Do Not Match", "Password Do Not Match", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (privilege.SelectedIndex == 0 || privilege.Text == "--SELECT--")
            {
                MessageBox.Show("User Type is required", "Select User Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {

                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("firstname", firstname.Text);
                cmd.Parameters.Add("lastname", lastname.Text);
                cmd.Parameters.Add("email", email.Text);
                cmd.Parameters.Add("password", password.Text);
                cmd.Parameters.Add("id", id.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Admin Information Updated Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                ua.loadData();
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }
    }
}
