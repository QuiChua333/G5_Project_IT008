using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.StaffManagementVM
{
    internal class StaffManagementViewModel:BaseViewModel
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
        public ICommand FirstLoadCM { get; set; }
        public StaffManagementViewModel()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    //StaffList = new ObservableCollection<StaffDTO>(await StaffService.Ins.GetAllStaff());
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    //MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    //mb.ShowDialog();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    //mb.ShowDialog();
                }
            });
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; },
                (p) =>
                {
                    listView = p;
                });
        }
    }
}
