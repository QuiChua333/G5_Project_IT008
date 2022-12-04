using CinemaManagementProject.DTOs;
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

namespace CinemaManagementProject.View.Admin.CustomerManagement
{
    /// <summary>
    /// Interaction logic for CustomerManagementPage.xaml
    /// </summary>
    public partial class CustomerManagementPage : Page
    {
        public CustomerManagementPage()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;

            switch (cbbFilter.SelectedValue)
            {
                case "Mã khách hàng":
                    return ((item as CustomerDTO).Id.ToString().IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Tên khách hàng":
                    return ((item as CustomerDTO).Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Số điện thoại":
                    return ((item as CustomerDTO).PhoneNumber.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                default:
                    return ((item as CustomerDTO).Id.ToString().IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }
}
