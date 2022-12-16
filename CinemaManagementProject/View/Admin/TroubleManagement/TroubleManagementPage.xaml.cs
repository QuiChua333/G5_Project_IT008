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

namespace CinemaManagementProject.View.Admin.TroubleManagement
{
    /// <summary>
    /// Interaction logic for TroubleManagementPage.xaml
    /// </summary>
    public partial class TroubleManagementPage : Page
    {
        public TroubleManagementPage()
        {
            InitializeComponent();
        }

        private void TroubleItem_TroubleItemClick(object sender, EventArgs e)
        {
            TroubleInfomation troubleInfomation = new TroubleInfomation();
            troubleInfomation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            troubleInfomation.Show();
            
        }

        private void TroubleItem_EditButtonClick(object sender, EventArgs e)
        {
            EditTroubleInformation troubleInfomation = new EditTroubleInformation();
            troubleInfomation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            troubleInfomation.Show();
        }
    }
}
