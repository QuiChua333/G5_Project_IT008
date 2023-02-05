using CinemaManagementProject.DTOs;
using Microsoft.Office.Interop.Excel;
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
using Page = System.Windows.Controls.Page;

namespace CinemaManagementProject.View.Admin.HistoryManagement
{
    /// <summary>
    /// Interaction logic for ExportHistoryPage.xaml
    /// </summary>
    public partial class ExportHistoryPage : Page
    {
        public ExportHistoryPage()
        {
            InitializeComponent();
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            //result.Content = _ListView.Items.Count;
            view.Filter = Filter;
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;

            switch (cbbFilter.SelectedValue)
            {
                case "Mã đơn":
                    return (((item as BillDTO).BillCode).IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Khách hàng":
                    return ((item as BillDTO).CustomerName.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Điện thoại":
                    {
                        if ((item as BillDTO).PhoneNumber != null)
                            return ((item as BillDTO).PhoneNumber.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                        else return false;
                    }
                default:
                    return (((item as BillDTO).BillCode).IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

        }
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = sender as ComboBox;

            if (cbbmonth != null && timepicker != null)
            {
                switch (cbb.SelectedIndex)
                {
                    case 0:
                        {
                            cbbmonth.Visibility = System.Windows.Visibility.Collapsed;
                            timepicker.Visibility = System.Windows.Visibility.Collapsed;
                            bdexporthp.Visibility = System.Windows.Visibility.Collapsed;

                            break;
                        }
                    case 1:
                        {
                            cbbmonth.Visibility = System.Windows.Visibility.Collapsed;
                            timepicker.Visibility = System.Windows.Visibility.Visible;
                            bdexporthp.Visibility = System.Windows.Visibility.Visible;

                            break;
                        }
                    case 2:
                        {
                            cbbmonth.Visibility = System.Windows.Visibility.Visible;
                            timepicker.Visibility = System.Windows.Visibility.Collapsed;
                            bdexporthp.Visibility = System.Windows.Visibility.Visible;

                            break;
                        }
                }
            }

        }
    }
}
