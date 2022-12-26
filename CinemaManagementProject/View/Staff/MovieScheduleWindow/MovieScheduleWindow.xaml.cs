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

namespace CinemaManagementProject.View.Staff.MovieScheduleWindow
{
    /// <summary>
    /// Interaction logic for MovieScheduleWindow.xaml
    /// </summary>
    public partial class MovieScheduleWindow : Window
    {
        Border ShowTimeSelected = null;
        public MovieScheduleWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            _Room.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ShowTimeSelected != null)
            {
                ShowTimeSelected.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#D9D9D9");
                if (_Room.Visibility == Visibility.Visible)
                    _Room.Visibility = Visibility.Collapsed;
                ShowTimeSelected = null;
                return;
            }
                
            if (ShowTimeSelected == null)
            {
                ShowTimeSelected = (Border)sender;
                ShowTimeSelected.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#F19D9D");
                if (_Room.Visibility == Visibility.Collapsed)
                    _Room.Visibility = Visibility.Visible;
                return;
            }
            

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
