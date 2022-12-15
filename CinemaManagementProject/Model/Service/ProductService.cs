using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    public class ProductService
    {
        private ProductService() { }
        private static ProductService _ins;
        public static  ProductService Ins
        {
            get
            {
                if(_ins == null)
                    _ins = new ProductService();
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<List<ProductDTO>> GetAllProduct()
        {
            try
            {
                using (CinemaManagementProjectEntities db = new CinemaManagementProjectEntities())
                {
                    List<ProductDTO> productDTOs = await (
                        from p in db.Products
                        where !(bool)p.IsDeleted
                        select new ProductDTO
                        {
                            Id = p.Id,
                            ProductName = p.ProductName,
                            ProductImage = p.ProductImage,
                            Price = (float)p.Price,
                            Category = p.ProductType,
                            Quantity = (int)p.ProductStorage.Quantity,
                        }
                    ).ToListAsync();
                    return productDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string)> UpdateProduct(ProductDTO editedProduct)
        {
            try
            {
                using (var db = new CinemaManagementProjectEntities())
                {
                    Product prod = await db.Products.FindAsync(editedProduct.Id);

                    if (prod is null)
                    {
                        return (false, "Sản phẩm không tồn tại");
                    }

                    bool IsExistProdName = await db.Products.AnyAsync((p) => p.Id != prod.Id && p.ProductName == editedProduct.ProductName);
                    if (IsExistProdName)
                    {
                        return (false, "Tên sản phẩm này đã tồn tại! Vui lòng chọn tên khác");
                    }
                    prod.ProductName = editedProduct.ProductName;
                    prod.Price = editedProduct.Price;
                    prod.ProductImage = editedProduct.ProductImage;
                    prod.ProductType = editedProduct.Category;

                    await db.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch (EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string)> DeleteProduct(int id)
        {
            try
            {
                using (var db = new CinemaManagementProjectEntities())
                {
                    Product prod = await (from p in db.Products
                                          where p.Id == id && !(bool)p.IsDeleted
                                          select p).FirstOrDefaultAsync();

                    if (prod is null)
                    {
                        return (false, "Sản phẩm không tồn tại");
                    }
                    prod.IsDeleted = true;
                    await db.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch (EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string)> AddNewProduct(ProductDTO newProduct)
        {
            try
            {
                using (var db = new CinemaManagementProjectEntities())
                {
                    Product prod = await db.Products.Where((p) => p.ProductName == newProduct.ProductName).FirstOrDefaultAsync();

                    if (prod != null)
                    {
                        if (prod.IsDeleted == false)
                        {
                            return (false, "Sản phẩm đã tồn tại");
                        }

                        //Sản phẩm đã xóa nhưng add lại cùng tên
                        prod.ProductName = newProduct.ProductName;
                        prod.Price = newProduct.Price;
                        prod.ProductType = newProduct.Category;
                        prod.ProductImage = newProduct.ProductImage;
                        prod.IsDeleted = false;
                        prod.ProductStorage.Quantity = 0;
                        await db.SaveChangesAsync();
                        newProduct.Id = prod.Id;
                    }
                    else
                    {
                        Product product = new Product
                        {
                            ProductName = newProduct.ProductName,
                            Price = newProduct.Price,
                            ProductType = newProduct.Category,
                            IsDeleted = false,
                            ProductImage = newProduct.ProductImage,
                        };
                        db.Products.Add(product);
                        await db.SaveChangesAsync();
                        newProduct.Id = product.Id;
                    }
                    return (true, "Thêm sản phẩm thành công");
                }
            }
            catch (EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
    }
}
