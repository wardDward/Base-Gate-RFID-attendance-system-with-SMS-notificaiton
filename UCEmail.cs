using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_RFID_PROJECT
{
    public partial class UCEmail : UserControl
    {
        public UCEmail()
        {
            InitializeComponent();
            FillCombo();
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string sql = "select * from admin where id = '" + AdminLogin.email + "'";
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            OracleDataReader dr;
            conn.Open();

            dr = cmd.ExecuteReader();
            dr.Read();
            txtFrom.Text = dr.GetString(3);

            conn.Close();
        }

   

        private void button1_Click(object sender, EventArgs e)
        {
           
        }


        public void FillCombo() {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string sql = "select email from faculty_email";
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            OracleDataReader dr;
            conn.Open();

            dr = cmd.ExecuteReader();
            while (dr.Read()) {
                string email = dr.GetString(0);
                txtTo.Items.Add(email);
            }
            conn.Close();

        }
        private void UCEmail_Load(object sender, EventArgs e)
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
         
        }

        private void label6_Click(object sender, EventArgs e)
        {
            RegisterEmail re = new RegisterEmail(this);
            re.Show();
        }

     

        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (txtTo.SelectedIndex == 0 || txtTo.Text == "--SELECT--")
                {
                    MessageBox.Show("There are no Receipient to be sent at.", "Receipient is empty", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (txtBody.Text == "" && lblLocaction.Text == "") { 
                    MessageBox.Show("There are no Receipient to be sent at.", "Body is required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (lblLocaction.Text == "openFileDialog1") {
                    lblLocaction.Text = "";
                }
                else
                {

                    // Set the cursor to WaitCursor
                    this.Cursor = Cursors.WaitCursor;


                    MailMessage mail = new MailMessage();
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress(txtFrom.Text);

                    mail.To.Add(txtTo.SelectedItem.ToString());
                    mail.Subject = txtTitle.Text;
                    mail.Body = txtBody.Text;

                    if (!string.IsNullOrEmpty(lblLocaction.Text))
                    {
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(lblLocaction.Text);
                        mail.Attachments.Add(attachment);
                    }

                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(txtFrom.Text, "rjsjibszxhlmicmt");
                    smtp.EnableSsl = true;

                    smtp.Send(mail);


                    MessageBox.Show("Mail has been successfully sent", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblLocaction.Text = "";
                    txtBody.Text = "";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Set the cursor back to DefaultCursor
                this.Cursor = Cursors.Default;
            }
        }

        private void txtTo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // User selected a file
                lblLocaction.Text = openFileDialog1.FileName;
                // Do something with the selected file
            }
            else
            {
                // User cancelled the operation
                openFileDialog1.Reset(); // Clear the value of the OpenFileDialog
            }
        }
    }
}
