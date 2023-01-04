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
        public ICommand LoadEditTroubleWindowCM { get; set; }
        public ICommand UpdateTroubleWindowCM { get; set; }

        private int _troubleID;
        public int TroubleID
        {
            get { return _troubleID; }
            set { _troubleID = value; }
        }

        public async void LoadEditTrouble(EditTroubleWindow w1)
        {
            IsImageChanged = false;
            TroubleType = SelectedItem.TroubleType;
            w1.staffName.Text = SelectedItem.StaffName;
            w1.cbxStatusError.Text = SelectedItem.TroubleStatus;
            w1.submitdate.Text =( (DateTime)SelectedItem.SubmittedAt).ToShortDateString();
            if (Properties.Settings.Default.isEnglish == false)
            {
                if (SelectedItem.Level == "Bình thường") w1.cbblevel.Text = "Bình thường";
                if (SelectedItem.Level == "Nghiêm trọng") w1.cbblevel.Text = "Nghiêm trọng";
            }
            else
            {
                if (SelectedItem.Level == "Bình thường") w1.cbblevel.Text = "Normal";
                if (SelectedItem.Level == "Nghiêm trọng") w1.cbblevel.Text = "Serious";
            }

            Description = SelectedItem.Description;
            TroubleID = SelectedItem.Id;

            ImageSource = await CloudinaryService.Ins.LoadImageFromURL(SelectedItem.Image); ;
        }
        public async Task UpdateTroubleFunc(EditTroubleWindow p)
        {
            if (TroubleID.ToString() != null && IsValidData())
            {

                TroubleDTO tb = new TroubleDTO
                {
                    Id = TroubleID,
                    TroubleType = TroubleType,
                    Level = Level.Tag.ToString(),
                    Description = Description,
                    StaffId = StaffVM.currentStaff.Id,
                };

                if (IsImageChanged)
                {
                    Task<string> uploadImage = CloudinaryService.Ins.UploadImage(filepath);
                    if (SelectedItem.Image != null)
                    {
                        await CloudinaryService.Ins.DeleteImage(SelectedItem.Image);
                    }

                    tb.Image = await uploadImage;

                    if (tb.Image is null)
                    {
                        if (Properties.Settings.Default.isEnglish == false)
                            CustomMessageBox.ShowOk("Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại", "Thông báo", "OK", Views.CustomMessageBoxImage.Error);
                        else CustomMessageBox.ShowOk("An error occurred while saving the image. Please try again", "Notify", "OK", Views.CustomMessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    tb.Image = SelectedItem.Image;
                }

                (bool successUpdateTB, string messageFromUpdateTB) = await TroubleService.Ins.UpdateTroubleInfo(tb);

                if (successUpdateTB)
                {
                    isSaving = false;
                    if (Properties.Settings.Default.isEnglish == false)
                        CustomMessageBox.ShowOk("Cập nhật thành công!", "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    else CustomMessageBox.ShowOk("Update successful!", "Notify", "OK", Views.CustomMessageBoxImage.Success);
                    await GetData();

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
