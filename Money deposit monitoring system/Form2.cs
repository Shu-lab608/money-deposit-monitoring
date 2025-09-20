using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Money_deposit_monitoring_system
{
    public partial class Form2 : Form
    {
        string connectionString = "server=localhost;database=money_db;uid=root;pwd=;";

        public Form2()
        {
            InitializeComponent();

            // Prevent the form from being resizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Disable the maximize button
            this.MaximizeBox = false;

            // Optional: keep the form centered on screen
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // LINKLABEL CLICK → show Terms & Conditions in a popup form
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form popup = new Form
            {
                Text = "Terms & Conditions",
                Size = new System.Drawing.Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,

                // Make popup non-resizable too
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false
            };

            RichTextBox rtb = new RichTextBox
            {
                Dock = DockStyle.Top,
                Height = 320,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Text = "Terms & Conditions\n\n1. Rule one...\n2. Rule two...\nAdd your full terms here..."
            };

            Button btnClose = new Button
            {
                Text = "Close",
                Width = 100,
                Height = 30,
                Top = rtb.Bottom + 10,
                Left = (popup.ClientSize.Width - 100) / 2
            };
            btnClose.Click += (s, args) => popup.Close();

            popup.Controls.Add(rtb);
            popup.Controls.Add(btnClose);
            popup.ShowDialog();
        }

        // CREATE BUTTON CLICK → save data to MySQL and go to Form1
        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text.Trim();
            string middleName = textBox2.Text.Trim();
            string lastName = textBox3.Text.Trim();
            string gender = radioButton1.Checked ? "Male" : radioButton2.Checked ? "Female" : "";
            string email = textBox4.Text.Trim();
            string password = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Please fill all required fields and select gender.");
                return;
            }

            string fullName = firstName + " " + middleName + " " + lastName;
            string query = "INSERT INTO deposits (name, email, password, gender) VALUES (@Name, @Email, @Password, @Gender)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Gender", gender);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User created successfully!");

                    // Clear fields
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;

                    // Redirect to Form1
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // RADIOBUTTON & TEXTBOX EVENTS (optional)
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
    }
}
