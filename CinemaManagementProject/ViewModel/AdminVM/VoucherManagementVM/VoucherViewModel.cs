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

namespace CinemaManagementProject.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
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
        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadAddWindowCM { get; set; }
        public ICommand LoadViewCM { get; set; }

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

            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new AddInfoPage();
            });

        }
    }
}
