using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace CinemaManagementProject.View.Login
{
    /// <summary>
    /// Interaction logic for ChangePassPage.xaml
    /// </summary>
    public partial class ChangePassPage : Page
    {
        public ChangePassPage()
        {
            InitializeComponent();
        }


        private void ConfirmText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(NewPassText.Text == ConfirmText.Password)
            {
                Error.Content = "Chính xác";
                Error.Foreground = new SolidColorBrush(Colors.DarkSeaGreen);
            }
            else
            {
                Error.Content = "Chưa chính xác";
                Error.Foreground = new SolidColorBrush(Colors.Red);
            }    

        }
    }
}
