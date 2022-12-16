using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
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
        public async Task EditStaff(Window p)
        {
            MatKhau = SelectedItem.UserPass;

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
                        CustomMessageBox.ShowOk("Email không hợp lệ", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                        return;
                    }
                }
            }

            (bool isValid, string error) = IsValidData(Utils.Operation.UPDATE);
            if (isValid)
            {
                StaffDTO staff = new StaffDTO();
                staff.Id = SelectedItem.Id;
                staff.StaffName = Fullname;
                staff.Gender = Gender.Content.ToString();
                staff.DateOfBirth = Born;
                staff.PhoneNumber = Phone;
                staff.Position = Role.Content.ToString();
                staff.StartDate = StartDate;
                staff.UserName = TaiKhoan;
                staff.Email = Mail;
                (bool successUpdateStaff, string messageFromUpdateStaff) = await StaffService.Ins.UpdateStaff(staff);

                if (successUpdateStaff)
                {
                    LoadStaffListView(Utils.Operation.UPDATE, staff);
                    p.Close();
                    CustomMessageBox.ShowOk(messageFromUpdateStaff, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdateStaff, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk(error, "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
            }
        }
    }
}
