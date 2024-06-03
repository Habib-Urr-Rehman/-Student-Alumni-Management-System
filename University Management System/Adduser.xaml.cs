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
 
    public partial class Adduser : Window
    {
        EAD_ProjectContext dd = new EAD_ProjectContext();
        public Adduser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(t1.Text) || string.IsNullOrWhiteSpace(t2.Text) || t3.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            // Handle possible null reference
            string selectedRole = t3.SelectedItem.ToString();

            // Extract the role string without type information
            string roleString = selectedRole.Substring(selectedRole.LastIndexOf(":") + 2);

            // Check if the username and password combination already exists
            bool userExists = dd.Users.Any(u => u.Username == t1.Text && u.Password == t2.Text);
            if (userExists)
            {
                MessageBox.Show("User with the same username and password already exists.");
                return; // Exit the method without adding the user
            }

            // Add the user if the username and password combination doesn't exist
            dd.Users.Add(new User { Username = t1.Text, Password = t2.Text, Role = roleString });
            dd.SaveChanges();
            ManageUsers.ff.ItemsSource = dd.Users.ToList();
            this.Close();
        }


    }
}
