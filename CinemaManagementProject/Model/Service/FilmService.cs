using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CinemaManagementProject.Model.Service
{
    public class FilmService
    {
        private FilmService() { }
        private static FilmService _ins;
        public static FilmService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new FilmService();
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<List<FilmDTO>> GetAllFilm()
        {
            try
            {
                using (CinemaManagementProjectEntities db = new CinemaManagementProjectEntities())
                {
                    List<FilmDTO> filmDTOs = await (
                        from p in db.Films
                        select new FilmDTO
                        {
                            Id = p.Id,
                            FilmName = p.FilmName,
                            FilmType = p.FilmType,
                            Country = p.Country,
                            Genre = p.Genre,
                            DurationFilm = (int)p.Duration,
                            Author = p.Author,
                            FilmInfor=p.FilmInfo,
                            ReleaseDate = (DateTime)p.ReleaseDate,
                            Image = p.Image,
                        }
                    ).ToListAsync();
                    return filmDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string, FilmDTO)> AddMovie(FilmDTO newMovie)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Film m = context.Films.Where((Film mov) => mov.FilmName == newMovie.FilmName).FirstOrDefault();

                    if (m != null)
                    {
                        if (m.IsDeleted == false)
                        {
                            return (false, "Tên phim đã tồn tại", null);
                        }
                        //Khi phim đã bị xóa nhưng được add lại với cùng tên => update lại phim đã xóa đó với thông tin là 
                        // phim mới thêm thay vì add thêm
                        m.FilmName = newMovie.FilmName;
                        m.Duration = newMovie.DurationFilm;
                        m.Country = newMovie.Country;
                        m.FilmInfo = newMovie.FilmInfor;
                        m.ReleaseDate = newMovie.ReleaseDate;
                        m.FilmType = newMovie.FilmType;
                        m.Author = newMovie.Author;
                        m.Genre = newMovie.Genre;
                        m.Image = newMovie.Image;

                        m.IsDeleted = false;

                        await context.SaveChangesAsync();
                        newMovie.Id = m.Id;
                    }
                    else
                    {
                        Film mov = new Film
                        {
                            FilmName = newMovie.FilmName,
                            Duration = newMovie.DurationFilm,
                            Country = newMovie.Country,
                            FilmInfo = newMovie.FilmInfor,
                            ReleaseDate = newMovie.ReleaseDate,
                            FilmType = newMovie.FilmType,
                            Author = newMovie.Author,
                            Genre = newMovie.Genre,
                            Image = newMovie.Image,
                            IsDeleted = false,
                        };
                        context.Films.Add(mov);
                        await context.SaveChangesAsync();
                        newMovie.Id = mov.Id;
                    }
                }

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return (false, "DbEntityValidationException", null);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}", null);
            }
            return (true, "Thêm phim thành công", newMovie);
        }
        public async Task<(bool, string)> DeleteMovie(int Id)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Film film = await (from p in context.Films
                                         where p.Id == Id && p.IsDeleted==false
                                         select p).FirstOrDefaultAsync();
                    if (film == null)
                    {
                        return (false, "Phim không tồn tại!");
                    }

                    if (film.Image != null)
                    {
                        CloudinaryService.Ins.DeleteImage(film.Image);
                        film.Image = null;
                    }
                    film.IsDeleted = true;
                    context.Films.Remove(film);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return (false, $"Error Server {e.Message}");
            }
            return (true, "Xóa phim thành công");
        }
        public async Task<(bool, string)> UpdateMovie(FilmDTO updatedMovie)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Film film = context.Films.Find(updatedMovie.Id);

                    if (film is null)
                    {
                        return (false, "Phim không tồn tại");
                    }

                    bool IsExistMovieName = context.Films.Any((Film mov) => mov.Id != film.Id && mov.FilmName == updatedMovie.FilmName);
                    if (IsExistMovieName)
                    {
                        return (false, "Tên phim đã tồn tại!");
                    }


                    film.FilmName = updatedMovie.FilmName;
                    film.Duration = updatedMovie.DurationFilm;
                    film.Country = updatedMovie.Country;
                    film.FilmInfo = updatedMovie.FilmInfor;
                    film.ReleaseDate = updatedMovie.ReleaseDate;
                    film.FilmType = updatedMovie?.FilmType;
                    film.Author = updatedMovie.Author;
                    film.Genre = updatedMovie.Genre;
                    film.Image = updatedMovie.Image;
                    film.Id = updatedMovie.Id;

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch (DbEntityValidationException)
            {
                return (false, "DbEntityValidationException");
            }
            catch (DbUpdateException e)
            {
                return (false, $"DbUpdateException: {e.Message}");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
        }
    }
    
}

