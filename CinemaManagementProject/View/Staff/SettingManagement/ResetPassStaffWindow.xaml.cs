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

namespace CinemaManagementProject.View.Staff.SettingManagement
{
    /// <summary>
    /// Interaction logic for ResetPassWindow.xaml
    /// </summary>
    public partial class ResetPassStaffWindow : Window
    {
        public ResetPassStaffWindow()
        {
            InitializeComponent();
        }

        private void ConfirmText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(NewPassText.Password.Length == 0)
            {
                Error.Content = "Không được để trống password";
                Error.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }    
            if (NewPassText.Password == ConfirmText.Password)
            {
                Error.Content = "Chính xác";
                Error.Foreground = new SolidColorBrush(Colors.DarkSeaGreen);
                ConfirmButton.Opacity = 1;
                ConfirmButton.IsEnabled = true;
            }
            else
            {
                Error.Content = "Chưa chính xác";
                Error.Foreground = new SolidColorBrush(Colors.Red);
                ConfirmButton.Opacity = 0.6;
                ConfirmButton.IsEnabled = false;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
