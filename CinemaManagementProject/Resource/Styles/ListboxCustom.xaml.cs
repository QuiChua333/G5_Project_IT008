using CinemaManagementProject.View.Admin.FoodManagement;
using CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagementProject.Resource.Styles
{
    public partial class ListboxCustom : ResourceDictionary
    {
        public ListboxCustom()
        {
        }

        private void EditButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EditFoodWindow EditFoodWD = new EditFoodWindow();
            EditFoodWD.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            EditFoodWD.ShowDialog();
        }
    }
}
