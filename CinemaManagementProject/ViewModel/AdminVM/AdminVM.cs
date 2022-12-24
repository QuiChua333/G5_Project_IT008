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
using System.Windows.Navigation;
using System.Windows.Controls;

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
        private string _staffNameIcon { get; set; }
        public string StaffNameIcon
        {
            get
            {
                return _staffNameIcon;
            }
            set
            {
                _staffNameIcon = value;
                OnPropertyChanged();
            }
        }
        private string _staffName { get; set; }
        public string StaffName
        {
            get
            {
                return _staffName;
            }
            set
            {
                _staffName = value;
                OnPropertyChanged();
            }
        }
        private string _staffEmail { get; set; }
        public string StaffEmail
        {
            get
            {
                return _staffEmail;
            }
            set
            {
                _staffEmail = value;
                OnPropertyChanged();
            }
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
        public ICommand OpenAvatarPopupCommand { get; set; }
        public ICommand CloseAvatarPopupCommand { get; set; }
        public ICommand SwitchToSettingTab { get; set; }
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
            if(currentStaff != null)
            {
                FormatStaffDisplayNameToIcon();
                SetInfomationToView();
            } 
            else
            {
                StaffNameIcon = "Ad";
                StaffName = "Admin";
                StaffEmail = "admin@gmail.com";
            }    
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
            OpenAvatarPopupCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = Visibility.Visible;
            });
            CloseAvatarPopupCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = Visibility.Hidden;
            });
            SwitchToSettingTab = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CustomMessageBox.ShowOk("Please add view and logic in setting tab!!!", ":)))))", "Ok");
            });
        }

        public void FormatStaffDisplayNameToIcon()
        {
            string staffName = currentStaff.StaffName;
            string[] trimNames = staffName.Split(' ');
            StaffNameIcon = trimNames[trimNames.Length - 1][0].ToString() + trimNames[0][0].ToString();
        }
        public void SetInfomationToView()
        {
            StaffName = currentStaff.StaffName;
            StaffEmail = currentStaff.Email;
        }
    }
}
