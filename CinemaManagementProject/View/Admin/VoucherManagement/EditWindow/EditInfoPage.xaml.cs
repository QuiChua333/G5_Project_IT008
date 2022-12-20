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

namespace CinemaManagementProject.View.Admin.VoucherManagement.EditWindow
{
    /// <summary>
    /// Interaction logic for EditInfoPage.xaml
    /// </summary>
    public partial class EditInfoPage : Page
    {
        public EditInfoPage()
        {
            InitializeComponent();
        }

        private void yes_Checked(object sender, RoutedEventArgs e)
        {
            VoucherViewModel.Status2 = true;
        }

        private void no_Checked(object sender, RoutedEventArgs e)
        {
            VoucherViewModel.Status2 = false;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
