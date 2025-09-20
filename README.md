# Money Deposit Monitoring

A simple C# GUI system created as a small but functional **Money Deposit Monitoring System**, connected to a **MariaDB** database via **XAMPP**.

---

## Introduction

The **Money Deposit Monitoring System** is built with **C# (Windows Forms/WPF)** and **MariaDB**.  
It allows you to track deposits, balances, and transaction history through a user-friendly graphical interface.  

This project was developed to practice and demonstrate **C# database connectivity** concepts.

---

## Features

- Add new deposits  
- Monitor user balances  
- View transaction history in the GUI  
- Integrated with MariaDB via XAMPP  
- Simple and lightweight interface  

---

## Technologies Used

- **Language:** C# (.NET Framework / .NET 6 or later)  
- **Database:** MariaDB (via XAMPP)  
- **IDE:** Visual Studio 2012  
- **Connector:** MySQL Connector for .NET (`MySql.Data`)  

---
Configure Database Connection

Update your connection string (App.config or directly in your code):
<connectionStrings>
  <add name="MoneyDB"
       connectionString="server=localhost;user id=root;password=;database=money_db;"
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>


---

## Installation & Setup

### Clone Repository
```bash
git clone https://github.com/Shu-lab608/money-deposit-monitoring.git
cd money-deposit-monitoring
