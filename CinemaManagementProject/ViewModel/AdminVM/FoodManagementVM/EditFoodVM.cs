using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Model;
using CinemaManagementProject.View.Admin.FoodManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using CinemaManagementProject.Utils;
using System.Windows.Media.Media3D;
using CinemaManagementProject.Resource.Styles;

namespace CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementVM : BaseViewModel
    {
        #region Biến dùng trong phần EditFood
        private string _displayName { get; set; }
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }
        
        #endregion

        public async Task LoadEditFood(EditFoodWindow wd)
        {
            if (SelectedItem != null)
            {
                Id = SelectedItem.Id;
                DisplayName = SelectedItem.ProductName;
                if (SelectedItem.Category == "Đồ ăn")
                    wd.FilterCategory.SelectedIndex = 0;
                else
                    wd.FilterCategory.SelectedIndex = 1;
                Category = wd.FilterCategory.Text;
                Price = SelectedItem.Price;
                Quantity = SelectedItem.Quantity;
                Image = SelectedItem.ProductImage;
                IsImageChanged = false;
                ImageSource = await CloudinaryService.Ins.LoadImageFromURL(Image);
            }
        }
        public async Task SaveFood(Window wd)
        {
            if(Id != null && !string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(Price.ToString()))
            {
                ProductDTO EditedProduct = new ProductDTO();
                EditedProduct.Id = Id;
                EditedProduct.ProductName = DisplayName;
                EditedProduct.Category = Category;
                EditedProduct.Price = Price;
                EditedProduct.Quantity = Quantity;
                if(IsImageChanged)
                {
                    if (Image != null)
                    {
                        await CloudinaryService.Ins.DeleteImage(Image);
                    }

                    EditedProduct.ProductImage = await Task.Run(() => CloudinaryService.Ins.UploadImage(filePath));

                    if (EditedProduct.ProductImage is null)
                    {
                        CustomMessageBox.ShowOk(isEN ? "Error arises during the saving process. Please try again!" : "Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại!", isEN ? "Error" : "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                        return;
                    }
                }  
                else
                    EditedProduct.ProductImage = Image;

                (bool successUpdateProduct, string messageFromUpdateProduct) = await ProductService.Ins.UpdateProduct(EditedProduct);

                if(successUpdateProduct)
                {
                    wd.Close();
                    LoadProductListView(Operation.UPDATE, EditedProduct);
                    CustomMessageBox.ShowOk(messageFromUpdateProduct, isEN ? "Notice" : "Thông báo", "Ok", Views.CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdateProduct, isEN ? "Error" : "Lỗi", isEN ? "Cancel" : "Hủy", Views.CustomMessageBoxImage.Error);
                }    
            }
            else
            {
                CustomMessageBox.ShowOk(isEN ? "Please enter the full information!":"Vui lòng nhập đầy đủ thông tin!",isEN? "Warning": "Cảnh báo", "Ok", Views.CustomMessageBoxImage.Warning);
            }
        }
    }
}
