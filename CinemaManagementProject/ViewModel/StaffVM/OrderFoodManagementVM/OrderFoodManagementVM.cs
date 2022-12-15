using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.View.Staff.OrderFoodManagement;
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
using System.Windows.Media;

namespace CinemaManagementProject.ViewModel.StaffVM.OrderFoodManagementVM
{
    public class OrderFoodManagementVM : BaseViewModel
    {
        //
        //_foodList = danh sách đồ ăn hiển thị ra màn hình dựa theo filter Combobox
        //
        private ObservableCollection<ProductDTO> _foodList;
        public ObservableCollection<ProductDTO> FoodList
        {
            get => _foodList;
            set
            {
                _foodList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ProductDTO> _orderList;
        public ObservableCollection<ProductDTO> OrderList
        {
            get => _orderList;
            set
            {
                _orderList = value;
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

        private ComboBoxItem selectedItemFilter;
        public ComboBoxItem SelectedItemFilter
        {
            get { return selectedItemFilter; }
            set { selectedItemFilter = value; OnPropertyChanged(); }
        }
        //
        //
        //
        private float _totalPrice;
        public float TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; OnPropertyChanged(); }
        }
        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }
        //
        //Command
        //

        public ICommand FirstLoadCM { get; set; } // The first time load Page
        public ICommand FilterComboboxFoodCM { get; set; } // The first time load Page
        public ICommand SelectedProductToBillCM { get; set; } // Chuyển đồ ăn to bill

        //
        //
        //

        public OrderFoodManagementVM()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    FoodList = new ObservableCollection<ProductDTO>();
                    OrderList = new ObservableCollection<ProductDTO>();
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
                catch (Exception e)
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
            SelectedProductToBillCM = new RelayCommand<ListBox>((p) => { return true; }, (p) =>
            {
                if(SelectedItem != null)
                {
                    MessageBox.Show(SelectedItem.ProductName);
                }
            });
        }
    }
}
