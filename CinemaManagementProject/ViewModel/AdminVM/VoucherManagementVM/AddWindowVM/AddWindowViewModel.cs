using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.View.Admin.VoucherManagement.EditWindow;
using CinemaManagementProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagementProject.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel:BaseViewModel
    {
        public static bool Unlock = false;
        public bool LoadAddInfoPageStatus = true;
        public bool LoadAddVoucherPageStatus = false;
        #region binding data and save to database here

        private string _VoucherReleaseName;
        public string VoucherReleaseName
        {
            get { return _VoucherReleaseName; }
            set { _VoucherReleaseName = value; OnPropertyChanged(); }
        }

        private double _Price;
        public double Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged(); }
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
        private ObservableCollection<VoucherDTO> _ListViewVoucher;
        public ObservableCollection<VoucherDTO> ListViewVoucher
        {
            get { return _ListViewVoucher; }
            set { _ListViewVoucher = value; OnPropertyChanged(); }
        }

        private static ObservableCollection<VoucherDTO> _StoreAllMini;
        public static ObservableCollection<VoucherDTO> StoreAllMini
        {
            get { return _StoreAllMini; }
            set { _StoreAllMini = value; }
        }
        private ComboBoxItem _SelectedCbbFilter;
        public ComboBoxItem SelectedCbbFilter
        {
            get { return _SelectedCbbFilter; }
            set
            {
                _SelectedCbbFilter = value; OnPropertyChanged();
                ChangeListViewSource();
            }
        }
        private SolidColorBrush _BtnInfoColor;
        public SolidColorBrush BtnInfoColor
        {
            get { return _BtnInfoColor; }
            set { _BtnInfoColor = value; OnPropertyChanged(); }
        }

        private SolidColorBrush _BtnAddColor;
        public SolidColorBrush BtnAddColor
        {
            get { return _BtnAddColor; }
            set { _BtnAddColor = value; OnPropertyChanged(); }
        }
        public async Task SaveNewBigVoucherFunc()
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
            VoucherReleaseDTO vr = new VoucherReleaseDTO
            {
                StartDate = StartDate,
                EndDate = EndDate,
                EnableMerge = EnableMerge,
                MinimizeTotal = MinimizeTotal,
                TypeObject = TypeObject.Content.ToString(),
                Price = Price,
                VoucherReleaseName = VoucherReleaseName,
                VoucherReleaseStatus = Status,
            };

            (bool isSucess, string addSuccess, VoucherReleaseDTO newVoucherRelease) = await VoucherService.Ins.CreateVoucherRelease(vr);

            if (isSucess)
            {

                Unlock = true;
                selectedItem= newVoucherRelease;
                EditInfoPage w = new EditInfoPage();
                LoadEditInfoViewDataFunc(w);
                mainFrame.Content = w;
                CustomMessageBox.ShowOk(addSuccess, "Thông báo", "Ok", CustomMessageBoxImage.Success);
                ListBigVoucher.Insert(0,newVoucherRelease);
                try
                {
                    (VoucherReleaseDTO voucherReleaseDetail, _) = await VoucherService.Ins.GetVoucherReleaseDetails(newVoucherRelease.VoucherReleaseCode);
                    SelectedItem = voucherReleaseDetail;
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
                //catch (Exception e)
                //{
                //    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                //}

            }
            else
            {
                CustomMessageBox.ShowOk(addSuccess, "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public void ChangeListViewSource()
        {
            if (SelectedCbbFilter is null) return;

            NumberSelected = 0;
            if (WaitingMiniVoucher != null)
                WaitingMiniVoucher.Clear();

            ListViewVoucher = new ObservableCollection<VoucherDTO>();

            if (SelectedCbbFilter.Content.ToString() == "Toàn bộ")
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini);
            }
            else
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini.Where(v => v.VoucherStatus == SelectedCbbFilter.Tag.ToString()).ToList());
            }
        }
        public void GetVoucherList()
        {
            if (SelectedCbbFilter is null) return;

            ListViewVoucher = new ObservableCollection<VoucherDTO>();
            try
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini);
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



        public void ActiveButton(Button p)
        {
            if (p is null) return;
            p.IsEnabled = true;
        }

        private void ChangeColorBtn()
        {
            if (LoadAddInfoPageStatus == true) BtnInfoColor = Brushes.Transparent;
            else BtnInfoColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#CCCCCC");
            if (LoadAddVoucherPageStatus==true) BtnAddColor= Brushes.Transparent;
            else BtnAddColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#CCCCCC");

        }
    }


}
