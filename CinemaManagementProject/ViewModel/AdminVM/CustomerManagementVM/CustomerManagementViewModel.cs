using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using CinemaManagementProject.Model.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ICommand GetListViewCommand { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public CustomerManagementViewModel()
        {
           
            GetListViewCommand = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                listView = p;
            });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                CustomerList = new ObservableCollection<CustomerDTO>();
                var c = new CinemaManagementProjectEntities();
                var cuslist = c.Customers;
                int i = 1;
                foreach(var cus in cuslist)
                {
                    CustomerDTO dto = new CustomerDTO();
                    dto.Id = i;
                    dto.Name = cus.CustomerName;
                    dto.PhoneNumber = (int)cus.PhoneNumber;
                    dto.Email = cus.Email;
                    CustomerList.Add(dto);
                    i++;
                }
            });
        }
    }
}
