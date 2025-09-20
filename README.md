#Money Deposit Monitoring
A simple C# GUI system made out of my boredom.
This project is a small but functional Money Deposit Monitoring System, connected to a MariaDB database via XAMPP.

#Introduction

The Money Deposit Monitoring System is built with C# (Windows Forms/WPF) and MariaDB.
It lets you track deposits, balances, and transaction history in a user-friendly GUI.

I started this project as a way to learn C# database connectivity—and to kill boredom 😅.

#Features
- Add new deposits

- Monitor user balances

- View transaction history in the GUI

- Integrated with MariaDB via XAMPP

- Simple and lightweight interface

#Technologies Used

- Language: C# (.NET Framework / .NET 6 or later)

- Database: MariaDB (via XAMPP)

- IDE: Visual Studio 2012

- Connector: MySQL Connector for .NET (MySql.Data)

Clone Repository
git clone https://github.com/Shu-lab608/money-deposit-monitoring.git
cd money-deposit-monitoring

#Setup Database (MariaDB)

- Start XAMPP and run Apache + MySQL/MariaDB.

- Go to http://localhost/phpmyadmin
.

- Create a new database (e.g., money_db).

- Import the SQL file from /Database/money_db.sql.

Configure Database Connection

Update your connection string (App.config or in code): (Below)
<connectionStrings>
  <add name="MoneyDB"
       connectionString="server=localhost;user id=root;password=;database=money_db;"
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>


##Run the Project on your IDE
