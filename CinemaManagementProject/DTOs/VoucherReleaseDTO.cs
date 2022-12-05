using CinemaManagementProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class VoucherReleaseDTO : BaseViewModel
    {
        public int Id { get; set; }
        public string VoucherReleaseCode { get; set; }
        public string VoucherReleaseName { get; set; }
        public double Price { get; set; }
        public bool VoucherReleaseStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TypeObject { get; set; }
        public double MinimizeTotal { get; set; }
        public bool EnableMerge { get; set; }
        public IList<VoucherDTO> Vouchers { get; set; }
        public int VCount { get; set; }
        public int UnusedVCount { get; set; }
    }
}
