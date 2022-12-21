using CinemaManagementProject.Utils;
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
        public string Description { get; set; }
        public Nullable<float> RepairCost { get; set; }

        public string RepairCostStr
        {
            get
            {
                return Helper.FormatVNMoney((float)RepairCost);
            }
        }
        public Nullable<System.DateTime> SubmittedAt { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string TroubleStatus { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Level { get; set; }
        public string Image { get; set; }
        public string StaffName { get; set; }
    }
}
