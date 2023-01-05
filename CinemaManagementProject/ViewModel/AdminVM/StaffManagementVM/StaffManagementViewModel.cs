using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Admin.CustomerManagement;
using CinemaManagementProject.View.Admin.StaffManagement;
using CinemaManagementProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementViewModel:BaseViewModel
    {
        ListView listView;

        #region Biến lưu dữ liệu thêm

        private string _Fullname;
        public string Fullname
        {
            get { return _Fullname; }
            set { _Fullname = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _Gender;
        public ComboBoxItem Gender
        {
            get { return _Gender; }
            set { _Gender = value; OnPropertyChanged(); }
        }

        private Nullable<System.DateTime> _Born;
        public Nullable<System.DateTime> Born
        {
            get { return _Born; }
            set { _Born = value; OnPropertyChanged(); }
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
        private ComboBoxItem _Role;
        public ComboBoxItem Role
        {
            get { return _Role; }
            set { _Role = value; OnPropertyChanged(); }
        }

        private Nullable<System.DateTime> _StartDate;
        public Nullable<System.DateTime> StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged(); }
        }

        private string _TaiKhoan;
        public string TaiKhoan
        {
            get { return _TaiKhoan; }
            set { _TaiKhoan = value; OnPropertyChanged(); }
        }

        private string _MatKhau;
        public string MatKhau
        {
            get { return _MatKhau; }
            set { _MatKhau = value; OnPropertyChanged(); }
        }

        private string _RePass;
        public string RePass
        {
            get { return _RePass; }
            set { _RePass = value; OnPropertyChanged(); }
        }


        #endregion

        private ObservableCollection<StaffDTO> _staffList;
        public ObservableCollection<StaffDTO> StaffList
        {

            get => _staffList;
            set
            {
                _staffList = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetListViewCommand { get; set; }
        public ICommand GetPasswordCommand { get; set; }
        public ICommand GetRePasswordCommand { get; set; }
        public ICommand AddStaffCommand { get; set; }
        public ICommand EditStaffCommand { get; set; }
        public ICommand ChangePassCommand { get; set; }
        public ICommand DeleteStaffCommand { get; set; }

        public ICommand OpenAddStaffCommand { get; set; }
        public ICommand OpenEditStaffCommand { get; set; }
        public ICommand OpenChangePassCommand { get; set; }

        public ICommand MouseMoveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand FirstLoadCM { get; set; }

        private StaffDTO _SelectedItem;
        public StaffDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }

        public static Grid MaskName { get; set; }
        public StaffManagementViewModel()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    List<StaffDTO> staffDTOs = await StaffService.Ins.GetAllStaff();

                    StaffList = new ObservableCollection<StaffDTO>();

                    if (Properties.Settings.Default.isEnglish == false)
                        StaffList = new ObservableCollection<StaffDTO>(staffDTOs);
                    else
                    {
                        foreach(StaffDTO staff in staffDTOs)
                        {
                            StaffDTO newstaff = new StaffDTO();
                            newstaff.Id = staff.Id;
                            newstaff.MaNV = staff.MaNV;
                            newstaff.StaffName = staff.StaffName; 
                            newstaff.DateOfBirth = staff.DateOfBirth;
                            newstaff.Email = staff.Email;
                            newstaff.PhoneNumber = staff.PhoneNumber;
                            newstaff.StartDate = staff.StartDate;
                            newstaff.UserName = staff.UserName;
                            newstaff.UserPass = staff.UserPass;
                            newstaff.Avatar = staff.Avatar;
                            newstaff.BenefitContribution=staff.BenefitContribution;
                            if (staff.Gender == "Nam") newstaff.Gender = "Male";
                            else newstaff.Gender = "Female";
                            if (staff.Position == "Nhân viên") newstaff.Position = "Staff";
                            else newstaff.Position = "Manager";
                            StaffList.Add(newstaff);
                        }
                    }
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
            });
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; },
                (p) =>
                {
                    listView = p;
                });
            GetPasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; },
                (p) =>
                {
                    MatKhau = p.Password;
                });
            GetRePasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; },
                (p) =>
                {
                    RePass = p.Password;
                });

            AddStaffCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; },
                async (p) =>
                {
                    IsSaving = true;
                    await AddStaff(p);
                    IsSaving = false;
                });
            EditStaffCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; },
                async (p) =>
                {
                    IsSaving = true;
                    await EditStaff(p);
                    IsSaving = false;
                });
            ChangePassCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; },
                async (p) =>
                {
                    IsSaving = true;
                    await ChangePass(p);
                    IsSaving = false;
                });
            DeleteStaffCommand = new RelayCommand<Window>((p) => { return true; },
                 async (p) =>
                 {
                     var kq = CustomMessageBox.ShowOkCancel("Bạn có chắc muốn xoá nhân viên này không?", "Cảnh báo","OK", "Cancel",Views.CustomMessageBoxImage.Warning);
                     if (kq == CustomMessageBoxResult.OK)
                     {
                         (bool successDeleteStaff, string messageFromDeleteStaff) = await StaffService.Ins.DeleteStaff(SelectedItem.Id.ToString());
                         if (successDeleteStaff)
                         {
                             LoadStaffListView(Utils.Operation.DELETE);
                             if (Properties.Settings.Default.isEnglish == false)
                                 CustomMessageBox.ShowOk("Xóa thành công!", "Thông báo", "OK", CustomMessageBoxImage.Success);
                             else CustomMessageBox.ShowOk("Delete successful!", "Notify", "OK", CustomMessageBoxImage.Success);
                         }
                         else
                         {
                             if (Properties.Settings.Default.isEnglish == false)
                                 CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                             else CustomMessageBox.ShowOk("System Error", "Error", "OK", CustomMessageBoxImage.Error);
                         }
                     }
                 });

            OpenAddStaffCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    AddStaffWindow wd= new AddStaffWindow();
                    ResetData();
                    wd.ShowDialog();

                });
            OpenEditStaffCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    EditStaffWindow wd = new EditStaffWindow();
                    ResetData();

                    wd._FullName.Text = SelectedItem.StaffName;
                    wd.Gender.Text = SelectedItem.Gender;
                    wd.Date.Text = SelectedItem.DateOfBirth.ToString();
                    wd._Phone.Text = SelectedItem.PhoneNumber.ToString();
                    wd.Role.Text = SelectedItem.Position;
                    wd.StartDate.Text = SelectedItem.StartDate.ToString();
                    wd._TaiKhoan.Text = SelectedItem.UserName;
                    wd._Mail.Text = SelectedItem.Email;

                    Fullname = SelectedItem.StaffName;
                    Phone = SelectedItem.PhoneNumber;
                    TaiKhoan = SelectedItem.UserName;
                    Mail = SelectedItem.Email;

                    wd.ShowDialog();
                });

            OpenChangePassCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChangePasswordWindow wd = new ChangePasswordWindow();
                MatKhau = null;
                RePass = null;
                wd.ShowDialog();
            });

            CloseCommand = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;

                if (w != null)
                {
                    if (w is ChangePasswordWindow)
                    {
                        w.Close();
                        return;
                    }
                    w.Close();
                }
            }
            );

            MouseMoveCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            }
           );
        }

        private  void LoadStaffListView(Operation oper, StaffDTO staff = null)
        {

            switch (oper)
            {
                case Operation.CREATE:
                    StaffList.Add(staff);
                    break;
                case Operation.UPDATE:
                    var movieFound = StaffList.FirstOrDefault(s => s.Id == staff.Id);
                    StaffList[StaffList.IndexOf(movieFound)] = staff;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < StaffList.Count; i++)
                    {
                        if (StaffList[i].Id == SelectedItem?.Id)
                        {
                            StaffList.Remove(StaffList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        void ResetData()
        {
            StartDate = null;
            Fullname = null;
            Gender = null;
            Born = null;
            Role = null;
            Phone = null;
            TaiKhoan = null;
            MatKhau = null;
            Mail = null;
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
        private (bool valid, string error) IsValidData(Operation oper)
        {

            //if (string.IsNullOrEmpty(Fullname) || Gender is null || StartDate is null || Born is null || Role is null || string.IsNullOrEmpty(TaiKhoan))
            //{
            //    return (false, "Thông tin nhân viên thiếu! Vui lòng bổ sung");
            //}

            if (oper == Operation.CREATE || oper == Operation.UPDATE_PASSWORD)
            {
                if (string.IsNullOrEmpty(MatKhau))
                {
                    return (false, Properties.Settings.Default.isEnglish == false ? "Vui lòng nhập mật khẩu" : "Please enter a password");
                }
                if (MatKhau != RePass)
                    return (false, Properties.Settings.Default.isEnglish == false ? "Mật khẩu và mật khẩu nhập lại không trùng khớp!" : "Password and re-entered password do not match!");
            }

            (bool ageValid, string error) = ValidateAge((DateTime)Born);

            if (!ageValid)
            {
                return (false, error);
            }

            if (!Helper.IsPhoneNumber(Phone))
            {
                return (false, Properties.Settings.Default.isEnglish == false ? "Số điện thoại không hợp lệ": "Invalid phone number");
            }

            return (true, null);
        }
    }
}
    

