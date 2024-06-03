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

namespace University_Management_System
{
 
    public partial class SuperAdminPanel : Window
    {
        public SuperAdminPanel()
        {
            InitializeComponent();
            AdminPage p1 = new AdminPage();
            myframeSuper.Content = p1;
        }

        private void Export(object sender, RoutedEventArgs e)
        {

        }

        private void Manage_Student(object sender, RoutedEventArgs e)
        {
            AdminPage z = new AdminPage();
            myframeSuper.Content = z;
        }

        private void Manage_Users(object sender, RoutedEventArgs e)
        {
            ManageUsers mm=new ManageUsers();
            myframeSuper.Content = mm;
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
          
            MainWindow mm = new MainWindow();
            mm.Show();
            this.Close();
        }
    }
}
