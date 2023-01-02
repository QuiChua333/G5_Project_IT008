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

namespace CinemaManagementProject.View.Staff.Trouble
{
    /// <summary>
    /// Interaction logic for TroublePage.xaml
    /// </summary>
    public partial class TroublePage : Page
    {
        public TroublePage()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;   
            return ((item as TroubleDTO).TroubleType.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);  
        }
        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Listview.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(Listview.ItemsSource).Refresh();
        }
    }
}
