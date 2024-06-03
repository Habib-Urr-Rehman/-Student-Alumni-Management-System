using System;
using System.Collections.Generic;
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
using University_Management_System.Models;
using OfficeOpenXml;
using ExcelDataReader;
using System.Data;
using Microsoft.Win32;


namespace University_Management_System
{

  
    public partial class AdminPage : Page
    {
        EAD_ProjectContext aa = new EAD_ProjectContext();
        public static DataGrid gg;
        public AdminPage()
        {
            InitializeComponent();
            mygridadmin.ItemsSource = aa.Students.ToList();
            gg = mygridadmin;
            PopulateCityFilterComboBox();
            PopulateSessionFilterComboBox();
            PopulateDegreeFilterComboBox();
        }

        private void Edi(object sender, RoutedEventArgs e)
        {
            if ((mygridadmin.SelectedItem as Student) != null)
            {
                int id = (mygridadmin.SelectedItem as Student).Id;
                EditStudent aa = new EditStudent(id);
                aa.ShowDialog();

            }
        }

        private void Del(object sender, RoutedEventArgs e)
        {
            int id = (mygridadmin.SelectedItem as Student).Id;
            if (id != null)
            {
                Student? u = aa.Students.Where(x => x.Id == id).First();
                aa.Students.Remove(u);
                aa.SaveChanges();
                mygridadmin.ItemsSource = aa.Students.ToList();
            }
        }




        private void Add(object sender, RoutedEventArgs e)
        {
            AddStudentBYAdmin aa = new AddStudentBYAdmin();
            aa.ShowDialog();
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
                    worksheet.Cells[1, 1].Value = "Roll No";
                    worksheet.Cells[1, 2].Value = "Name";
                    worksheet.Cells[1, 3].Value = "City";
                    worksheet.Cells[1, 4].Value = "Session";
                    worksheet.Cells[1, 5].Value = "Degree";

                    // Assuming your grid is named 'dataGrid', you can iterate through its items and add them to the Excel sheet
                    int row = 2; // Start from the second row (after the header)
                    foreach (var item in mygridadmin.Items)
                    {
                        // Assuming each item is a Student object
                        var student = (Student)item;
                        worksheet.Cells[row, 1].Value = student.RollNo;
                        worksheet.Cells[row, 2].Value = student.Name;
                        worksheet.Cells[row, 3].Value = student.City;
                        worksheet.Cells[row, 4].Value = student.Session;
                        worksheet.Cells[row, 5].Value = student.Degree;
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
                            mygridadmin.ItemsSource = null;

                            // Get existing roll numbers from the database
                            var existingRollNumbers = aa.Students.Select(s => s.RollNo).ToList();

                            // Create a new list to hold imported student data
                            List<Student> importedStudents = new List<Student>();

                            // Iterate through each row in the DataTable
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Assuming the columns are in the order of Roll No, Name, City, Session, Degree
                                string rollNo = row.Field<string>("Roll No");

                                // Check if the roll number already exists in the database
                                if (existingRollNumbers.Contains(rollNo))
                                {
                                    // Skip this row as it's a duplicate
                                    continue;
                                }

                                string name = row.Field<string>("Name");
                                string city = row.Field<string>("City");

                                // Handle session conversion from double to int
                                double sessionDouble;
                                if (!double.TryParse(row.Field<object>("Session").ToString(), out sessionDouble))
                                {
                                    MessageBox.Show($"Error parsing session value for {rollNo}. Skipping this row.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    continue; // Skip to next row if parsing fails
                                }
                                int session = (int)sessionDouble;

                                string degree = row.Field<string>("Degree");

                                // Create a new Student object
                                Student student = new Student
                                {
                                    RollNo = rollNo,
                                    Name = name,
                                    City = city,
                                    Session = session,
                                    Degree = degree
                                };

                                // Add the student to the list
                                importedStudents.Add(student);
                            }

                            // Assuming your DbContext instance is named 'aa'
                            // Add imported students to the database
                            aa.Students.AddRange(importedStudents);
                            aa.SaveChanges();

                            // Refresh the DataGrid with the imported data from the database
                            mygridadmin.ItemsSource = aa.Students.ToList();
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
            string selectedCity = cityFilterComboBox.SelectedItem?.ToString();
            int? selectedSession = sessionFilterComboBox.SelectedItem as int?;
            string selectedDegree = degreeFilterComboBox.SelectedItem?.ToString();

            using (var context = new EAD_ProjectContext())
            {
                var filteredData = context.Students.AsQueryable();

                // Apply city filter
                if (!string.IsNullOrEmpty(selectedCity) && selectedCity != "Clear")
                {
                    filteredData = filteredData.Where(student => student.City == selectedCity);
                }

                // Apply session filter
                if (selectedSession.HasValue)
                {
                    filteredData = filteredData.Where(student => student.Session == selectedSession);
                }

                // Apply degree filter
                if (!string.IsNullOrEmpty(selectedDegree) && selectedDegree != "Clear")
                {
                    filteredData = filteredData.Where(student => student.Degree == selectedDegree);
                }

                // Update data grid
                mygridadmin.ItemsSource = filteredData.ToList();
            }
        }

        private void PopulateCityFilterComboBox()
        {
            cityFilterComboBox.Items.Clear();
            // Add "Clear" option
            cityFilterComboBox.Items.Add("Clear");
            var cities = aa.Students.Select(student => student.City).Distinct().ToList();
            foreach (var city in cities)
            {
                cityFilterComboBox.Items.Add(city);
            }
        }

        private void PopulateSessionFilterComboBox()
        {
            sessionFilterComboBox.Items.Clear();
            // Add "Clear" option
            sessionFilterComboBox.Items.Add("Clear");
            var sessions = aa.Students.Select(student => student.Session).Distinct().ToList();
            foreach (var session in sessions)
            {
                sessionFilterComboBox.Items.Add(session);
            }
        }

        private void PopulateDegreeFilterComboBox()
        {
            degreeFilterComboBox.Items.Clear();
            // Add "Clear" option
            degreeFilterComboBox.Items.Add("Clear");
            var degrees = aa.Students.Select(student => student.Degree).Distinct().ToList();
            foreach (var degree in degrees)
            {
                degreeFilterComboBox.Items.Add(degree);
            }
        }


        

        private void Searchbyroll(object sender, RoutedEventArgs e)
        {
           
                string searchText = ByNameAndRoll.Text;

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var filteredStudents = aa.Students.Where(std => std.Name.Contains(searchText) || std.RollNo.Equals(searchText)).ToList();
                    mygridadmin.ItemsSource = filteredStudents;
                }
                else
                {
                    MessageBox.Show("Please enter a search query.");
                    mygridadmin.ItemsSource = aa.Students.ToList();
                }
         }

        private void Logout(object sender, RoutedEventArgs e)
        {
         
            
            MainWindow mm = new MainWindow();
            mm.Show();
            Window.GetWindow(this).Close();
        }




        
    }
}
