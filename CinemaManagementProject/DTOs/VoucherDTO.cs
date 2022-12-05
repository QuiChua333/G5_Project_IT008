using CinemaManagementProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class VoucherDTO : BaseViewModel
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }
        public int VoucherReleaseId { get; set; }
        public int VoucherStatus { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        // 
        public double MenhGia { get; set; }
        public bool EnableMerge { get; set; }
        public int TypeObject { get; set; }

    }
}
