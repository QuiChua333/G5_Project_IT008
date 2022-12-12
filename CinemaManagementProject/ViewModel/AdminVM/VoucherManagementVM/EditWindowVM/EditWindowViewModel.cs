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
using CinemaManagementProject.Views;
using CinemaManagementProject.Model.Service;
using System.Collections.ObjectModel;
using CinemaManagementProject.View.Admin.VoucherManagement.AddWindow;

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

       
        VoucherReleaseDTO oldVer = new VoucherReleaseDTO();
        public static bool IsUpdate=false;
        public static bool HaveUsed = false;


        public void LoadEditInfoViewDataFunc(EditInfoPage w)
        {
            if (SelectedItem != null)
            {
                VoucherReleaseCode = selectedItem.VoucherReleaseCode;
                VoucherReleaseName = SelectedItem.VoucherReleaseName;
                Price = SelectedItem.Price;
                Status2 = SelectedItem.VoucherReleaseStatus;
                if (Status2)
                {
                    w.yes.IsChecked = true;
                    w.no.IsChecked = false;
                    w.no.IsEnabled = false;
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
                if (IsUpdate)
                {
                    w.btnSave.Visibility = Visibility.Visible;
                    w.btnExit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    w.btnSave.Visibility = Visibility.Collapsed;
                    w.btnExit.Visibility = Visibility.Visible;
                }    
            }
        }
        public async Task UpdateBigVoucherFunc()
        {
            if (string.IsNullOrEmpty(VoucherReleaseName))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin", "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            if (Price >= MinimizeTotal)
            {
                CustomMessageBox.ShowOk("Mệnh giá voucher phải bé hơn tổng tối thiểu", "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            if (StartDate > EndDate)
            {
                CustomMessageBox.ShowOk("Ngày hiệu lực không hợp lệ", "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }

            if (SelectedItem != null)
                oldVer = SelectedItem;
            VoucherReleaseDTO vr = new VoucherReleaseDTO
            {
                VoucherReleaseCode = SelectedItem.VoucherReleaseCode,
                StartDate = StartDate,
                EndDate = EndDate,
                EnableMerge = EnableMerge,
                MinimizeTotal = MinimizeTotal,
                TypeObject = TypeObject.Content.ToString(),
                Price = Price,
                VoucherReleaseName = VoucherReleaseName,
                VoucherReleaseStatus = Status2,
            };

            (bool isSucess, string addSuccess) = await VoucherService.Ins.UpdateVoucherRelease(vr);

            if (isSucess)
            {
                CustomMessageBox.ShowOk(addSuccess, "Thông báo", "Ok", CustomMessageBoxImage.Success);

                try
                {
                    ListBigVoucher = new ObservableCollection<VoucherReleaseDTO>(await VoucherService.Ins.GetAllVoucherReleases());
                    (VoucherReleaseDTO voucherReleaseDetail, _) = await VoucherService.Ins.GetVoucherReleaseDetails(oldVer.VoucherReleaseCode);
                    SelectedItem = voucherReleaseDetail;
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                    NumberSelected = 0;
                    EditInfoPage w = new EditInfoPage();
                    LoadEditInfoViewDataFunc(w);
                    mainFrame.Content = w;
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }

            }
            else
            {
                CustomMessageBox.ShowOk(addSuccess, "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public async Task DeleteMiniVoucherFunc()
        {
            if (WaitingMiniVoucher.Count == 0)
            {
                CustomMessageBox.ShowOk("Danh sách chọn đang trống!", "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }

            (bool deleteSuccess, string messageFromDelete) = await VoucherService.Ins.DeteleVouchers(WaitingMiniVoucher);

            if (deleteSuccess)
            {
                CustomMessageBox.ShowOk(messageFromDelete, "Thông báo", "Ok", CustomMessageBoxImage.Success);
                try
                {
                    (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.VoucherReleaseCode);
                    SelectedItem = voucherReleaseDetail;
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                    if (AddVoucherPage.TopCheck != null )
                    {
                        AddVoucherPage.TopCheck.IsChecked = false;
                    }
                    if (AddVoucherPageActive.TopCheck != null )
                    {
                        AddVoucherPageActive.TopCheck.IsChecked = false;
                    }
                    NumberSelected = 0;
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk(messageFromDelete, "Lỗi", "Ok", CustomMessageBoxImage.Error);
            }
        }
        public void CheckAllMiniVoucherFunc(bool func)
        {
            if (func)
            {
                WaitingMiniVoucher.Clear();
                foreach (var item in StoreAllMini)
                {
                    if (item.VoucherStatus != "Ðã phát hành")
                    {
                        WaitingMiniVoucher.Add(item.Id);
                        item.IsChecked = true;
                    }

                }
                NumberSelected = WaitingMiniVoucher.Count;
            }
            else
            {
                WaitingMiniVoucher.Clear();
                foreach (var item in StoreAllMini)
                {
                    if (item.VoucherStatus != "Ðã phát hành")
                    {
                        item.IsChecked = false;
                    }

                }
                NumberSelected = 0;
            }
        }




    }
}
