using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class MovieDTO
    {
        public MovieDTO()
        {
            FilmType = "2D";
            ReleaseDate = null;
        }
        public int Id { get; set; }
        public string FilmName { get; set; }
        public Nullable<int> Duration { get; set; }
        public string FilmType { get; set; }
        public string Country { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string FilmInfo { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public virtual ICollection<ShowtimeDTO> ShowTimes { get; set; }
    }
}
