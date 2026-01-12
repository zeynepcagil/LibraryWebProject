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
6. After execution, the `library` database along with all tables (**Books, Users, Loans, Reviews**), relationships, stored procedures, and data will be created.

### ⚠️ Important Notes
- If the database already exists, you may need to **delete the old one first**.
- **Local Copy Only:** Once the script is executed, the database is created **only on your local machine**. It is **not automatically connected** to the original database on someone else’s computer.

---

## 📸 Project Screenshots

### 🔐 Authentication & Entry
**Sign Up Screen:** Clean interface for new student registration.
![Sign Up Screen](screenshots/signup.png)

### 🛡️ Admin Dashboard & Management
**Admin Panel:** The central hub for library management.
![Admin Panel](screenshots/admin_panel.png)

**Add New Book:** Interface for cataloging new inventory.
![Add New Book](screenshots/add_new_book.png)

**Lend Book:** Dedicated workflow for assigning books to students.
![Lend Book](screenshots/lend_book.png)


### 👤 Student Experience
**My Library Page:** Students can track their active loans and check due dates.
![My Library Page](screenshots/my_library.png)

**Reviews:** Users can rate books and leave feedback.
![Review Page](screenshots/review.png)

---

## ✅ Completed Features

### 1. Advanced Authentication & Navigation
- **Role-Based Redirection:** When logging in, the system automatically checks if the user is an **Admin** or a **Student**.
  - **Admins** are redirected to the Admin Homepage with management tools.
  - **Students** are redirected to the User Homepage.
- **Session Management:** Secure session handling ensures users remain logged in and their UserID is accessible for transactions (like writing reviews).
- **Navigation:** Dynamic navbar that adjusts links based on the user's role (e.g., "Books Management" is hidden from Students).

### 2. Admin Privileges (Librarian)
- **Inventory Management:** Full CRUD capabilities for Books. Admin can add new books with details (Author, Category, Stock, Page Count, etc.).
- **Smart Return System & Fine Calculation:**
  - **Grace Period Logic:** A 24-hour grace period is applied after the due date.
  - **Automated Fines:** If a book is returned late (beyond grace period), the system automatically calculates the fine: `(Late Days * Daily Rate)`.
  - **Visual Alerts:** The system displays color-coded alerts (Yellow ⚠️ for fines, Green ✅ for on-time returns) during the return process.
- **Borrowing System:**
  - Dedicated **"Loan Book"** workflow.
  - Automatic stock decrement upon successful loan.

### 3. User Features (Student)
- **My Library Dashboard:**
  - Students can view their active loans in a modern card layout.
  - **Real-time Fine Tracking:** If a book is overdue, the system calculates and displays the **Current Fine Amount** in red directly on the dashboard.
  - **Status Indicators:** Badges show "Days Left" (Green/Yellow) or "Overdue" (Red).
- **Book Details & Interaction:**
  - Students can view detailed book information.
  - **Review & Rating System:** Logged-in students can rate books (1-10) and leave comments.
  - **Average Rating:** The system automatically recalculates the book's average score after every new review.

### 4. UI/UX Modernization
- **Living Dashboard:** The Home page features a "Live Occupancy" widget simulating library capacity.
- **Card Design:** Replaced standard tables with modern Bootstrap Cards, Shadows, and Badge indicators for a cleaner look.
- **Responsive:** Optimized for both desktop and mobile views.

---

## 📂 Project Architecture Notes

Please pay attention to the following file structures during development:

### Models
* `Context.cs`: Handles database connections (`Books`, `Users`, `Loans`, `Reviews`).
* `MyBookViewModel.cs`: A DTO (Data Transfer Object) used to carry Book details + Fine Amount to the User View.

### Controllers
* `AdminController`: Manages general Admin navigation.
* `BookController`: The core logic hub. Handles:
  - Listing & Creating Books.
  - **Return Logic** (with Fine Calculation).
  - **Review Logic** (AddReview).
  - Book Details.
* `UserController`: Handles Student-specific views, specifically `MyBooks`, which includes on-the-fly fine calculation for display.
* `AccountController`: Manages Login/Register logic, Session creation, and Role checks.

### appsettings.json
**Change the password in the given position according to your database server** `"DefaultConnection": "Server=localhost,1433;Database=library;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"`