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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SE_RFID_PROJECT
{
    public partial class UCSMS : UserControl
    {
        public UCSMS()
        {
            InitializeComponent();

        }

        private void UCSMS_Load(object sender, EventArgs e)
        {
            loadPort();
            richTextBox2.Text = "";
            richTextBox2.ReadOnly = true;
            richTextBox2.ForeColor = Color.Black;

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

        public void loadMessage()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "SELECT REPLACE(REPLACE(REPLACE(message, '@Parent', 'Dela Cruz'), '@Student', 'MoyMoy'), '@Time','7:00') as new_value from sms where message_for = :message_for";
            OracleCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("message_for", message_for.SelectedItem.ToString());
            OracleDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                    richTextBox2.Text = dr.GetString(0);
            }
            else
            {
                richTextBox2.Text = "";
            }
            con.Close();

        }

        public void loadPort()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "select port,message from sms";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                label6.Text = dr.GetString(0);
                textBox1.Text = dr.GetString(0);


            }
            con.Close();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);

            string sql = "select message from sms where message_for = :message_for";

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("message_for", message_for.SelectedItem.ToString());
            OracleDataReader dr;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                richTextBox1.Text = dr.GetString(0);
                loadMessage();
            }
            else
            {
                richTextBox1.Text = "";
                richTextBox2.Text = "";
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con = new OracleConnection(conStr);


            string selectSql = "select * from sms where message_for = '" + message_for.SelectedItem.ToString() + "'";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = selectSql;
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr;
            con.Open();
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                if (dr.GetString(2) == "Time IN")
                {
                    string sqlUpdate = "update sms set message = :message, port = :port where message_for = :message_for";
                    OracleCommand cmd1 = con.CreateCommand();
                    cmd1.CommandText = sqlUpdate;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Add("message", richTextBox1.Text);
                    cmd1.Parameters.Add("port", textBox1.Text);
                    cmd1.Parameters.Add("message_for", message_for.SelectedItem.ToString());
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("Time In Message Has Been Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadMessage();
                    loadPort();
                    con.Close();
                    return;

                }
                else if (dr.GetString(2) == "Time OUT")
                {
                    string sqlUpdate = "update sms set message = :message, port = :port where message_for = :message_for";
                    OracleCommand cmd1 = con.CreateCommand();
                    cmd1.CommandText = sqlUpdate;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Add("message", richTextBox1.Text);
                    cmd1.Parameters.Add("port", textBox1.Text);
                    cmd1.Parameters.Add("message_for", message_for.SelectedItem.ToString());
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("Time Out Message Has Been Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadMessage();
                    loadPort();
                    con.Close();
                    return;

                }
                else if (dr.GetString(2) == "Student Late")
                {
                    string sqlUpdate = "update sms set message = :message, port = :port where message_for = :message_for";
                    OracleCommand cmd1 = con.CreateCommand();
                    cmd1.CommandText = sqlUpdate;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Add("message", richTextBox1.Text);
                    cmd1.Parameters.Add("port", textBox1.Text);
                    cmd1.Parameters.Add("message_for", message_for.SelectedItem.ToString());
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("Late Student Message Has Been Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadMessage();
                    loadPort();
                    con.Close();
                    return;

                }
                else if (dr.GetString(2) == "No Time IN Record")
                {
                    string sqlUpdate = "update sms set message = :message, port = :port where message_for = :message_for";
                    OracleCommand cmd1 = con.CreateCommand();
                    cmd1.CommandText = sqlUpdate;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Add("message", richTextBox1.Text);
                    cmd1.Parameters.Add("port", textBox1.Text);
                    cmd1.Parameters.Add("message_for", message_for.SelectedItem.ToString());
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("No Time In Message has been Updated Succesfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadMessage();
                    loadPort();
                    con.Close();
                    return;

                }
                else
                {
                    MessageBox.Show("Something Went Wrong, setting up the message for "+ " " + dr.GetString(2), "SMS Message Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (message_for.SelectedItem.ToString() == "--Select--")
            {
                MessageBox.Show("There are no selected Notification", "Oooops", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                string sqlInsert = "insert into sms(message,message_for,port)values(:message,:message_for,:port)";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sqlInsert;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("message", richTextBox1.Text);
                cmd1.Parameters.Add("message_for", message_for.SelectedItem.ToString());
                cmd1.Parameters.Add("port", textBox1.Text);
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Message Has Been Set Successfully" + " " + message_for.SelectedItem.ToString(), "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadMessage();
                loadPort();
                con.Close();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
