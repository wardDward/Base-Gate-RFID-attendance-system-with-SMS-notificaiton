using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace SE_RFID_PROJECT
{
    public partial class UCAddAdmin : UserControl
    {
        public UCAddAdmin()
        {
            InitializeComponent();
            loadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddAdminAcc aac = new AddAdminAcc(this);
            aac.Show();
        }

        private void UCAddAdmin_Load(object sender, EventArgs e)
        {
            Color[] colors = {
              ColorTranslator.FromHtml("#99dbe4"), // start color
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

        public void loadData()
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql = "select * from admin";

            try
            {

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                DataTable dtbl = new DataTable();

                OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
                adp.Fill(dtbl);
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dtbl;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
         

            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);
            string sql =
                "SELECT * from admin where id LIKE '" + textBox1.Text + "%' OR lastname LIKE '" + textBox1.Text + "%'";

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            DataTable dtbl = new DataTable();
            conn.Open();
            OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
            adp.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            conn.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Search Admin ID or Name";
                textBox1.ForeColor = Color.Gray;
            }
            loadData();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search Admin ID or Name")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Gray;
            }
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/ORCL; USER ID=system; PASSWORD=root;";
            OracleConnection conn = new OracleConnection(conStr);

            string colname = dataGridView1.Columns[e.ColumnIndex].Name;
            string sql = "SELECT * from admin where id = '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'";

            if (colname == "colEdit")
            {
                UpdateAdmin updateAdmin = new UpdateAdmin(this);
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    updateAdmin.id.Text = dr.GetString(0);
                    updateAdmin.firstname.Text = dr.GetString(1);
                    updateAdmin.lastname.Text = dr.GetString(2);
                    updateAdmin.email.Text = dr.GetString(3);
                    updateAdmin.password.Text = dr.GetString(4);
                    updateAdmin.confirm_password.Text = dr.GetString(4);
                    updateAdmin.privilege.Text = dr.GetString(5);


                    updateAdmin.ShowDialog();
                }

                dr.Close();
                conn.Close();
            }
        }
    }
}
