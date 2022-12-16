using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public Nullable<int> SeatQuantity { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
