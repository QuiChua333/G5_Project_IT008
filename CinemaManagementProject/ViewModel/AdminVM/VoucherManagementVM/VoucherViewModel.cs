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
        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadAddWindowCM { get; set; }
        public ICommand LoadAddInfopageCM { get; set; }
        public ICommand SaveNewBigVoucherCM { get; set; }
        public ICommand LoadAddVoucherPageCM { get; set; }
        public ICommand LoadInforCM { get; set; }
        public ICommand LoadInfoBigVRCM { get; set; }
        public ICommand LoadEditInfoPageCM { get; set; }
        public ICommand UpdateBigVoucherCM { get;set; }

        public VoucherViewModel()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                GetCurrentDate = DateTime.Today;
                StartDate = EndDate = DateTime.Today;

                try
                {
                    //Loading UI Handler Here
                    ListBigVoucher = new ObservableCollection<VoucherReleaseDTO>(await VoucherService.Ins.GetAllVoucherReleases());
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu","Lỗi", "OK",CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            });
            LoadAddWindowCM = new RelayCommand<object>((p) => { return true;}, async (p)=>
            {
                try
                {
                   AddWindow wd = new AddWindow();
                    Unlock = false;
                    wd.ShowDialog();
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

                AddVoucherPage w = new AddVoucherPage();
                GetVoucherList();
               
                mainFrame.Content = w;

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
                   await Task.Run(async () =>
                   {
                       (VoucherReleaseDTO voucherRelease, bool haveAny) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.VoucherReleaseCode);
                       SelectedItem = voucherRelease;

                       
                   });
                  

                   w.ShowDialog();
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
           });

            UpdateBigVoucherCM = new RelayCommand<object>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateBigVoucherFunc();
                IsSaving = false;
            });

        }
       


    }
}
