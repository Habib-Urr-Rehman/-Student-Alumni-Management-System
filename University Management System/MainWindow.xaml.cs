using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using University_Management_System.Models;

namespace University_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EAD_ProjectContext q=new EAD_ProjectContext();  

        public MainWindow()
        {
            InitializeComponent();
        }

   
        private void Button_Click(object sender, RoutedEventArgs e)
        {
         

            string username = t1.Text;
            string password = t2.Text;

          
            var user = q.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (string.IsNullOrWhiteSpace(t1.Text) || string.IsNullOrWhiteSpace(t2.Text))
                    {
                        MessageBox.Show("Please fill all the fields.");
                        return;
                    }
           else  if (user != null)
            {
                // User found, now check the user's role and open the corresponding panel

                if (user.Role == "Clerk")
                {
                
                    ClerkPanel clerkPanel = new ClerkPanel();
                 
                    clerkPanel.Show();
                    this.Close();
                }
                else if (user.Role == "Admin")
                {
                  
                    AdminPanel adminPanel = new AdminPanel();
                  
                    adminPanel.Show();
                    this.Close();
                }
                else if (user.Role == "Super Admin")
                {
                 
                    SuperAdminPanel superAdminPanel = new SuperAdminPanel();
                   
                    superAdminPanel.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid role for user.");
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    // Assuming you have a database context named 'q'
        //    // Replace 'q' with your actual database context name

        //    string username = t1.Text;
        //    string password = t2.Text;

        //    // Validate the username and password (You may have your own validation logic here)

        //    // Retrieve the user from the database based on the provided username and password
        //    var user = q.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        //    if (user != null)
        //    {
        //        // User found, now check the user's role and open the corresponding panel

        //        switch (user.Role)
        //        {
        //            case "Clerk":
        //                this.Close();
        //                ClerkPanel clerkPanel = new ClerkPanel();
        //                clerkPanel.ShowDialog();
        //                // Close the login window after opening the panel

        //                break;
        //            case "Admin":
        //                this.Close();
        //                AdminPanel adminPanel = new AdminPanel();
        //                adminPanel.ShowDialog();
        //                // Close the login window after opening the panel

        //                break;
        //            case "Super Admin":
        //                this.Close();
        //                SuperAdminPanel superAdminPanel = new SuperAdminPanel();
        //                superAdminPanel.ShowDialog();
        //                // Close the login window after opening the panel

        //                break;
        //            default:
        //                MessageBox.Show("Invalid role for user.");
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Invalid username or password.");
        //    }
        //}

        public void  Closeee()
        {
           
        }
    }
}

//Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EAD_Project;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

