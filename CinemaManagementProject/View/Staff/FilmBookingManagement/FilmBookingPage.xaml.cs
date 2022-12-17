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

namespace CinemaManagementProject.View.Staff.FilmBooking
{
    /// <summary>
    /// Interaction logic for FilmBookingPage.xaml
    /// </summary>
    public partial class FilmBookingPage : Page
    {
        public FilmBookingPage()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as FilmDTO).FilmName.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void SearchBox_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewFilm.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(ListViewFilm.ItemsSource).Refresh();
        }
    }
}
