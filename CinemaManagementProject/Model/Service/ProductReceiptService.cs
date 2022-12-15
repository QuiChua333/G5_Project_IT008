using CinemaManagementProject.DTOs;
using CinemaManagementProject.ViewModel.AdminVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    public class ProductReceiptService
    {
        public ProductReceiptService()
        {

        }
        private static ProductReceiptService _ins;
        public static ProductReceiptService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new ProductReceiptService();
                return _ins;    
            }
            private set => _ins = value;
            
        }

        public async Task<List<ProductReceiptDTO>> GetProductReceipt()
        {
            List<ProductReceiptDTO> productReceipts;
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    productReceipts = await (from pr in context.ProductReceipts
                                             orderby pr.CreatedAt descending
                                             select new ProductReceiptDTO
                                             {
                                                 Id = pr.Id,
                                                 ProductId = (int)pr.ProductId,
                                                 ProductName = pr.Product.ProductName,
                                                 StaffId = pr.Staff.Id,
                                                 StaffName = pr.Staff.StaffName,
                                                 Quantity = (int)pr.Quantity,
                                                 ImportPrice = (float)pr.ImportPrice,
                                                 CreatedAt = (DateTime)pr.CreatedAt,
                                             }).ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return productReceipts;
        }
        public async Task<(bool, string)> CreateProductReceipt(int productId, int quantity, float price)
        {
            try
            {
                using (var db = new CinemaManagementProjectEntities())
                {
                    Product prod = await db.Products.FindAsync(productId);
                   
                    prod.ProductStorage.Quantity += quantity;

                    ProductReceipt pR = new ProductReceipt
                    {
                        ImportPrice = price,
                        ProductId = productId,
                        CreatedAt = DateTime.Now,
                        Quantity = quantity,
                        StaffId = AdminVM.currentStaff.Id,
                    };
                    db.ProductReceipts.Add(pR);
                    await db.SaveChangesAsync();
                    return (true, "Lưu thông tin nhập hàng thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
    }
}
