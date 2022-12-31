using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CinemaManagementProject.View.Staff.SettingManagement
{
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
        }
        
        private void iconEditName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _staffName.Focus();
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            GreenRectangle.Width = 60;
            GreenRectangle.Height = 60;

        }
        private void GreenRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            GreenRectangle.Width = 50;
            GreenRectangle.Height = 50;
        }

        private void YellowRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            YellowRectangle.Width = 60;
            YellowRectangle.Height = 60;
        }

        private void YellowRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            YellowRectangle.Width = 50;
            YellowRectangle.Height = 50;
        }

        private void PinkRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            PinkRectangle.Width = 60;
            PinkRectangle.Height = 60;
        }
        private void PinkRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            PinkRectangle.Width = 50;
            PinkRectangle.Height = 50;
        }

        private void BlueRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            BlueRectangle.Width = 60;
            BlueRectangle.Height = 60;
        }

        private void BlueRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            BlueRectangle.Width = 50;
            BlueRectangle.Height = 50;
        }

    }
}
