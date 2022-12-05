using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel:BaseViewModel
    {
        #region binding data and save to database here
        private string _VoucherReleaseName;
        public string VoucherReleaseName
        {
            get { return _VoucherReleaseName; }
            set { _VoucherReleaseName = value; OnPropertyChanged(); }
        }

        private double _MenhGia;
        public double MenhGia
        {
            get { return _MenhGia; }
            set { _MenhGia = value; OnPropertyChanged(); }
        }

        private static bool _Status;
        public static bool Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private DateTime _GetCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return _GetCurrentDate; }
            set { _GetCurrentDate = value; OnPropertyChanged(); }
        }

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged(); }
        }

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; OnPropertyChanged(); }
        }

        private bool _EnableMerge;
        public bool EnableMerge
        {
            get { return _EnableMerge; }
            set { _EnableMerge = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _TypeObject;
        public ComboBoxItem TypeObject
        {
            get { return _TypeObject; }
            set { _TypeObject = value; OnPropertyChanged(); }
        }

        private double _MinimizeTotal;
        public double MinimizeTotal
        {
            get { return _MinimizeTotal; }
            set { _MinimizeTotal = value; OnPropertyChanged(); }
        }
        #endregion
        public ICommand SaveNewBigVoucherCM { get; set; }
    }


}
