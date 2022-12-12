using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class SeatDTO
    {
        public int Id { get; set; }
        public Nullable<int> SeatNumber { get; set; }
        public string SeatRow { get; set; }
        public Nullable<int> RoomId { get; set; }

        public virtual Room Room { get; set; }
        public virtual ICollection<SeatSettingDTO> SeatSettings { get; set; }
        public virtual ICollection<TicketDTO> Tickets { get; set; }
    }
}
