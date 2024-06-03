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
    
    public partial class AdminPanel : Window
    {
        AdminPage aa=new AdminPage();
        public AdminPanel()
        {
            InitializeComponent();
            myframe.Content= aa;
        }
    }
}
