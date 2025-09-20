using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Money_deposit_monitoring_system
{
    public partial class Form3 : Form
    {
        private int userId;
        private string userName;
        private decimal balance;
        private string connectionString = "server=localhost;database=money_db;uid=root;pwd=;";

        // 🔹 Constructor with 3 arguments
        public Form3(int id, string name, decimal startingBalance)
        {
            InitializeComponent();
            userId = id;
            userName = name;
            balance = startingBalance;

            SetupForm();
        }

        // 🔹 Constructor with 2 arguments (auto-fetch balance from DB)
        public Form3(int id, string name)
        {
            InitializeComponent();
            userId = id;
            userName = name;

            // Fetch balance from database automatically
            balance = GetBalanceFromDatabase(userId);

            SetupForm();
        }

        // 🔹 Common setup (non-resizable form, centered)
        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Always get the latest balance from DB when the form loads
            balance = GetBalanceFromDatabase(userId);

            label1.Text = "Welcome, " + userName + "!";
            lblBalance.Text = "Available Balance: ₱" + balance.ToString("N2");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?",
                                                  "Logout Confirmation",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            decimal depositAmount = PromptForAmount("Deposit Amount");

            if (depositAmount <= 0)
            {
                MessageBox.Show("Invalid deposit amount.");
                return;
            }

            UpdateBalanceInDatabase(depositAmount, true); // true = deposit

            // Refresh from DB
            balance = GetBalanceFromDatabase(userId);

            lblBalance.Text = "Available Balance: ₱" + balance.ToString("N2");
            MessageBox.Show("Deposited ₱" + depositAmount.ToString("N2") + " successfully!");
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            decimal withdrawAmount = PromptForAmount("Withdraw Amount");

            if (withdrawAmount <= 0)
            {
                MessageBox.Show("Invalid withdrawal amount.");
                return;
            }

            if (withdrawAmount > balance)
            {
                MessageBox.Show("Insufficient balance!");
                return;
            }

            UpdateBalanceInDatabase(withdrawAmount, false); // false = withdraw

            // Refresh from DB
            balance = GetBalanceFromDatabase(userId);

            lblBalance.Text = "Available Balance: ₱" + balance.ToString("N2");
            MessageBox.Show("Withdrew ₱" + withdrawAmount.ToString("N2") + " successfully!");
        }

        // ✅ Update DB and let SQL do the math
        private void UpdateBalanceInDatabase(decimal amount, bool isDeposit)
        {
            string query;

            if (isDeposit)
            {
                query = @"
                INSERT INTO balances (user_id, balance)
                VALUES (@UserId, @Amount)
                ON DUPLICATE KEY UPDATE balance = balance + @Amount";
            }
            else
            {
                query = @"
                INSERT INTO balances (user_id, balance)
                VALUES (@UserId, -@Amount)
                ON DUPLICATE KEY UPDATE balance = balance - @Amount";
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Amount", amount);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private decimal GetBalanceFromDatabase(int userId)
        {
            decimal currentBalance = 0;
            string query = "SELECT balance FROM balances WHERE user_id = @UserId LIMIT 1";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentBalance = Convert.ToDecimal(result);
                }
            }

            return currentBalance;
        }

        private decimal PromptForAmount(string title)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 160,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label()
            {
                Left = 20,
                Top = 20,
                Width = 250,
                Text = "Enter amount:"
            };

            TextBox textBox = new TextBox()
            {
                Left = 20,
                Top = 50,
                Width = 240
            };

            Button confirmation = new Button()
            {
                Text = "OK",
                Left = 180,
                Width = 80,
                Top = 80,
                DialogResult = DialogResult.OK
            };

            confirmation.Click += delegate { prompt.Close(); };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            decimal amount = 0;
            if (prompt.ShowDialog() == DialogResult.OK && decimal.TryParse(textBox.Text, out amount))
            {
                return amount;
            }
            return 0;
        }
    }
}
