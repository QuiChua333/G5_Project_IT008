using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.View.Admin.FoodManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using CinemaManagementProject.Utils;
using System.Windows.Media;
using Microsoft.Win32;
using System.Net.Cache;
using System.Windows.Media.Imaging;

namespace CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementVM : ViewModel.BaseViewModel
    {
        //
        //_foodList = danh sách đồ ăn hiển thị ra màn hình dựa theo filter Combobox
        //
        private ObservableCollection<ProductDTO> _foodList;
        public ObservableCollection<ProductDTO> FoodList
        {
            get => _foodList;
            set {
                _foodList = value;
                OnPropertyChanged();
            }
        }
        //
        //_storeAllFood = tất cả đồ ăn trong kho
        //
        private static ObservableCollection<ProductDTO> _storeAllFood;
        public static ObservableCollection<ProductDTO> StoreAllFood
        {
            get => _storeAllFood;
            set
            {
                _storeAllFood = value;
            }
        }
        //
        //_SelectedItem = Đồ ăn đang được chọn
        //
        private ProductDTO _SelectedItem;
        public ProductDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }
       
        //
        private bool isLoadding;
        public bool IsLoadding
        {
            get { return isLoadding; }
            set { isLoadding = value; OnPropertyChanged(); }
        }
        //
        //
        //
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; OnPropertyChanged(); }
        }
        private string category;
        public string Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged(); }
        }
        public ComboBoxItem SelectedItemFilter
        {
            get { return selectedItemFilter; }
            set { selectedItemFilter = value; OnPropertyChanged(); }
        }
        private ComboBoxItem selectedItemFilter;
        //
        //
        //
        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }
        string filePath;
        public bool IsImageChanged;
        //
        //
        //
        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged(); }
        }
        //
        //Command
        //

        public ICommand FirstLoadCM { get; set; } // The first time load Page
        public ICommand FilterComboboxFoodCM{ get; set; } // The first time load Page
        public ICommand OpenImportFoodCM { get; set; } //Open Import food
        public ICommand ImportFoodCM { get; set; } // Thêm sản phẩm
        public ICommand CloseImportFoodCM { get; set; } // Close Import food window
        public ICommand OpenEditFoodCM { get; set; } // OPen edit food window
        public ICommand SaveEditFoodCM { get; set; }
        public ICommand CloseEditFoodCM { get; set; }
        public ICommand RemoveFoodCM { get; set; } // OPen remove food window
        public ICommand OpenAddFoodCM { get; set; } // Open add food window
        public ICommand AddFoodCM { get; set; } // Add Food
        public ICommand CloseAddFoodCM { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand ImportFoodChangeCommand { get; set; }
        //
        //
        //

        public FoodManagementVM()
        {
            IsImageChanged = false;
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    FoodList = new ObservableCollection<ProductDTO>();
                    IsLoadding = true;
                    StoreAllFood = new ObservableCollection<ProductDTO>(await Task.Run(() => ProductService.Ins.GetAllProduct()));
                    IsLoadding = false;
                    FoodList = new ObservableCollection<ProductDTO>(StoreAllFood);
                }
                catch (EntityException e)
                {
                    MessageBox.Show(e.Message);
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "Ok");
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok");
                }
            });
            FilterComboboxFoodCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                try
                {
                    FoodList = new ObservableCollection<ProductDTO>();
                    string category = SelectedItemFilter.Content.ToString();
                    if (category == "Tất cả")
                    {
                        FoodList = new ObservableCollection<ProductDTO>(StoreAllFood);
                    }    
                    else
                    {
                        for (int i = 0; i < StoreAllFood.Count; i++)
                            if (StoreAllFood[i].Category == category)
                                FoodList.Add(StoreAllFood[i]);
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show(e.Message);
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "Ok");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok");
                }
            });
            OpenImportFoodCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ImportFoodWindow wd = new ImportFoodWindow();
                LoadImportFoodWindow(wd);
                wd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                wd.ShowDialog();
            });
            ImportFoodCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                await ImportFood(p);
            });
            CloseImportFoodCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    //MaskName.Visibility = Visibility.Collapsed;
                    RenewData();
                    w.Close();
                }
            });
            OpenEditFoodCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                EditFoodWindow wd = new EditFoodWindow();
                
                await LoadEditFood(wd);

                wd.ShowDialog();
            });
            SaveEditFoodCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                await SaveFood(p);
            });
            CloseEditFoodCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                RenewData();
                p.Close();
            });
            RemoveFoodCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                Id = SelectedItem.Id;
                if(CustomMessageBox.ShowOkCancel("Bạn có muốn xóa sản phẩm này không?", "Cảnh báo", "Xóa", "Không", Views.CustomMessageBoxImage.Warning) == Views.CustomMessageBoxResult.OK)
                {
                    (bool isSuccessDelete, string messageReturn) = await ProductService.Ins.DeleteProduct(Id);

                    if(isSuccessDelete)
                    {
                        LoadProductListView(Operation.DELETE);
                        CustomMessageBox.ShowOkCancel(messageReturn, "Thành công", "Ok", "Hủy", Views.CustomMessageBoxImage.Success);
                    }    
                }   
            });
            OpenAddFoodCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddFoodWindow wd = new AddFoodWindow();
                wd.ShowDialog();
            });
            AddFoodCM = new RelayCommand<Window>((p) => { return true; }, async(p) =>
            {
                await AddProduct(p);
            });
            CloseAddFoodCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                RenewData();
                p.Close();
            });
            UploadImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == true)
                {
                    filePath = openfile.FileName;
                    LoadImage();
                    IsImageChanged = true;
                    return;
                }
                IsImageChanged = false;
            });
            ImportFoodChangeCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedProduct != null && SelectedProduct.ProductImage != null)
                {
                    ImageSource = await CloudinaryService.Ins.LoadImageFromURL(SelectedProduct.ProductImage);
                }
            });
        }
        public void RenewData()
        {
            DisplayName = "";
            Image = "";
            Category = "";
            Price = 0;
        }
        public void LoadProductListView(Operation oper, ProductDTO product = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    FoodList.Add(product);
                    StoreAllFood.Add(product);
                    break;
                case Operation.UPDATE:
                    var productFound = FoodList.FirstOrDefault(s => s.Id == product.Id);
                    FoodList[FoodList.IndexOf(productFound)] = product;
                    StoreAllFood[StoreAllFood.IndexOf(productFound)] = product;
                    break;
                case Operation.UPDATE_PROD_QUANTITY:
                    var productFounded = FoodList.FirstOrDefault(s => s.Id == SelectedProduct.Id);
                    ProductDTO updatedProd = new ProductDTO()
                    {
                        Id = productFounded.Id,
                        ProductName = productFounded.ProductName,
                        Category = productFounded.Category,
                        Quantity = productFounded.Quantity + Quantity,
                        ProductImage = productFounded.ProductImage,
                        Price = productFounded.Price,
                    };
                    FoodList[FoodList.IndexOf(productFounded)] = updatedProd;
                    StoreAllFood[StoreAllFood.IndexOf(productFounded)] = updatedProd;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < FoodList.Count; i++)
                    {
                        if (FoodList[i].Id == Id)
                        {
                            FoodList.Remove(FoodList[i]);
                            StoreAllFood.Remove(StoreAllFood[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            _image.EndInit();

            ImageSource = _image;
        }
    }
}
