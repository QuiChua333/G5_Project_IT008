using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Admin.MovieManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using CinemaManagementProject.DTOs;
using System.IO;
using System.Windows.Media.Imaging;

namespace CinemaManagementProject.ViewModel.AdminVM.MovieManagementVM
{
    public partial class MovieManagementVM : BaseViewModel
    {
        public ICommand LoadInforMovieCM { get; set; }

        public async void LoadInforMovie(DetailMovieWindow w1)
        {
            filmName = SelectedItem.FilmName;
            filmDirector = SelectedItem.Author;
            filmGenre = SelectedItem.Genre;
            filmCountry = SelectedItem.Country;
            filmDuration = SelectedItem.DurationFilm.ToString();
            filmDescribe = SelectedItem.FilmInfor;
            filmYear = SelectedItem.ReleaseDate.Year.ToString();
            filmType = SelectedItem.FilmType;
            ImageSource = await CloudinaryService.Ins.LoadImageFromURL(SelectedItem.Image);
        }
    }
}
