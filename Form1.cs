using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_RFID_PROJECT
{
    public partial class Form1 : Form
    {
        static Form1 _obj;
        public static Form1 Instance
        {
            get { 
                if(_obj == null) {
                    _obj = new Form1();
                }
                return _obj;
            }
        }

        public Panel PnlContainer
        {
            get { return panelContainer; }
            set { panelContainer = value; }
        }

        public Button BackButton {
            get { return btnBack; }
            set { btnBack = value;  }
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);
        public Form1()
        {
            InitializeComponent();

            ChangeButtonColor(button1);

            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string sql = "select * from admin where id = '" + AdminLogin.first + "'";
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            OracleDataReader dr;
            conn.Open();

            dr = cmd.ExecuteReader();
            dr.Read();
            label2.Text = dr.GetString(1);

            if (dr.GetString(5) == "normal")
            {
                button7.Visible = false;
            }
            else {
                button7.Visible = true;
            }

            conn.Close();


            
        }
        private Button currentButton;

        private void ChangeButtonColor(Button clickedButton)
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.Transparent;
            }

            // Set the background color of the clicked navigation button

            clickedButton.BackColor = Color.LightBlue;

            // Update the currently selected button
            currentButton = clickedButton;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btnBack.Visible = false;
            _obj = this;

            UCHome uc = new UCHome();
            uc.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(uc);
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            panelContainer.Controls["UCHome"].BringToFront();
            ChangeButtonColor(button1);

            btnBack.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Form1.Instance.PnlContainer.Controls.ContainsKey("UCStudentList"))
            {
                UCStudentList usl = new UCStudentList();
                usl.Dock = DockStyle.Fill;
                Form1.Instance.PnlContainer.Controls.Add(usl);
            }
            Form1.Instance.PnlContainer.Controls["UCStudentList"].BringToFront();
            ChangeButtonColor(button2);
            Form1.Instance.BackButton.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Form1.Instance.PnlContainer.Controls.ContainsKey("UCSMS"))
            {
                UCSMS usl = new UCSMS();
                usl.Dock = DockStyle.Fill;
                Form1.Instance.PnlContainer.Controls.Add(usl);
            }
            Form1.Instance.PnlContainer.Controls["UCSMS"].BringToFront();
            ChangeButtonColor(button4);

            Form1.Instance.BackButton.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!Form1.Instance.PnlContainer.Controls.ContainsKey("UCEmail"))
            {
                UCEmail usl = new UCEmail();
                usl.Dock = DockStyle.Fill;
                Form1.Instance.PnlContainer.Controls.Add(usl);
            }
            Form1.Instance.PnlContainer.Controls["UCEmail"].BringToFront();
            ChangeButtonColor(button5);
            Form1.Instance.BackButton.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are sure you wanted to Logout?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                AdminLogin login = new AdminLogin();
                login.Show();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelContainer.Controls["UCHome"].BringToFront();
            ChangeButtonColor(button1);
            btnBack.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Form1.Instance.PnlContainer.Controls.ContainsKey("UCAttendanceReport"))
            {
                UCAttendanceReport usl = new UCAttendanceReport();
                usl.Dock = DockStyle.Fill;
                Form1.Instance.PnlContainer.Controls.Add(usl);
            }
            Form1.Instance.PnlContainer.Controls["UCAttendanceReport"].BringToFront();
            ChangeButtonColor(button3);

            Form1.Instance.BackButton.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!Form1.Instance.PnlContainer.Controls.ContainsKey("UCAddAdmin"))
            {
                UCAddAdmin usl = new UCAddAdmin();
                usl.Dock = DockStyle.Fill;
                Form1.Instance.PnlContainer.Controls.Add(usl);
            }
            Form1.Instance.PnlContainer.Controls["UCAddAdmin"].BringToFront();
            ChangeButtonColor(button7);
            Form1.Instance.BackButton.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }
    }
}
