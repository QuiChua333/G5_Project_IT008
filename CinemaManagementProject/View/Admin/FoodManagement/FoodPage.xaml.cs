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

namespace CinemaManagementProject.View.Admin.FoodManagement
{
    /// <summary>
    /// Interaction logic for FoodPage.xaml
    /// </summary>
    public partial class FoodPage : Page
    {
        public FoodPage()
        {
            InitializeComponent();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddFoodWindow ProductInfo = new AddFoodWindow();
            ProductInfo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ProductInfo.Show();
        }

        private void FoodItem_EditButtonClick(object sender, EventArgs e)
        {
            EditFoodWindow editFoodWindow = new EditFoodWindow();
            editFoodWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            editFoodWindow.Show();
        }
    }
}
