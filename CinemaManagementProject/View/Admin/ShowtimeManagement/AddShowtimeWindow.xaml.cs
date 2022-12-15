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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CinemaManagementProject.View.Admin.ShowtimeManagement
{
    /// <summary>
    /// Interaction logic for AddShowtimeWindow.xaml
    /// </summary>
    public partial class AddShowtimeWindow : Window
    {
        public AddShowtimeWindow()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }
        private void _moviePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void _moviePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0)
                tb.Text = "0";
        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA5B9D6");
            btn.Background = new SolidColorBrush(Colors.Red);
        }
        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddSuatChieu_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    {
                        this.Close();
                        return;
                    }
            }
        }

        private void AddSuatChieu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
