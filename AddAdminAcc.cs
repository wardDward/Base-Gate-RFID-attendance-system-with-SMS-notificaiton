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

namespace SE_RFID_PROJECT
{
    public partial class AddAdminAcc : Form
    {
        UCAddAdmin ua;
        public AddAdminAcc(UCAddAdmin ua)
        {
            InitializeComponent();
            autoId();
            textBox4.PasswordChar = '*';
            textBox5.PasswordChar = '*';
            this.ua = ua;

        }

        public void autoId()
        {
            try
            {
                string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(conStr);
                string sql = "select MAX(id) from admin";

                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                con.Open();

                var maxid = cmd.ExecuteScalar() as string;

                if (maxid == null)
                {
                    id.Text = "11" + "2546";
                }
                else
                {
                    int intval = int.Parse(maxid.Substring(2, 4));
                    intval++;
                    id.Text = String.Format("11{0:0000}", intval);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear() {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox1.SelectedIndex = 0;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);



        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "INSERT INTO ADMIN(id,firstname,lastname,email,password,privilege)" +
                "VALUES(:id,:firstname,:lastname,:email,:password,:privilege)";


            if (
                textBox1.Text == "" ||
                textBox2.Text == "" ||
                textBox3.Text == "" ||
                textBox4.Text == ""
                )
            {
                MessageBox.Show("Please make sure not to leave any empty field in Admin information", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } else if (textBox5.Text == "") {
                MessageBox.Show("Please Confirm Your Password", "Confirm Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (textBox5.Text != textBox4.Text)
            {
                MessageBox.Show("Your Password Do Not Match", "Password Do Not Match", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (comboBox1.SelectedIndex == 0 || comboBox1.Text == "--SELECT--") {
                MessageBox.Show("User Type is required", "Select User Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("id", id.Text);
                cmd.Parameters.Add("firstname", textBox1.Text);
                cmd.Parameters.Add("lastname", textBox2.Text);
                cmd.Parameters.Add("email", textBox3.Text);
                cmd.Parameters.Add("password", textBox5.Text);
                cmd.Parameters.Add("privilege", comboBox1.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Admin Added Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                autoId();
                con.Close();
                Clear();
                ua.loadData();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
