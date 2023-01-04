using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Staff.TicketWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CinemaManagementProject.ViewModel.StaffVM.TicketVM
{
    public partial class TicketWindowViewModel : BaseViewModel
    {

        public static ShowtimeDTO CurrentShowtime;
        public static FilmDTO tempFilm;
        public static string showTimeRoom;
        public static List<Label> listlabel = new List<Label>();
        public static bool IsEnglish = false;

        public ICommand SelectedSeatCM { get; set; }
        public ICommand LoadStatusSeatCM { get; set; }
        public ICommand SetStatusSeatCM { get; set; }
       

        #region Biến binding

        private int _currChoose;
        public int currChoose
        {
            get { return _currChoose; }
            set { _currChoose = value; OnPropertyChanged(); }
        }
        private int _isBooked;
        public int isBooked
        {
            get { return _isBooked; }
            set { _isBooked = value; OnPropertyChanged(); }
        }
        private int _isReady;
        public int isReady
        {
            get { return _isReady; }
            set { _isReady = value; OnPropertyChanged(); }
        }
        private string _price;
        public string price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }

        private SeatSettingDTO _SelectedSeat;
        public SeatSettingDTO SelectedSeat
        {
            get { return _SelectedSeat; }
            set { _SelectedSeat = value; OnPropertyChanged(); }
        }

        private string _seatQuantity;
        public string seatQuantity
        {
            get { return _seatQuantity; }
            set { _seatQuantity = value; OnPropertyChanged(); }
        }

        private string _showTimeRoomNumber;
        public string showTimeRoomNumber
        {
            get { return _showTimeRoomNumber; }
            set { _showTimeRoomNumber = value; OnPropertyChanged(); }
        }

        private string _showTime;
        public string showTime
        {
            get { return _showTime; }
            set { _showTime = value; OnPropertyChanged(); }
        }

        private string _startTime;
        public string startTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(); }
        }

        private string _endTime;
        public string endTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(); }
        }

        private string _txtFilm;
        public string txtFilm
        {
            get { return _txtFilm; }
            set { _txtFilm = value; OnPropertyChanged(); }
        }

        private string _showDateBefore;
        public string showDateBefore
        {
            get { return _showDateBefore; }
            set { _showDateBefore = value; OnPropertyChanged(); }
        }

        private string _showDateAfter;
        public string showDateAfter
        {
            get { return _showDateAfter; }
            set { _showDateAfter = value; OnPropertyChanged(); }
        }

        private ImageSource _imgSourceFilm;
        public ImageSource imgSourceFilm
        {
            get { return _imgSourceFilm; }
            set { _imgSourceFilm = value; OnPropertyChanged(); }
        }

        private List<SeatSettingDTO> _ListSeat;
        public List<SeatSettingDTO> ListSeat
        {
            get { return _ListSeat; }
            set { _ListSeat = value; OnPropertyChanged(); }
        }

        private List<SeatSettingDTO> _ListStatusSeat;
        public List<SeatSettingDTO> ListStatusSeat
        {
            get { return _ListStatusSeat; }
            set
            {
                _ListStatusSeat = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SeatSettingDTO> _ListSeat1;
        public ObservableCollection<SeatSettingDTO> ListSeat1
        {
            get { return _ListSeat1; }
            set { _ListSeat1 = value; OnPropertyChanged(); }
        }
        private ObservableCollection<SeatSettingDTO> _ListSeat2;
        public ObservableCollection<SeatSettingDTO> ListSeat2
        {
            get { return _ListSeat2; }
            set { _ListSeat2 = value; OnPropertyChanged(); }
        }

        private  static List<SeatSettingDTO> _WaitingList;
        public static List<SeatSettingDTO> WaitingList
        {
            get { return _WaitingList; }
            set { _WaitingList = value; }
        }

        private string _totalPrice;
        public string TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; OnPropertyChanged(); }
        }

        private string _totalSeat;
        public string TotalSeat
        {
            get { return _totalSeat; }
            set { _totalSeat = value; OnPropertyChanged(); }
        }

        #endregion

        public async Task GenerateSeat()
        {


            ListSeat = new List<SeatSettingDTO>(await SeatService.Ins.GetSeatsByShowtime(CurrentShowtime.Id));
            ListStatusSeat = new List<SeatSettingDTO>();
            ListSeat1 = new ObservableCollection<SeatSettingDTO>();
            ListSeat2 = new ObservableCollection<SeatSettingDTO>();
            WaitingList = new List<SeatSettingDTO>();

            //if (OrderFoodManagementVM.IsBacking)
            //{
            //    OrderFoodManagementVM.IsBacking = false;
            //    TicketBookingPage tk = new TicketBookingPage();
            //    tk.SeatListBox1.ItemTemplate.LoadContent();

            //}

            foreach (var item in ListSeat)
            {
                if (item.SeatPosition.Length == 2 && item.SeatPosition[1] < '4')
                {
                    ListSeat1.Add(item);
                }
                else
                {
                    ListSeat2.Add(item);
                }
                if (item.SeatStatus)
                    ListStatusSeat.Add(item);
            }
            
            
        }
        public void WaitingSeatList(Label p)
        {
            string id = p.Content.ToString();
            if (!string.IsNullOrEmpty(id))
            {
                foreach (var item in WaitingList)
                {
                    if (item.SeatPosition == id)
                    {
                        WaitingList.RemoveAll(r => r.SeatPosition == id);
                        ReCalculate();
                        return;
                    }
                }
                WaitingList.Add(SelectedSeat);
                ReCalculate();
            }
        }

        public void ReCalculate(SeatSettingDTO seat = null)
        {
            float totalprice = 0;
            foreach (var item in WaitingList)
            {
                totalprice += CurrentShowtime.Price;
            }

            TotalPrice = Helper.FormatVNMoney(totalprice);


            TotalSeat = "";
            for (int i = 0; i < WaitingList.Count; i++)
            {
                if (WaitingList[i] is null)
                {
                    return;
                }
                if (i == 0)
                    TotalSeat += WaitingList[i].SeatPosition;
                else
                    TotalSeat += ", " + WaitingList[i].SeatPosition;
            }
        }

        public bool IsExist(string id)
        {
            foreach (var item in WaitingList)
            {
                if (item.SeatPosition == id) return true;

            }
            return false;
        }

        DateTime start, end;
        public void CaculateTime()
        {
            start = CurrentShowtime.ShowDate;
            start = start.Add(CurrentShowtime.StartTime);
            end = start.AddMinutes(tempFilm.DurationFilm);
        }

        public async void Output_ToString()
        {
            showTimeRoomNumber = showTimeRoom;
            imgSourceFilm = await CloudinaryService.Ins.LoadImageFromURL(tempFilm.Image);
            txtFilm = tempFilm.FilmName;
            startTime = CurrentShowtime.StartTime.ToString("hh\\:mm");
            endTime = end.ToString("HH:mm");
            showTime = startTime + " - " + endTime;
            showDateAfter = start.ToString("dd-MM-yyyy");
            showDateBefore = end.ToString("dd-MM-yyyy");
            price = Helper.FormatVNMoney(CurrentShowtime.Price);
        }
    }

}
