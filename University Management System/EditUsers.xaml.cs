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

    public partial class EditUsers : Window
    {
        EAD_ProjectContext dd = new EAD_ProjectContext();
        User? obj;
        public int idd { get; set; }
        public EditUsers(int tempp)
        {
            InitializeComponent();
            idd = tempp;
            obj = dd.Users.Where(x => x.Id == idd).FirstOrDefault();
            t1.Text = obj.Username;
            t2.Text = obj.Password;
            // Find the corresponding role in ComboBox items and set it as the selected item
            foreach (var item in t3.Items)
            {
                if (item is ComboBoxItem comboBoxItem && comboBoxItem.Content.ToString() == obj.Role)
                {
                    t3.SelectedItem = item;
                    break;
                }
            }
        }

        //private void Edit_Users(object sender, RoutedEventArgs e)
        //{
        //    obj.Username = t1.Text;
        //    obj.Password = t2.Text;
        //    obj.Role = t3.SelectedItem?.ToString();
        //    dd.SaveChanges();
        //    ManageUsers.ff.ItemsSource = dd.Users.ToList();
        //    this.Close();
        //}
        private void Edit_Users(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (obj != null)
                {
               
                    if (string.IsNullOrWhiteSpace(t1.Text) || string.IsNullOrWhiteSpace(t2.Text) || t3.SelectedItem == null)
                    {
                        MessageBox.Show("Please fill all the fields.");
                        return;
                    }

                
                    obj.Username = t1.Text;
                    obj.Password = t2.Text;

                    string selectedRole = t3.SelectedItem.ToString();
                    string roleString = selectedRole.Substring(selectedRole.LastIndexOf(":") + 2);

         
                    bool userExists = dd.Users.Any(u => u.Username == obj.Username && u.Password == obj.Password && u.Id != obj.Id);
                    if (userExists)
                    {
                        MessageBox.Show("User with the same username and password already exists.");
                        return;
                    }

                    obj.Role = roleString;

                  
                    dd.SaveChanges();
                    ManageUsers.ff.ItemsSource = dd.Users.ToList();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
