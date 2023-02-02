using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using System.Windows.Forms;
using System.Data;
using System.Windows.Media.Media3D;
using CinemaManagementProject.View.Admin.ShowtimeManagement;
using MaterialDesignThemes.Wpf;

namespace CinemaManagementProject.ViewModel.AdminVM.ShowtimeManagementVM
{
    public partial class ShowtimeMangementViewModel : BaseViewModel
    {
        private DateTime _EndTime;
        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; OnPropertyChanged(); }
        }
        public ICommand LoadAddShowtimeCM { get; set; }
        public ICommand SaveCM { get; set; }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }
        public async Task SaveShowtimeFunc(Window p)
        {
            if (IsValidData())
            {
                if(Showtime.TimeOfDay < DateTime.Now.TimeOfDay)
                {
                    CustomMessageBox.ShowOk("Vui lòng chọn thời gian chiếu sau " + DateTime.Now.TimeOfDay, "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                    return;
                }
                ShowtimeDTO temp = new ShowtimeDTO
                {               
                    FilmId = movieSelected.Id,
                    RoomId = ShowtimeRoom.Id,
                    ShowDate = showtimeDate,
                    StartTime = Showtime.TimeOfDay,
                    Price = moviePrice,
                    //IsDeleted = false,
                };

                (bool IsSuccess, string message) = await ShowtimeService.Ins.AddShowtime(temp);


                if (IsSuccess)
                {
                    IsSaving = false;
                    CustomMessageBox.ShowOk(message, "Thông báo","OK",Views.CustomMessageBoxImage.Success );

                    p.Close();
                    //ShadowMask.Visibility = Visibility.Collapsed;

                    await ReloadShowtimeList(-1);
                    GetShowingMovieByRoomInDate(SelectedRoomId);
                }
                else
                {
                    CustomMessageBox.ShowOk(message, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Cảnh báo", "Vui lòng nhập đầy đủ thông tin!", "OK", Views.CustomMessageBoxImage.Warning);
            }
        }
        public void CalculateRunningTime()
        {
            if (movieSelected != null)
            {
                EndTime = Showtime.AddMinutes((double)movieSelected.DurationFilm);
            }
        }
        public void RenewData()
        {
            movieSelected = null;
            showtimeDate = GetCurrentDate;
            ShowtimeRoom = null;
            Showtime = new DateTime();
            EndTime = new DateTime();
            moviePrice = 45000;
        }
    }
}
