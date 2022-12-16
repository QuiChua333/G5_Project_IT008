using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.Model.Service;
using System.Windows.Media;
using System.Data.Entity.Validation;
using CinemaManagementProject.Model;
using CinemaManagementProject.View.Admin.MovieManagement;
using CinemaManagementProject.Utils;
using CinemaManagementProject.Views;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

namespace CinemaManagementProject.ViewModel.AdminVM.MovieManagementVM
{
    public partial class MovieManagementVM : BaseViewModel
    {
        private string _filmID;
        public string filmID
        {
            get { return _filmID; }
            set { _filmID = value; OnPropertyChanged(); }
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

        private string _filmDirector;
        public string filmDirector
        {
            get { return _filmDirector; }
            set { _filmDirector = value; OnPropertyChanged(); }
        }

        private string _filmCountry;
        public string filmCountry
        {
            get { return _filmCountry; }
            set { _filmCountry = value; OnPropertyChanged(); }
        }

        private string _filmDuration;
        public string filmDuration
        {
            get { return _filmDuration; }
            set { _filmDuration = value; OnPropertyChanged(); }
        }

        private string _filmDescribe;
        public string filmDescribe
        {
            get { return _filmDescribe; }
            set { _filmDescribe = value; OnPropertyChanged(); }
        }

        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }

        private string _filmYear;
        public string filmYear
        {
            get { return _filmYear; }
            set { _filmYear = value; OnPropertyChanged(); }
        }
        private string _filmType;
        public string filmType
        {
            get { return _filmType; }
            set { _filmType = value; OnPropertyChanged(); }
        }

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged(); }
        }

        private bool isloadding;
        public bool IsLoadding
        {
            get { return isloadding; }
            set { isloadding = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }

        private FilmDTO _selectedItem;
        public FilmDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        string filepath;
        bool IsImageChanged = false;

        public static Grid MaskName { get; set; }

        System.Windows.Controls.ListView MainListView;

        private List<string> _ListCountrySource;
        public List<string> ListCountrySource
        {
            get { return _ListCountrySource; }
            set { _ListCountrySource = value; OnPropertyChanged(); }
        }

        private List<string> _GenreList;
        public List<string> GenreList
        {
            get => _GenreList;
            set
            {
                _GenreList = value;
            }
        }
        private ObservableCollection<FilmDTO> _filmList;
        public ObservableCollection<FilmDTO> FilmList
        {
            get => _filmList;
            set
            {
                _filmList = value;
                OnPropertyChanged();
            }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand LoadDeleteMovieCM { get; set; }
        public ICommand SaveMovieCM { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand UpdateMovieCM { get; set; }
        public ICommand ExportFileCM { get; set; }
        public MovieManagementVM()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                FilmList = new ObservableCollection<FilmDTO>();
                try
                {
                    IsLoadding = true;
                    FilmList = new ObservableCollection<FilmDTO>(await Task.Run(() => FilmService.Ins.GetAllFilm()));
                    IsLoadding = false;
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
            LoadInforMovieCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem == null) return;
                RenewWindowData();
                DetailMovieWindow w1 = new DetailMovieWindow();
                LoadInforMovie(w1);
                w1.ShowDialog();
            });
            LoadAddMovieCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                Window w1 = new AddMovieWindow();
                w1.ShowDialog();

            });
            LoadEditMovieCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                EditMovieWindow w1 = new EditMovieWindow();
                LoadEditMovie(w1);
                w1.ShowDialog();
            });
            LoadDeleteMovieCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
     
                string message = "Bạn có chắc muốn xoá phim này không? Dữ liệu không thể phục hồi sau khi xoá!";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                {
                    IsLoadding = true;

                    (bool successDelMovie, string messageFromDelMovie) = await FilmService.Ins.DeleteMovie(SelectedItem.Id);

                    IsLoadding = false;

                    if (successDelMovie)
                    {
                        LoadMovieListView(Operation.DELETE);
                        SelectedItem = null;
                        CustomMessageBox.ShowOk(messageFromDelMovie, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromDelMovie, "Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });
            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == true)
                {
                    IsImageChanged = true;
                    filepath = openfile.FileName;
                    LoadImage();
                    return;
                }
                IsImageChanged = false;

            });
            UpdateMovieCM = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateMovieFunc(p);
                IsSaving = false;
            });
            SaveMovieCM = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;

                await SaveMovieFunc(p);

                IsSaving = false;
            });
            ExportFileCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ExportToFileFunc();
            });
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                SelectedItem = null;
                p.Close();
            });
        }
        public void LoadMovieListView(Operation oper = Operation.READ, FilmDTO m = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    FilmList.Add(m);
                    break;
                case Operation.UPDATE:
                    var movieFound = FilmList.FirstOrDefault(x => x.Id == m.Id);
                    FilmList[FilmList.IndexOf(movieFound)] = m;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < FilmList.Count; i++)
                    {
                        if (FilmList[i].Id == SelectedItem?.Id)
                        {
                            FilmList.Remove(FilmList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        public void ExportToFileFunc()
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true };
            if (sfd.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];


                ws.Cells[1, 1] = "Tên phim";
                ws.Cells[1, 2] = "Loại phim";
                ws.Cells[1, 3] = "Quốc gia";
                ws.Cells[1, 4] = "Thể loại";
                ws.Cells[1, 5] = "Thời lượng phim (phút)";

                int i2 = 2;
                foreach (var item in FilmList)
                {

                    ws.Cells[i2, 1] = item.FilmName;
                    ws.Cells[i2, 2] = item.FilmType;
                    ws.Cells[i2, 3] = item.Country;
                    ws.Cells[i2, 4] = item.Genre;
                    ws.Cells[i2, 5] = item.DurationFilm;
                    i2++;
                }
                ws.SaveAs(sfd.FileName);
                wb.Close();
                app.Quit();

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                CustomMessageBox.ShowOk("Xuất file thành công", "Thông báo", "OK", CustomMessageBoxImage.Success);

            }
        }
        public void RenewWindowData()
        {
            filmName = null;
            filmGenre = null;
            filmDirector = null;
            filmCountry = null;
            filmDuration = null;
            filmDescribe = null;
            ImageSource = null;
            filmYear = null;
            filepath = null;
            filmType = null;
           
        }
        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filepath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;
        }
        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(filmName) && filmCountry != null
                && !string.IsNullOrEmpty(filmDirector) && !string.IsNullOrEmpty(filmDescribe)
                 && filmGenre != null && !string.IsNullOrEmpty(filmYear)
                && !string.IsNullOrEmpty(filmDuration) && !string.IsNullOrEmpty(filmType);
        }
    }
}
