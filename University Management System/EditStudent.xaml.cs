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
   
    public partial class EditStudent : Window
    {
        EAD_ProjectContext dd = new EAD_ProjectContext();
        Student? obj;
        public int idd { get; set; }
        public EditStudent(int temp)
        {
            InitializeComponent();
            idd = temp;
            obj=dd.Students.Where(x=>x.Id == idd).FirstOrDefault();
            t1.Text = obj.RollNo;
            t2.Text=obj.Name ;
            t3.Text = obj.City ;
            t4.Text = obj.Session.ToString();
            t5.Text = obj.Degree;

            

        }


        private void Editnow(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if any field is empty
                if (string.IsNullOrWhiteSpace(t1.Text) || string.IsNullOrWhiteSpace(t2.Text) || string.IsNullOrWhiteSpace(t3.Text) || string.IsNullOrWhiteSpace(t4.Text) || string.IsNullOrWhiteSpace(t5.Text))
                {
                    MessageBox.Show("Please fill all the fields.");
                    return;
                }

                // Check if the edited roll number already exists for another student
                bool rollNoExists = dd.Students.Any(s => s.RollNo == t1.Text && s.Id != obj.Id);
                if (rollNoExists)
                {
                    MessageBox.Show("Another student with the same RollNo already exists.");
                    return;
                }
                if (!int.TryParse(t4.Text, out int session))
                {
                    MessageBox.Show("Invalid session number.");
                    return;
                }


                // Update student properties
                obj.RollNo = t1.Text;
                obj.Name = t2.Text;
                obj.City = t3.Text;
                obj.Session = Convert.ToInt32(t4.Text);
                obj.Degree = t5.Text;

                // Save changes
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
