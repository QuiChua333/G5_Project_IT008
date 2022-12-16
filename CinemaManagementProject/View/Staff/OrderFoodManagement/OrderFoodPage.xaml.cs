using CinemaManagementProject.Component.Filter;
using CinemaManagementProject.DTOs;
using CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM;
using CinemaManagementProject.ViewModel.StaffVM.OrderFoodManagementVM;
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

namespace CinemaManagementProject.View.Staff.OrderFoodManagement
{
    /// <summary>
    /// Interaction logic for OrderFoodPage.xaml
    /// </summary>
    public partial class OrderFoodPage : Page
    {
        public OrderFoodPage()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as ProductDTO).ProductName.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void SearchBox_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource).Refresh();
        }

        private void FilterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderFoodManagementVM.StoreAllFood is null) return;
            var viewmodel = (OrderFoodManagementVM)DataContext;
            if (viewmodel.FilterComboboxFoodCM.CanExecute(true))
                viewmodel.FilterComboboxFoodCM.Execute(FilterCombobox);
        }
    }
}
