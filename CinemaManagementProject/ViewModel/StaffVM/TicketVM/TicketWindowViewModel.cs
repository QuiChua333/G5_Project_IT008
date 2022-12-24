using CinemaManagementProject.View.Admin.FoodManagement;
using CinemaManagementProject.View.Staff.TicketWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CinemaManagementProject.View.Staff.OrderFoodManagement;
using CinemaManagementProject.DTOs;
using CinemaManagementProject.ViewModel.StaffVM.TicketBillVM;

namespace CinemaManagementProject.ViewModel.StaffVM.TicketVM
{
    public partial class TicketWindowViewModel : BaseViewModel
    {
        public ICommand CloseTicketWindowCM { get; set; }
        public ICommand MinimizeTicketWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand LoadFoodPageCM { get; set; }
        public ICommand LoadTicketBookingPageCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public TicketWindowViewModel()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await GenerateSeat();
                CaculateTime();
                Output_ToString();
                ReCalculate();
                
                seatQuantity = ListSeat.Count.ToString();
            });
            CloseTicketWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DataContext = new TicketWindowViewModel();
                    w.Close();
                }
            });
            MinimizeTicketWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.WindowState = WindowState.Minimized;
                }
            });
            MouseMoveWindowCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = Window.GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
            SelectedSeatCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                Label lb = p.Children.OfType<Label>().FirstOrDefault();
                Image img = p.Children.OfType<Image>().FirstOrDefault();
                if (lb != null)
                {
                   
                    if (IsExist(lb.Content.ToString()))
                    {
                        
                        img.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/isReady.png"));
                        WaitingSeatList(lb);
                        currChoose = WaitingList.Count;
                        return;
                    }
                    if (WaitingList.Count + 1 > 7)
                    {
                        CustomMessageBox.ShowOk("Bạn chỉ được đặt tối đa 7 ghế!", "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
           
                        return;
        }
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/currChoose.png"));
                    WaitingSeatList(lb);
                    currChoose = WaitingList.Count;
    }


            });
            LoadStatusSeatCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                Label lb = p.Children.OfType<Label>().FirstOrDefault();
                Image img = p.Children.OfType<Image>().FirstOrDefault();
                if (lb != null)
                {
                    foreach (var item in ListSeat)
                    {
                        if (item.SeatPosition == lb.Content.ToString() && item.SeatStatus == true)
                        {
                            img.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/isBooked.png"));
                            lb.Content = "";
                            p.IsEnabled = false;
                        }
                        if (item.SeatPosition == lb.Content.ToString() && item.SeatStatus == false)
                        {
                            img.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/isReady.png"));
                        }

                    }
                    isBooked = ListSeat.Count(x => x.SeatStatus == true);
                    isReady = ListSeat.Count(x => x.SeatStatus == false); 
                }
            });
            LoadTicketBookingPageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                tk.TicketBookingFrame.Content = new TicketBookingPage();
            });

            LoadFoodPageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (WaitingList.Count == 0)
                {
                    CustomMessageBox.ShowOk("Vui lòng chọn ghế trước khi sang bước tiếp theo", "Cảnh báo", "Ok", Views.CustomMessageBoxImage.Warning);

                    return;
                }
                //if (OrderFoodPageViewModel.ListOrder != null)
                //{
                //    OrderFoodPageViewModel.ListOrder.Clear();
                //}
                TicketWindow tk = Application.Current.Windows.OfType<TicketWindow>().FirstOrDefault();
                tk.TicketBookingFrame.Content = new OrderFoodPage();
            });
        }
    }

}
