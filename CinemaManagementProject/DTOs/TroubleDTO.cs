using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class TroubleDTO
    {
        public int Id { get; set; }
        public string TroubleType { get; set; }
        public string TroubleInfo { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public string TroubleStatus { get; set; }
        public int StaffId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
