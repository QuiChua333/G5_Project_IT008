using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagementProject.Model.Service
{
    public class MovieService
    {
        private MovieService() { }

        private static MovieService _ins;
        public static MovieService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new MovieService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        public async Task<List<MovieDTO>> GetAllMovie()
        {
            List<MovieDTO> movies = null;

            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    movies = await (from movie in context.Films
                                    //where !movie.IsDeleted
                                    select new MovieDTO
                                    {
                                        Id = movie.Id,
                                        FilmName = movie.FilmName,
                                        Duration = movie.Duration,
                                        Country = movie.Country,
                                        FilmInfo = movie.FilmInfo,
                                        ReleaseDate = movie.ReleaseDate,
                                        FilmType = movie.FilmType,
                                        Author = movie.Author,
                                        //Image = movie.Image,
                                        //Genre = (from genre in movie.Genres
                                        //          select new GenreDTO { DisplayName = genre.DisplayName, Id = genre.Id }
                                        //        ).ToList(),
                                    }
                     ).ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return movies;
        }
        public async Task<List<MovieDTO>> GetShowingMovieByDay(DateTime date)
        {
            List<MovieDTO> movieList = new List<MovieDTO>();
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var MovieIdList = await (from showSet in context.ShowTimeSettings
                                             where DbFunctions.TruncateTime(showSet.ShowDate) == date.Date
                                             select showSet into S
                                             from show in S.ShowTimes
                                             select new
                                             {
                                                 MovieId = show.FilmId,
                                                 ShowTime = show,
                                             }).GroupBy(m => m.MovieId).ToListAsync();
                    for (int i = 0; i < MovieIdList.Count(); i++)
                    {
                        int id = (int)MovieIdList[i].Key;

                        List<ShowtimeDTO> showtimeDTOsList = new List<ShowtimeDTO>();
                        MovieDTO mov = null;
                        foreach (var m in MovieIdList[i])
                        {                           
                            showtimeDTOsList.Add(new ShowtimeDTO
                            {
                                Id = m.ShowTime.Id,
                                FilmId = m.ShowTime.FilmId,
                                StartTime = m.ShowTime.StartTime,
                                RoomId = (int)m.ShowTime.ShowTimeSetting.RoomId,
                                ShowDate = (DateTime)m.ShowTime.ShowTimeSetting.ShowDate,
                                Price = m.ShowTime.Price
                            });                           
                            if (mov is null)
                            {
                                Film movie = m.ShowTime.Film;

                                if (movie is null)
                                {
                                    movie = await context.Films.FindAsync(m.ShowTime.FilmId);
                                }
                                mov = new MovieDTO
                                {
                                    Id = movie.Id,
                                    FilmName = movie.FilmName,
                                    Duration = movie.Duration,
                                    Country = movie.Country,
                                    FilmInfo = movie.FilmInfo,
                                    ReleaseDate = movie.ReleaseDate,
                                    FilmType = movie.FilmType,
                                    Author = movie.Author,
                                    //Image = movie.Image,
                                    //Genres = (from genre in movie.Genres
                                    //          select new GenreDTO { DisplayName = genre.DisplayName, Id = genre.Id }
                                    //                 ).ToList(),
                                };
                            }
                        }
                        movieList.Add(mov);
                        movieList[i].ShowTimes = showtimeDTOsList.OrderBy(s => s.StartTime).ToList();
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return movieList;
        }
    }
}
