using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_RFID_PROJECT
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
       
            textBox2.PasswordChar = '*';
        }

        public static string firstname;
        public static string email;

        public static string first
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public static string adminEmail {
            get { return email; }
            set { email = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string sql = "select * from admin where id = :id and password = :password";
            OracleCommand cmd = conn.CreateCommand();

            if (textBox1.Text == "Admin ID")
            {
                MessageBox.Show("Admin ID is required", "Admin ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox2.Text == "Password")
            {
                MessageBox.Show("Password is required", "Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text != null || textBox2.Text != null)
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("id", textBox1.Text.ToString());
                cmd.Parameters.Add("password", textBox2.Text.ToString());
                OracleDataReader dr;
                conn.Open();

                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    first = textBox1.Text;
                    email = textBox1.Text;

                    Form1 dashboard = new Form1();
                    dashboard.Show();
                }
                else
                {
                    MessageBox.Show("No Admin Account Exist ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = "";
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Admin ID";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Admin ID")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Password";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            StudentProfile sp = new StudentProfile();
            sp.Show();
            this.Hide();
        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection conn = new OracleConnection(conStr);

                string sql = "select * from admin where id = :id and password = :password";
                OracleCommand cmd = conn.CreateCommand();

                if (textBox1.Text == "Admin ID")
                {
                    MessageBox.Show("Admin ID is required", "Admin ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (textBox2.Text == "Password")
                {
                    MessageBox.Show("Password is required", "Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (textBox1.Text != null || textBox2.Text != null)
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("id", textBox1.Text.ToString());
                    cmd.Parameters.Add("password", textBox2.Text.ToString());
                    OracleDataReader dr;
                    conn.Open();

                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        this.Hide();
                        first = textBox1.Text;
                        email = textBox1.Text;

                        Form1 dashboard = new Form1();
                        dashboard.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Admin Account Exist ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = "";
                    }
                }
            }
        }
    }
}
