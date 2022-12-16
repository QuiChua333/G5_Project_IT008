using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using CinemaManagementProject.View.Admin.MovieManagement;
using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using System.Globalization;
using System.Security.Cryptography.Xml;

namespace CinemaManagementProject.ViewModel.AdminVM.MovieManagementVM
{
    public partial class MovieManagementVM : BaseViewModel
    {

        public ICommand LoadEditMovieCM { get; set; }

        public async void LoadEditMovie(EditMovieWindow w1)
        {
            IsImageChanged = false;
            filmID = SelectedItem.Id.ToString();
            filmName = SelectedItem.FilmName;
            filmGenre = SelectedItem.Genre;
            filmYear = SelectedItem.ReleaseDate.Year.ToString();
            filmDirector = SelectedItem.Author;
            filmCountry = SelectedItem.Country;
            filmDuration = SelectedItem.DurationFilm.ToString();
            filmDescribe = SelectedItem.FilmInfor;
            filmType = SelectedItem.FilmType;
            Image = SelectedItem.Image;

            ImageSource = await CloudinaryService.Ins.LoadImageFromURL(SelectedItem.Image);
        }
        public async Task UpdateMovieFunc(Window p)
        {
            
            if (filmID != null && IsValidData())
            {
                FilmDTO film = new FilmDTO
                {
                    Id = int.Parse(filmID),
                    FilmName = filmName,
                    Country = filmCountry,
                    Author = filmDirector,
                    FilmInfor = filmDescribe,
                    Genre = filmGenre,
                    ReleaseDate = DateTime.ParseExact($"01/01/{filmYear}", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DurationFilm = int.Parse(filmDuration),
                    FilmType = filmType,
                };

                if (IsImageChanged)
                {
                    if (Image != null)
                    {
                        await CloudinaryService.Ins.DeleteImage(Image);
                    }

                    film.Image = await CloudinaryService.Ins.UploadImage(filepath);
                    if (film.Image is null)
                    {
                        CustomMessageBox.ShowOk("Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại", "Thông báo", "OK", Views.CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    film.Image = Image;
                }

                (bool successUpdateMovie, string messageFromUpdateMovie) = await FilmService.Ins.UpdateMovie(film);

                if (successUpdateMovie)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromUpdateMovie, "Thông báo", "OK",Views.CustomMessageBoxImage.Success);
                    LoadMovieListView(Operation.UPDATE, film); 
                  
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdateMovie, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo",  "OK",Views.CustomMessageBoxImage.Warning);
            }
        }

    }
}
