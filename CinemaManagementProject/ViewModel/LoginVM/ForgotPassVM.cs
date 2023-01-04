using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using System.Net.PeerToPeer.Collaboration;
using System.IO;
using System.Net;
using CinemaManagementProject.Model;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;

namespace CinemaManagementProject.ViewModel.LoginVM
{
    public class ForgotPassVM :BaseViewModel
    {
        private string _currentEmail { get; set; }
        public string CurrentEmail
        {
            get { return _currentEmail; }
            set { _currentEmail = value; }
        }
        private string _currentCode { get; set; }
        public string CurrentCode
        {
            get { return _currentCode; }
            set { _currentCode = value; }
        }
        private int randomCode;

        //Variable used for ChangePassword page
        private string _newPassword { get; set; }
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; }
        }
        private bool isEN = Properties.Settings.Default.isEnglish;
        public ICommand LoginPageCM { get; set; }
        public ICommand SendEmailCM { get; set; }
        public ICommand CheckCodeCM { get; set; }
        public ICommand ConfirmNewPassCM { get; set; }
        public ForgotPassVM()
        {
            LoginPageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoginVM.MainFrame.Content = new LoginPage();
            });
            SendEmailCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if(CheckValidEmail(CurrentEmail, p))
                {
                    if (Helper.CheckEmailStaff(CurrentEmail))
                    {
                        Random randomNumber = new Random();
                        randomCode = randomNumber.Next(111111, 999999);
                        SendMailToStaff(CurrentEmail, randomCode);
                    }
                    else
                        p.Content = isEN ? "Not a company employee account!":"Không phải tài khoản nhân viên công ty!";
                }

            });
            CheckCodeCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (CurrentCode == randomCode.ToString())
                    LoginVM.MainFrame.Content = new ChangePassPage();
                else
                    p.Content = isEN? "The code has just enter is incorrect!" : "Mã code vừa nhập chưa chính xác!";
            });
            ConfirmNewPassCM = new RelayCommand<PasswordBox>((p) => { return true; }, async (p) =>
            {
                if (NewPassword == p.Password)
                {
                    using (var db = new CinemaManagementProjectEntities())
                    {
                        Staff updateStaff = await db.Staffs.FirstOrDefaultAsync(x => x.Email == CurrentEmail);

                        if (updateStaff == null)
                            return;

                        updateStaff.UserPass = NewPassword;
                        await db.SaveChangesAsync();
                    }

                    LoginVM.MainFrame.Content = new LoginPage();
                }
            });
        }
        public bool CheckValidEmail(string email, Label lableError)
        {
            if (string.IsNullOrEmpty(email))
            {
                lableError.Content = isEN? "Please enter full infomation" : "Vui lòng nhập đủ thông tin";
                return false;
            }
            if(!EmailFormat.IsValidEmail(email))
            {
                lableError.Content = isEN? "Please enter the right email address" : "Vui lòng nhập đúng địa chỉ Email";
                return false;
            }
            lableError.Content = "";
            return true;
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
                mail.Subject = isEN? "Regain the login password" : "Lấy lại mật khẩu đăng nhập";
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
                MessageBox.Show(ex.ToString());
            }
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
