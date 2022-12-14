using CinemaManagementProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM
{
    public class AdminVM : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand VoucherCommand { get; set; }
        public ICommand ShowTimeViewCommand { get; set; }
        public ICommand CustomerViewCommand { get; set; }
        public ICommand HistoryViewCommand { get; set; }
        public ICommand StaffViewCommand { get; set; }
        public ICommand FilmViewCommand { get; set; }
        private void Voucher(object obj) => CurrentView = new VoucherManagementVM.VoucherViewModel();
        private void ShowTime(object obj) => CurrentView = new ShowtimeManagementVM.ShowtimeMangementViewModel();
        private void Customer(object obj) => CurrentView = new CustomerManagementVM.CustomerManagementViewModel();
        private void History(object obj) => CurrentView = new Import_ExportManagementVM.Import_ExportManagementViewModel();
        private void Staff(object obj) => CurrentView = new StaffManagementVM.StaffManagementViewModel();
        private void Film(object obj) => CurrentView = new MovieManagementVM.MovieManagementVM();



        public AdminVM()
        {
            VoucherCommand = new RelayCommand(Voucher);
            ShowTimeViewCommand = new RelayCommand(ShowTime);
            CustomerViewCommand = new RelayCommand(Customer);
            HistoryViewCommand = new RelayCommand(History);
            StaffViewCommand = new RelayCommand(Staff);
            FilmViewCommand = new RelayCommand(Film);
            _currentView = new VoucherManagementVM.VoucherViewModel();

        }
    }
}
