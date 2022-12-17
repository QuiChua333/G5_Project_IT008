using CinemaManagementProject.View.Admin.FoodManagement;
using CinemaManagementProject.View.Staff.TicketWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CinemaManagementProject.ViewModel.StaffVM.TicketVM
{
    public partial class TicketWindowViewModel : BaseViewModel
    {
        public ICommand CloseTicketWindowCM { get; set; }
        public ICommand MinimizeTicketWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand LoadFoodPageCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public TicketWindowViewModel()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await GenerateSeat();
                CaculateTime();
                Output_ToString();
                seatQuantity = "128";
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
                    foreach (var st in ListStatusSeat)
                        if (lb.Content.ToString() == st.SeatPosition)
                        {
                            CustomMessageBox.ShowOk("Ghế này đã được đặt, vui lòng chọn ghế khác!", "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                            return;
                        }
                    if (IsExist(lb.Content.ToString()))
                    {

                        img.Source = new BitmapImage(new Uri("/CinemaManagementProject;component/Resource/Image/isReady.png"));
                        WaitingSeatList(lb);
                        return;
                    }
                    if (WaitingList.Count + 1 > 7)
                    {
                        CustomMessageBox.ShowOk("Bạn chỉ được đặt tối đa 7 ghế!", "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);

                        return;
                    }
                    img.Source = new BitmapImage(new Uri("/CinemaManagementProject;component/Resource/Image/currChoose.png"));
                    WaitingSeatList(lb);
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
                            img.Source = new BitmapImage(new Uri("/CinemaManagementProject;component/Resource/Image/isBooked.png"));
                            lb.Content = "";
                            return;
                        }

                    }
                }
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
                tk.TicketBookingFrame.Content = new FoodPage();
            });
        }
    }

}
