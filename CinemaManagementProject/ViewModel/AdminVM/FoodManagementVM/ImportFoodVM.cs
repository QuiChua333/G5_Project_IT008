using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Admin.FoodManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementVM : BaseViewModel
    {
        #region Biến thêm dùng khi thêm sản phẩm
        //
        // SelectedProduct: Dùng khi thêm sản phẩm
        //
        public ProductDTO SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged(); }
        }
        private ProductDTO _selectedProduct;
        //
        //
        //
        public float Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }
        private float _price;
        //
        //
        //
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged(); }
        }
        private int _quantity;
        //
        //
        //
        #endregion
        public void LoadImportFoodWindow(ImportFoodWindow wd)
        {
            SelectedProduct = null;
            ImageSource = null;
            Price = 0;
            Quantity = 0;
        }
        public async Task ImportFood(Window p)
        {
            if (SelectedProduct != null)
            {
                if (Price >= 0 && Quantity > 0)
                {
                    (bool isSuccessCreate, string MessageReturn) = await ProductReceiptService.Ins.CreateProductReceipt(SelectedProduct.Id, Quantity, Price);
                    if(isSuccessCreate)
                    {
                        LoadProductListView(Operation.UPDATE_PROD_QUANTITY);
                        p.Close();
                        CustomMessageBox.ShowOk(MessageReturn,isEN? "Notice":"Thông báo", "Ok", Views.CustomMessageBoxImage.Success);
                    }    
                }
                else
                {
                    CustomMessageBox.ShowOkCancel( isEN? "Invalid input price or quantity!" : "Giá nhập hoặc số lượng không hợp lệ", isEN? "Warning":"Cảnh báo", "OK", isEN? "Cancel" :"Hủy");
                }
            }
            else
            {
                CustomMessageBox.ShowOkCancel(isEN ? "Please choose the product to import" : "Vui lòng chọn sản phẩm cần nhập",isEN? "Waring" : "Cảnh báo", "OK", isEN? "Cancel" : "Hủy");
            }
            
        }
    }

}
