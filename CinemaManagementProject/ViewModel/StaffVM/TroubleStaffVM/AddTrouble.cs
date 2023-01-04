using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CinemaManagementProject.View.Staff.Trouble;
using CinemaManagementProject.Utils;
using CinemaManagementProject.Views;

namespace CinemaManagementProject.ViewModel.StaffVM.TroubleStaffVM
{
    public partial class TroublePageViewModel : BaseViewModel
    {
        private DateTime getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return getCurrentDate; }
            set { getCurrentDate = value; OnPropertyChanged(); }
        }
        public ICommand SaveTroubleCM { get; set; }

        public async Task SaveTroubleFunc(AddTroubleWindow p)
        {
            if (filepath != null && IsValidData())
            {
                string troubleImage = await CloudinaryService.Ins.UploadImage(filepath);
                if (troubleImage is null)
                {
                    if (Properties.Settings.Default.isEnglish == false)
                        CustomMessageBox.ShowOk("Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại", "Thông báo", "OK", Views.CustomMessageBoxImage.Error);
                    else CustomMessageBox.ShowOk("An error occurred while saving the image. Please try again", "Notify", "OK", Views.CustomMessageBoxImage.Error);
                    return;
                }

                TroubleDTO trouble = new TroubleDTO
                {
                    TroubleType = TroubleType,
                    Level = Level.Tag.ToString(),
                    Description = Description,
                    Image = troubleImage,
                    StaffId = StaffVM.currentStaff.Id,
                    StaffName= StaffVM.currentStaff.StaffName,
                };

                (bool successAddtrouble, string messageFromAddtrouble, TroubleDTO newtrouble) = await TroubleService.Ins.CreateNewTrouble(trouble);

                if (successAddtrouble)
                {
                    IsSaving = false;
                    if (Properties.Settings.Default.isEnglish == false)
                        CustomMessageBox.ShowOk("Thêm sự cố thành công", "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    else CustomMessageBox.ShowOk("Add trouble successful", "Notify", "OK", Views.CustomMessageBoxImage.Success);
                    GetAllTrouble = new System.Collections.ObjectModel.ObservableCollection<TroubleDTO>(await TroubleService.Ins.GetAllTrouble());
                    TroubleList = new System.Collections.ObjectModel.ObservableCollection<TroubleDTO>(GetAllTrouble);
                    p.Close();
                }
                else
                {
                    if (Properties.Settings.Default.isEnglish == false)
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                    else CustomMessageBox.ShowOk("System Error", "Error", "OK", CustomMessageBoxImage.Error);
                }
            }
            else
            {
                if (Properties.Settings.Default.isEnglish == false)
                    CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                else CustomMessageBox.ShowOk("Please enter enough information!", "Warning", "OK", Views.CustomMessageBoxImage.Warning);
            }
        }
    }
}

