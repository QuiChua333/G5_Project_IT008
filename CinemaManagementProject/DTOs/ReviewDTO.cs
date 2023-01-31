using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace CinemaManagementProject.DTOs
{
    public class ReviewDTO:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        //public string ReviewDate { get; set; }
        public string BillCode { get; set; }
        public string FilmStar { get; set; }
        public string FilmReview { get; set; }
        public string CustomerName { get; set; }
        public string ShortName { get; set; }
        public  bool IsDeleted { get; set; }
        private bool isRespond;
        public bool IsRespond
        {
            get { return isRespond; }
            set { SetField(ref isRespond, value, "IsRespond"); }
        }
        public List<bool> _starList;
        public List<bool> StarList
        {
            get
            {
                return _starList;
            }
            set
            {
                SetField(ref _starList, value, "StarList");
            }
        }

        public ReviewDTO()
        {
            StarList = new List<bool>();
            for (int i = 0; i < 5; i++)
            {
                bool star = false;
                StarList.Add(star);
            }
        }

    }
}
