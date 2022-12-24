using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CinemaManagementProject.Model;
using CinemaManagementProject.View.Login;
using CinemaManagementProject.Views;
using System.Windows;

namespace CinemaManagementProject.ViewModel.AdminVM
{
    public class AdminVM : BaseViewModel
    {
        public static StaffDTO currentStaff;
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand VoucherCommand { get; set; }
        public ICommand ShowTimeViewCommand { get; set; }
        public ICommand CustomerViewCommand { get; set; }
        public ICommand HistoryViewCommand { get; set; }
        public ICommand StaffViewCommand { get; set; }
        public ICommand FilmViewCommand { get; set; }
        public ICommand FoodCommand { get; set; }
        public ICommand TroubleCommand { get; set; }
        public ICommand StatisticalViewCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        private void Food(object obj) => CurrentView = new FoodManagementVM.FoodManagementVM();
        private void Voucher(object obj) => CurrentView = new VoucherManagementVM.VoucherViewModel();
        private void ShowTime(object obj) => CurrentView = new ShowtimeManagementVM.ShowtimeMangementViewModel();
        private void Customer(object obj) => CurrentView = new CustomerManagementVM.CustomerManagementViewModel();
        private void History(object obj) => CurrentView = new Import_ExportManagementVM.Import_ExportManagementViewModel();
        private void Staff(object obj) => CurrentView = new StaffManagementVM.StaffManagementViewModel();
        private void Film(object obj) => CurrentView = new MovieManagementVM.MovieManagementVM();
        public void Statistical(object obj) => CurrentView = new StatisticalManagementVM.StatisticalManagementVM();
        private void Trouble(object obj) => CurrentView = new TroubleManagementVM.TroubleManagementViewModel();



        public AdminVM()
        {
            VoucherCommand = new RelayCommand(Voucher);
            ShowTimeViewCommand = new RelayCommand(ShowTime);
            CustomerViewCommand = new RelayCommand(Customer);
            HistoryViewCommand = new RelayCommand(History);
            StaffViewCommand = new RelayCommand(Staff);
            FilmViewCommand = new RelayCommand(Film);
            FoodCommand = new RelayCommand(Food);
            _currentView = new VoucherManagementVM.VoucherViewModel();
            StatisticalViewCommand = new RelayCommand(Statistical);
            TroubleCommand = new RelayCommand(Trouble);
            LogOutCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (CustomMessageBox.ShowOkCancel("Bạn thật sự muốn đăng xuất không?", "Cảnh báo", "Đăng xuất", "Không", Views.CustomMessageBoxImage.Information) == CustomMessageBoxResult.OK)
                {
                    if (p != null)
                    {
                        LoginWindow loginwd = new LoginWindow();
                        loginwd.Show();
                        p.Close();
                    }
                }
            });
        }
    }
}
