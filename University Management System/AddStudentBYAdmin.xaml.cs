using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using University_Management_System.Models;

namespace University_Management_System
{
    /// <summary>
    /// Interaction logic for AddStudentBYAdmin.xaml
    /// </summary>
    public partial class AddStudentBYAdmin : Window
    {
        EAD_ProjectContext dd=new EAD_ProjectContext();
        public AddStudentBYAdmin()
        {
            InitializeComponent();
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    dd.Students.Add(new Student { RollNo = t1.Text, Name = t2.Text, City = t3.Text, Session = Convert.ToInt32(t4.Text), Degree = t5.Text });
        //    dd.SaveChanges();
        //    AdminPage.gg.ItemsSource = dd.Students.ToList();
        //    this.Close();
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
           
                if (string.IsNullOrWhiteSpace(t1.Text) || string.IsNullOrWhiteSpace(t2.Text))
                {
                    MessageBox.Show("Please fill all the fields.");
                    return;
                }

            
                bool rollNoExists = dd.Students.Any(s => s.RollNo == t1.Text);
                if (rollNoExists)
                {
                    MessageBox.Show("Student with the same RollNo already exists.");
                    return;
                }

          
                if (!int.TryParse(t4.Text, out int session))
                {
                    MessageBox.Show("Invalid session number.");
                    return;
                }

              
                dd.Students.Add(new Student { RollNo = t1.Text, Name = t2.Text, City = t3.Text, Session = session, Degree = t5.Text });
                dd.SaveChanges();
                AdminPage.gg.ItemsSource = dd.Students.ToList();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



    }
}
