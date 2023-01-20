using CinemaManagementProject.DTOs;
using CinemaManagementProject.View.Admin.HelpScreen;
using CinemaManagementProject.View.Admin.MovieManagement;
using CinemaManagementProject.View.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.HelpScreenVM
{
    public class HelpScreenVM : BaseViewModel
    {
        public ICommand CloseCM { get; set; }
        public ICommand Load_General_Term { get; set; }
        public ICommand Load_Trading_Terms { get; set; }
        public ICommand Load_Frequently_asked_questions { get; set; }
        public ICommand Load_Privacy_Policy { get; set; }

        public HelpScreenVM()
        {
            Load_General_Term = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new General_Term();
                w1.ShowDialog();
            });
            Load_Trading_Terms = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new Trading_Terms();
                w1.ShowDialog();
            });
            Load_Frequently_asked_questions = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new Frequently_asked_questions();
                w1.ShowDialog();
            });
            Load_Privacy_Policy = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new Privacy_Policy();
                w1.ShowDialog();
            });
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
