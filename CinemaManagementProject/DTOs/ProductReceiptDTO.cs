using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class ProductReceiptDTO
    {
        public ProductReceiptDTO()
        {
        }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ImportPrice { get; set; }
        public string ImportPriceStr
        {
            get
            {
                return Helper.FormatVNMoney(ImportPrice);
            }
        }
        public int Quantity { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
