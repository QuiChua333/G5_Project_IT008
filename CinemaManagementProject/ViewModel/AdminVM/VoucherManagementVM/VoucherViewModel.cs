using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CinemaManagementProject.Views;
using System.Windows;
using CinemaManagementProject.View.Admin.VoucherManagement.AddWindow;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using CinemaManagementProject.View.Admin.VoucherManagement.EditWindow;
using CinemaManagementProject.Utils;


namespace CinemaManagementProject.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {

        public Frame mainFrame { get; set; } 
        public ObservableCollection<VoucherReleaseDTO> _ListBigVoucher;
        public ObservableCollection<VoucherReleaseDTO> ListBigVoucher
        {
            get { return _ListBigVoucher; }
            set { _ListBigVoucher = value; OnPropertyChanged(); }
        }
        private VoucherReleaseDTO selectedItem;
        public VoucherReleaseDTO SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }
        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }
        private bool _IsReleaseVoucherLoading;
        public bool IsReleaseVoucherLoading
        {
            get { return _IsReleaseVoucherLoading; }
            set { _IsReleaseVoucherLoading = value; OnPropertyChanged(); }
        }
        public static bool IsEnglish = false;
        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadAddWindowCM { get; set; }
        public ICommand LoadAddInfopageCM { get; set; }
        public ICommand SaveNewBigVoucherCM { get; set; }
        public ICommand LoadAddVoucherPageCM { get; set; }
        public ICommand LoadInforCM { get; set; }
        public ICommand LoadInfoBigVRCM { get; set; }
        public ICommand LoadEditInfoPageCM { get; set; }
        public ICommand UpdateBigVoucherCM { get;set; }
        public ICommand LoadDeleteVoucherCM { get; set; }
        public ICommand DeleteMiniVoucherCM { get; set; }
        public ICommand LoadAddMiniVoucherCM { get; set; }
        public ICommand LessVoucherCM { get; set; }
        public ICommand MoreVoucherCM { get; set; }
        public ICommand SaveMiniVoucherCM { get; set; }
        public ICommand FirstLoadMiniCM { get; set; }
        public ICommand StoreWaitingListCM { get; set; }
        public ICommand CheckAllMiniVoucherCM { get; set; }
        public ICommand LoadAddListMiniVoucherCM { get; set; }
        public ICommand SaveListMiniVoucherCM { get; set; }
        public ICommand ReleaseVoucherExcelCM { get; set; }
        public ICommand OpenReleaseVoucherCM { get; set; }
        public ICommand ResetSelectedNumberCM { get; set; }
        public ICommand CalculateNumberOfVoucherCM { get; set; }
        public ICommand MoreEmailCM { get; set; }
        public ICommand AcceptEmailCM { get; set; }
        public ICommand ShowListEmailCM { get; set; }
        public ICommand ReleaseVoucherCM { get; set; }
        public ICommand RefreshEmailListCM { get; set; }
        public ICommand LessEmailCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
   

        public VoucherViewModel()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                GetCurrentDate = DateTime.Today;
                StartDate = EndDate = DateTime.Today;
                IsEnglish = Properties.Settings.Default.isEnglish;

                try
                {
                    //Loading UI Handler Here
                    ListBigVoucher = new ObservableCollection<VoucherReleaseDTO>(await VoucherService.Ins.GetAllVoucherReleases());
                }
                catch (System.Data.Entity.Core.EntityException e)
                {;
                    CustomMessageBox.ShowOk(IsEnglish? "Unable to connect to database": "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK",CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error":"Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            });
            FirstLoadMiniCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                try
                {
                    //Loading UI Handler Here
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(selectedItem.Vouchers);
                    if (Properties.Settings.Default.isEnglish == true)
                    {
                        foreach (VoucherDTO item in selectedItem.Vouchers)
                        {
                            item.VoucherStatus = ConvertVoucherStatusToEnglish(item.VoucherStatus);
                        }
                    }
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            });

            LoadAddWindowCM = new RelayCommand<object>((p) => { return true;}, async (p)=>
            {
                try
                {
                   AddWindow wd = new AddWindow();
                    Unlock = false;
                    SelectedItem = null;
                    wd.ShowDialog();
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            });

            LoadAddInfopageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                LoadAddInfoPageStatus = true;
                LoadAddVoucherPageStatus = false;
                ChangeColorBtn();
                SetDefault();
                p.Content = new AddInfoPage();
                mainFrame = p;
            });
            SaveNewBigVoucherCM = new RelayCommand<object>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await SaveNewBigVoucherFunc();
                IsSaving = false;
            });
            LoadAddVoucherPageCM = new RelayCommand<Button>((p) =>
            {
                if (Unlock == false) return false;
                else
                    return true;
            }, (p) =>
            {
                 if (p is null) return;
                ActiveButton(p);
                LoadAddVoucherPageStatus=true;
                LoadAddInfoPageStatus= false;
                ChangeColorBtn();
                if (selectedItem == null)
                {
                    AddVoucherPage w = new AddVoucherPage();
                    GetVoucherList();
                    mainFrame.Content = w;
                }
                else
                {
                    if (selectedItem.VoucherReleaseStatus == true)
                    {
                        AddVoucherPageActive w = new AddVoucherPageActive();
                        GetVoucherList();
                        mainFrame.Content = w;
                    }
                    else
                    {
                        AddVoucherPage w = new AddVoucherPage();
                        GetVoucherList();
                        mainFrame.Content = w;
                    }
                }
                WaitingMiniVoucher = new List<int>();
                NumberSelected = 0;
            });
            LoadInforCM = new RelayCommand<Button>((p) =>
            {
                if (Unlock == false) return false;
                else
                    return true;
            },
            (p) =>
            {
                LoadAddVoucherPageStatus = false;
                LoadAddInfoPageStatus = true;
                ChangeColorBtn();
                EditInfoPage w = new EditInfoPage();
                LoadEditInfoViewDataFunc(w);
                mainFrame.Content = w;
            });

            LoadEditInfoPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
                EditInfoPage w = new EditInfoPage();
                LoadEditInfoViewDataFunc(w);
                p.Content = w;
                Unlock = true;
            });
            LoadInfoBigVRCM = new RelayCommand<object>((p) =>
            {
                return true;
            },
           async (p) =>
           {
               if (SelectedItem is null) return;
               try
               {
                   IsUpdate = true;
                   EditWindow w = new EditWindow();
                   IsReleaseVoucherLoading = true;
                   await Task.Run(async () =>
                   {
                       (VoucherReleaseDTO voucherRelease, bool haveAny) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.VoucherReleaseCode);
                       SelectedItem = voucherRelease;
                       if (Properties.Settings.Default.isEnglish == true)
                       {
                           SelectedItem.TypeObject = ConvertTypeObjectToEnglish(SelectedItem.TypeObject);
                       }
                       ListViewVoucher = new ObservableCollection<VoucherDTO>(voucherRelease.Vouchers); 
                       StoreAllMini = new ObservableCollection<VoucherDTO>(voucherRelease.Vouchers);

                   });
                   IsReleaseVoucherLoading = false;

                   w.ShowDialog();
               }
               catch (System.Data.Entity.Core.EntityException e)
               {
               
                   CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
               }
               catch (Exception e)
               {

                   CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);
               }
           });

            UpdateBigVoucherCM = new RelayCommand<object>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateBigVoucherFunc();
                IsSaving = false;
            });

            LoadDeleteVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedItem is null) return;
                if (SelectedItem.UnusedVCount != SelectedItem.VCount)
                {
                    CustomMessageBox.ShowOk(IsEnglish? "Existing issued vouchers" : "Tồn tại voucher đã phát hành", IsEnglish ? "Cannot delete":"Không thể xóa", "Ok", CustomMessageBoxImage.Error);
                }

                else
                {
                    string message = IsEnglish? "Are you sure you want to delete this release? Data cannot be recovered after deletion!" : "Bạn có chắc muốn xoá đợt phát hành này không? Dữ liệu không thể phục hồi sau khi xoá!";
                    CustomMessageBoxResult result = CustomMessageBox.ShowOkCancel(message, IsEnglish?"Warning":"Cảnh báo", "Yes", "No", CustomMessageBoxImage.Warning);

                    if (result == CustomMessageBoxResult.OK)
                    {
                        (bool deleteSuccess, string messageFromDelete) = await VoucherService.Ins.DeteleVoucherRelease(SelectedItem.VoucherReleaseCode);

                        if (deleteSuccess)
                        {
                            ListBigVoucher.Remove(SelectedItem);
                            CustomMessageBox.ShowOk(messageFromDelete, IsEnglish?"Notification":"Thông báo", "Ok", CustomMessageBoxImage.Success);
                        }
                        else
                        {
                            CustomMessageBox.ShowOk(messageFromDelete,IsEnglish?"Error":"Lỗi", "Ok", CustomMessageBoxImage.Error);
                        }
                    }
                }
            });
            DeleteMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await DeleteMiniVoucherFunc();
            });
            LoadAddMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddVoucher w = new AddVoucher();
                ListMiniVoucher = new ObservableCollection<VoucherDTO>();
                ListMiniVoucher.Add(new VoucherDTO
                {
                    VoucherCode = "",
                    VoucherReleaseId = SelectedItem.Id
                });
                w.ShowDialog();
            });
            LessVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LessVoucherFunc();
            });
            MoreVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (ListMiniVoucher[ListMiniVoucher.Count - 1].VoucherCode.Length < 5 && ListMiniVoucher[ListMiniVoucher.Count - 1].VoucherCode!="")
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Code length must be 5 characters or more!" : "Độ dài mã phải từ 5 ký tự trở lên!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                    return;
                }
                for (int i = ListMiniVoucher.Count - 2; i >= 0; i--)
                {
                    if (ListMiniVoucher[ListMiniVoucher.Count - 1].VoucherCode == ListMiniVoucher[i].VoucherCode && !String.IsNullOrEmpty(ListMiniVoucher[ListMiniVoucher.Count - 1].VoucherCode))
                    {
                        CustomMessageBox.ShowOk(IsEnglish ? "There is a duplicate code":"Đã có mã bị trùng", IsEnglish?"Warning":"Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                        return;
                    }
                }

                ListMiniVoucher.Add(new VoucherDTO
                {
                    VoucherCode = "",
                    VoucherReleaseId = SelectedItem.Id
                });
            });
            SaveMiniVoucherCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                await SaveMiniVoucherFunc();
                if (IsSucceedSaveMini)
                {
                    IsSucceedSaveMini = false;
                    p.Close();
                }

            });

            StoreWaitingListCM = new RelayCommand<CheckBox>((p) => { return true; }, (p) =>
            {
                int voucherId = int.Parse(p.Content.ToString());

                if (WaitingMiniVoucher.Contains(voucherId))
                {
                    WaitingMiniVoucher.Remove(voucherId);
                    VoucherDTO item = StoreAllMini.First(v => v.Id == voucherId);
                    item.IsChecked = false;
                }
                else
                {
                    WaitingMiniVoucher.Add(voucherId);
                    VoucherDTO item = StoreAllMini.First(v => v.Id == voucherId);
                    item.IsChecked = true;
                }
                NumberSelected = WaitingMiniVoucher.Count;
                if (!StoreAllMini.Any(v => v.IsChecked))
                {
                    if (AddVoucherPage.TopCheck != null)
                    {
                        AddVoucherPage.TopCheck.IsChecked = false;
              
                    }
                    if (AddVoucherPageActive.TopCheck != null)
                    {
                        AddVoucherPageActive.TopCheck.IsChecked = false;
                    }
                }

            });
            CheckAllMiniVoucherCM = new RelayCommand<CheckBox>((p) => { return true; }, (p) =>
            {
                if (p.IsChecked == true)
                {
                    CheckAllMiniVoucherFunc(true);
                }
                else
                {
                    CheckAllMiniVoucherFunc(false);
                }

            });
            LoadAddListMiniVoucherCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddListVoucher w = new AddListVoucher();
                w.ShowDialog();
            });
            SaveListMiniVoucherCM = new RelayCommand<Button>((p) => { return true; }, async (p) =>
            {
                string oldstring = p.Content.ToString();

                p.Content = "";
                p.IsHitTestVisible = false;
                await SaveListMiniVoucherFunc();

                p.Content = oldstring;
                p.IsHitTestVisible = true;
                if (IsSucceedSaveMini)
                {
                    AddListVoucher tk = Application.Current.Windows.OfType<AddListVoucher>().FirstOrDefault();
                    tk.Close();
                    IsSucceedSaveMini= false;
                }
                
            });
            ReleaseVoucherExcelCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (WaitingMiniVoucher.Count == 0)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Voucher list is Empty":"Danh sách voucher đang trống!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                    return;
                }

                ReleaseVoucherList = new ObservableCollection<VoucherDTO>();

                for (int i = 0; i < WaitingMiniVoucher.Count; i++)
                {
                    for (int j = 0; j < StoreAllMini.Count; j++)
                    {
                        if (WaitingMiniVoucher[i] == StoreAllMini[j].Id)
                        {
                            VoucherDTO temp = new VoucherDTO
                            {
                                Id = WaitingMiniVoucher[i],
                                VoucherCode = StoreAllMini[j].VoucherCode,
                                VoucherStatus = StoreAllMini[j].VoucherStatus
                            };
                            ReleaseVoucherList.Add(temp);
                            break;
                        }
                    }
                }
                foreach (var item in ReleaseVoucherList)
                {
                    if (item.VoucherStatus == "Đã sử dụng" || item.VoucherStatus == "Used")
                    {
                        CustomMessageBox.ShowOk(IsEnglish?"Existing used voucher!":"Tồn tại voucher đã sử dụng!", IsEnglish?"Warning":"Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                IsExport = false;
                await ExportVoucherFunc();
                if (IsExport)
                {
                    (bool release, string message) = await VoucherService.Ins.ReleaseMultiVoucher(WaitingMiniVoucher);

                    if (release)
                    {
                        CustomMessageBox.ShowOk(message, IsEnglish ? "Notification" : "Thông báo", "Ok", CustomMessageBoxImage.Success);
                        WaitingMiniVoucher.Clear();
                        try
                        {
                            (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.VoucherReleaseCode);

                            SelectedItem = voucherReleaseDetail;
                            if (Properties.Settings.Default.isEnglish == true)
                            {
                                foreach (VoucherDTO item in selectedItem.Vouchers)
                                {
                                    item.VoucherStatus = ConvertVoucherStatusToEnglish(item.VoucherStatus);
                                }
                            }
                            ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                            StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
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
                        catch (System.Data.Entity.Core.EntityException)
                        {
                            CustomMessageBox.ShowOk(IsEnglish ? "Unable to connect to database" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                        }
                        catch (Exception)
                        {
                            CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                        }
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(message, IsEnglish ? "Error" : "Lỗi", "OK", CustomMessageBoxImage.Error);

                    }
                    IsExport = false;
                    return;
                }
            });

            OpenReleaseVoucherCM = new RelayCommand<MenuItem>((p) => { return true;  }, (p) =>
            {
                ReleaseVoucher w = new ReleaseVoucher();
                w.ShowDialog();
            });
           

            ResetSelectedNumberCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NumberSelected = 0;
                WaitingMiniVoucher.Clear();
                foreach (var item in StoreAllMini)
                    item.IsChecked = false;
            });
            CalculateNumberOfVoucherCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                if (p is null) return;
                ComboBoxItem selectedNum = (ComboBoxItem)p.SelectedItem;
                ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(selectedNum.Content.ToString())));
            });
            MoreEmailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                foreach (var item in ListCustomerEmail)
                {
                    if (!RegexUtilities.IsValidEmail(item.Email))
                    {
                        CustomMessageBox.ShowOk(IsEnglish ? "Invalid Email!":"Email không hợp lệ!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                        return;
                    }
                }

                for (int i = ListCustomerEmail.Count - 2; i >= 0; i--)
                {
                    if (ListCustomerEmail[ListCustomerEmail.Count - 1].Email == ListCustomerEmail[i].Email)
                    {
                        CustomMessageBox.ShowOk(IsEnglish ? "Email has been duplicated!":"Email đã bị trùng!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);

                        return;
                    }
                }

                ListCustomerEmail.Add(new CustomerEmail
                {
                    Email = "",
                    IsReadonly = false,
                    IsEnable = true
                });
                ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));

            });
            LessEmailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListCustomerEmail.RemoveAt(selectedWaitingVoucher);
                ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));
            });
            AcceptEmailCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                foreach (var item in ListCustomerEmail)
                {
                    if (!RegexUtilities.IsValidEmail(item.Email) || item.Email=="")
                    {
                        CustomMessageBox.ShowOk(IsEnglish ? "Invalid Email!" : "Email không hợp lệ!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                for (int i = ListCustomerEmail.Count - 2; i >= 0; i--)
                {
                    if (ListCustomerEmail[ListCustomerEmail.Count - 1].Email == ListCustomerEmail[i].Email)
                    {
                        CustomMessageBox.ShowOk(IsEnglish ? "Email has been duplicated!" : "Email đã bị trùng!", IsEnglish ? "Warning" : "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);

                        return;
                    }
                }
                p.Close();

            });
            ShowListEmailCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListEmail w = new ListEmail();
                switch (ReleaseCustomerList.Tag.ToString())
                {
                    case "TOP_5_CUSTOMER":
                        {
                            VoucherViewModel.NumberCustomer = 5;
                            w.addnewemail.IsEnabled = false;

                            break;
                        }
                    case "NEW_CUSTOMER":
                        {
                            VoucherViewModel.NumberCustomer = 0;
                            w.addnewemail.IsEnabled = false;
                            break;
                        }
                    case "COMMON":
                        {
                            VoucherViewModel.NumberCustomer = -1;
                            w.addnewemail.IsEnabled = true;
                            break;
                        }
                }
                w.ShowDialog();
            });
            ReleaseVoucherCM = new RelayCommand<ReleaseVoucher>((p) =>
            {
                if (IsReleaseVoucherLoading)
                {
                    return false;
                }
                return true;
            }, async
            (p) =>
            {
                IsReleaseVoucherLoading = true;

                await ReleaseVoucherFunc(p);

                IsReleaseVoucherLoading = false;
                p.Close();

            });
            RefreshEmailListCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await RefreshEmailList();
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (ReleaseCustomerList.Tag.ToString() == "COMMON")
                {
                    if (ListCustomerEmail.Count > 0)
                    {

                        ListCustomerEmail.RemoveAt(ListCustomerEmail.Count - 1);
                        ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));
                    }
                }
                
                p.Close();

            });
           



        }



    }
}
