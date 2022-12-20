using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class FilmDTO
    {
        public FilmDTO()
        {
            FilmType = "2D";
            
        }
        public int Id { get; set; }
        public string FilmName { get; set; }
        public int DurationFilm { get; set; }
        public string Country { get; set; }
        public string FilmType { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string FilmInfor { get; set; }
        public bool IsDeleted { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public virtual ICollection<ShowtimeDTO> ShowTimes { get; set; }
    }
}
