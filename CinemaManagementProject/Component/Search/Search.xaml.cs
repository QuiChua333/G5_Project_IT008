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

namespace CinemaManagementProject.Component.Search
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
            this.DataContext = this;
            PlaceHolder = "Search";
        }
        public string PlaceHolder { get; set; }
        public new double Height { get; set; }
        public new double Width { get; set; }
        public new double FontSize { get; set; }
        public double Corner { get; set; }
        public double IconSize { get; set; }
        public string Text
        {
            get
            {
                return SearchType.Text;
            }
        }
        public event EventHandler SearchTextChange;
        private void SearchType_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchType.Text = "";

        }
        private void SearchType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchType.Text))
                SearchType.Text = PlaceHolder;
        }
        protected void SearchType_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchTextChange?.Invoke(this, e);
        }
    }
}
