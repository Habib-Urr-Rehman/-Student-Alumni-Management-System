# Student Management System

## Overview

This is a WPF application for managing student records and user accounts. The application features a user-friendly interface designed using XAML for the frontend, and C# for the backend logic. It utilizes Entity Framework for database operations. The project consists of two main database tables: `Student` and `User`. 

The system supports three types of users: Clerk, Admin, and Super Admin, each with varying levels of access and functionality.

## User Roles and Permissions

### Clerk
- **View Student Records:** Clerks can view student records.
- **Search and Filter:** Clerks can search and apply filters based on City, Session, and Degree.
- **Restrictions:** Clerks do not have permissions to add, edit, or delete student records.

### Admin
- **All Clerk Functionalities:** Admins inherit all functionalities available to Clerks.
- **Add, Edit, and Delete:** Admins can add, edit, and delete student records.
- **Export and Import:** 
  - **Export:** Admins can export student data to an Excel sheet at a user-selected location.
  - **Import:** Admins can import student data from an Excel file. A sample file (`EAD project.xlsx`) is provided for testing the import functionality.

### Super Admin
- **All Admin Functionalities:** Super Admins inherit all functionalities available to Admins and Clerks.
- **Manage Users:** 
  - **Add, Edit, and Delete Users:** Super Admins can manage user accounts, including adding new users as Clerk, Admin, or Super Admin, as well as editing and deleting existing users.
  - **Search and Filter Users:** Super Admins can search users by username and apply filters based on username, password, and role.
- **Import and Export User Data:** 
  - **Export:** Super Admins can export user data to an Excel sheet.
  - **Import:** Super Admins can import user data from an Excel file. A sample file (`Users.xlsx`) is provided for testing the import functionality.

## Project Setup

### Database Setup
1. **Create Database and Tables:** The SQL script for creating the database and tables is provided in the `Database.txt` file.
### Entity Framework Setup:
- After creating the database and tables, run the following scaffold command:
  ```
  Scaffold-DbContext "Your_Connection_String" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
  ```
- Replace `"Your_Connection_String"` with the connection string of your database.
- This command will generate a `Models` folder containing all the necessary classes.
- If you want to make a new project, run the above command. Otherwise, if you want to run the downloaded project, the `Models` folder is already provided, so there is no need to run the scaffold command.

### Required Packages
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.EntityFrameworkCore.Design`
- `EPPlus`
- `ExcelDataReader.DataSet`

### Running the Application
1. Ensure the database is created and configured.
2. Restore the required packages.
3. Run the application from Visual Studio or your preferred IDE.

## Sample Data Files
- **Student Import Sample:** `EAD project.xlsx`
- **User Import Sample:** `Users.xlsx`

## Notes
- Ensure the database connection string is correctly configured in the application settings.
- Use the provided sample files to test the import functionality.
- Follow the provided database creation script to set up your database before running the scaffold command.

This project demonstrates a complete student management system with varying user access levels and functionalities, providing a robust and flexible solution for educational institutions.
