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
using System.Windows.Shapes;

namespace CinemaManagementProject.View.Admin.VoucherManagement.AddWindow
{
    /// <summary>
    /// Interaction logic for ReleaseVoucher.xaml
    /// </summary>
    public partial class ReleaseVoucher : Window
    {
        public ReleaseVoucher()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
       

            VoucherViewModel.WaitingMiniVoucher.Clear();
            this.Close();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = DateTime.Today.ToString();
        }
    }
}
