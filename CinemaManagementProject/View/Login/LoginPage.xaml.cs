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

namespace CinemaManagementProject.View.Login
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            if (Properties.Settings.Default.isRemidUserAndPass)
            {
                PasswordBoxUser.Password = Properties.Settings.Default.userPassSetting;
            }
        }
        public LoginPage(string newPass)
        {
            InitializeComponent();
            PasswordBoxUser.Password = "";
        }

        private void LoginPg_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Properties.Settings.Default.isRemidUserAndPass)
            //{
            //    PasswordBoxUser.Password = Properties.Settings.Default.userPassSetting;
            //}
        }

        private void ButtonLogin_TouchEnter(object sender, TouchEventArgs e)
        {
            MessageBox.Show("Click enter");
        }

        private void ButtonLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                MessageBox.Show("Click enter");
        }

        private void PasswordBoxUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                MessageBox.Show("Click enter");
        }
    }
}
