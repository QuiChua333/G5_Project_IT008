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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using CinemaManagementProject.ViewModel.LoginVM;
using CinemaManagementProject.Utils;
using CinemaManagementProject.Model;

namespace CinemaManagementProject.View.Login
{
    /// <summary>
    /// Interaction logic for ForgotPassPage.xaml
    /// </summary>
    public partial class ForgotPassPage : Page
    {
        public ForgotPassPage()
        {
            InitializeComponent();
        }
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string CurrentEmail = EmailBox.Text;
            if (EmailFormat.IsValidEmail(CurrentEmail))
            {
                if(Helper.CheckEmailStaff(CurrentEmail))
                {
                    InfoText.Text = "Chúng tôi đã gửi mã có 6 chữ số đến tài khoản " + SetHideEmail(EmailBox.Text);
                    ContainerTextBoxCodeAndError.Margin = new Thickness(0, 70, 0, 0);
                    ContainerText.Margin = new Thickness(0, 0, 0, 0);
                    IconField.Kind = PackIconKind.ConfirmationNumber;
                    TextHelper.Text = "Nhập mã tại đây";
                    EmailBox.Text = "";
                    BtnCheckCode.Visibility = Visibility.Visible;
                    BtnSend.Visibility = Visibility.Hidden;
                    CodeBox.Visibility = Visibility.Visible;
                    EmailBox.Visibility = Visibility.Hidden;
                }    
            }
            EmailBox.Focus();
        }
        private string SetHideEmail(string email)
        {
            int start = 0;
            int end = 0;
            int length = email.Length;
            for(int i = 0; i < length; i++)
                if (email[i] == '@')
                    end = i - 1;
            start = end - (int)((float)1 / 3 * (end - start + 1)) + 1;
            string secure = "";
            for (int i = 0; i < end - start + 1; i++)
                secure += '*';
            email = email.Remove(start, length - start) + secure + email.Remove(0, end + 1);
            return email;
        }

        private void BtnCheckCode_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
