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
        public ProductReceiptDTO(int id, float importPrice, int quantity, int staffId)
        {
            Id = id;
            ImportPrice = importPrice;
            Quantity = quantity;
            StaffId = staffId;
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
        public DateTime CreatedAt { get; set; }
    }
}
