using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Admin.CustomerManagement;
using CinemaManagementProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity.Spatial;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.CustomerManagementVM
{   
    public class CustomerManagementViewModel : BaseViewModel
    {
        ListView listView;
        int selectedyear;
        private bool IsSaving = false;

        #region Biến lưu dữ liệu thêm

        private string _Fullname;
        public string Fullname
        {
            get { return _Fullname; }
            set { _Fullname = value; OnPropertyChanged(); }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; OnPropertyChanged(); }
        }

        private string _Mail;
        public string Mail
        {
            get { return _Mail; }
            set { _Mail = value; OnPropertyChanged(); }
        }

        #endregion
        private ObservableCollection<CustomerDTO> _customerList;
        public ObservableCollection<CustomerDTO> CustomerList
        {

            get => _customerList;
            set
            {
                _customerList = value;
                OnPropertyChanged();
            }
        }

        private bool _IsGettingSource;
        public bool IsGettingSource
        {
            get { return _IsGettingSource; }
            set { _IsGettingSource = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedPeriod;
        public ComboBoxItem SelectedPeriod
        {
            get { return _SelectedPeriod; }
            set { _SelectedPeriod = value; OnPropertyChanged(); }
        }

        private string _SelectedTime;
        public string SelectedTime
        {
            get { return _SelectedTime; }
            set { _SelectedTime = value; OnPropertyChanged(); }
        }

        private CustomerDTO _SelectedItem;
        public CustomerDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        public ICommand EditCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }

        public ICommand OpenEditCustomerCM { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand GetListViewCommand { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand ChangePeriodCM { get; set; }



        public CustomerManagementViewModel()
        {
           
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                listView = p;
            });

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                selectedyear = DateTime.Now.Year;
                CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCustomer()));
            });

            ChangePeriodCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsGettingSource = true;
                if (SelectedPeriod != null)
                {
                    switch (SelectedPeriod.Tag.ToString())
                    {
                        case "Theo năm":
                            {
                                if (SelectedPeriod != null)
                                {
                                    await LoadSourceByYear();
                                    IsGettingSource = false;
                                }
                                return;
                            }
                        case "Theo tháng":
                            {
                                if (SelectedPeriod != null)
                                {
                                    await LoadSourceByMonth();
                                    IsGettingSource = false;
                                }
                                return;
                            }
                        case "Toàn bộ":
                            {
                                if (SelectedPeriod != null)
                                {
                                    CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCustomer()));
                                    IsGettingSource = false;
                                }
                                return;
                            }
                    }
                }

            });

            EditCustomerCommand = new RelayCommand<Window>((p) =>
            {
                if (IsSaving)
                {
                    return false;
                }
                return true;
            }, async (p) =>
            {
                IsSaving = true;

                await EditCustomer(p);
                CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCustomer()));
                IsSaving = false;
            });

            DeleteCustomerCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {

               CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(Properties.Settings.Default.isEnglish == false?"Bạn có chắc muốn xoá khách hàng này không?": "Are you sure you want to delete this customer?", Properties.Settings.Default.isEnglish == false ? "Cảnh báo":"Warning", "OK", "Cancel", Views.CustomMessageBoxImage.Warning);
                if (kq == CustomMessageBoxResult.OK)
                {
                    IsGettingSource = true;

                    (bool isSuccess, string messageFromUpdate) = await CustomerService.Ins.DeleteCustomer(SelectedItem.Id);

                    IsGettingSource = false;

                    if (isSuccess)
                    {
                        LoadCustomerListView(Utils.Operation.DELETE);
                        if (Properties.Settings.Default.isEnglish == false)
                            CustomMessageBox.ShowOk("Xóa thành công!", "Thông báo", "OK", CustomMessageBoxImage.Success);
                        else CustomMessageBox.ShowOk("Delete successful!", "Notify", "OK", CustomMessageBoxImage.Success);

                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromUpdate, Properties.Settings.Default.isEnglish == false? "Thông báo": "Notify", "OK", CustomMessageBoxImage.Error);

                    }
                }
            });

            OpenEditCustomerCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                EditCustomerWindow wd = new EditCustomerWindow();
                Fullname = SelectedItem.CustomerName;
                Phone = SelectedItem.PhoneNumber;
                Mail = SelectedItem.Email;
                wd.ShowDialog();
            });
            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (p != null)
                {
                    p.Close();
                }
            });
        }
        public async Task LoadSourceByYear()
        {
            if (SelectedTime is null) return;
            if (SelectedTime.Length != 4) return;
            try
            {
                CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(int.Parse(SelectedTime.ToString())));
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                if(Properties.Settings.Default.isEnglish == false)
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish == false)
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("System Error", "Error", "OK", CustomMessageBoxImage.Error);
            }
            selectedyear = int.Parse(SelectedTime.ToString());

        }
        public async Task LoadSourceByMonth()
        {
            if (SelectedTime is null) return;
            if (SelectedTime.ToString().Length == 4) return;
            try
            { 
                if (SelectedTime == "Tháng 1"|| SelectedTime == "January")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 1));
                if (SelectedTime == "Tháng 2" || SelectedTime == "February")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 2));
                if (SelectedTime == "Tháng 3" || SelectedTime == "March")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 3));
                if (SelectedTime == "Tháng 4" || SelectedTime == "April")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 4));
                if (SelectedTime == "Tháng 5" || SelectedTime == "May")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 5));
                if (SelectedTime == "Tháng 6" || SelectedTime == "June")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 6));
                if (SelectedTime == "Tháng 7" || SelectedTime == "July")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 7));
                if (SelectedTime == "Tháng 8" || SelectedTime == "August")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 8));
                if (SelectedTime == "Tháng 9" || SelectedTime == "September")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 9));
                if (SelectedTime == "Tháng 10" || SelectedTime == "October")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 10));
                if (SelectedTime == "Tháng 11" || SelectedTime == "November")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 11));
                if (SelectedTime == "Tháng 12" || SelectedTime == "December")
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCustomerByTime(selectedyear, 12));
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish == false)
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish == false)
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("System Error", "Error", "OK", CustomMessageBoxImage.Error);
            }
        }
        public async Task EditCustomer(Window p)
        {
            if (!string.IsNullOrEmpty(Mail))
            {
                if (!Utils.RegexUtilities.IsValidEmail(Mail))
                {
                    if (Properties.Settings.Default.isEnglish == false)
                        CustomMessageBox.ShowOk("Email không hợp lệ", "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
                    else CustomMessageBox.ShowOk("Invalid email", "Warning", "OK", CustomMessageBoxImage.Warning);
                    
                    return;
                }
            }

            (bool isValid, string error) = IsValidData(Utils.Operation.UPDATE);
            if (isValid)
            {
                CustomerDTO cus = new CustomerDTO();
                cus.Id = SelectedItem.Id;
                cus.CustomerName = Fullname;
                cus.PhoneNumber = Phone;
                cus.Email = Mail;
                cus.Expense = SelectedItem.Expense;

                (bool isSuccess, string messageFromUpdate) = await CustomerService.Ins.UpdateCustomerInfo(cus);

                if (isSuccess)
                {
                    LoadCustomerListView(Utils.Operation.UPDATE, cus);
                    p.Close();
                    if (Properties.Settings.Default.isEnglish == false)
                        CustomMessageBox.ShowOk("Cập nhật thành công!", "Thông báo", "OK", CustomMessageBoxImage.Success);
                    else CustomMessageBox.ShowOk("Update successful!", "Thông báo", "OK", CustomMessageBoxImage.Success);
                    
                }
                else
                {
                    if (Properties.Settings.Default.isEnglish == false)
                        CustomMessageBox.ShowOk(messageFromUpdate, "Thông báo", "OK", CustomMessageBoxImage.Error);
                    else CustomMessageBox.ShowOk(messageFromUpdate, "Warning", "OK", CustomMessageBoxImage.Error);
                }
            }
            else
            {
                if (Properties.Settings.Default.isEnglish == false)
                    CustomMessageBox.ShowOk(error, "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
                else CustomMessageBox.ShowOk(error, "Cảnh báo", "OK", CustomMessageBoxImage.Warning);

            }
        }

        private (bool valid, string error) IsValidData(Operation oper)
        {
            if (string.IsNullOrEmpty(Fullname))
            {
                return (false, Properties.Settings.Default.isEnglish == false? "Thông tin thiếu! Vui lòng bổ sung": "Missing information! Please add");
            }
            if (!Helper.IsPhoneNumber(Phone))
            {
                return (false, Properties.Settings.Default.isEnglish == false ? "Số điện thoại không hợp lệ" : "Invalid phone number");
            }
            return (true, null);
        }
        public void LoadCustomerListView(Operation oper, CustomerDTO cus = null)
        {
            switch (oper)
            {
                case Operation.UPDATE:
                    var cusfound = CustomerList.FirstOrDefault(c => c.Id == cus.Id);
                    CustomerList[CustomerList.IndexOf(cusfound)] = cus;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < CustomerList.Count; i++)
                    {
                        if (CustomerList[i].Id == SelectedItem?.Id)
                        {
                            CustomerList.Remove(CustomerList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
