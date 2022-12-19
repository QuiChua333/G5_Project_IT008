using CinemaManagementProject.DTOs;
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
        public static StaffDTO currentStaff;
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand ShowTimeViewCommand { get; set; }
        public ICommand CustomerViewCommand { get; set; }
        public ICommand HistoryViewCommand { get; set; }
        public ICommand StaffViewCommand { get; set; }
        
        public ICommand TroubleCommand { get; set; }
        private void ShowTime(object obj) => CurrentView = new ShowtimeManagementVM.ShowtimeMangementViewModel();
        private void Customer(object obj) => CurrentView = new CustomerManagementVM.CustomerManagementViewModel();
        private void History(object obj) => CurrentView = new Import_ExportManagementVM.Import_ExportManagementViewModel();
        private void Staff(object obj) => CurrentView = new StaffManagementVM.StaffManagementViewModel();

        private void Trouble(object obj) => CurrentView = new TroubleManagementVM.TroubleManagementViewModel();



        public AdminVM()
        {

            ShowTimeViewCommand = new RelayCommand(ShowTime);
            CustomerViewCommand = new RelayCommand(Customer);
            HistoryViewCommand = new RelayCommand(History);
            StaffViewCommand = new RelayCommand(Staff);
            TroubleCommand = new RelayCommand(Trouble);



        }
    }
}
