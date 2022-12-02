﻿using CinemaManagementProject.Utilities;
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
        public ICommand FoodCommand { get; set; }
        private void Food(object obj) => CurrentView = new FoodManagementVM.FoodManagementVM();
        public AdminVM()
        {
            FoodCommand = new RelayCommand(Food);

            //StartPage
            _currentView = new FoodManagementVM.FoodManagementVM();
        }
    }
}