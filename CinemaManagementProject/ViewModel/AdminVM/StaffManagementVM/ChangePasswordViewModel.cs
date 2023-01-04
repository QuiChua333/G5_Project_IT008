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
        public async Task ChangePass(Window p)
        {

            (bool isValid, string error) = IsValidPassword(Utils.Operation.UPDATE_PASSWORD);

            if (isValid)
            {
                (bool updatePassSuccesss, string message) = await StaffService.Ins.UpdatePassword(SelectedItem.Id, MatKhau);
                if (updatePassSuccesss)
                {
                    p.Close();
                    CustomMessageBox.ShowOk(message, Properties.Settings.Default.isEnglish == false ? "Thông báo" : "Notify", "OK", Views.CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(message, Properties.Settings.Default.isEnglish == false ? "Lỗi" : "Error", "OK", Views.CustomMessageBoxImage.Error);
                }

            }
            else
            {
                CustomMessageBox.ShowOk(error, Properties.Settings.Default.isEnglish == false ? "Cảnh báo" : "Warning", "OK", Views.CustomMessageBoxImage.Warning );
            }
        }
        public (bool valid, string error) IsValidPassword(Operation oper)
        {
            if (oper == Operation.CREATE || oper == Operation.UPDATE_PASSWORD)
            {
                if (string.IsNullOrEmpty(MatKhau))
                {
                    return (false, Properties.Settings.Default.isEnglish == false ? "Vui lòng nhập mật khẩu": "Please enter a password");
                }
                if (MatKhau != RePass)
                    return (false, Properties.Settings.Default.isEnglish == false ? "Mật khẩu và mật khẩu nhập lại không trùng khớp!": "Password and re-entered password do not match!");
            }
            return (true, null);
        }
    }
}
