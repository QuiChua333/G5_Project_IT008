using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class TicketDTO
    {
        public TicketDTO()
        {
        }
        public int Id { get; set; }
        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        private float _price;
        public float Price { get { return (float)Math.Truncate(_price); } set { _price = value; } }

        //Use when show bill details 
        public int SeatPosition { get; set; }
    }
}
