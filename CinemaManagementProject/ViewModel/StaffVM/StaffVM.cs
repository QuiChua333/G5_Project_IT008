using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utilities;
using CinemaManagementProject.ViewModel.AdminVM.FoodManagementVM;
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
        private void OrderFood(object obj) => CurrentView = new OrderFoodManagementVM.OrderFoodManagementVM();
        private void FilmBooking(object obj) => CurrentView = new FilmBookingVM.FilmBookingVM();
        public StaffVM()
        {
            OrderFoodCommand = new RelayCommand(OrderFood);
            FilmBookingCommand = new RelayCommand(FilmBooking);
            //StartPage
            _currentView = new OrderFoodManagementVM.OrderFoodManagementVM();
        }
    }
}
