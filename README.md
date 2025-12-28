# 📚 Library Automation Project

This document contains setup instructions for the database and an overview of the current project status, features, and architecture.

---

## 🗄️ Library Database Setup

This repository contains the structure and data of the **library** database. Your colleagues can create the same database on their local SQL Server by following the steps below.

### Requirements
- SQL Server (Express or full version)
- SQL Server Management Studio (SSMS)

### Setup Steps
1. Download or copy the `script.sql` file from this repository.
2. Open **SSMS** and connect to your server.
3. Open a **New Query** window.
4. Paste the entire code from `script.sql` into the query window **or** open it via `File → Open → script.sql`.
5. Click **Execute**.
6. After execution, the `library` database along with all tables, relationships, stored procedures, and data will be created.

### ⚠️ Important Notes
- If the database already exists, you may need to **delete the old one first**.
- **Local Copy Only:** Once the script is executed, the database is created **only on your local machine**. It is **not automatically connected** to the original database on someone else’s computer.
- Every person will have their own independent copy of the database.

---

## ✅ Completed Features

### 1. Advanced Authentication & Navigation
- **Role-Based Redirection:** When logging in, the system automatically checks if the user is an **Admin** or a **Student**.
  - **Admins** are redirected to the Admin Homepage with management tools.
  - **Students** are redirected to the User Homepage.
- **Error Handling:** The login page provides specific error messages for incorrect passwords or usernames.
- **Navigation:** Seamless navigation is implemented for Login, Logout, and page transitions.

### 2. Admin Privileges (Librarian)
- **Book Management:** Admins have exclusive access to add new books to the library inventory.
- **Borrowing System:**
  - Dedicated **"Borrow Book"** page for Admins.
  - The Admin selects a **User** and a **Book** from the database.
  - **Logic:** There is no manual "mark as borrowed" checkbox; clicking the "Borrow" button automatically decreases stock and creates a loan record in the system.

### 3. User Features (Student)
- **My Books:** Upon logging in, students can view the books they have currently borrowed.
- **Tracking:** The system displays:
  - The **Due Date** for the book.
  - A **Remaining Days** counter (automatically calculated).
  - Status indicators (e.g., if a book is overdue).

---

## 📂 Project Architecture Notes

Please pay attention to the following file structures during development:

### Models
* `Context.cs`: Handles database connections (`Books`, `Users`, `Loans`).

### Controllers
* `AdminController`: Manages Librarian operations (Borrowing flow, User selection).
* `BookController`: Handles book creation and listing.
* `UserController`: Handles Student-specific views (My Borrowed Books).
* `AccountController`: Manages Login/Register logic and Role checks.

### apsettings.json
**Change the password in the given position according to your database server**     
"DefaultConnection": "Server=localhost,1433;Database=library;User Id=sa; **Password=----------**;TrustServerCertificate=True;"
