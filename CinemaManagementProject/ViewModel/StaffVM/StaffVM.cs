using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.View.Login;
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
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

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
        private Brush _mainColor { get; set; }
        public Brush MainColor
        {
            get { return _mainColor; }
            set { _mainColor = value; OnPropertyChanged(); }
        }
        private ImageSource _avatarSource { get; set; }
        public ImageSource AvatarSource
        {
            get { return _avatarSource; }
            set { _avatarSource = value; OnPropertyChanged(); }
        }
        private bool isEN = Properties.Settings.Default.isEnglish;
        public ICommand FirstLoadCM { get; set; }
        public ICommand OrderFoodCommand { get; set; }
        public ICommand FilmBookingCommand { get; set; }
        public ICommand TroubleCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand OpenAvatarPopupCommand { get; set; }
        public ICommand CloseAvatarPopupCommand { get; set; }
        public ICommand SwitchToSettingTab { get; set; }
        public ICommand SettingCommand { get; set; }

        private void OrderFood(object obj)
        {
            OrderFoodManagementVM.OrderFoodManagementVM.checkOnlyFoodOfPage = true;
            CurrentView = new OrderFoodManagementVM.OrderFoodManagementVM();
        }
        private void FilmBooking(object obj)
        {
            OrderFoodManagementVM.OrderFoodManagementVM.checkOnlyFoodOfPage = false;
            CurrentView = new FilmBookingVM.FilmBookingVM();  
        } 
        private void Trouble(object obj) => CurrentView = new TroubleStaffVM.TroublePageViewModel();
        private void Setting(object obj) => CurrentView = new SettingStaffVM.SettingStaffVM();
        public StaffVM()
        {
            OrderFoodCommand = new RelayCommand(OrderFood);
            FilmBookingCommand = new RelayCommand(FilmBooking);
            TroubleCommand = new RelayCommand(Trouble);
            SettingCommand = new RelayCommand(Setting);
            //StartPage
            _currentView = new FilmBookingVM.FilmBookingVM();
            LogOutCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (CustomMessageBox.ShowOkCancel(isEN ? "Do you really want to log out?" : "Bạn thật sự muốn đăng xuất không?", isEN ? "Warning" : "Cảnh báo", isEN ? "Log out" : "Đăng xuất", isEN ? "No" : "Không", Views.CustomMessageBoxImage.Warning) == CustomMessageBoxResult.OK)
                {
                    if (p != null)
                    {
                        LoginWindow loginwd = new LoginWindow();
                        loginwd.Show();
                        RenewData();
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
            SwitchToSettingTab = new RelayCommand<RadioButton>((p) => { return true; }, (p) =>
            {
                p.IsChecked = true;
                CurrentView = new SettingStaffVM.SettingStaffVM();
            });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainColor = (SolidColorBrush)new BrushConverter().ConvertFrom(Properties.Settings.Default.MainAppColor);
                if (currentStaff.Avatar != null)
                    AvatarSource = LoadAvatarImage(currentStaff.Avatar);
                else
                    AvatarSource = null;
                StaffName = currentStaff.StaffName;
                StaffEmail = currentStaff.Email;
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
                CurrentView = new FilmBookingVM.FilmBookingVM();
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
        public BitmapImage LoadAvatarImage(byte[] data)
        {
            MemoryStream strm = new MemoryStream();
            strm.Write(data, 0, data.Length);
            strm.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public void RenewData()
        {
            currentStaff = null;
            StaffEmail = "";
            StaffName = "";
            AvatarSource = null;
        }
    }
}
