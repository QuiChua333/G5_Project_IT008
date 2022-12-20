using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM;
using CinemaManagementProject.ViewModel.StaffVM.TroubleStaffVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.StaffVM
{
    class StaffVM : BaseViewModel
    {

        public static StaffDTO currentStaff;
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand OrderFoodCommand { get; set; }
        public ICommand FilmBookingCommand { get; set; }
        private void OrderFood(object obj)
        {
            CurrentView = new OrderFoodManagementVM.OrderFoodManagementVM();
            OrderFoodManagementVM.OrderFoodManagementVM.checkOnlyFoodOfPage = true;
        }
        private void FilmBooking(object obj) => CurrentView = new FilmBookingVM.FilmBookingVM();
        private void Trouble(object obj) => CurrentView = new TroubleStaffVM.TroublePageViewModel();
        public StaffVM()
        {
            OrderFoodCommand = new RelayCommand(OrderFood);
            FilmBookingCommand = new RelayCommand(FilmBooking);
            TroubleCommand = new RelayCommand(Trouble);
            //StartPage
            _currentView = new FilmBookingVM.FilmBookingVM();
        }
    }
}
