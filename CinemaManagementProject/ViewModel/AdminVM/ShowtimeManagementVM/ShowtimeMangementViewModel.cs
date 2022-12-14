using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CinemaManagementProject.View.Admin.ShowtimeManagement;
using CinemaManagementProject.Model.Service;

namespace CinemaManagementProject.ViewModel.AdminVM.ShowtimeManagementVM
{
    public partial class ShowtimeMangementViewModel:BaseViewModel
    {
            public static Grid ShadowMask { get; set; }

            private Label resultLabel;
            public Label ResultLabel
            {
                get { return resultLabel; }
                set { resultLabel = value; OnPropertyChanged(); }
            }

            private bool isLoading;
            public bool IsLoading
            {
                get { return isLoading; }
                set { isLoading = value; OnPropertyChanged(); }
            }

            // this is for  binding data
            private MovieDTO _movieSelected;
            public MovieDTO movieSelected
            {
                get { return _movieSelected; }
                set { _movieSelected = value; OnPropertyChanged(); }
            }

            private DateTime _showtimeDate;
            public DateTime showtimeDate
            {
                get { return _showtimeDate; }
                set { _showtimeDate = value; OnPropertyChanged(); }
            }

            private DateTime _Showtime;
            public DateTime Showtime
            {
                get { return _Showtime; }
                set
                {
                    _Showtime = value; OnPropertyChanged(); CalculateRunningTime();
            }
            }

            private RoomDTO _ShowtimeRoom;
            public RoomDTO ShowtimeRoom
            {
                get { return _ShowtimeRoom; }
                set { _ShowtimeRoom = value; OnPropertyChanged(); }
            }

            private float _moviePrice;
            public float moviePrice
            {
                get { return _moviePrice; }
                set { _moviePrice = value; OnPropertyChanged(); }
            }

            private List<MovieDTO> listMovieAllRoom = new List<MovieDTO>();



            private ObservableCollection<MovieDTO> _showtimeList; // this is  for the main listview
            public ObservableCollection<MovieDTO> ShowtimeList
            {
                get { return _showtimeList; }
                set { _showtimeList = value; OnPropertyChanged(); }
            }


            private ObservableCollection<MovieDTO> _movieList; // for adding showtime
            public ObservableCollection<MovieDTO> MovieList
            {
                get => _movieList;
                set
                {
                    _movieList = value;
                    OnPropertyChanged();
                }
            }

            private List<RoomDTO> _ListRoom;    // for adding showtime
            public List<RoomDTO> ListRoom
            {
                get { return _ListRoom; }
                set { _ListRoom = value; OnPropertyChanged(); }
            }



            private DateTime _getCurrentDate;
            public DateTime GetCurrentDate
            {
                get { return _getCurrentDate; }
                set { _getCurrentDate = value; }
            }
            private string _setCurrentDate;
            public string SetCurrentDate
            {
                get { return _setCurrentDate; }
                set { _setCurrentDate = value; }
            }


            private DateTime _SelectedDate;  //  changing the listview when select day
            public DateTime SelectedDate
            {
                get { return _SelectedDate; }
                set { _SelectedDate = value; OnPropertyChanged(); }
            }

            private MovieDTO _selectedItem; //the item being selected
            public MovieDTO SelectedItem
            {
                get { return _selectedItem; }
                set { _selectedItem = value; OnPropertyChanged(); }
            }

            private MovieDTO _oldselectedItem; //the item being selected
            public MovieDTO oldSelectedItem
            {
                get { return _oldselectedItem; }
                set { _oldselectedItem = value; OnPropertyChanged(); }
            }



            public int SelectedRoomId = -1;


            public ICommand ChangedRoomCM { get; set; }

            public ICommand LoadDeleteShowtimeCM { get; set; }
            public ICommand MaskNameCM { get; set; }
            public ICommand FirstLoadCM { get; set; }
            public ICommand CalculateRunningTimeCM { get; set; }
            public ICommand SelectedDateCM { get; set; }
            public ICommand SaveResultNameCM { get; set; }

        public ShowtimeMangementViewModel()
        {
            CalculateRunningTimeCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                CalculateRunningTime();
            });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                GetCurrentDate = DateTime.Today;
                SelectedDate = GetCurrentDate;
                showtimeDate = GetCurrentDate;
                await ReloadShowtimeList();
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, async (p) =>
            {
                ShadowMask = p;
                await ReloadShowtimeList();
            });
            LoadAddShowtimeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                GenerateListRoom();
                RenewData();
                AddShowtimeWindow temp = new AddShowtimeWindow();

                try
                {
                    IsLoading = true;
                    MovieList = new ObservableCollection<MovieDTO>(await MovieService.Ins.GetAllMovie());
                    IsLoading = false;
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi","OK",Views.CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
                temp.ShowDialog();
            });
            SaveCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                IsSaving = true;

                await SaveShowtimeFunc(p);

                IsSaving = false;

            });
            LoadDeleteShowtimeCM = new RelayCommand<ListBox>((p) => { if (SelectedShowtime is null) return false; return true; }, async (p) =>
            {
                string message = "Bạn có chắc muốn xoá suất chiếu này không?";

                //Kiểm tra suất chiếu đã có người đặt ghế nào chưa để có thông báo phù hợp
                bool isShowHaveBooking = await ShowtimeService.Ins.CheckShowtimeHaveBooking(SelectedShowtime.Id);
                if (isShowHaveBooking)
                {
                    message = $"Suất chiếu này có ghế đã được đặt. Bạn có muốn xoá không?";
                    var kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Yes","No", Views.CustomMessageBoxImage.Warning);
                    if (kq== Views.CustomMessageBoxResult.None)
                    {
                        return;
                    }
                }
                else
                {
                    var kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Yes", "No", Views.CustomMessageBoxImage.Warning);
                    if (kq==Views.CustomMessageBoxResult.None)
                    {
                        return;
                    }
                }

                int showtimeId = SelectedShowtime.Id;
                (bool deleteSuccess, string messageFromDelete) = await ShowtimeService.Ins.DeleteShowtime(SelectedShowtime.Id);
                if (deleteSuccess)
                {
                    for (int i = 0; i < ListShowtimeofMovie.Count; i++)
                    {
                        if (ListShowtimeofMovie[i].Id == showtimeId)
                            ListShowtimeofMovie.RemoveAt(i);
                    }
                    oldSelectedItem = SelectedItem;
                    SelectedShowtime = null;
                    CustomMessageBox.ShowOk(messageFromDelete, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);

                    await ReloadShowtimeList(-1);
                    GetShowingMovieByRoomInDate(SelectedRoomId);
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromDelete, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            });
            ChangedRoomCM = new RelayCommand<RadioButton>((p) => { return true; }, async (p) =>
            {
                switch (p.Name)
                {
                    case "all":
                        {
                            SelectedRoomId = -1;
                            GetShowingMovieByRoomInDate(SelectedRoomId);
                            break;
                        }
                    case "r1":
                        {
                            SelectedRoomId = 1;
                            await ReloadShowtimeList(1);
                            break;
                        }
                    case "r2":
                        {
                            SelectedRoomId = 2;
                            await ReloadShowtimeList(2);
                            break;
                        }
                    case "r3":
                        {
                            SelectedRoomId = 3;
                            await ReloadShowtimeList(3);
                            break;
                        }
                    case "r4":
                        {
                            SelectedRoomId = 4;
                            await ReloadShowtimeList(4);
                            break;
                        }
                    case "r5":
                        {
                            SelectedRoomId = 5;
                            await ReloadShowtimeList(5);
                            break;
                        }
                }
            });
            LoadInfor_EditShowtime = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Infor_EditFunc();
            });
            CloseEditCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                //ShadowMask.Visibility = Visibility.Collapsed;
                p.Close();
                SelectedShowtime = null;
            });
            LoadSeatCM = new RelayCommand<ListBox>((p) => { return true; }, async (p) =>
            {
                if (SelectedShowtime != null)
                {
                    await GenerateSeat();
                    if (SelectedShowtime.Price.ToString().Length > 5)
                        moviePrice = float.Parse(SelectedShowtime.Price.ToString()/*.Remove(5, 5)*/);
                    else
                        moviePrice = float.Parse(SelectedShowtime.Price.ToString());
                }

            });
            EditPriceCM = new RelayCommand<Label>((p) => { return true; }, async (p) =>
            {
                if (SelectedShowtime is null) return;
                if (p.Content.ToString() == "Lưu") return;

                (bool IsSuccess, string message) = await ShowtimeService.Ins.UpdateTicketPrice(SelectedShowtime.Id, moviePrice);

                if (IsSuccess)
                {
                    SelectedShowtime.Price = moviePrice;
                    CustomMessageBox.ShowOk(message, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(message, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            });
            SelectedDateCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ReloadShowtimeList(-1);
                GetShowingMovieByRoomInDate(SelectedRoomId);
            });
            SaveResultNameCM = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                ResultLabel = p;
            });
        }


        public async Task ReloadShowtimeList(int id = -1)
        {

            if (id != -1)
            {
                try
                {
                    GetShowingMovieByRoomInDate(id);
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
                catch (Exception)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    ShowtimeList = new ObservableCollection<MovieDTO>();
                    IsLoading = true;

                    listMovieAllRoom = await Task.Run(() => MovieService.Ins.GetShowingMovieByDay(SelectedDate));

                    ShowtimeList = new ObservableCollection<MovieDTO>(listMovieAllRoom);
                    IsLoading = false;
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
                catch (Exception)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            //ResultLabel.Content = ShowtimeList.Count;
        }
        private void GetShowingMovieByRoomInDate(int roomId)
        {
            if (roomId == -1)
            {
                ShowtimeList = new ObservableCollection<MovieDTO>(listMovieAllRoom);
                //ResultLabel.Content = ShowtimeList.Count;
                return;
            }
            List<MovieDTO> listMoviesByRoom = listMovieAllRoom.Where(m => m.ShowTimes.Any(s => s.RoomId == roomId )).Select(m => new MovieDTO
            {
                Id = m.Id,
                FilmName = m.FilmName,
                Duration = m.Duration,
                Country = m.Country,
                FilmInfo = m.FilmInfo,
                ReleaseDate = m.ReleaseDate,
                FilmType = m.FilmType,
                Author = m.Author,
                //Image = m.Image,
                //Genres = new List<GenreDTO>(m.Genres),
                ShowTimes = m.ShowTimes.Where(s => s.RoomId == roomId ).ToList()
            }).ToList();
            ShowtimeList = new ObservableCollection<MovieDTO>(listMoviesByRoom);
        }
        public void GenerateListRoom()
        {
            ListRoom = new List<RoomDTO>();
            for (int i = 0; i <= 4; i++)
            {
                RoomDTO temp = new RoomDTO
                {
                    Id = i + 1,
                };
                ListRoom.Add(temp);
            }
        }
        public bool IsValidData()
        {
            return movieSelected != null
                && !string.IsNullOrEmpty(showtimeDate.ToString())
                && !string.IsNullOrEmpty(Showtime.ToString())
                && ShowtimeRoom != null && !(moviePrice == 0);
        }
    }
}



