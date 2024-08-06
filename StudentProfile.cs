using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace SE_RFID_PROJECT
{
    public partial class StudentProfile : Form
    {
       

        private System.Windows.Forms.Timer timer3;

        public StudentProfile()
        {
            InitializeComponent();
            label10.BackColor = ColorTranslator.FromHtml("#0066FF");
            // Create an array of colors for the gradient
            Color[] colors = {
            ColorTranslator.FromHtml("#52a2de"), // start color
            ColorTranslator.FromHtml("#99dbe4"),
            ColorTranslator.FromHtml("#75aee2") // end color
           };

            // Create a new LinearGradientBrush
            LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0), // start point
                new Point(panel1.Width, panel1.Height), // end point
                colors[0], // start color
                colors[colors.Length - 1] // end color
            );
            brush.InterpolationColors = new ColorBlend
            {
                Colors = colors,
                Positions = new[] { 0f, 0.5f, 1f }
            };



            rfid.Focus();

            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();

            text = label10.Text;
            label10.Text = "";

            timer1.Start();

            timeCheck.Visible = false;
            timeCheck.Text = "";

            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            lblID.Visible= false;
            lblName.Visible = false;
            lblGrade.Visible = false;
            lblSection.Visible = false;
            lblTime.Visible = false;
            lblDate.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            pic.Visible = false;

            // Create a new Timer object
            timer3 = new System.Windows.Forms.Timer();

            // Set the interval to 5 seconds
            timer3.Interval = 5000;
            timer3.Enabled = true;
            // Add an event handler for the Tick event
            timer3.Tick += new EventHandler(timer3_Tick);

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
            AdminLogin login = new AdminLogin();
            login.Show();
           
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);


        private string text;
        private int len = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (len < text.Length)
            {
                label10.Text = label10.Text + text.ElementAt(len);
                len++;
            }
            else if (len == text.Length)
            {
                len = 0;
                label10.Text = "";
                label10.Text = label10.Text + text.ElementAt(len);
                len++;
            }
            else { 
                timer1.Stop();
            }


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer2.Start();

        
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            // Check if the Text property is empty or null
            if (rfid.Text == "")
            {

                timeCheck.Text = "";
                rfid.Text = "";
                lblID.Text = "";
                lblName.Text = "";
                lblGrade.Text = "";
                lblSection.Text = "";
                pic.BackgroundImage = Image.FromFile(Application.StartupPath + @"\image\studentProfile.jpg");


                label10.Visible = true;
                pictureBox2.Visible = true;

                timeCheck.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                lblID.Visible = false;
                lblName.Visible = false;
                lblGrade.Visible = false;
                lblSection.Visible = false;
                lblTime.Visible = false;
                lblDate.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = false;
                panel8.Visible = false;
                pic.Visible = false;

                // Stop the Timer control
                timer3.Stop();
            }
        }


        private Queue<string> messageQueue = new Queue<string>();
        private bool isSendingMessages = false;

        private async void rfid_TextChanged(object sender, EventArgs e)
        {

            if (rfid.Text == "")
            {
                // Start the Timer control
                timer3.Start();
            }
            else
            {
                // Stop the Timer control
                timer3.Stop();
            }


            pictureBox2.Visible = false;
            label10.Visible = false;

            timeCheck.Visible = true;

            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            lblID.Visible = true;
            lblName.Visible = true;
            lblGrade.Visible = true;
            lblSection.Visible = true;
            lblTime.Visible = true;
            lblDate.Visible = true;
            panel3.Visible = true;
            panel4.Visible = true;
            panel5.Visible = true;
            panel6.Visible = true;
            panel7.Visible = true;
            panel8.Visible = true;
            pic.Visible = true;

            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";

            OracleConnection conn = new OracleConnection(conStr);
            string query = "SELECT student.image, student.id,student.firstname,student.lastname,student.contact,grade.section,grade.grade,student.middlename FROM student INNER JOIN grade ON student.id = grade.student_id where student.rfid = :rfid";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("rfid", rfid.Text);
            conn.Open();
            OracleDataReader dr;
            dr = cmd.ExecuteReader();
            dr.Read();
            if (rfid.TextLength >= 10)
            {
                if (dr.HasRows)
                {
                    rfid.Text = "";
                    lblID.Text = dr.GetString(1);
                    lblName.Text = dr.GetString(3) + " " + dr.GetString(2);
                    lblGrade.Text = dr.GetString(6);
                    lblSection.Text = dr.GetString(5);

                    long len = dr.GetBytes(0, 0, null, 0, 0);
                    byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                    dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len) + 1);

                    MemoryStream ms = new MemoryStream(array);
                    Bitmap bitmap = new Bitmap(ms);
                    pic.BackgroundImage = bitmap;

                    rfid.Enabled = false;

                    await Task.Delay(200);
                    rfid.Enabled = true;
                    rfid.Focus();

                        // Get the current time
                        DateTime currentTime = DateTime.Now;

                        // Check if the current time is between 5AM and 7AM
                        if (currentTime.Hour >= 5 && currentTime.Hour < 7)
                        {
                        // Run the method for the 5AM-7AM time range
                            string message = dr.GetString(3) + "," + dr.GetString(2) + "," + dr.GetInt64(4).ToString() + "," + DateTime.Now.ToShortTimeString();
                            messageQueue.Enqueue(message);
                            onTime(dr.GetString(1), dr.GetString(3), dr.GetString(2), dr.GetString(7));
                        }
                        // Check if the current time is between 7AM and 12AM
                        else if (currentTime.Hour >= 7 && currentTime.Hour < 21)
                        {

                        // Run the method for the 7AM-12AM time range
                        string message = dr.GetString(3) + "," + dr.GetString(2) + "," + dr.GetInt64(4).ToString() + "," + DateTime.Now.ToShortTimeString();
                            messageQueue.Enqueue(message);
                            lateTime(dr.GetString(1), dr.GetString(3), dr.GetString(2), dr.GetString(7));
                        }
                        else if (currentTime.Hour >= 21 && currentTime.Hour < 24)
                    {

                        // Run the method for the 7AM-12AM time range
                        string message = dr.GetString(3) + "," + dr.GetString(2) + "," + dr.GetInt64(4).ToString() + "," + DateTime.Now.ToShortTimeString();
                            messageQueue.Enqueue(message);

                            outTIme(dr.GetString(1), dr.GetString(3), dr.GetString(2), dr.GetString(7));
                        }
                    else
                        {
                             // Do nothing if the current time is outside the specified time ranges
                             MessageBox.Show("Current time is not within the specified time ranges.", "Time Range is invalid", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                             rfid.Focus();
                             timeCheck.Text = "";
                             rfid.Text = "";
                             lblID.Text = "";
                             lblName.Text = "";
                             lblGrade.Text = "";
                             lblSection.Text = "";
                             pic.BackgroundImage = Image.FromFile(Application.StartupPath + @"\image\studentProfile.jpg");
                    }

                }
                else
                {
                    rfid.Focus();
                    timeCheck.Text = "NO RECORD FOUND!";
                    rfid.Text = "";
                    lblID.Text = "";
                    lblName.Text = "";
                    lblGrade.Text = "";
                    lblSection.Text = "";
                    pic.BackgroundImage = Image.FromFile(Application.StartupPath + @"\image\studentProfile.jpg");

                }


            }
            conn.Close();
        }




        public async void onTime(string id, string lastname, string firstname,string middlename)
        {
            int cid = 0;

            string _timeIN = "";
            string _timeOUT = "";

            string conStr1 = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con2 = new OracleConnection(conStr1);

            string sql = "select id,time_in,time_out from attendance where student_id = :id and (date_attendance between :date1 and :date2) ";
            con2.Open();
            OracleCommand cmd = con2.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("id", id);
            cmd.Parameters.Add("date1", DateTime.Now.ToString("MM/dd/yyyy"));
            cmd.Parameters.Add("date2", DateTime.Now.ToString("MM/dd/yyyy"));

            OracleDataReader dr;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                _timeIN = dr.GetString(1);
                _timeOUT = dr.GetString(2);
                cid = dr.GetInt32(0);
            }

            dr.Close();
            con2.Close();

            if (cid == 0 && (_timeIN == "" || _timeIN == "-------"))
            {
                timeCheck.Text = "      ***   TIME IN    ***    ";

                string connect = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(connect);
                con.Open();
                string sql1 = "INSERT INTO attendance(student_id,firstname,lastname,middlename,time_in,date_attendance)VALUES(:id,:firstname,:lastname,:middlename,:time_in,:date_attendance)";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("id", id);
                cmd1.Parameters.Add("firstname", firstname);
                cmd1.Parameters.Add("lastname", lastname);
                cmd1.Parameters.Add("middlename", middlename);
                cmd1.Parameters.Add("time_in", DateTime.Now.ToShortTimeString());
                cmd1.Parameters.Add("date_attendance", DateTime.Now.ToString("MM/dd/yyyy"));
                cmd1.ExecuteNonQuery();

                // Start background task to send messages if not already running
                if (!isSendingMessages)
                {
                    isSendingMessages = true;
                    await Task.Run(() => SendMessagesTimeIN());
                }


                con.Close();
            }
            else if (_timeOUT == "-------")
            {
                timeCheck.Text = "      ***   TIME OUT    ***    ";

                string connect = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(connect);
                con.Open();
                OracleCommand cmd3 = con.CreateCommand();
                string sql11 = "update attendance set time_out = :time_out where student_id = :id";
                cmd3.CommandText = sql11;
                cmd3.CommandType = CommandType.Text;
                cmd3.Parameters.Add("time_out", DateTime.Now.ToShortTimeString());
                cmd3.Parameters.Add("id", id);
                cmd3.ExecuteNonQuery();

                // Start background task to send messages if not already running
                if (!isSendingMessages)
                {
                    isSendingMessages = true;
                    await Task.Run(() => SendMessagesTimeOUT());
                }

                con.Close();
            }
            else
            {

                timeCheck.Text = "* ALREADY TAPPPED IN *";
            }
        }

        public async void lateTime(string id, string lastname, string firstname, string middlename)
        {
            int cid = 0;

            string _timeIN = "";
            string _timeOUT = "";

            string conStr1 = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con2 = new OracleConnection(conStr1);

            string sql = "select id,time_in,time_out from attendance where student_id = :id and (date_attendance between :date1 and :date2) ";
            con2.Open();
            OracleCommand cmd = con2.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("id", id);
            cmd.Parameters.Add("date1", DateTime.Now.ToString("MM/dd/yyyy"));
            cmd.Parameters.Add("date2", DateTime.Now.ToString("MM/dd/yyyy"));

            OracleDataReader dr;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                _timeIN = dr.GetString(1);
                _timeOUT = dr.GetString(2);
                cid = dr.GetInt32(0);
            }

            dr.Close();
            con2.Close();

            if (cid == 0 && (_timeIN == "" || _timeIN == "-------"))
            {
                timeCheck.Text = "      ***   TIME IN    ***    ";

                string connect = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(connect);
                con.Open();
                string sql1 = "INSERT INTO attendance(student_id,firstname,lastname,middlename,time_in,date_attendance)VALUES(:id,:firstname,:lastname,:middlename,:time_in,:date_attendance)";
                OracleCommand cmd1 = con.CreateCommand();
                cmd1.CommandText = sql1;
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.Add("id", id);
                cmd1.Parameters.Add("firstname", firstname);
                cmd1.Parameters.Add("lastname", lastname);
                cmd1.Parameters.Add("middlename", middlename);
                cmd1.Parameters.Add("time_in", DateTime.Now.ToShortTimeString());
                cmd1.Parameters.Add("date_attendance", DateTime.Now.ToString("MM/dd/yyyy"));
                cmd1.ExecuteNonQuery();


                // Start background task to send messages if not already running
                if (!isSendingMessages)
                {
                    isSendingMessages = true;
                    await Task.Run(() => SendMessagesStudentLate());
                }


                con.Close();
            }
            else if (_timeOUT == "-------")
            {
                timeCheck.Text = "      ***   TIME OUT    ***    ";

                string connect = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(connect);
                con.Open();
                OracleCommand cmd3 = con.CreateCommand();
                string sql11 = "update attendance set time_out = :time_out where student_id = :id";
                cmd3.CommandText = sql11;
                cmd3.CommandType = CommandType.Text;
                cmd3.Parameters.Add("time_out", DateTime.Now.ToShortTimeString());
                cmd3.Parameters.Add("id", id);
                cmd3.ExecuteNonQuery();

                // Start background task to send messages if not already running
                if (!isSendingMessages)
                {
                    isSendingMessages = true;
                    await Task.Run(() => SendMessagesTimeOUT());
                }

                con.Close();
            }
            else
            {

                timeCheck.Text = "* ALREADY TAPPPED IN *";
            }

        }

        public async void outTIme(string id, string lastname, string firstname, string middlename)
        {
            int cid = 0;

            string _timeIN = "";
            string _timeOUT = "";

            string conStr1 = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection con2 = new OracleConnection(conStr1);

            string sql = "select id,time_in,time_out from attendance where student_id = :id and (date_attendance between :date1 and :date2) ";
            con2.Open();
            OracleCommand cmd = con2.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("id", id);
            cmd.Parameters.Add("date1", DateTime.Now.ToString("MM/dd/yyyy"));
            cmd.Parameters.Add("date2", DateTime.Now.ToString("MM/dd/yyyy"));

            OracleDataReader dr;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                _timeIN = dr.GetString(1);
                _timeOUT = dr.GetString(2);
                cid = dr.GetInt32(0);
            }

            dr.Close();
            con2.Close();

            if (_timeOUT == "-------")
            {
                timeCheck.Text = "      ***   TIME OUT    ***    ";

                string connect = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(connect);
                con.Open();
                OracleCommand cmd3 = con.CreateCommand();
                string sql11 = "update attendance set time_out = :time_out where student_id = :id";
                cmd3.CommandText = sql11;
                cmd3.CommandType = CommandType.Text;
                cmd3.Parameters.Add("time_out", DateTime.Now.ToShortTimeString());
                cmd3.Parameters.Add("id", id);
                cmd3.ExecuteNonQuery();

                // Start background task to send messages if not already running
                if (!isSendingMessages)
                {
                    isSendingMessages = true;
                    await Task.Run(() => SendMessagesTimeOUT());
                }

                con.Close();
            }
            else if (cid == 0)
            {
                timeCheck.Text = "      ***   TIME OUT    ***    ";

                string connect = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
                OracleConnection con = new OracleConnection(connect);
                con.Open();
                OracleCommand cmd3 = con.CreateCommand();
                string sql11 = "INSERT INTO attendance(student_id,firstname,lastname,middlename,time_out,date_attendance)VALUES(:id,:firstname,:lastname,:middlename,:time_out,:date_attendance)";
                cmd3.CommandText = sql11;
                cmd3.CommandType = CommandType.Text;
                cmd3.Parameters.Add("id", id);
                cmd3.Parameters.Add("firstname", firstname);
                cmd3.Parameters.Add("lastname", lastname);
                cmd3.Parameters.Add("middlename", middlename);
                cmd3.Parameters.Add("time_out", DateTime.Now.ToShortTimeString());
                cmd3.Parameters.Add("date_attendance", DateTime.Now.ToString("MM/dd/yyyy"));

                cmd3.ExecuteNonQuery();

                // Start background task to send messages if not already running
                if (!isSendingMessages)
                {
                    isSendingMessages = true;
                    await Task.Run(() => SendMessagesNoTimeIN());
                }

                con.Close();
            }
            else
            {

                timeCheck.Text = "* ALREADY TAPPPED IN *";

            }
        }



        // sms message method
        private async void SendMessagesTimeIN()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";

            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                string[] parts = message.Split(',');
                string firstname = parts[1];
                string lastname = parts[0];
                long contact = long.Parse(parts[2]);
                DateTime timestamp = DateTime.Parse(parts[3]);

                await Task.Delay(5000); // Add a delay of 5 seconds between sending messages

                try
                {
                    string SMSSQL = "SELECT port, REPLACE(REPLACE(REPLACE(message, '@Parent', '" + lastname + "'), '@Student', '" + firstname + "'), '@Time', '" + DateTime.Now.ToShortTimeString() + "') AS new_value FROM sms WHERE message_for = 'Time IN'";

                    using (OracleConnection conn = new OracleConnection(conStr))
                    {
                        using (OracleCommand cmd = new OracleCommand(SMSSQL, conn))
                        {
                            conn.Open();

                            using (OracleDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    using (SerialPort sp = new SerialPort())
                                    {
                                        sp.PortName = dr.GetString(0);
                                        sp.Open();
                                        sp.WriteLine("AT" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGS=\"" + 0 + contact + "\"" + Environment.NewLine);
                                        await Task.Delay(100);

                                        string smsText = dr.GetString(1) + Environment.NewLine;
                                        sp.WriteLine(smsText);
                                        await Task.Delay(100);

                                        sp.Write(new byte[] { 26 }, 0, 1);
                                        await Task.Delay(200);
                                    }
                                }
                            }
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending SMS: " + ex.Message, "SMS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            isSendingMessages = false;
        }

        private async void SendMessagesTimeOUT()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";

            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                string[] parts = message.Split(',');
                string firstname = parts[1];
                string lastname = parts[0];
                long contact = long.Parse(parts[2]);
                DateTime timestamp = DateTime.Parse(parts[3]);

                await Task.Delay(5000); // Add a delay of 5 seconds between sending messages

                try
                {
                    string SMSSQL = "SELECT port, REPLACE(REPLACE(REPLACE(message, '@Parent', '" + lastname + "'), '@Student', '" + firstname + "'), '@Time', '" + DateTime.Now.ToString("HH:mm:ss") + "') AS new_value FROM sms WHERE message_for = 'Time OUT'";

                    using (OracleConnection conn = new OracleConnection(conStr))
                    {
                        using (OracleCommand cmd = new OracleCommand(SMSSQL, conn))
                        {
                            conn.Open();

                            using (OracleDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    using (SerialPort sp = new SerialPort())
                                    {
                                        sp.PortName = dr.GetString(0);
                                        sp.Open();
                                        sp.WriteLine("AT" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGS=\"" + 0 + contact + "\"" + Environment.NewLine);
                                        await Task.Delay(100);

                                        string smsText = dr.GetString(1) + " " + DateTime.Now.ToShortDateString() + Environment.NewLine;
                                        sp.WriteLine(smsText);
                                        await Task.Delay(100);

                                        sp.Write(new byte[] { 26 }, 0, 1);
                                        await Task.Delay(200);
                                    }
                                }
                            }
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending SMS: " + ex.Message, "SMS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            isSendingMessages = false;
        }

        private async void SendMessagesStudentLate()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";

            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                string[] parts = message.Split(',');
                string firstname = parts[1];
                string lastname = parts[0];
                long contact = long.Parse(parts[2]);
                DateTime timestamp = DateTime.Parse(parts[3]);

                await Task.Delay(5000); // Add a delay of 5 seconds between sending messages

                try
                {
                    string SMSSQL = "SELECT port, REPLACE(REPLACE(REPLACE(message, '@Parent', '" + lastname + "'), '@Student', '" + firstname + "'), '@Time', '" + timestamp + "') AS new_value FROM sms WHERE message_for = 'Student Late'";

                    using (OracleConnection conn = new OracleConnection(conStr))
                    {
                        using (OracleCommand cmd = new OracleCommand(SMSSQL, conn))
                        {
                            conn.Open();

                            using (OracleDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    using (SerialPort sp = new SerialPort())
                                    {
                                        sp.PortName = dr.GetString(0);
                                        sp.Open();
                                        sp.WriteLine("AT" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGS=\"" + 0 + contact + "\"" + Environment.NewLine);
                                        await Task.Delay(100);

                                        string smsText = dr.GetString(1) + Environment.NewLine;
                                        sp.WriteLine(smsText);
                                        await Task.Delay(100);

                                        sp.Write(new byte[] { 26 }, 0, 1);
                                        await Task.Delay(200);
                                    }
                                }
                            }
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending SMS: " + ex.Message, "SMS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            isSendingMessages = false;
        }

        private async void SendMessagesNoTimeIN()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";

            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                string[] parts = message.Split(',');
                string firstname = parts[1];
                string lastname = parts[0];
                long contact = long.Parse(parts[2]);
                DateTime timestamp = DateTime.Parse(parts[3]);

                await Task.Delay(5000); // Add a delay of 5 seconds between sending messages

                try
                {
                    string SMSSQL = "SELECT port, REPLACE(REPLACE(REPLACE(message, '@Parent', '" + lastname + "'), '@Student', '" + firstname + "'), '@Time', '" + DateTime.Now.ToString("HH:mm:ss") + "') AS new_value FROM sms WHERE message_for = 'No Time IN Record'";

                    using (OracleConnection conn = new OracleConnection(conStr))
                    {
                        using (OracleCommand cmd = new OracleCommand(SMSSQL, conn))
                        {
                            conn.Open();

                            using (OracleDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    using (SerialPort sp = new SerialPort())
                                    {
                                        sp.PortName = dr.GetString(0);
                                        sp.Open();
                                        sp.WriteLine("AT" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
                                        await Task.Delay(100);

                                        sp.WriteLine("AT+CMGS=\"" + 0 + contact + "\"" + Environment.NewLine);
                                        await Task.Delay(100);

                                        string smsText = dr.GetString(1) + Environment.NewLine;
                                        sp.WriteLine(smsText);
                                        await Task.Delay(100);

                                        sp.Write(new byte[] { 26 }, 0, 1);
                                        await Task.Delay(200);
                                    }
                                }
                            }
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending SMS: " + ex.Message,"SMS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            isSendingMessages = false;
        }

        private void StudentProfile_Load(object sender, EventArgs e)
        {
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
