using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;

namespace CinemaManagementProject.ViewModel.AdminVM.MovieManagementVM
{
    //public partial class MovieManagementVM : BaseViewModel
    //{
    //    public ICommand LoadAddMovieCM { get; set; }
    //    public async Task SaveMovieFunc(Window p)
    //    {
    //        if (IsValidData())
    //        {
    //            FilmDTO film = new FilmDTO
    //            {
    //                FilmName = filmName,
    //                Country = filmCountry,
    //                Author = filmDirector,
    //                FilmInfor = filmDescribe,
    //                //Image = movieImage,
    //                Genre = filmGenre,
    //                ReleaseDate = DateTime.Parse(filmYear),
    //                DurationFilm = int.Parse(filmDuration),
    //            };



    //            (bool successAddMovie, string messageFromAddMovie, FilmDTO newMovie) = await FilmService.Ins.AddMovie(film);

    //            if (successAddMovie)
    //            {
    //                isSaving = false;
    //                CustomMessageBox.ShowOk(messageFromAddMovie,"","Ok");
    //                //LoadMovieListView(Operation.CREATE, newMovie);
    //                MaskName.Visibility = Visibility.Collapsed;
    //                p.Close();
    //            }
    //            else
    //            {
    //                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok");            
    //            }
    //        }
    //        else
    //        {
    //            CustomMessageBox.ShowOk("Cảnh báo", "Vui lòng nhâph đầy đủ thông tin", "Ok");
    //        }
    //    }
    //    public bool IsValidData()
    //    {
    //        return !string.IsNullOrEmpty(filmName) && filmCountry != null
    //            && !string.IsNullOrEmpty(filmDirector) && !string.IsNullOrEmpty(filmDescribe)
    //             && filmGenre != null && !string.IsNullOrEmpty(filmYear)
    //            && !string.IsNullOrEmpty(filmDuration);
    //    }
    //}
}
