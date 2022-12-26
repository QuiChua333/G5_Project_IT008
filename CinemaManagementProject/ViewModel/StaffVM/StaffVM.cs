using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.View.Login;
using CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM;
using CinemaManagementProject.ViewModel.StaffVM.TroubleStaffVM;
using CinemaManagementProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace CinemaManagementProject.ViewModel.StaffVM
{
    class StaffVM : BaseViewModel
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
        public ICommand OrderFoodCommand { get; set; }
        public ICommand FilmBookingCommand { get; set; }
        public ICommand TroubleCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand OpenAvatarPopupCommand { get; set; }
        public ICommand CloseAvatarPopupCommand { get; set; }
        public ICommand SwitchToSettingTab { get; set; }
        private void OrderFood(object obj)
        {
            CurrentView = new OrderFoodManagementVM.OrderFoodManagementVM();
            OrderFoodManagementVM.OrderFoodManagementVM.checkOnlyFoodOfPage = true;
        }
        private void FilmBooking(object obj) => CurrentView = new FilmBookingVM.FilmBookingVM();
        private void Trouble(object obj) => CurrentView = new TroubleStaffVM.TroublePageViewModel();
        public StaffVM()
        {
            if (currentStaff != null)
            {
                FormatStaffDisplayNameToIcon();
                SetInfomationToView();
            }
            else
            {
                StaffNameIcon = "St";
                StaffName = "Staff";
                StaffEmail = "staff@gmail.com";
            }
            OrderFoodCommand = new RelayCommand(OrderFood);
            FilmBookingCommand = new RelayCommand(FilmBooking);
            TroubleCommand = new RelayCommand(Trouble);
            //StartPage
            _currentView = new FilmBookingVM.FilmBookingVM();
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
