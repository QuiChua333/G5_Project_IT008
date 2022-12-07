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
                //if (SelectedItem.VoucherReleaseStatus == false)
                //{
                //    w.releasebtn.Visibility = Visibility.Collapsed;
                //    w.releasebtn2.Visibility = Visibility.Collapsed;
                //}

                //else
                //{
                //    w.releasebtn.Visibility = Visibility.Visible;
                //    w.releasebtn2.Visibility = Visibility.Visible;
                //}

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
        }
       


    }
}
