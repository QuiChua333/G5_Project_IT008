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
                    return ((item as CustomerDTO).CustomerName.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
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
                switch (s.Tag.ToString())
                {
                    case "Theo năm":
                        {
                            boder.Visibility = Visibility.Visible;
                            Time1.Visibility = Visibility.Visible;
                            GetYearSource(Time1);
                            return;
                        }
                    case "Theo tháng":
                        {
                            boder.Visibility = Visibility.Visible;
                            Time1.Visibility = Visibility.Visible;
                            GetMonthSource(Time1);
                            return;
                        }
                    case "Toàn bộ":
                        {
                            boder.Visibility = Visibility.Collapsed;
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
            if (Properties.Settings.Default.isEnglish == false)
            {
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
            }
            else
            {
                l.Add("January");
                l.Add("February");
                l.Add("March");
                l.Add("April");
                l.Add("May");
                l.Add("June");
                l.Add("July");
                l.Add("August");
                l.Add("September");
                l.Add("October");
                l.Add("November");
                l.Add("December");
            }
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
