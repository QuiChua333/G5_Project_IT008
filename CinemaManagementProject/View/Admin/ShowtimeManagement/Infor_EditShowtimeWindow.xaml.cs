using CinemaManagementProject.Model;
using CinemaManagementProject.ViewModel.AdminVM.ShowtimeManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CinemaManagementProject.View.Admin.ShowtimeManagement
{
    /// <summary>
    /// Interaction logic for Infor_EditShowtimeWindow.xaml
    /// </summary>
    public partial class Infor_EditShowtimeWindow : Window
    {
        Border ShowTimeSelected = null;

        public Infor_EditShowtimeWindow()
        {
            InitializeComponent();
        }

        

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ShowTimeSelected != null)
            {
                ShowTimeSelected.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#D9D9D9");
               
                ShowTimeSelected = null;
               
                return;
            }

            if (ShowTimeSelected == null)
            {
                ShowTimeSelected = (Border)sender;
                ShowTimeSelected.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#F19D9D");
               
                return;
            }
        }


        public static bool IsEdit = false;
       
        private void _showtimePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;

            if (t.Text.Length <= 0)
                t.Text = "1";
        }
        private void EditWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void EditWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        
    }
    
}
