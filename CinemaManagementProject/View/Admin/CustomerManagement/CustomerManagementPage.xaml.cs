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
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;

            switch (cbbFilter.SelectedValue)
            {
                case "Mã khách hàng":
                    return ((item as CustomerDTO).Id.ToString().IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Tên khách hàng":
                    return ((item as CustomerDTO).Name.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Số điện thoại":
                    return ((item as CustomerDTO).PhoneNumber.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                default:
                    return ((item as CustomerDTO).Id.ToString().IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void periodbox1_Loaded(object sender, RoutedEventArgs e)
        {
            GetYearSource(Time1);
            return;
        }
        private void periodbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem s = (ComboBoxItem)periodbox1.SelectedItem;
            if(Time1!= null)
            {
                switch (s.Content.ToString())
                {
                    case "Theo năm":
                        {
                            Time1.Visibility = Visibility.Visible;
                            GetYearSource(Time1);
                            return;
                        }
                    case "Theo tháng":
                        {
                            Time1.Visibility = Visibility.Visible;
                            GetMonthSource(Time1);
                            return;
                        }
                    case "Toàn bộ":
                        {
                            Time1.Visibility = Visibility.Collapsed;
                            return;
                        }

                }

            }
        }
        public void GetYearSource(ComboBox cbb)
        {
            if (cbb is null) return;

            List<string> l = new List<string>();

            int now = -1;
            for (int i = 2020; i <= System.DateTime.Now.Year; i++)
            {
                now++;
                l.Add(i.ToString());
            }
            cbb.ItemsSource = l;
            cbb.SelectedIndex = now;
        }
        public void GetMonthSource(ComboBox cbb)
        {
            if (cbb is null) return;

            List<string> l = new List<string>();

            l.Add("Tháng 1");
            l.Add("Tháng 2");
            l.Add("Tháng 3");
            l.Add("Tháng 4");
            l.Add("Tháng 5");
            l.Add("Tháng 6");
            l.Add("Tháng 7");
            l.Add("Tháng 8");
            l.Add("Tháng 9");
            l.Add("Tháng 10");
            l.Add("Tháng 11");
            l.Add("Tháng 12");

            cbb.ItemsSource = l;
            cbb.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }
}
