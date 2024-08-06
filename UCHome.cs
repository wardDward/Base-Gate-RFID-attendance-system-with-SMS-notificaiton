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
    public partial class UCHome : UserControl
    {
        public UCHome()
        {
            InitializeComponent();

            label7.Text = DateTime.Now.ToShortDateString();
            label8.Text = DateTime.Now.ToLongTimeString();

            studentTotal();
            arrival();
            departure();
        }

        public void studentTotal()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql = "select Count(*) from student";
            OracleCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            var count = cmd.ExecuteScalar();
            studentNO.Text = count.ToString();
            conn.Close();

        }

        public void arrival()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql = "select Count(*) from attendance where time_in IS NOT NULL and date_attendance = '"+ DateTime.Now.ToString("MM/dd/yyyy") + "'";
            OracleCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            var count = cmd.ExecuteScalar();
            arrivalNo.Text = count.ToString();
            conn.Close();

        }

        public void departure()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql = "select Count(*) from attendance where NOT time_out = '-------' and date_attendance = '" + DateTime.Now.ToString("MM/dd/yyyy") + "'";
            OracleCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            var count = cmd.ExecuteScalar();
            deptNo.Text = count.ToString();
            conn.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void UCHome_Load(object sender, EventArgs e)
        {
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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label7.Text = DateTime.Now.ToShortDateString();
            label8.Text = DateTime.Now.ToLongTimeString();

            arrival();
            departure();
            studentTotal();
            timer1.Start();
        }
    }
}
