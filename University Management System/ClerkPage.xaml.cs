
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using University_Management_System.Models;

namespace University_Management_System
{
    public partial class ClerkPage : Page
    {
        EAD_ProjectContext db = new EAD_ProjectContext();

        public ClerkPage()
        {
            
            InitializeComponent();
            mygrid.ItemsSource = db.Students.ToList();
            PopulateCityFilterComboBox();
            PopulateSessionFilterComboBox();
            PopulateDegreeFilterComboBox();
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
                mygrid.ItemsSource = filteredData.ToList();
            }
        }

        private void PopulateCityFilterComboBox()
        {
            cityFilterComboBox.Items.Clear();
            // Add "Clear" option
            cityFilterComboBox.Items.Add("Clear");
            var cities = db.Students.Select(student => student.City).Distinct().ToList();
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
            var sessions = db.Students.Select(student => student.Session).Distinct().ToList();
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
            var degrees = db.Students.Select(student => student.Degree).Distinct().ToList();
            foreach (var degree in degrees)
            {
                degreeFilterComboBox.Items.Add(degree);
            }
        }



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string srch = Search1.Text;
            if (!string.IsNullOrWhiteSpace(srch))
            {
                var filteredStudents = db.Students.Where(student => student.RollNo.Equals(srch)).ToList();
                mygrid.ItemsSource = filteredStudents;
            }
            else
            {
                MessageBox.Show("Please enter a roll number to search.");
                mygrid.ItemsSource = db.Students.ToList();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

       
            MainWindow mm=new MainWindow();
            mm.Show();
            Window.GetWindow(this).Close();
        }

      
    }
}
