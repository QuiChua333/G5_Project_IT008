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
using MaterialDesignThemes.Wpf;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

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
            private FilmDTO _movieSelected;
            public FilmDTO movieSelected
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

            private List<FilmDTO> listMovieAllRoom = new List<FilmDTO>();



            private ObservableCollection<FilmDTO> _showtimeList; // this is  for the main listview
            public ObservableCollection<FilmDTO> ShowtimeList
            {
                get { return _showtimeList; }
                set { _showtimeList = value; OnPropertyChanged(); }
            }


            private ObservableCollection<FilmDTO> _movieList; // for adding showtime
            public ObservableCollection<FilmDTO> MovieList
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

        private bool isEdit;
        public bool IsEdit
        {
            get { return isEdit; }
            set { isEdit = value; OnPropertyChanged(); }
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

            private FilmDTO _selectedItem; //the item being selected
            public FilmDTO SelectedItem
            {
                get { return _selectedItem; }
                set { _selectedItem = value; OnPropertyChanged(); }
            }

            private FilmDTO _oldselectedItem; //the item being selected
            public FilmDTO oldSelectedItem
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
        public ICommand LoadStatusSeatCM { get; set; }



        public ShowtimeMangementViewModel()
        {

            LoadStatusSeatCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                Label lb = p.Children.OfType<Label>().FirstOrDefault();
                Image img = p.Children.OfType<Image>().FirstOrDefault();
                if (lb != null)
                {
                    foreach (var item in ListSeat)
                    {
                        if (item.SeatPosition == lb.Content.ToString() && item.SeatStatus == true)
                        {
                            img.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/isBooked.png"));
                            lb.Content = "";
                            p.IsEnabled = false;
                        }
                        if (item.SeatPosition == lb.Content.ToString() && item.SeatStatus == false)
                        {
                            img.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/isReady.png"));
                        }

                    }
                    isBooked = ListSeat.Count(x => x.SeatStatus == true);
                    isReady = ListSeat.Count(x => x.SeatStatus == false);
                }
            });

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
                IsEdit = false;
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
                    MovieList = new ObservableCollection<FilmDTO>(await FilmService.Ins.GetAllFilm());
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
                    ListSeat1 = new ObservableCollection<SeatSettingDTO>();
                    ListSeat2 = new ObservableCollection<SeatSettingDTO>();

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
            EditPriceCM = new RelayCommand<MaterialDesignThemes.Wpf.PackIcon>((p) => { return true; }, async (p) =>
            {
                if (SelectedShowtime is null) return;
                IsEdit = !IsEdit;
                if (IsEdit)
                {
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentSaveCheckOutline;
                }
                else
                {
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pencil;
                }
                if (p.Kind == MaterialDesignThemes.Wpf.PackIconKind.ContentSaveCheckOutline) return;

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
                    ShowtimeList = new ObservableCollection<FilmDTO>();
                    IsLoading = true;

                    listMovieAllRoom = await Task.Run(() => FilmService.Ins.GetShowingMovieByDay(SelectedDate));

                    ShowtimeList = new ObservableCollection<FilmDTO>(listMovieAllRoom);
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
                ShowtimeList = new ObservableCollection<FilmDTO>(listMovieAllRoom);
                //ResultLabel.Content = ShowtimeList.Count;
                return;
            }
            List<FilmDTO> listMoviesByRoom = listMovieAllRoom.Where(m => m.ShowTimes.Any(s => s.RoomId == roomId )).Select(m => new FilmDTO
            {
                Id = m.Id,
                FilmName = m.FilmName,
                DurationFilm = m.DurationFilm,
                Country = m.Country,
                FilmInfor = m.FilmInfor,
                ReleaseDate = m.ReleaseDate,
                FilmType = m.FilmType,
                Author = m.Author,
                Image = m.Image,
                Genre = m.Genre,
                ShowTimes = m.ShowTimes.Where(s => s.RoomId == roomId ).ToList()
            }).ToList();
            ShowtimeList = new ObservableCollection<FilmDTO>(listMoviesByRoom);
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



