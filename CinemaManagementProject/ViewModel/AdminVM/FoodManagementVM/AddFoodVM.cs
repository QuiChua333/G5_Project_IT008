
using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementVM: BaseViewModel
    {
        public async Task AddProduct(Window wd)
        {
            if(!string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(Category))
            {
                ProductDTO newProduct = new ProductDTO();
                newProduct.ProductName = DisplayName;
                newProduct.Category = Category;
                newProduct.Price = Price;
                newProduct.Quantity = 0;
                newProduct.ProductImage = await CloudinaryService.Ins.UploadImage(filePath);

                if (newProduct.ProductImage is null)
                {
                    CustomMessageBox.ShowOk(isEN? "Error arises during the saving process. Please try again!":"Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại!", isEN? "Error" : "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                    return;
                }
                (bool issuccessAddFood, string messageReturn) = await ProductService.Ins.AddNewProduct(newProduct);
                if(issuccessAddFood)
                {
                    wd.Close();
                    LoadProductListView(Utils.Operation.CREATE, newProduct);
                    CustomMessageBox.ShowOk(messageReturn, isEN? "Notice" : "Thông báo", "Ok", Views.CustomMessageBoxImage.Success);
                    filePath = null;
                    using (var db = new CinemaManagementProjectEntities())
                    {
                        ProductStorage prodStorage = db.ProductStorages.Find(newProduct.Id);

                        if (prodStorage == null)
                        {
                            ProductStorage pS = new ProductStorage
                            {
                                ProductId = newProduct.Id,
                                Quantity = 0,
                            };
                            db.ProductStorages.Add(pS);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    CustomMessageBox.ShowOk(messageReturn, isEN? "Error" : "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk(isEN ? "You have not entered data yet!" : "Bạn chưa nhập dữ liệu!", isEN? "Warning" : "Cảnh báo", "Ok", Views.CustomMessageBoxImage.Warning);
            }
        }
    }
}
