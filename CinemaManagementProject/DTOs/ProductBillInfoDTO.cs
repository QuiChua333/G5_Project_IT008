using CinemaManagementProject.Model;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class ProductBillInfoDTO
    {
        public int Id { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> BillId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public float PrizePerProduct { get; set; }
        public string PrizePerProductStr
        {
            get
            {
                return Helper.FormatVNMoney(PrizePerProduct);
            }
        }
        public string TotalPriceStr
        {
            get
            {
                return Helper.FormatVNMoney(Quantity * PrizePerProduct);
            }
        }
        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }
    }
}
