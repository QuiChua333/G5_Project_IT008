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

namespace CinemaManagementProject.View.Admin.VoucherManagement.AddWindow
{
    /// <summary>
    /// Interaction logic for AddVoucherPageActive.xaml
    /// </summary>
    public partial class AddVoucherPageActive : Page
    {
        public static CheckBox TopCheck;
        public static ComboBox CBB;
        public AddVoucherPageActive()
        {
            InitializeComponent();
            TopCheck = topcheckbox;
            CBB = cbb;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TopCheck is null) return;
            TopCheck.IsChecked = false;
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as VoucherDTO).VoucherCode.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);

        }
        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Listviewmini.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                result.Text = Listviewmini.Items.Count.ToString();
                CollectionViewSource.GetDefaultView(Listviewmini.ItemsSource).Refresh();
            }
        }
    }
}
