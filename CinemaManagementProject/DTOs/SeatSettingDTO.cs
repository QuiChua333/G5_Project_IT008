using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class SeatSettingDTO
    {
        public int Id { get; set; }
        public Nullable<int> SeatId { get; set; }
        public Nullable<int> ShowTimeId { get; set; }
        public Nullable<int> SeatStatus { get; set; }

        public virtual SeatDTO Seat { get; set; }
        public virtual ShowtimeDTO ShowTime { get; set; }
        public string SeatPosition
        {
            get
            {
                return $"{Seat.SeatRow}{Seat.SeatNumber}";
            }
        }
    }
}
