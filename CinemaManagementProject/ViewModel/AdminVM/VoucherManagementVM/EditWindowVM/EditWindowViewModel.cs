using CinemaManagementProject.View.Admin.VoucherManagement.EditWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CinemaManagementProject.DTOs;
using System.Windows;
using System.Windows.Controls;

namespace CinemaManagementProject.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel: BaseViewModel
    {
        private string _VoucherReleaseCode;
        public string VoucherReleaseCode
        {
            get { return _VoucherReleaseCode; }
            set { _VoucherReleaseCode = value; OnPropertyChanged(); }
        }
        private static bool status2;
        public static bool Status2
        {
            get { return status2; }
            set { status2 = value; }
        }
        private int numberSelected;
        public int NumberSelected
        {
            get { return numberSelected; }
            set { numberSelected = value; OnPropertyChanged(); }
        }
        private static List<int> waitingMiniVoucher;
        public static List<int> WaitingMiniVoucher
        {
            get { return waitingMiniVoucher; }
            set { waitingMiniVoucher = value; }
        }





        public void LoadEditInfoViewDataFunc(EditInfoPage w)
        {
            if (SelectedItem != null)
            {
                VoucherReleaseCode=selectedItem.VoucherReleaseCode;
                VoucherReleaseName = SelectedItem.VoucherReleaseName;
                Price = SelectedItem.Price;
                Status2 = SelectedItem.VoucherReleaseStatus;
                if (Status2)
                {
                    w.yes.IsChecked = true;
                    w.no.IsChecked = false;
                }
                else
                {
                    w.yes.IsChecked = false;
                    w.no.IsChecked = true;
                }
                StartDate = SelectedItem.StartDate;
                EndDate = SelectedItem.EndDate;
                EnableMerge = SelectedItem.EnableMerge;
                TypeObject.Content = SelectedItem.TypeObject;
                MinimizeTotal = SelectedItem.MinimizeTotal;
                w.unused.Content = SelectedItem.UnusedVCount;
                w.total.Content = SelectedItem.VCount;
                w.btnSave.Visibility = Visibility.Collapsed;
                w.btnExit.Visibility = Visibility.Visible;
            }
        }
        //public void LoadEditInfoViewDataAfterSaveFunc(EditInfoPage z)
        //{
        //    VoucherReleaseDTO vr = new VoucherReleaseDTO
        //    {
        //        VoucherReleaseCode = VoucherReleaseCode,
        //        StartDate = StartDate,
        //        EndDate = EndDate,
        //        EnableMerge = EnableMerge,
        //        MinimizeTotal = MinimizeTotal,
        //        TypeObject = TypeObject.Content.ToString(),
        //        Price = Price,
        //        VoucherReleaseName = VoucherReleaseName,
        //        VoucherReleaseStatus = Status,
        //    };
        //    if (Status2)
        //    {
        //        z.yes.IsChecked = true;
        //        z.no.IsChecked = false;
        //    }
        //    else
        //    {
        //        z.yes.IsChecked = false;
        //        z.no.IsChecked = true;
        //    }
        //    z.unused.Content = vr.UnusedVCount.ToString();
        //        z.total.Content = vr.VCount.ToString();
        //    z.btnSave.Visibility = Visibility.Collapsed;
        //    z.btnExit.Visibility = Visibility.Visible;


        //}

    }
}
