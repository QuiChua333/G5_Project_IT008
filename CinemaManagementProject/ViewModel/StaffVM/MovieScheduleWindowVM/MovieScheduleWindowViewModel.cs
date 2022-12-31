using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CinemaManagementProject.ViewModel.StaffVM.TicketVM;
using CinemaManagementProject.View.Staff.TicketWindow;

namespace CinemaManagementProject.ViewModel.StaffVM.MovieScheduleWindowVM
{
    public class MovieScheduleWindowViewModel : BaseViewModel
    {
        #region
        public ICommand VisibleSeat { get; set; }

        private List<ShowtimeDTO> _ListShowtime;
        public List<ShowtimeDTO> ListShowtime
        {
            get { return _ListShowtime; }
            set { _ListShowtime = value; OnPropertyChanged(); }
        }


        private ShowtimeDTO _selectedShowtime;

        public ShowtimeDTO SelectedShowtime
        {
            get { return _selectedShowtime; }
            set { _selectedShowtime = value; OnPropertyChanged(); GetShowtimeRoom(); }
        }

        private string _ShowTimeRoom;
        public string ShowTimeRoom  
        {
            get { return _ShowTimeRoom; }
            set { _ShowTimeRoom = value; OnPropertyChanged(); }
        }

        public static FilmDTO tempFilmbinding;
        #endregion
        public MovieScheduleWindowViewModel()
        {
            VisibleSeat = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (ShowTimeRoom != null)
                {
                    TicketWindowViewModel.CurrentShowtime = SelectedShowtime;
                    TicketWindowViewModel.tempFilm = tempFilmbinding;
                    TicketWindowViewModel.showTimeRoom = ShowTimeRoom;
                    TicketWindow w = new TicketWindow();
                    w.ShowDialog();
                }
            });
            

        }
        public void GetShowtimeRoom()
        {
            ShowTimeRoom = "Room 0" + SelectedShowtime.RoomId.ToString();
        }
    }

}
