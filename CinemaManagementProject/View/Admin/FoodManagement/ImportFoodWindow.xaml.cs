using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CinemaManagementProject.View.Admin.FoodManagement
{
    /// <summary>
    /// Interaction logic for ImportFoodWindow.xaml
    /// </summary>
    public partial class ImportFoodWindow : Window
    {
        public ImportFoodWindow()
        {
            InitializeComponent();
        }

        private void PriceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PriceTextBox.Text = "";
        }
        private void TextQuantity_GotFocus(object sender, RoutedEventArgs e)
        {
            TextQuantity.Text = "";
        }
    }
}
