using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SE_RFID_PROJECT
{
    public partial class RegisterEmail : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        UCEmail ue;
        public RegisterEmail(UCEmail ue)
        {
            this.ue = ue;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "INSERT INTO faculty_email(firstname,lastname,email,adviserory_section)" +
                "VALUES(:firstname,:lastname,:email,:advisory)";


            if (
                textBox1.Text == "" ||
                textBox2.Text == "" ||
                textBox3.Text == "" 
                )
            {
                MessageBox.Show("Please make sure not to leave any empty field in information", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (comboBox1.SelectedIndex == 0 || comboBox1.Text == "--SELECT--")
            {
                MessageBox.Show("Advisory is required", "Select Advisory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("firstname", textBox1.Text);
                cmd.Parameters.Add("lastname", textBox2.Text);
                cmd.Parameters.Add("email", textBox3.Text);
                cmd.Parameters.Add("advisory", comboBox1.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Email Added Succesfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                ue.FillCombo();

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                comboBox1.SelectedIndex = 0;

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
