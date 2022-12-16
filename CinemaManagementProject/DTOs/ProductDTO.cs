using CinemaManagementProject.Component.FoodItem;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagementProject.DTOs
{
    public class ProductDTO
    {
        public ProductDTO()
        {

        }

        public ProductDTO(int id, string image, string name, string category, float price)
        {
            Id = id;
            ProductImage = image;
            ProductName = name;
            Category = category;
            Price = price;
        }
        public int Id { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string PriceStr
        {
            get
            {
                return Helper.FormatVNMoney(Price);
            }
        }
        public float Price { get; set; }
    }
}
