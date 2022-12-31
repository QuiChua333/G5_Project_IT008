using CinemaManagementProject.Model;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class StaffDTO
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public byte[] Avatar { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<ProductReceipt> ProductReceives { get; set; }
        public virtual ICollection<Trouble> Troubles { get; set; }

        public float BenefitContribution { get; set; }
        public string BenefitContributionStr
        {
            get
            {
                return Helper.FormatVNMoney(BenefitContribution);
            }
        }

    }
}
