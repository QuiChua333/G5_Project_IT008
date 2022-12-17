using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class ShowtimeDTO
    {
        public int Id { get; set; }
        public Nullable<int> FilmId { get; set; }
        public TimeSpan StartTime { get; set; }
        public float Price { get; set; }

        public virtual Film Film { get; set; }
        public System.DateTime ShowDate { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<SeatSetting> SeatSettings { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
