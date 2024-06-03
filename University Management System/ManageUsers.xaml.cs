using ExcelDataReader;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using University_Management_System.Models;
//using OfficeOpenXml;
//using ExcelDataReader;

namespace University_Management_System
{

    public partial class ManageUsers : Page
    {
        EAD_ProjectContext zz = new EAD_ProjectContext();
        public static DataGrid ff;
        public ManageUsers()
        {
            InitializeComponent();
            mygridsuperadmin.ItemsSource = zz.Users.ToList();
            ff = mygridsuperadmin;
            PopulateUserNameFilterComboBox();
            PopulatePasswordFilterComboBox();
            PopulateRoleFilterComboBox();

        }



        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox comboBox))
                return;


            if (comboBox.SelectedItem?.ToString() == "Clear")
            {

                comboBox.SelectedItem = null;
            }


            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string Uname = usernameFilterComboBox.SelectedItem?.ToString();
            string passw = passwordFilterComboBox.SelectedItem?.ToString();
            string userrole = RoleFilterComboBox.SelectedItem?.ToString();

            using (var context = new EAD_ProjectContext())
            {
                var filteredData = context.Users.AsQueryable();

                // Apply city filter
                if (!string.IsNullOrEmpty(Uname) && Uname != "Clear")
                {
                    filteredData = filteredData.Where(user => user.Username == Uname);
                }

                // Apply session filter
                if (!string.IsNullOrEmpty(passw) && passw != "Clear")
                {
                    filteredData = filteredData.Where(student => student.Password == passw);
                }

                // Apply degree filter
                if (!string.IsNullOrEmpty(userrole) && userrole != "Clear")
                {
                    filteredData = filteredData.Where(student => student.Role == userrole);
                }

                // Update data grid
                mygridsuperadmin.ItemsSource = filteredData.ToList();
            }
        }

        private void PopulateUserNameFilterComboBox()
        {
            usernameFilterComboBox.Items.Clear();
            // Add "Clear" option
            usernameFilterComboBox.Items.Add("Clear");
            var usersnames = zz.Users.Select(uu => uu.Username).Distinct().ToList();
            foreach (var u in usersnames)
            {
                usernameFilterComboBox.Items.Add(u);
            }



         
        }

        private void PopulatePasswordFilterComboBox()
        {
            passwordFilterComboBox.Items.Clear();
            // Add "Clear" option
            passwordFilterComboBox.Items.Add("Clear");
            var pass = zz.Users.Select(st => st.Password).Distinct().ToList();
            foreach (var p in pass)
            {
                passwordFilterComboBox.Items.Add(p);
            }
        }

        private void PopulateRoleFilterComboBox()
        {
            RoleFilterComboBox.Items.Clear();
            // Add "Clear" option
            RoleFilterComboBox.Items.Add("Clear");
            var rolee = zz.Users.Select(student => student.Role).Distinct().ToList();
            foreach (var r in rolee)
            {
                RoleFilterComboBox.Items.Add(r);
            }
        }











        //private void PopulateUserNameFilterComboBox()
        //{
        //    usernameFilterComboBox.Items.Clear();
        //    var usersnames = zz.Users.Select(uu => uu.Username).Distinct().ToList();
        //    foreach (var u in usersnames)
        //    {
        //        usernameFilterComboBox.Items.Add(u);
        //    }
        //}

        //private void PopulatePasswordFilterComboBox()
        //{
        //    passwordFilterComboBox.Items.Clear();
        //    var pass = zz.Users.Select(st => st.Password).Distinct().ToList();
        //    foreach (var p in pass)
        //    {
        //        passwordFilterComboBox.Items.Add(p);
        //    }
        //}

        //private void PopulateRoleFilterComboBox()
        //{
        //    RoleFilterComboBox.Items.Clear();
        //    var rolee = zz.Users.Select(student => student.Role).Distinct().ToList();
        //    foreach (var r in rolee)
        //    {
        //        RoleFilterComboBox.Items.Add(r);
        //    }
        //}
        //private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string selectedCity = usernameFilterComboBox.SelectedItem?.ToString();
        //    string selectedSession = passwordFilterComboBox.SelectedItem?.ToString();
        //    string selectedDegree = RoleFilterComboBox.SelectedItem?.ToString();

        //    using (var context = zz)
        //    {
        //        var filteredData = context.Users
        //                            .Where(ur =>
        //                                (selectedCity == null || ur.Username== selectedCity) &&
        //                                (selectedSession == null || ur.Password == selectedSession) &&
        //                                (selectedDegree == null || ur.Role == selectedDegree))
        //                            .ToList();

        //        mygridsuperadmin.ItemsSource = filteredData;
        //    }
        //}

        private void Searchbyusername(object sender, RoutedEventArgs e)
        {

            string searchText = ByUserName.Text;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var filteredStudents = zz.Users.Where(std => std.Username.Contains(searchText)).ToList();
                mygridsuperadmin.ItemsSource = filteredStudents;
            }
            else
            {
                MessageBox.Show("Please enter a search query.");
                mygridsuperadmin.ItemsSource = zz.Users.ToList();
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
          

            MainWindow mm = new MainWindow();
            mm.Show();
            Window.GetWindow(this).Close();
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a new Excel package
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Add a new worksheet to the Excel package
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Grid Data");

                    // Set headers for the columns
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "UserName";
                    worksheet.Cells[1, 3].Value = "Password";
                    worksheet.Cells[1, 4].Value = "Role";
                   

                    // Assuming your grid is named 'dataGrid', you can iterate through its items and add them to the Excel sheet
                    int row = 2; // Start from the second row (after the header)
                    foreach (var item in mygridsuperadmin.Items)
                    {
                        // Assuming each item is a Student object
                        var u = (User)item;
                        worksheet.Cells[row, 1].Value = u.Id;
                        worksheet.Cells[row, 2].Value = u.Username;
                        worksheet.Cells[row, 3].Value = u.Password;

                        worksheet.Cells[row, 4].Value = u.Role;
                        
                        row++;
                    }

                    // Save the Excel package to a file
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        FileInfo file = new FileInfo(saveFileDialog.FileName);
                        excelPackage.SaveAs(file);
                        MessageBox.Show("Export successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void Import(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var openFileDialog = new OpenFileDialog();
        //        openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
        //        if (openFileDialog.ShowDialog() == true)
        //        {
        //            using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
        //            {
        //                using (var reader = ExcelReaderFactory.CreateReader(stream))
        //                {
        //                    // Choose the first sheet
        //                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
        //                    {
        //                        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
        //                    });

        //                    // Assuming the first table is the one you want to import
        //                    var dataTable = dataSet.Tables[0];

        //                    // Clear existing items in the DataGrid
        //                    mygridsuperadmin.ItemsSource = null;

        //                    // Get existing usernames from the database
        //                    var existingUsernames = zz.Users.Select(u => u.Username).ToList();

        //                    // Create a new list to hold imported user data
        //                    List<User> importedUsers = new List<User>();

        //                    // Iterate through each row in the DataTable
        //                    foreach (DataRow row in dataTable.Rows)
        //                    {
        //                        // Explicitly convert the values to strings
        //                        string username = row["UserName"].ToString();
        //                        string password = row["Passwords"].ToString();
        //                        string role = row["Role"].ToString();

        //                        // Check if the username already exists in the database
        //                        if (existingUsernames.Equals(username))
        //                        {
        //                            // Skip this row as it's a duplicate
        //                            continue;
        //                        }

        //                        // Create a new User object
        //                        User newUser = new User
        //                        {
        //                            Username = username,
        //                            Password = password,
        //                            Role = role
        //                        };

        //                        // Add the user to the list of imported users
        //                        importedUsers.Add(newUser);
        //                    }

        //                    // Add imported users to the database
        //                    zz.Users.AddRange(importedUsers);
        //                    zz.SaveChanges();

        //                    // Refresh the DataGrid with the imported data from the database
        //                    mygridsuperadmin.ItemsSource = zz.Users.ToList();
        //                }
        //            }

        //            MessageBox.Show("Import successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error importing data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        private void Import(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            // Choose the first sheet
                            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                            {
                                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                            });

                            // Assuming the first table is the one you want to import
                            var dataTable = dataSet.Tables[0];

                            // Clear existing items in the DataGrid
                            mygridsuperadmin.ItemsSource = null;

                            // Get existing usernames and passwords from the database
                            var existingUsers = zz.Users.ToDictionary(u => new { Username = u.Username.Trim(), Password = u.Password.Trim() });

                            // Create a new list to hold imported user data
                            List<User> importedUsers = new List<User>();

                            // Iterate through each row in the DataTable
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Explicitly convert the values to strings
                                string username = row["UserName"].ToString().Trim();
                                string password = row["Passwords"].ToString().Trim();
                                string role = row["Role"].ToString().Trim();

                                // Check if the username and password combination already exists in the database
                                if (existingUsers.ContainsKey(new { Username = username, Password = password }))
                                {
                                    // Skip this row as it's a duplicate
                                    continue;
                                }

                                // Create a new User object
                                User newUser = new User
                                {
                                    Username = username,
                                    Password = password,
                                    Role = role
                                };

                                // Add the user to the list of imported users
                                importedUsers.Add(newUser);
                            }

                            // Add imported users to the database
                            zz.Users.AddRange(importedUsers);
                            zz.SaveChanges();

                            // Refresh the DataGrid with the imported data from the database
                            mygridsuperadmin.ItemsSource = zz.Users.ToList();
                        }
                    }

                    MessageBox.Show("Import successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error importing data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_users(object sender, RoutedEventArgs e)
        {
            Adduser u = new Adduser();
            u.ShowDialog();

        }

        private void Del_Users(object sender, RoutedEventArgs e)
        {
            int id = (mygridsuperadmin.SelectedItem as User).Id;
            if (id != null)
            {
                User? u = zz.Users.Where(x => x.Id == id).First();
                zz.Users.Remove(u);
                zz.SaveChanges();
                mygridsuperadmin.ItemsSource = zz.Users.ToList();
            }
        }

        private void Edit_Users(object sender, RoutedEventArgs e)
        {

          
                if ((mygridsuperadmin.SelectedItem as User) != null)
                {
                    int id = (mygridsuperadmin.SelectedItem as User).Id;
                    EditUsers aaa = new EditUsers(id);
                    aaa.ShowDialog();

                }
        }
    }
}
