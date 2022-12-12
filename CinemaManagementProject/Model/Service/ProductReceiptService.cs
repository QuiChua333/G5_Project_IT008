using CinemaManagementProject.DTOs;
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
        private ProductReceiptService() { }

        private static ProductReceiptService _ins;
        public static ProductReceiptService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ProductReceiptService();
                }
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
                    productReceipts = await (from pr in context.ProductReceives
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
                                                 CreatedAt = pr.CreatedAt,
                                             }).ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return productReceipts;
        }

        public async Task<List<ProductReceiptDTO>> GetProductReceipt(int month)
        {
            List<ProductReceiptDTO> productReceipts;
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    productReceipts = await (from pr in context.ProductReceives
                                             where ((DateTime)pr.CreatedAt).Year == DateTime.Today.Year && ((DateTime)pr.CreatedAt).Month == month
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
                                                 CreatedAt = pr.CreatedAt
                                             }).ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return productReceipts;
        }
        private string CreateNextProdReceiptId(string maxId)
        {
            //NVxxx
            if (maxId is null)
            {
                return "PRC001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(3)) + 1}";
            return "PRC" + newIdString.Substring(newIdString.Length - 3, 3);
        }
        public async Task<(bool, string, ProductReceiptDTO)> CreateProductReceipt(ProductReceiptDTO newPReceipt)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Product prod = await context.Products.FindAsync(newPReceipt.ProductId);
                    //prod.Quantity += newPReceipt.Quantity;
                    prod.ProductStorage.Quantity += newPReceipt.Quantity;
                    int maxId = context.ProductReceives.Max(pr => pr.Id);

                    ProductReceive pR = new ProductReceive
                    {
                        //Id = CreateNextProdReceiptId(maxId),
                        Id=maxId+1,
                        ImportPrice = newPReceipt.ImportPrice,
                        ProductId = newPReceipt.ProductId,
                        CreatedAt = DateTime.Now,
                        Quantity = newPReceipt.Quantity,
                        StaffId = newPReceipt.StaffId,
                    };
                    context.ProductReceives.Add(pR);
                    await context.SaveChangesAsync();

                    newPReceipt.Id = pR.Id;
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
            return (true, "Lưu thông tin nhập hàng thành công", newPReceipt);
        }
    }
}
