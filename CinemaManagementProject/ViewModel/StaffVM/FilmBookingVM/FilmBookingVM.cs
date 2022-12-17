using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagementProject.ViewModel.StaffVM.FilmBookingVM
{
    public class FilmBookingVM : BaseViewModel
    {
        private string _filmID;
        public string filmID
        {
            get { return _filmID; }
            set { _filmID = value; OnPropertyChanged(); }
        }
        //
        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }

        private string _filmName;
        public string filmName
        {
            get { return _filmName; }
            set { _filmName = value; OnPropertyChanged(); }
        }

        private string _filmGenre;
        public string filmGenre
        {
            get => _filmGenre;
            set { _filmGenre = value; OnPropertyChanged(); }
        }

        private string _filmDuration;
        public string filmDuration
        {
            get { return _filmDuration; }
            set { _filmDuration = value; OnPropertyChanged(); }
        }
        //
        private DateTime _releaseDate { get; set; }
        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; OnPropertyChanged(); }
        }
        //
        private ObservableCollection<FilmDTO> _filmShowTimeList;
        public ObservableCollection<FilmDTO> FilmShowTimeList
        {
            get => _filmShowTimeList;
            set
            {
                _filmShowTimeList = value;
                OnPropertyChanged();
            }
        }
        //
        private ObservableCollection<FilmDTO> _storeAllFilmShowTime;
        public ObservableCollection<FilmDTO> StoreAllFilmShowTime
        {
            get => _storeAllFilmShowTime;
            set
            {
                _storeAllFilmShowTime = value;
                OnPropertyChanged();
            }
        }
        //
        //
        //
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        //
        //
        //
        //Command
        public ICommand FirstLoadCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand LoadDeleteMovieCM { get; set; }

        public FilmBookingVM()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                FilmShowTimeList = new ObservableCollection<FilmDTO>();
                try
                {
                    FilmShowTimeList = new ObservableCollection<FilmDTO>(await Task.Run(() => FilmService.Ins.GetAllFilm()));
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "Ok");
                }
                catch (Exception)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok");
                }
            });
        }
    }
}
