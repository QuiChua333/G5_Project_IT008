using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Staff.MovieScheduleWindow;
using CinemaManagementProject.ViewModel.StaffVM.MovieScheduleWindowVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private ObservableCollection<string> _currentGenreSource;
        public ObservableCollection<string> CurrentGenreSource
        {
            get => _currentGenreSource;
            set
            {
                _currentGenreSource = value;
                OnPropertyChanged();
            }
        }
        
        private string _selectedItemFilter;
        public string SelectedItemFilter
        {
            get => _selectedItemFilter;
            set
            {
                _selectedItemFilter = value;
                OnPropertyChanged();
            }
        }
        //
        
        private FilmDTO _selectedItem;
        public FilmDTO SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
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
        public ICommand SelectedDateChangedCM { get; set; }
        public ICommand SelectedGenreChanged { get; set; }
        public ICommand OpenBuyTicketWindow { get; set; }

        public FilmBookingVM()
        {
            FirstLoadCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                SelectedDate = DateTime.Now;
                FilmShowTimeList = new ObservableCollection<FilmDTO>();
                try
                {
                    FilmShowTimeList = new ObservableCollection<FilmDTO>(await Task.Run(() => FilmService.Ins.GetShowingMovieByDay(SelectedDate)));
                    GetAllCurrentGenre(p);
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
            SelectedDateChangedCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                if(SelectedDate != null)
                {
                    FilmShowTimeList = new ObservableCollection<FilmDTO>();
                    try
                    {
                        FilmShowTimeList = new ObservableCollection<FilmDTO>(await Task.Run(() => FilmService.Ins.GetShowingMovieByDay(SelectedDate)));
                        GetAllCurrentGenre(p);
                    }
                    catch (System.Data.Entity.Core.EntityException)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "Ok");
                    }
                    catch (Exception)
                    {
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok");
                    }
                }    
            });
            SelectedGenreChanged = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                if (SelectedDate != null)
                {
                    if(SelectedItemFilter != null)
                    {
                        FilmShowTimeList = new ObservableCollection<FilmDTO>();
                        try
                        {
                            FilmShowTimeList = new ObservableCollection<FilmDTO>(await Task.Run(() => FilmService.Ins.GetShowingMovieByDay(SelectedDate)));
                            SelectFilmByFilter();
                        }
                        catch (System.Data.Entity.Core.EntityException)
                        {
                            CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "Ok");
                        }
                        catch (Exception)
                        {
                            CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok");
                        }
                    }
                }
            });
            OpenBuyTicketWindow = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                //Ngày chiếu phim _ShowDate
                //Lấy hết giờ chiếu _ShowTimeList
                //Phòng chiếu _Room
                // Lấy hết phim _ImgFilm _TxtFilm
                MovieScheduleWindow wd;
                if (SelectedItem != null)
                {
                    try
                    {
                        MovieScheduleWindowViewModel.tempFilmbinding = SelectedItem;
                        wd = new MovieScheduleWindow();

                        if (wd != null)
                        {
                            if (SelectedItem != null)
                            {
                                wd._ShowTimeList.ItemsSource = SelectedItem.ShowTimes;
                                wd._ImgFilm.ImageSource = await CloudinaryService.Ins.LoadImageFromURL(SelectedItem.Image);
                                wd._ShowDate.Content = SelectedDate.ToString("dd-MM-yyyy");
                                wd._TxtFilm.Content = SelectedItem?.FilmName ?? "";
                                wd.ShowDialog();
                            }
                        }
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                }
            });
        }
        public void GetAllCurrentGenre(ComboBox filter)
        {
            CurrentGenreSource = new ObservableCollection<string>();
            CurrentGenreSource.Add("Tất cả");
            for (int i = 0; i < FilmShowTimeList.Count; i++)
            {
                if (!CurrentGenreSource.Contains(FilmShowTimeList[i].Genre))
                {
                    CurrentGenreSource.Add(FilmShowTimeList[i].Genre);
                }    
            }
        }
        public void SelectFilmByFilter()
        {
            ObservableCollection<FilmDTO> temp = new ObservableCollection<FilmDTO>();
            string SelectedFilterText = SelectedItemFilter.ToString();
            if(SelectedFilterText == "Tất cả")
            {
                return;
            }   
            else
            {
                for (int i = 0; i < FilmShowTimeList.Count; i++)
                {
                    if (FilmShowTimeList[i].Genre == SelectedFilterText)
                    {
                        temp.Add(FilmShowTimeList[i]);
                    }    
                }
                FilmShowTimeList = temp;
            }    
        }
    }
}
