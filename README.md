# 📚 Smart Library Management System

This project is a library management solution developed with **ASP.NET Core MVC**. It focuses on automating library assets, borrowing workflows, and enforcing smart business rules for user management.

---

## 🚀 Project Status & Features

The currently active modules in the system are as follows:

### 1. Authentication & Authorization
- **Login:** Users can log in using their email and password.
- **Role Management:** The system automatically detects whether the logged-in user is a **Student** or an **Admin**.
  - **Admin:** Redirected to the Management Panel.
  - **Student:** Redirected to the Book Search page.
- **Register:** New users can register to the system as students.

### 2. Book Management
- **Listing:** All books are listed on the home page.
- **Search & Filter:**
  - Search by book title or author.
  - Filter by categories.
  - Stock status check.
- **Add Book:** Only users with Admin privileges can add new books.

### 3. Borrowing System (Core)
- **Admin Panel:** The Librarian (Admin) initiates the borrowing process by selecting a user and a book from a dedicated interface.
- **Stock Tracking:** When a borrowing transaction occurs, the system automatically decreases the **Stock** quantity by 1.
- **Logging:** The transaction is recorded in the `Loans` table.

### 4. Student Panel (My Books)
- **My Books:** Students can view the books assigned to them after logging in.
- **Time Tracking:**
  - Last delivery date (Due Date).
  - Remaining days.
  - "Overdue" alerts for late returns.

---

## 🛠️ Setup Instructions

Follow these steps to run the project on your local machine:

### 1. Database Setup
We will use the `script.sql` file located in the project root directory.
1. Open SQL Server Management Studio (SSMS) or Azure Data Studio.
2. Open and **Execute** the `script.sql` file.
   - *This action will create the `library` database and load the Seed Data.*

### 2. Connection String Configuration
Open the `appsettings.json` file and update the `ConnectionStrings` section with your own SQL Server credentials:

```json
"ConnectionStrings": {
  "Context": "Server=LOCALHOST;Database=library;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
### 3. Running the Project
Open your terminal and run the following command:
```
dotnet run
```

## 📂 Project Architecture
The project is developed following the **MVC (Model-View-Controller)** architecture.

**Models:**
- `Context.cs`: Entity Framework Core database context.
- `Book.cs`: Book data and stock logic.
- `User.cs`: User information and role definitions.
- `Loan.cs`: Borrowing records.

**Controllers:**
- `AdminController`: Manages borrowing operations.
- `UserController`: Manages the Student panel (MyBooks).
- `BookController`: Manages book listing and addition.
- `AccountController`: Manages Login/Register operations.

---

## 📝 Future Plans (To-Do)
- [ ] Return Process: Implementation of book return interface.
- [ ] Penalty System: Logic for calculating fines for overdue books.
- [ ] Reviews: Feature for students to rate and comment on books.
