using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Admin.VoucherManagement.AddWindow;
using CinemaManagementProject.View.Admin.VoucherManagement.EditWindow;
using CinemaManagementProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
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
        private ObservableCollection<VoucherDTO> _ListMiniVoucher;
        public ObservableCollection<VoucherDTO> ListMiniVoucher
        {
            get { return _ListMiniVoucher; }
            set { _ListMiniVoucher = value; OnPropertyChanged(); }
        }
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
        private int selectedWaitingVoucher;
        public int SelectedWaitingVoucher
        {
            get { return selectedWaitingVoucher; }
            set { selectedWaitingVoucher = value; OnPropertyChanged(); }
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
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }
        

        private string firstChar;
        public string FirstChar
        {
            get { return firstChar; }
            set { firstChar = value; OnPropertyChanged(); }
        }

        private string lastChar;
        public string LastChar
        {
            get { return lastChar; }
            set { lastChar = value; OnPropertyChanged(); }
        }
        public async Task SaveNewBigVoucherFunc()
        {
            if (string.IsNullOrEmpty(VoucherReleaseName))
            {
                CustomMessageBox.ShowOk(IsEnglish?"Please enter enough information!":"Vui lòng nhập đủ thông tin!", IsEnglish ? "Warning":"Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            if (Price >= MinimizeTotal)
            {
                CustomMessageBox.ShowOk(IsEnglish?"Voucher face value must be less than the minimum total":"Mệnh giá voucher phải bé hơn tổng tối thiểu", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            if (StartDate > EndDate)
            {
                CustomMessageBox.ShowOk(IsEnglish?"Invalid effective date":"Ngày hiệu lực không hợp lệ", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
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
            if (Properties.Settings.Default.isEnglish == true)
            {
                vr.TypeObject = ConvertTypeObjectToVN(vr.TypeObject);
            }

            (bool isSucess, string addSuccess, VoucherReleaseDTO newVoucherRelease) = await VoucherService.Ins.CreateVoucherRelease(vr);

            if (isSucess)
            {
                IsUpdate = false;
                Unlock = true;
                selectedItem= newVoucherRelease;
                EditInfoPage w = new EditInfoPage();
                LoadEditInfoViewDataFunc(w);
                mainFrame.Content = w;
                CustomMessageBox.ShowOk(addSuccess, IsEnglish?"Notification":"Thông báo", "Ok", CustomMessageBoxImage.Success);
                ListBigVoucher.Insert(0,newVoucherRelease);
                try
                {
                    (VoucherReleaseDTO voucherReleaseDetail, _) = await VoucherService.Ins.GetVoucherReleaseDetails(newVoucherRelease.VoucherReleaseCode);
                    SelectedItem = voucherReleaseDetail;
                    if (Properties.Settings.Default.isEnglish == true)
                    {
                        SelectedItem.TypeObject = ConvertTypeObjectToEnglish(SelectedItem.TypeObject);
                    }
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                }

            }
            else
            {
                CustomMessageBox.ShowOk(addSuccess, IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public async Task<bool> CheckExistFunc()
        {
            (bool result, string message) = await VoucherService.Ins.GetListVoucher(ListMiniVoucher[ListMiniVoucher.Count - 1].VoucherCode);
            if (!result)
            {
                CustomMessageBox.ShowOk(message, IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return false;
            }
            else return true;
           
            
        }
        public bool IsSucceedSaveMini = false;
        public async Task SaveMiniVoucherFunc()
        {
            foreach (VoucherDTO item in ListMiniVoucher)
            {
                if (string.IsNullOrEmpty(item.VoucherCode))
                {
                    CustomMessageBox.ShowOk(IsEnglish?"Fields cannot be left blank":"Các trường không được để trống", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                    return;
                }
            }
            if (ListMiniVoucher[ListMiniVoucher.Count - 1].VoucherCode.Length < 5)
            {
                CustomMessageBox.ShowOk(IsEnglish ? "Code length must be 5 characters or more!" : "Độ dài mã phải từ 5 ký tự trở lên!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            for (int i = ListMiniVoucher.Count - 2; i >= 0; i--)
            {
                if (ListMiniVoucher[ListMiniVoucher.Count - 1].VoucherCode == ListMiniVoucher[i].VoucherCode)
                {
                    CustomMessageBox.ShowOk(IsEnglish?"There is a duplicate code":"Đã có mã bị trùng", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                    return;
                }
            }

            (bool createSuccess, string message, List<VoucherDTO> newListCode) = await VoucherService.Ins.CreateVoucher(SelectedItem.Id, new List<VoucherDTO>(ListMiniVoucher));

            if (createSuccess)
            {
                IsSucceedSaveMini = true;
;                CustomMessageBox.ShowOk(message, IsEnglish ? "Notification" : "Thông báo", "Ok", CustomMessageBoxImage.Success);
                if (ListViewVoucher != null) ListViewVoucher = new ObservableCollection<VoucherDTO>(ListViewVoucher.Concat(newListCode));
                else ListViewVoucher = new ObservableCollection<VoucherDTO>(newListCode);
                SelectedItem.Vouchers = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);

                for (int i = 0; i < ListBigVoucher.Count; i++)
                {
                    if (ListBigVoucher[i].Id == selectedItem.Id)
                    {
                        VoucherReleaseDTO clone = new VoucherReleaseDTO()
                        {
                            Id = selectedItem.Id,
                            VoucherReleaseName = selectedItem.VoucherReleaseName,
                            StartDate = selectedItem.StartDate,
                            EndDate = selectedItem.EndDate,
                            MinimizeTotal = selectedItem.MinimizeTotal,
                            Price = selectedItem.Price,
                            TypeObject = selectedItem.TypeObject,
                            VoucherReleaseStatus = selectedItem.VoucherReleaseStatus,
                            VCount = ListViewVoucher.Count,
                            UnusedVCount = SelectedItem.UnusedVCount + newListCode.Count,
                        };
                        ListBigVoucher[i] = clone;
                        SelectedItem = ListBigVoucher[i];
                        return;
                    }
                }
                try
                {
                    (VoucherReleaseDTO voucherReleaseDetail, _) = await VoucherService.Ins.GetVoucherReleaseDetails(selectedItem.VoucherReleaseCode);
                    SelectedItem = voucherReleaseDetail;
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                }
                if (AddVoucherPage.TopCheck !=null && AddVoucherPage.CBB != null)
                {
                    AddVoucherPage.TopCheck.IsChecked = false;
                    AddVoucherPage.CBB.SelectedIndex = 0;
                }
                if (AddVoucherPageActive.TopCheck!=null && AddVoucherPageActive.CBB != null)
                {
                    AddVoucherPageActive.TopCheck.IsChecked = false;
                    AddVoucherPageActive.CBB.SelectedIndex = 0;
                }
              
                NumberSelected = 0;


            }
            else
            {
                CustomMessageBox.ShowOk(message, IsEnglish ? "Error" : "Lỗi", "Ok",CustomMessageBoxImage.Error);
            }
        }
        public async Task SaveListMiniVoucherFunc()
        {
            if (Quantity == 0  || string.IsNullOrEmpty(FirstChar) || string.IsNullOrEmpty(LastChar))
            {
                CustomMessageBox.ShowOk(IsEnglish?"Can't be left blank!":"Không được để trống!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            
            (string error, List<string> listCode) = await Task<(string, List<string>)>.Run(() => Helper.GetListCode(Quantity,  FirstChar, LastChar,selectedItem));
            if (error != null)
            {
                CustomMessageBox.ShowOk(error, IsEnglish ? "Error" : "Lỗi", "Ok", CustomMessageBoxImage.Error);
                return;
            }

            IsReleaseVoucherLoading = true;
            (bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode) = await Task<(bool createSuccess, string createRandomSuccess, List<VoucherDTO> newListCode)>.Run(() => VoucherService.Ins.CreateRandomVoucherList(SelectedItem, listCode));
            IsReleaseVoucherLoading = false;

            if (createSuccess)
            {
                IsSucceedSaveMini = true;
                CustomMessageBox.ShowOk(createRandomSuccess, IsEnglish ? "Notification" : "Thông báo", "Ok", CustomMessageBoxImage.Success);

                ListViewVoucher = new ObservableCollection<VoucherDTO>(ListViewVoucher.Concat(newListCode));

                SelectedItem.Vouchers = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);

                for (int i = 0; i < ListBigVoucher.Count; i++)
                {
                    if (ListBigVoucher[i].Id == selectedItem.Id)
                    {
                        VoucherReleaseDTO clone = new VoucherReleaseDTO()
                        {
                            Id = selectedItem.Id,
                            VoucherReleaseName = selectedItem.VoucherReleaseName,
                            StartDate = selectedItem.StartDate,
                            EndDate = selectedItem.EndDate,
                            MinimizeTotal = selectedItem.MinimizeTotal,
                            Price = selectedItem.Price,
                            TypeObject = selectedItem.TypeObject,
                            VoucherReleaseStatus = selectedItem.VoucherReleaseStatus,
                            VCount = ListViewVoucher.Count,
                            UnusedVCount = SelectedItem.UnusedVCount + newListCode.Count,
                        };
                        ListBigVoucher[i] = clone;
                        SelectedItem = ListBigVoucher[i];
                        return;
                    }
                }
                try
                {
                    (VoucherReleaseDTO voucherReleaseDetail, _) = await VoucherService.Ins.GetVoucherReleaseDetails(selectedItem.VoucherReleaseCode);
                    SelectedItem = voucherReleaseDetail;
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                }
                if (AddVoucherPage.TopCheck != null && AddVoucherPage.CBB != null)
                {
                    AddVoucherPage.TopCheck.IsChecked = false;
                    AddVoucherPage.CBB.SelectedIndex = 0;
                }
                if (AddVoucherPageActive.TopCheck != null && AddVoucherPageActive.CBB != null)
                {
                    AddVoucherPageActive.TopCheck.IsChecked = false;
                    AddVoucherPageActive.CBB.SelectedIndex = 0;
                }

                NumberSelected = 0;
            }
            else
            {
                CustomMessageBox.ShowOk(createRandomSuccess, IsEnglish ? "Error" : "Lỗi", "Ok", CustomMessageBoxImage.Error);
            }
        }

        public void ChangeListViewSource()
        {
            if (SelectedCbbFilter is null) return;

            NumberSelected = 0;
            if (WaitingMiniVoucher != null)
                WaitingMiniVoucher.Clear();

            ListViewVoucher = new ObservableCollection<VoucherDTO>();

            if (SelectedCbbFilter.Tag.ToString() == "Toàn bộ")
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini);
            }
            else
            {

                

                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini.Where(v => v.VoucherStatus.Trim() == SelectedCbbFilter.Tag.ToString().Trim()).ToList());
                if (Properties.Settings.Default.isEnglish == true)
                {
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini.Where(v => ConvertVoucherStatusToVN(v.VoucherStatus) == SelectedCbbFilter.Tag.ToString()).ToList());
                }
            }
        }
        public void GetVoucherList()
        {
            if (SelectedCbbFilter == null) return;

            ListViewVoucher = new ObservableCollection<VoucherDTO>();
            try
            {
                ListViewVoucher = new ObservableCollection<VoucherDTO>(StoreAllMini);
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

            }
        }

        public void LessVoucherFunc()
        {
            ListMiniVoucher.RemoveAt(SelectedWaitingVoucher);
        }

        public void ActiveButton(System.Windows.Controls.Button p)
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

        private void SetDefault()
        {
            VoucherReleaseName= string.Empty;
            MinimizeTotal = 0;
            Price = 0;
            StartDate= DateTime.Now; EndDate= DateTime.Now;
            EnableMerge=false;
        }

        public string ConvertVoucherStatusToEnglish(string status)
        {
            switch (status)
            {
                case "Toàn bộ":
                    {
                        return "All";
                    }
                case "Chưa phát hành":
                    {
                        return "Not released yet";
                    }
                case "Đã phát hành":
                    {
                        return "Published";
                    }
                default:
                    {
                        return "Used";
                    }
            }
        }
        public string ConvertVoucherStatusToVN(string status)
        {
            switch (status)
            {
                case "All":
                    {
                        return "Toàn bộ";
                    }
                case "Not released yet":
                    {
                        return "Chưa phát hành";
                    }
                case "Published":
                    {
                        return "Đã phát hành";
                    }
                default:
                    {
                        return "Đã sử dụng";
                    }
            }
        }




    }


}
