using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Admin.FoodManagement;
using CinemaManagementProject.View.Staff;
using CinemaManagementProject.View.Staff.MovieScheduleWindow;
using CinemaManagementProject.View.Staff.OrderFoodManagement;
using CinemaManagementProject.View.Staff.TicketWindow;
using CinemaManagementProject.ViewModel.StaffVM.TicketVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace CinemaManagementProject.ViewModel.StaffVM.TicketBillVM
{
    public class TicketBillVM : BaseViewModel
    {
        public class Food
        {
            public string DisplayName { get; set; }
            public float Price { get; set; }
            public string PriceStr
            {
                get
                {
                    return Helper.FormatVNMoney(Price);
                }
            }
            public float TotalPrice
            {
                get
                {
                    return Price * Quantity;
                }
            }
            public string TotalPriceStr
            {
                get
                {
                    return Helper.FormatVNMoney(TotalPrice);
                }
            }
            public int Quantity { get; set; }
        }

        public static ShowtimeDTO Showtime;
        public static FilmDTO Film;
        public static ObservableCollection<ProductDTO> ListFood;
        public static StaffDTO Staff;
        public static bool IsBacking;
        public CustomerDTO customerDTO;
        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }


        // biến bilding
        private bool _IsWalkinGuest;
        public bool IsWalkinGuest
        {
            get { return _IsWalkinGuest; }
            set { _IsWalkinGuest = value; OnPropertyChanged(); }
        }

        private bool _IsValidPhone;
        public bool IsValidPhone
        {
            get { return _IsValidPhone; }
            set { _IsValidPhone = value; OnPropertyChanged(); }
        }

        private bool _ShowPhone;
        public bool ShowPhone
        {
            get { return _ShowPhone; }
            set { _ShowPhone = value; OnPropertyChanged(); }
        }

        private bool _ShowPhoneError;
        public bool ShowPhoneError
        {
            get { return _ShowPhoneError; }
            set { _ShowPhoneError = value; OnPropertyChanged(); }
        }

        private bool _ShowSignUp;
        public bool ShowSignUp
        {
            get { return _ShowSignUp; }
            set { _ShowSignUp = value; OnPropertyChanged(); }
        }

        private bool _ShowInfoCustomer;
        public bool ShowInfoCustomer
        {
            get { return _ShowInfoCustomer; }
            set { _ShowInfoCustomer = value; OnPropertyChanged(); }
        }

        private bool _ShowDoneButton;
        public bool ShowDoneButton
        {
            get { return _ShowDoneButton; }
            set { _ShowDoneButton = value; OnPropertyChanged(); }
        }

        // biến value display
        private string _MovieName;
        public string MovieName
        {
            get { return _MovieName; }
            set { _MovieName = value; OnPropertyChanged(); }
        }

        private string _Date;
        public string Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged(); }
        }

        private string _Time;
        public string Time
        {
            get { return _Time; }
            set { _Time = value; OnPropertyChanged(); }
        }

        private string _Room;
        public string Room
        {
            get { return _Room; }
            set { _Room = value; OnPropertyChanged(); }
        }

        private string _Seat;
        public string Seat
        {
            get { return _Seat; }
            set { _Seat = value; OnPropertyChanged(); }
        }

        private string _Price;
        public string Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged(); }
        }

        private string _TotalPriceMovie;
        public string TotalPriceMovie
        {
            get { return _TotalPriceMovie; }
            set { _TotalPriceMovie = value; OnPropertyChanged(); }
        }

        private string _TotalPriceFood;
        public string TotalPriceFood
        {
            get { return _TotalPriceFood; }
            set { _TotalPriceFood = value; OnPropertyChanged(); }
        }

        private string _TotalPrice;
        public string TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; OnPropertyChanged(); }
        }

        private string _DiscountStr;
        public string DiscountStr
        {
            get { return _DiscountStr; }
            set { _DiscountStr = value; OnPropertyChanged(); }
        }

        private string _LastPriceStr;
        public string LastPriceStr
        {
            get { return _LastPriceStr; }
            set { _LastPriceStr = value; OnPropertyChanged(); }
        }

        // end
        private string _PhoneNumber;
        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; OnPropertyChanged(); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }

        private string _NameSignUp;
        public string NameSignUp
        {
            get { return _NameSignUp; }
            set { _NameSignUp = value; OnPropertyChanged(); }
        }

        private string _EmailSignUp;
        public string EmailSignUp
        {
            get { return _EmailSignUp; }
            set { _EmailSignUp = value; OnPropertyChanged(); }
        }

        private string _VoucherID;
        public string VoucherID
        {
            get { return _VoucherID; }
            set { _VoucherID = value; OnPropertyChanged(); }
        }

        // command
        public ICommand BackToFoodPageCM { get; set; }
        public ICommand CboxWalkinGuestCM { get; set; }
        public ICommand CheckPhoneNumberCM { get; set; }
        public ICommand OpenSignUpCM { get; set; }
        
        public ICommand SignUpCM { get; set; }
        public ICommand AddVoucherOnlyFoodCM { get; set; }
        public ICommand AddVoucherNoFoodCM { get; set; }
        public ICommand AddVoucherCM { get; set; }
        public ICommand DeleteVoucherCM { get; set; }
        public ICommand PayFoodCM { get; set; }
        public ICommand PayFullCM { get; set; }
        public ICommand PayMovieCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        private  List<SeatSettingDTO> _ListSeat;
        public  List<SeatSettingDTO> ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Food> _ListFoodDisplay;
        public ObservableCollection<Food> ListFoodDisplay
        {
            get => _ListFoodDisplay;
            set
            {
                _ListFoodDisplay = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<VoucherDTO> _ListVoucher;
        public ObservableCollection<VoucherDTO> ListVoucher
        {
            get => _ListVoucher;
            set
            {
                _ListVoucher = value;
                OnPropertyChanged();
            }
        }

        private VoucherDTO _SelectedItem;
        public VoucherDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private static bool IsBookMovie = false;
        private float TotalFullMoviePrice = 0;
        public float LastPrice;

        DateTime start, end;
        public void CaculateTime()
        {
            start = Showtime.ShowDate;
            start = start.Add(Showtime.StartTime);
            end = start.AddMinutes(Film.DurationFilm);
        }
       
        // hàm 

        public TicketBillVM()
        {
            // Biến khởi tạo
            IsBacking = false;
            customerDTO = new CustomerDTO();
            ListVoucher = new ObservableCollection<VoucherDTO>();
            //
            //
            Staff = StaffVM.currentStaff;
            //
            float TotalFullMoviePrice = 0;
            // Food
            ListFood = new ObservableCollection<ProductDTO>();
            ListFood = OrderFoodManagementVM.OrderFoodManagementVM.ListOrder;
            ListFoodDisplay = new ObservableCollection<Food>();
            
            for (int i = 0; i < ListFood.Count; i++)
            {
                Food tempFood = new Food();
                tempFood.DisplayName = ListFood[i].ProductName;
                tempFood.Quantity = ListFood[i].Quantity;
                tempFood.Price = ListFood[i].Price;
                ListFoodDisplay.Add(tempFood);
            }
            float TotalFood = 0;
            for (int i = 0; i < ListFoodDisplay.Count; i++)
            {
                TotalFood += ListFoodDisplay[i].TotalPrice;
            }
            TotalPriceFood = Helper.FormatVNMoney(TotalFood);



            //Film
            if (OrderFoodManagementVM.OrderFoodManagementVM.checkOnlyFoodOfPage == false)
            {
                FirtShow();
                
               
            }

            //Total price
            float Total = TotalFood + TotalFullMoviePrice;
            TotalPrice = Helper.FormatVNMoney(Total);

            //Discount
            float Discount = 0;
            DiscountStr = Helper.FormatVNMoney(Discount);
            if (ListFood.Count == 0)
            {
                LastPriceStr = TotalPriceMovie;
                LastPrice = TotalFullMoviePrice;
            }
            else if (IsBookMovie)
            {
                LastPriceStr = TotalPrice;
                LastPrice = Total;
            }
            else
            {
                LastPriceStr = TotalPriceFood;
                LastPrice = TotalFood;
            }

            // Display bool
            IsValidPhone = false;
            IsWalkinGuest = false;
            ShowPhone = true;
            ShowPhoneError = false;
            ShowSignUp = false;
            ShowDoneButton = false;

            CboxWalkinGuestCM = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                bool x = IsWalkinGuest;
                if (IsWalkinGuest)
                {
                    ShowPhone = false;
                    ShowDoneButton = true;
                }
                else
                {
                    ShowPhone = true;
                    ShowDoneButton = false;
                }
                ShowPhoneError = false;
                ShowSignUp = false;
                ShowInfoCustomer = false;
                PhoneNumber = "";
                NameSignUp = "";
                EmailSignUp = "";
            });
            BackToFoodPageCM = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                try
                {
                    IsBacking = true;
                    if (OrderFoodManagementVM.OrderFoodManagementVM.checkOnlyFoodOfPage)
                    {
                        StaffWindow tk = Application.Current.Windows.OfType<StaffWindow>().FirstOrDefault();
                        tk.Content.Content = new FoodPage();
                    }
                    else
                    {
                        TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                        tk.TicketBookingFrame.Content = new FoodPage();
                    }
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            });
            CheckPhoneNumberCM = new RelayCommand<object>((p) => { return true; },
            async (p) =>
            {
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    CustomerDTO customer = await CustomerService.Ins.FindCustomerInfo(PhoneNumber);
                    if (customer != null)
                    {
                        Name = customer.CustomerName;
                        Email = customer.Email;
                        IsValidPhone = true;
                        customerDTO = customer;
                    }
                    else
                    {
                        IsValidPhone = false;
                    }

                    if (IsValidPhone)
                    {
                        ShowPhoneError = false;
                        ShowSignUp = false;
                        ShowInfoCustomer = true;
                        ShowDoneButton = true;
                    }
                    else
                    {
                        if (Helper.IsPhoneNumber(PhoneNumber))
                        {
                            ShowPhoneError = true;
                            ShowInfoCustomer = false;
                            ShowDoneButton = false;
                            NameSignUp = "";
                            EmailSignUp = "";
                        }
                        else
                        {
                            CustomMessageBox.ShowOk("Số điện thoại không hợp lệ", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    CustomMessageBox.ShowOk("Số điện thoại không được để trống", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Error);
                }
                if (ListVoucher != null)
                {
                    ListVoucher.Clear();
                }

            });
            OpenSignUpCM = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                ShowSignUp = true;

            });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                FirtShow();

            });
            SignUpCM = new RelayCommand<object>((p) => { return true; },
            async (p) =>
            {
                CustomerDTO customer = new CustomerDTO();
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    if (Helper.IsPhoneNumber(PhoneNumber))
                    {
                        if (!string.IsNullOrEmpty(NameSignUp))
                        {
                            if (string.IsNullOrEmpty(EmailSignUp))
                            {
                                customer.PhoneNumber = PhoneNumber;
                                customer.CustomerName = NameSignUp;
                                IsSaving = true;

                                (bool successAddCustomer, string messageFromAddCustomer, string newCustomer) = await CustomerService.Ins.CreateNewCustomer(customer);
                                IsSaving = false;


                                if (successAddCustomer)
                                {
                                    CustomMessageBox.ShowOk(messageFromAddCustomer, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                                    NameSignUp = "";
                                    EmailSignUp = "";
                                    UpdateAddCustomer();
                                }
                                else
                                {
                                    CustomMessageBox.ShowOk(messageFromAddCustomer, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                if (RegexUtilities.IsValidEmail(EmailSignUp))
                                {
                                    customer.PhoneNumber = PhoneNumber;
                                    customer.CustomerName = NameSignUp;
                                    customer.Email = EmailSignUp;
                                    (bool successAddCustomer, string messageFromAddCustomer, string newCustomer) = await CustomerService.Ins.CreateNewCustomer(customer);
                                    if (successAddCustomer)
                                    {
                                        CustomMessageBox.ShowOk(messageFromAddCustomer, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                                        NameSignUp = "";
                                        EmailSignUp = "";
                                        UpdateAddCustomer();
                                    }
                                    else
                                    {
                                        CustomMessageBox.ShowOk(messageFromAddCustomer, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    CustomMessageBox.ShowOk("Email không hợp lệ", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                                }
                            }
                        }
                        else
                        {
                            CustomMessageBox.ShowOk("Vui lòng nhập họ và tên", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        CustomMessageBox.ShowOk("Số điện thoại không hợp lệ", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                }
                else
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập số điện thoại", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                }

            });
            AddVoucherOnlyFoodCM = new RelayCommand<object>((p) => { return true; },
            async (p) =>
            {
                if (!string.IsNullOrEmpty(VoucherID))
                {
                    (string error, VoucherDTO voucher) = await VoucherService.Ins.GetVoucherInfo(VoucherID);
                    if (error == null)
                    {
                        if (ListVoucher.Count == 0)
                        {
                            if (voucher.VoucherInfo.MinimizeTotal <= LastPrice)
                            {
                                ListVoucher.Add(voucher);
                                VoucherID = "";
                                Discount += (float)voucher.Price;
                                DiscountStr = Helper.FormatVNMoney(Discount);
                                LastPriceStr = Helper.FormatVNMoney(TotalFood - Discount);
                                LastPrice = TotalFood - Discount;
                            }
                            else
                            {
                                CustomMessageBox.ShowOk("Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney((float)voucher.VoucherInfo.MinimizeTotal) + " trở lên", "Cảnh báo","OK",Views.CustomMessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            if (voucher.EnableMerge && ListVoucher[0].EnableMerge == false)
                            {
                                CustomMessageBox.ShowOk("Voucher " + ListVoucher[0].VoucherCode + " không được dùng với các voucher khác", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                            }
                            else
                            {
                                if (voucher.TypeObject == "Vé xem phim")
                                {
                                    CustomMessageBox.ShowOk("Voucher không áp dụng cho đồ ăn", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                }
                                else
                                {
                                    if (!voucher.EnableMerge)
                                    {
                                        CustomMessageBox.ShowOk("Voucher này không được dùng với các voucher khác", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < ListVoucher.Count; i++)
                                        {
                                            if (ListVoucher[i].VoucherReleaseId == voucher.VoucherReleaseId)
                                            {
                                                CustomMessageBox.ShowOk("Voucher cùng đợt phát hành", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                                return;
                                            }
                                        }
                                        if (voucher.VoucherInfo.MinimizeTotal <= LastPrice)
                                        {
                                            ListVoucher.Add(voucher);
                                            VoucherID = "";
                                            Discount += (float)voucher.Price;
                                            DiscountStr = Helper.FormatVNMoney(Discount);
                                            LastPriceStr = Helper.FormatVNMoney(TotalFood - Discount);
                                            LastPrice = TotalFood - Discount;
                                        }
                                        else
                                        {
                                            CustomMessageBox.ShowOk("Voucher chỉ áp dụng cho hóa đơn từ \" + Helper.FormatVNMoney((float)voucher.VoucherInfo.MinimizeTotal) + \" trở lên", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(error, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                }
                else
                {
                    CustomMessageBox.ShowOk("Mã voucher rỗng", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                }
            });
            AddVoucherNoFoodCM = new RelayCommand<object>((p) => { return true; },
            async (p) =>
            {
                if (!string.IsNullOrEmpty(VoucherID))
                {
                    (string error, VoucherDTO voucher) = await VoucherService.Ins.GetVoucherInfo(VoucherID);
                    if (error == null)
                    {
                        if (ListVoucher.Count == 0)
                        {
                            if (voucher.VoucherInfo.MinimizeTotal <= LastPrice)
                            {
                                ListVoucher.Add(voucher);
                                VoucherID = "";
                                Discount += (float)voucher.Price;
                                DiscountStr = Helper.FormatVNMoney(Discount);
                                LastPriceStr = Helper.FormatVNMoney(TotalFullMoviePrice - Discount);
                                LastPrice = TotalFullMoviePrice - Discount;
                            }
                            else
                            {
                                CustomMessageBox.ShowOk("Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney((float)voucher.VoucherInfo.MinimizeTotal) + " trở lên", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            if (voucher.EnableMerge && ListVoucher[0].EnableMerge == false)
                            {
                                CustomMessageBox.ShowOk("Voucher " + ListVoucher[0].VoucherCode + " không được dùng với các voucher khác", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                            }
                            else
                            {
                                if (voucher.TypeObject == "Sản phẩm")
                                {
                                    CustomMessageBox.ShowOk("Voucher không áp dụng cho phim", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                }
                                else
                                {
                                    if (!voucher.EnableMerge)
                                    {
                                        CustomMessageBox.ShowOk("Voucher này không được dùng với các voucher khác", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < ListVoucher.Count; i++)
                                        {
                                            if (ListVoucher[i].VoucherReleaseId == voucher.VoucherReleaseId)
                                            {
                                                CustomMessageBox.ShowOk("Voucher cùng đợt phát hành", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                                return;
                                            }
                                        }
                                        if (voucher.VoucherInfo.MinimizeTotal <= LastPrice)
                                        {
                                            ListVoucher.Add(voucher);
                                            VoucherID = "";
                                            Discount += (float)voucher.Price;
                                            DiscountStr = Helper.FormatVNMoney(Discount);
                                            LastPriceStr = Helper.FormatVNMoney(TotalFullMoviePrice - Discount);
                                            LastPrice = TotalFullMoviePrice - Discount;
                                        }
                                        else
                                        {
                                            CustomMessageBox.ShowOk("Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney((float)voucher.VoucherInfo.MinimizeTotal) + " trở lên", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(error, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                }
                else
                {
                    CustomMessageBox.ShowOk("Mã voucher rỗng", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                }
            });
            AddVoucherCM = new RelayCommand<object>((p) => { return true; },
            async (p) =>
            {
                if (!string.IsNullOrEmpty(VoucherID))
                {
                    (string error, VoucherDTO voucher) = await VoucherService.Ins.GetVoucherInfo(VoucherID);
                    if (error == null)
                    {
                        if (ListVoucher.Count == 0)
                        {
                            if (voucher.VoucherInfo.MinimizeTotal <= LastPrice)
                            {
                                ListVoucher.Add(voucher);
                                VoucherID = "";
                                Discount += (float)voucher.Price;
                                DiscountStr = Helper.FormatVNMoney(Discount);
                                LastPriceStr = Helper.FormatVNMoney(Total - Discount);
                                LastPrice = Total - Discount;
                            }
                            else
                            {
                                CustomMessageBox.ShowOk("Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney((float)voucher.VoucherInfo.MinimizeTotal) + " trở lên", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            if (voucher.EnableMerge && ListVoucher[0].EnableMerge == false)
                            {
                                CustomMessageBox.ShowOk("Voucher " + ListVoucher[0].VoucherCode + " không được dùng với các voucher khác", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                            }
                            else
                            {
                                if (!voucher.EnableMerge)
                                {
                                    CustomMessageBox.ShowOk("Voucher này không được dùng với các voucher khác", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                }
                                else
                                {
                                    for (int i = 0; i < ListVoucher.Count; i++)
                                    {
                                        if (ListVoucher[i].VoucherReleaseId == voucher.VoucherReleaseId)
                                        {
                                            CustomMessageBox.ShowOk("Voucher cùng đợt phát hành", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                            return;
                                        }
                                    }
                                    if (voucher.VoucherInfo.MinimizeTotal <= LastPrice)
                                    {
                                        ListVoucher.Add(voucher);
                                        VoucherID = "";
                                        Discount += (float)voucher.Price;
                                        DiscountStr = Helper.FormatVNMoney(Discount);
                                        LastPriceStr = Helper.FormatVNMoney(Total - Discount);
                                        LastPrice = Total - Discount;
                                    }
                                    else
                                    {
                                        CustomMessageBox.ShowOk("Voucher chỉ áp dụng cho hóa đơn từ " + Helper.FormatVNMoney((float)voucher.VoucherInfo.MinimizeTotal) + " trở lên", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(error, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                }
                else
                {
                    CustomMessageBox.ShowOk("Mã voucher rỗng", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                }
            });
            DeleteVoucherCM = new RelayCommand<object>((p) => { return true; },
            (p) =>
                {
                    if (SelectedItem != null)
                    {
                        VoucherDTO temp = SelectedItem;
                        Discount -= (float)temp.Price;
                        LastPrice += (float)temp.Price;
                        LastPriceStr = Helper.FormatVNMoney(LastPrice);
                        DiscountStr = Helper.FormatVNMoney(Discount);
                        ListVoucher.Remove(SelectedItem);
                    }

                });
            PayFoodCM = new RelayCommand<object>((p) => { return true; },
            async (p) =>
            {
                try
                {
                    IsSaving = true;
                    List<ProductBillInfoDTO> productBills = new List<ProductBillInfoDTO>();
                    for (int i = 0; i < ListFood.Count; i++)
                    {
                        ProductBillInfoDTO temp = new ProductBillInfoDTO();
                        temp.ProductId = ListFood[i].Id;
                        temp.Quantity = ListFood[i].Quantity;
                        temp.ProductName = ListFood[i].ProductName;
                        temp.PrizePerProduct = ListFood[i].Price;
                        productBills.Add(temp);
                    }
                    BillDTO bill = new BillDTO();
                    if (!IsWalkinGuest)
                    {
                        bill.CustomerId = customerDTO.Id;
                    }
                    bill.StaffId = Staff.Id;
                    bill.TotalPrice = LastPrice;
                    bill.DiscountPrice = Discount;
                    bill.VoucherIdList = ListVoucher.Select(v => v.Id).ToList();
                    (bool successBooking, string messageFromBooking) = await BookingService.Ins.CreateProductOrder(bill, productBills);
                    if (successBooking)
                    {
                        IsSaving = false;
                        CustomMessageBox.ShowOk(messageFromBooking, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                        StaffWindow tk = Application.Current.Windows.OfType<StaffWindow>().FirstOrDefault();
                        tk.Content.Content = new OrderFoodPage();
                    }
                    else
                    {
                        IsSaving = false;
                        CustomMessageBox.ShowOk(messageFromBooking, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(e.Message, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                }
            });
            PayFullCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    try
                    {
                        IsSaving = true;
                        List<ProductBillInfoDTO> productBills = new List<ProductBillInfoDTO>();
                        for (int i = 0; i < ListFood.Count; i++)
                        {
                            ProductBillInfoDTO temp = new ProductBillInfoDTO();
                            temp.ProductId = ListFood[i].Id;
                            temp.Quantity = ListFood[i].Quantity;
                            temp.ProductName = ListFood[i].ProductName;
                            temp.PrizePerProduct = ListFood[i].Price;
                            productBills.Add(temp);
                        }
                        List<TicketDTO> tickets = new List<TicketDTO>();
                        for (int i = 0; i < ListSeat.Count; i++)
                        {
                            TicketDTO temp = new TicketDTO();
                            temp.ShowtimeId = Showtime.Id;
                            temp.SeatId = (int)ListSeat[i].SeatId;
                            temp.Price = Showtime.Price;
                            tickets.Add(temp);
                        }
                        BillDTO bill = new BillDTO();
                        if (!IsWalkinGuest)
                        {
                            bill.CustomerId = customerDTO.Id;
                        }
                        bill.StaffId = Staff.Id;
                        bill.TotalPrice = LastPrice;
                        bill.DiscountPrice = Discount;

                        bill.VoucherIdList = ListVoucher.Select(v => v.Id).ToList();
                        (bool successBooking, string messageFromBooking) = await BookingService.Ins.CreateFullOptionBooking(bill, tickets, productBills);
                        if (successBooking)
                        {
                            IsSaving = false;
                            CustomMessageBox.ShowOk(messageFromBooking, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                            TicketWindow ticketWindow = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                            MovieScheduleWindow movieScheduleWindow = Application.Current.Windows.OfType<MovieScheduleWindow>().FirstOrDefault();
                            ticketWindow.Close();
                            movieScheduleWindow.Close();
                        }
                        else
                        {
                            IsSaving = false;
                            CustomMessageBox.ShowOk(messageFromBooking, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                        }

                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        CustomMessageBox.ShowOk(e.Message, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                });

            PayMovieCM = new RelayCommand<object>((p) => { return true; },
                async (p) =>
                {
                    try
                    {
                        IsSaving = true;
                        List<TicketDTO> tickets = new List<TicketDTO>();
                        for (int i = 0; i < ListSeat.Count; i++)
                        {
                            TicketDTO temp = new TicketDTO();
                            temp.ShowtimeId = Showtime.Id;
                            temp.SeatId = (int)ListSeat[i].SeatId;
                            temp.Price = Showtime.Price;
                            tickets.Add(temp);
                        }
                        BillDTO bill = new BillDTO();
                        if (!IsWalkinGuest)
                        {
                            bill.CustomerId = customerDTO.Id;
                        }
                        bill.StaffId = Staff.Id;
                        bill.TotalPrice = LastPrice;
                        bill.DiscountPrice = Discount;
                        bill.VoucherIdList = ListVoucher.Select(v => v.Id).ToList();
                        (bool successBooking, string messageFromBooking) = await BookingService.Ins.CreateTicketBooking(bill, tickets);
                        if (successBooking)
                        {
                            IsSaving = false;
                            CustomMessageBox.ShowOk(messageFromBooking, "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                            TicketWindow ticketWindow = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                            MovieScheduleWindow movieScheduleWindow = Application.Current.Windows.OfType<MovieScheduleWindow>().FirstOrDefault();
                            ticketWindow.Close();
                            movieScheduleWindow.Close();
                        }
                        else
                        {
                            IsSaving = false;
                            CustomMessageBox.ShowOk(messageFromBooking, "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                        }
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }

                });

        }
        public async void UpdateAddCustomer()
        {
            CustomerDTO customer = await CustomerService.Ins.FindCustomerInfo(PhoneNumber);
            
            Name = customer.CustomerName;
            Email = customer.Email;
            ShowPhoneError = false;
            ShowSignUp = false;
            ShowInfoCustomer = true;
            ShowDoneButton = true;
            customerDTO = customer;
        }
        private void FirtShow()
        {
            Film = TicketWindowViewModel.tempFilm;
            Showtime = TicketWindowViewModel.CurrentShowtime;
            ListSeat = TicketWindowViewModel.WaitingList;
            MovieName = Film.FilmName;
            Date = Showtime.ShowDate.ToString("dd/MM/yyyy");
            CaculateTime();
            Time = start.ToString("HH:mm") + " - " + end.ToString("HH:mm");
            Seat = ListSeat[0].SeatPosition;
            for (int i = 1; i < ListSeat.Count; i++)
            {
                Seat += ", " + ListSeat[i].SeatPosition;
            }
            Room = "0" + Showtime.RoomId.ToString();
            Price = Helper.FormatVNMoney(Showtime.Price);
            TotalPriceMovie = Helper.FormatVNMoney(Showtime.Price * ListSeat.Count);
            TotalFullMoviePrice = Showtime.Price * ListSeat.Count;
            IsBookMovie = true;
        }
    }
}
