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
            w1.staffname.Text = SelectedItem.StaffName;
            w1.cbxStatusError.Text = SelectedItem.TroubleStatus;
            w1.submitdate.Text =( (DateTime)SelectedItem.SubmittedAt).ToShortDateString();
            Level.Content = SelectedItem.Level;
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
                    Level = Level.Content.ToString(),
                    Description = Description,
                    //StaffId = MainStaffViewModel.CurrentStaff.Id,
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
                        CustomMessageBox.ShowOk("Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại", "Thông báo","OK",Views.CustomMessageBoxImage.Error);
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
                    CustomMessageBox.ShowOk("Cập nhật thành công!", "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                    await GetData();

                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Thông báo", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin", "Thông báo", "OK", Views.CustomMessageBoxImage.Warning);
            }
        }

    }
}
