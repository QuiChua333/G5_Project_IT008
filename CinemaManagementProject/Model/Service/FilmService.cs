using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                            //where !p.IsDeleted
                        select new FilmDTO
                        {
                            FilmName = p.FilmName,
                            FilmType = (int)p.FilmType,
                            Country = p.Country,
                            Genre = p.Genre,
                            DurationFilm = (int)p.Duration,
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
    }
}

