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

namespace CinemaManagementProject.View.Admin.FoodManagement
{
    /// <summary>
    /// Interaction logic for EditFoodWindow.xaml
    /// </summary>
    public partial class EditFoodWindow : Window
    {
        public EditFoodWindow()
        {
            InitializeComponent();
        }

        private void EditFoodWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
