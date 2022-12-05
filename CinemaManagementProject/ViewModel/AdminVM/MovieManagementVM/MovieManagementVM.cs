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

namespace CinemaManagementProject.ViewModel.AdminVM.MovieManagementVM
{
    public class MovieManagementVM : BaseViewModel
    {
        private ObservableCollection<FilmDTO> _movieList;
        public ObservableCollection<FilmDTO> MovieList
        {
            get => _movieList;
            set
            {
                _movieList = value;
                OnPropertyChanged();
            }
        }
        public ICommand FirstLoadCM { get; set; }

        public MovieManagementVM()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                MovieList = new ObservableCollection<FilmDTO>();


                try
                {
                    
                    MovieList = new ObservableCollection<FilmDTO>(await Task.Run(() => FilmService.Ins.GetAllFilm()));
                    
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "Ok");
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok");
                }
            });
        }
    }
}
