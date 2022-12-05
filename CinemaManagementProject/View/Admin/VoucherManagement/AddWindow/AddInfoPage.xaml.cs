using CinemaManagementProject.ViewModel.AdminVM.VoucherManagementVM;
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

namespace CinemaManagementProject.View.Admin.VoucherManagement.AddWindow
{
    /// <summary>
    /// Interaction logic for AddInfoPage.xaml
    /// </summary>
    public partial class AddInfoPage : Page
    {
        public AddInfoPage()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            VoucherViewModel.Status = true;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            VoucherViewModel.Status = false;
        }
    }
}
