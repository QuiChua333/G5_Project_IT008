using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CinemaManagementProject.Views;
using static CinemaManagementProject.Views.CustomMessageBoxWindow;

namespace CinemaManagementProject
{
    public static class CustomMessageBox
    {
        
        public static CustomMessageBoxResult ShowOk(string messageBoxText, string caption, string okButtonText)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(messageBoxText, caption, MessageBoxButton.OK);
            msg.OkButtonSingleText = okButtonText;
            msg.ShowDialog();
            return msg.Result;
        }

       
        public static CustomMessageBoxResult ShowOk(string messageBoxText, string caption, string okButtonText, CustomMessageBoxImage icon)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(messageBoxText, caption, MessageBoxButton.OK, icon);
            msg.OkButtonSingleText = okButtonText;
            msg.ShowDialog();
            return msg.Result;
        }

      
        public static CustomMessageBoxResult ShowOkCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(messageBoxText, caption, MessageBoxButton.OKCancel);
            msg.OkButtonText = okButtonText;
            msg.CancelButtonText = cancelButtonText;

            msg.ShowDialog();
            return msg.Result;
        }

       
        public static CustomMessageBoxResult ShowOkCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText, CustomMessageBoxImage icon)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(messageBoxText, caption, MessageBoxButton.OKCancel, icon);
            msg.OkButtonText = okButtonText;
            msg.CancelButtonText = cancelButtonText;
            msg.ShowDialog();
            return msg.Result;
        }
    }
}
