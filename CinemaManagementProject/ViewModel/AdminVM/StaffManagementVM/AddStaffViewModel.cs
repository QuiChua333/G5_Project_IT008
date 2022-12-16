using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagementProject.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel : BaseViewModel
    {
        public async Task AddStaff(Window p)
        {
            if (Mail != null)
            {
                if (Mail.Trim() == "")
                {
                    Mail = null;
                }
                else
                {
                    if (!Utils.RegexUtilities.IsValidEmail(Mail))
                    {
                        CustomMessageBox.ShowOk("Email không hợp lệ","Cảnh báo","OK", Views.CustomMessageBoxImage.Warning);
                        return;
                    }
                }
            }

            (bool isValid, string error) = IsValidData(Operation.CREATE);
            if (isValid)
            {
                StaffDTO staff = new StaffDTO();
                staff.StaffName = Fullname;
                staff.Gender = Gender.Content.ToString();
                staff.DateOfBirth = Born;
                staff.PhoneNumber = Phone;
                staff.Position = Role.Content.ToString();
                staff.StartDate = StartDate;
                staff.UserName = TaiKhoan;
                staff.UserPass = MatKhau;
                staff.Email = Mail;

                (bool successAddStaff, string messageFromAddStaff, StaffDTO newStaff) = await StaffService.Ins.AddStaff(staff);

                if (successAddStaff)
                {
                    LoadStaffListView(Operation.CREATE, newStaff);
                    p.Close();
                    CustomMessageBox.ShowOk(messageFromAddStaff, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromAddStaff, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }

            }
            else
            {
                CustomMessageBox.ShowOk(error, "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
            }
        }
        private (bool, string) ValidateAge(DateTime birthDate)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = StartDate.Value.Year - birthDate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthDate.DayOfYear > today.DayOfYear) age--;

            if (age < 18) return (false, "Nhân viên chưa đủ 18 tuổi!");
            return (true, null);
        }
    }
}
