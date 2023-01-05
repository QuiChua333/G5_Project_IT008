using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using System.IO;
using CinemaManagementProject.View.Staff.SettingManagement;
using System.Security.RightsManagement;
using System.Windows.Controls;
using CinemaManagementProject.View.Login;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Windows.Media;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using CinemaManagementProject.Model;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using CinemaManagementProject.View.Staff.TicketWindow;
using CinemaManagementProject.View.Staff;
using Path = System.IO.Path;
using CinemaManagementProject.Properties;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CinemaManagementProject.View;

namespace CinemaManagementProject.ViewModel.StaffVM.SettingStaffVM
{
    public partial class SettingStaffVM : BaseViewModel
    {
        public StaffDTO currentStaff;
        public static Window parentWindow;
        private string _staffName { get; set; }
        public string StaffName
        {
            set
            {
                _staffName = value;
                OnPropertyChanged();
            }
            get { return _staffName; }
        }

        private string _staffEmail { get; set; }
        public string StaffEmail
        {
            set
            {
                _staffEmail = value;
                OnPropertyChanged();
            }
            get { return _staffEmail; }
        }
        private string _currentCode { get; set; }
        public string CurrentCode
        {
            get { return _currentCode; }
            set { _currentCode = value; }
        }
        private string _position { get; set; }
        public string Position
        {
            set
            {
                _position = value;
                OnPropertyChanged();
            }
            get { return _position; }
        }
        public string _avatarName { get; set; }
        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; OnPropertyChanged(); }
        }
        private bool _isEdit { get; set; }
        private ConfirmStaffWindow confirmWD { get; set; }
        public bool IsEdit
        {
            get { return _isEdit; }
            set { _isEdit = value; OnPropertyChanged(); }
        }
        private bool _isEditEmail { get; set; }
        public bool IsEditEmail
        {
            get { return _isEditEmail; }
            set { _isEditEmail = value; OnPropertyChanged(); }
        }
        private int randomCode;
        private string _error { get; set; }
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged(); }
        }
        private ImageSource _imageSource { get; set; }
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }
        private MaterialDesignThemes.Wpf.PackIconKind iconEditEmail { get; set; }
        public MaterialDesignThemes.Wpf.PackIconKind IconEditEmail {
            get { return iconEditEmail; }
            set { iconEditEmail = value; OnPropertyChanged(); }
        }
        public string filePath;
        private RegistryKey reg { get; set; }
        private bool _isCheckedAutoStart { get; set; }
        public bool IsCheckedAutoStart
        {
            get { return _isCheckedAutoStart; }
            set { _isCheckedAutoStart = value; OnPropertyChanged(); }
        }
        
        private bool _isCheckedRemindLogin { get; set; }
        public bool IsCheckedRemindLogin
        {
            get { return _isCheckedRemindLogin; }
            set { _isCheckedRemindLogin = value; OnPropertyChanged(); }
        }
        private Brush _colorPicked { get; set; }
        public Brush ColorPicked
        {
            get { return _colorPicked; }
            set { _colorPicked = value; OnPropertyChanged(); }
        }
        private bool _isToResetPage { get; set; }
        public bool IsToResetPage
        {
            get { return _isToResetPage; }
            set { _isToResetPage = value; OnPropertyChanged(); }
        }
        private bool _isEnglish { get; set; }
        public bool IsEnglish
        {
            get { return _isEnglish; }
            set { _isEnglish = value; OnPropertyChanged(); }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand EditNameCM { get; set; }
        public ICommand EditEmailCM { get; set; }
        public ICommand ConfirmButtonCM { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand ChangePassCM { get; set; }
        public ICommand AutoStartAppCM { get; set; }
        public ICommand RemindLoginAppCM { get; set; }
        public ICommand ColorPickerCM { get; set; }
        public ICommand CLoseColorPickerCM { get; set; }
        public ICommand ChooseColorCM { get; set; }
        public ICommand ConfirmCurrentPassCM { get; set; }
        public ICommand CloseResetPassCM { get; set; }
        public ICommand ChooseLanguageCM { get; set; }

        public SettingStaffVM()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsEnglish = Properties.Settings.Default.isEnglish;
                reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                currentStaff = StaffVM.currentStaff;
                StaffName = currentStaff.StaffName;
                StaffEmail = currentStaff.Email;
                Position = currentStaff.Position;
                if (reg.GetValue("CinemaManagementApp") == null)
                    IsCheckedAutoStart = false;
                else
                    IsCheckedAutoStart = true;
                CheckedRemindLogin();
                
                if (currentStaff.Avatar != null)
                    ImageSource = LoadAvatarImage(currentStaff.Avatar);
                else
                    ImageSource = null;
                ColorPicked = (SolidColorBrush)new BrushConverter().ConvertFrom(Properties.Settings.Default.MainAppColor);
                IsToResetPage = false;
                IsEditEmail = false;
                IconEditEmail = PackIconKind.Pencil;
                SetAvatarName(StaffName);
                IsEdit = false;
            });
            EditNameCM = new RelayCommand<MaterialDesignThemes.Wpf.PackIcon>((p) => { return true; }, async (p) =>
            { 
                if(string.IsNullOrEmpty(StaffName))
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Do not leave the blank name!" : "Không được để tên trống!", IsEnglish ? "Warning" : "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);

                    return;
                }
                if(IsEdit == false)
                {
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentSaveEdit;
                    IsEdit = true;
                }
                else
                {
                    (bool isSuccessEdit, string messageReturn) = await Task.Run(() => SettingService.Ins.EditName(StaffName, currentStaff.Id));
                    if(isSuccessEdit == false)
                    {
                        CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Error" : "Lỗi", "OK", IsEnglish ? "Cancel" : "Hủy", Views.CustomMessageBoxImage.Error);
                        return;
                    }
                    currentStaff.StaffName = StaffName;
                    CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Success" : "Thành công", "OK", IsEnglish ? "Cancel" : "Hủy", Views.CustomMessageBoxImage.Success);
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pencil;
                    IsEdit = false;
                }
            });
            EditEmailCM = new RelayCommand<MaterialDesignThemes.Wpf.PackIcon>((p) => { return true; }, async (p) =>
            { 
                if (IsEditEmail == false)
                {
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentSaveEdit;
                    IconEditEmail = PackIconKind.ContentSaveEdit;
                    IsEditEmail = true;
                }
                else
                {
                    if (!EmailFormat.IsValidEmail(StaffEmail))
                    {
                        CustomMessageBox.ShowOkCancel(IsEnglish ? "Invalid Email" : "Email không đúng", IsEnglish ? "Error" : "Lỗi", "Ok", IsEnglish ? "Cancel" : "Hủy", Views.CustomMessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Random randomNumber = new Random();
                        randomCode = randomNumber.Next(111111, 999999);
                        SendMailToStaff(StaffEmail, randomCode);

                        confirmWD = new ConfirmStaffWindow();
                        confirmWD.ShowDialog();
                    }
                    
                }
            });
            ConfirmButtonCM = new RelayCommand<Label>((p) => { return true; },async (p) =>
            {
                if (CurrentCode == randomCode.ToString())
                {
                    if (string.IsNullOrEmpty(StaffName))
                    {
                        CustomMessageBox.ShowOk(IsEnglish ? "Do not leave the blank name!" : "Không được để tên trống!", IsEnglish ? "Warning" : "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                        return;
                    }
                    (bool isSuccessEdit, string messageReturn) = await Task.Run(() => SettingService.Ins.EditEmail(StaffEmail, currentStaff.Id));
                    if (isSuccessEdit == false)
                    {
                        CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Error" : "Lỗi", "OK", IsEnglish ? "Cancel" : "Hủy", Views.CustomMessageBoxImage.Error);
                        return;
                    }
                    currentStaff.Email = StaffEmail;
                    CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Success" : "Thành công", "OK", IsEnglish ? "Cancel" : "Hủy", Views.CustomMessageBoxImage.Success);
                    IconEditEmail = PackIconKind.Pencil;
                    confirmWD.Close();
                    IsEditEmail = false;

                }
                else
                    Error = IsEnglish ? "This code is invalid!" : "Mã code vừa nhập chưa chính xác!";
            });
            UploadImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == true)
                {
                    filePath = openfile.FileName;
                    LoadImage();
                    StaffWindow tk = Application.Current.Windows.OfType<StaffWindow>().FirstOrDefault();
                    tk.IconAvatar.ImageSource = ImageSource;
                    tk.IconAvatarPopup.ImageSource = ImageSource;
                    
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        Staff updateStaff = context.Staffs.FirstOrDefault(x => x.Id == currentStaff.Id);

                        if (updateStaff == null)
                            return;
                        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        byte[] photo_aray = new byte[fs.Length];
                        fs.Read(photo_aray, 0, photo_aray.Length);
                        updateStaff.Avatar = photo_aray;
                        context.SaveChanges();
                        CustomMessageBox.ShowOk(IsEnglish ? "Update successful" : "Cập nhật thành công", IsEnglish ? "Notice" : "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    }
                }
            });
            ChangePassCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsToResetPage = false;
                ResetPassStaffWindow resetPassWD = new ResetPassStaffWindow();
                resetPassWD.ShowDialog();
            });
            ConfirmNewPassCM = new RelayCommand<PasswordBox>((p) => { return true; }, async (p) =>
            {
                try
                {
                    using (var db = new CinemaManagementProjectEntities())
                    {
                        Staff updateStaff = await db.Staffs.FirstOrDefaultAsync(x => x.Id == currentStaff.Id);

                        if (updateStaff == null)
                            return;

                        updateStaff.UserPass = p.Password;
                        await db.SaveChangesAsync();
                        CustomMessageBox.ShowOk(IsEnglish ? "Update successful" : "Cập nhật thành công", IsEnglish ? "Notice" : "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    }
                }
                catch(EntityException)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Lost database connection" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                }
            });
            AutoStartAppCM = new RelayCommand<ToggleButton>((p) => { return true; }, (p) =>
            {
                if(p.IsChecked == true)
                    reg.SetValue("CinemaManagementApp", System.Reflection.Assembly.GetExecutingAssembly().Location);
                else
                    reg.DeleteValue("CinemaManagementApp");
            });
            RemindLoginAppCM = new RelayCommand<ToggleButton>((p) => { return true; }, (p) =>
            {
                if (p.IsChecked == true)
                {
                    Properties.Settings.Default.userNameSetting = currentStaff.UserName;
                    Properties.Settings.Default.userPassSetting = currentStaff.UserPass;
                    Properties.Settings.Default.isRemidUserAndPass = true;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.userNameSetting = string.Empty;
                    Properties.Settings.Default.userPassSetting = string.Empty;
                    Properties.Settings.Default.isRemidUserAndPass = false;
                    Properties.Settings.Default.Save();
                }
            });
            ColorPickerCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = Visibility.Visible;
            });
            CLoseColorPickerCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = Visibility.Collapsed;
            });
            ChooseColorCM = new RelayCommand<Rectangle>((p) => { return true; }, (p) =>
            {
                StaffWindow tk = Application.Current.Windows.OfType<StaffWindow>().FirstOrDefault();
                ColorPicked = p.Fill;
                tk.Overlay.Fill = p.Fill;
                SolidColorBrush solidColorBrush = (SolidColorBrush)ColorPicked;
                Properties.Settings.Default.MainAppColor = solidColorBrush.Color.ToString();
                Properties.Settings.Default.Save();
            });
            ConfirmCurrentPassCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                if (p.Password == currentStaff.UserPass)
                {
                    IsToResetPage = true;
                    Error = string.Empty;
                }
                else
                    Error = IsEnglish ? "Wrong password" : "Sai mật khẩu";
            });
            CloseResetPassCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
                IsToResetPage = false;
                Error = string.Empty;
            });
            ChooseLanguageCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                if (p.SelectedItem == null)
                    return;
                bool isEN = !p.Text.Equals("English") ? true : false;
                Properties.Settings.Default.isEnglish = isEN;
                Properties.Settings.Default.Save();
                LanguageManager.SetLanguageDictionary(isEN ? LanguageManager.ELanguage.English : LanguageManager.ELanguage.VietNamese);
            });
        }
        public void SetAvatarName(string staffName)
        {
            string[] trimNames = staffName.Split(' ');
            AvatarName = trimNames[trimNames.Length - 1][0].ToString() + trimNames[0][0].ToString();
        }
        public void SendMailToStaff(string staffEmail, int randomCode)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string APP_EMAIL = appSettings["APP_EMAIL"];
                string APP_PASSWORD = appSettings["APP_PASSWORD"];

                //Tạo mail
                MailMessage mail = new MailMessage(APP_EMAIL, staffEmail);
                mail.To.Add(staffEmail);
                mail.Subject = IsEnglish ? "Verification of user accounts" : "Xác minh tài khoản người dùng";
                //Attach file
                mail.IsBodyHtml = true;

                string htmlBody;

                htmlBody = GetHTMLTemplate(randomCode.ToString());

                mail.Body = htmlBody;

                //tạo Server

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(APP_EMAIL, APP_PASSWORD);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
            }
        }
        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;
        }
        private void CheckedRemindLogin()
        {
            if(currentStaff.UserName == Properties.Settings.Default.userNameSetting)
                if (Properties.Settings.Default.isRemidUserAndPass)
                    IsCheckedRemindLogin = true;
                else
                    IsCheckedRemindLogin = false;
            else
                IsCheckedRemindLogin = false;
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
        public string GetHTMLTemplate(string ConfirmCode)
        {
            string resetPasswordTemplate = Helper.GetEmailTemplatePath(RESET_PASSWORD_FILE);
            string HTML = File.ReadAllText(resetPasswordTemplate).Replace("{RESET_PASSWORD_CODE}", ConfirmCode);
            return HTML;
        }
        const string RESET_PASSWORD_FILE = "EmailResetPassTemplate.txt";
    }
}
