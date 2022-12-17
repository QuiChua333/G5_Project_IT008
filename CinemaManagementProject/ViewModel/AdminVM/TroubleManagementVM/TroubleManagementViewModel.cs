using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.TroubleManagementVM
{
    public class TroubleManagementViewModel : BaseViewModel
    {
        private bool _isGettingSource;
        public bool IsGettingSource
        {
            get { return _isGettingSource; }
            set { _isGettingSource = value;OnPropertyChanged(); }
        }

        private ComboBoxItem _selectedCbbItem;
        public ComboBoxItem SelectedCbbItem 
        { 
            get { return _selectedCbbItem; } 
            set { _selectedCbbItem = value;OnPropertyChanged(); }
        }
        private ObservableCollection<TroubleDTO> _troubleList;
        public ObservableCollection<TroubleDTO> TroubleList
        {
            get { return _troubleList; }
            set { _troubleList = value;OnPropertyChanged(); }
        }
        private TroubleDTO _selectedItem;
        public TroubleDTO SelectedItem
        {
            get { return _selectedItem; }   
            set { _selectedItem = value;OnPropertyChanged(); }
        }
        private ComboBoxItem _selectedStatus;
        public ComboBoxItem SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged(); }
        }

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; OnPropertyChanged(); }
        }

        private DateTime _SelectedFinishDate;
        public DateTime SelectedFinishDate
        {
            get { return _SelectedFinishDate; }
            set { _SelectedFinishDate = value; OnPropertyChanged(); }
        }

        private float _RepairCost;
        public float RepairCost
        {
            get { return _RepairCost; }
            set { _RepairCost = value; OnPropertyChanged(); }
        }
        private bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            set { _isSaving = value; OnPropertyChanged(); }
        }

        public ICommand ReloadTroubleListCM { get; set; }
        public ICommand LoadDetailTroubleCM { get; set; }
        public ICommand UpdateTroubleListCM { get;set; }

        public TroubleManagementViewModel()
        {
            LoadDetailTroubleCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChoseWindow();
            });
            ReloadTroubleListCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
            });
            UpdateTroubleListCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
               
            });
        }

        private void ChoseWindow()
        {
            
        }
    }
}
