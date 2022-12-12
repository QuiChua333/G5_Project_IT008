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

namespace CinemaManagementProject.View.Admin.VoucherManagement.AddWindow
{
    /// <summary>
    /// Interaction logic for ListEmail.xaml
    /// </summary>
    public partial class ListEmail : Window
    {
        public ListEmail()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
