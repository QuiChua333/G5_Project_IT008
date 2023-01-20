using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagementProject.ViewModel.AdminVM.ReviewManagementVM
{
    public partial class ReviewManagementVM:BaseViewModel
    {
        private SeriesCollection _filmStarPie;
        public SeriesCollection FilmStarPie
        {
            get { return _filmStarPie; }
            set { _filmStarPie = value; OnPropertyChanged(); }
        }
        private List<FilmStatistical> _top5Film;
        public List<FilmStatistical> Top5Film
        {
            get { return _top5Film; }
            set { _top5Film = value; OnPropertyChanged(); }
        }
        private FilmStatistical _filmSelected;
        public FilmStatistical FilmSelected
        {
            get { return _filmSelected; }
            set { _filmSelected = value; OnPropertyChanged(); }
        }
        private string _textTitleRank;
        public string TextTitleRank
        {
            get { return _textTitleRank; }
            set { _textTitleRank = value; OnPropertyChanged(); }
        }
        public ICommand ItemClickCommand { get; set; }
        public ICommand ChangeOrderTypeCM { get; set; }
        public ICommand ChangeReviewFormatCM { get; set; }
        public ICommand ChangeSelectTopCM { get; set; }

        public async Task LoadTop5Film()
        {
            try
            {
                List < FilmStatistical > listFilm = await Task.Run(() => ReviewService.Ins.GetTop5FilmReview());
                FilmSelected = listFilm[0];
                Top5Film = listFilm;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("System error", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }
        public async Task LoadTop5FilmHasFilter()
        {
            try
            {
                List<FilmStatistical> listFilm = await Task.Run(() => ReviewService.Ins.GetTop5FilmReview(IsDes, IsTotalComment, TopSelected));
                FilmSelected = listFilm[0];
                Top5Film = listFilm;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("System error", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }

        public void LoadPieChar()
        {
            try
            {
                if (FilmSelected == null)
                    return;

                FilmStarPie = new SeriesCollection();

                var listStarToPie = FilmSelected.CountStar();
                for (int i = 0; i < 5; i++)
                {
                    PieSeries p = new PieSeries
                    {
                        Values = new ChartValues<float> { listStarToPie[i] },
                        Title = (i + 1).ToString(),
                    };
                    FilmStarPie.Add(p);
                }
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("System error", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }
        public void SetTextTitle()
        {
            IsEnglish = Properties.Settings.Default.isEnglish;
            if (TopSelected == 0)
            {
                TextTitleRank = IsEnglish ? "All films have been rated" : "Tất cả phim được đánh giá ";
                return;
            }    
            if(IsDes)
                if(IsTotalComment)
                    TextTitleRank = "Top " + TopSelected.ToString() + (IsEnglish ? " movies have the most reviews" : " phim có lượt đánh giá nhiều nhất");
                else
                    TextTitleRank = "Top " + TopSelected.ToString() + (IsEnglish ? " movies are most appreciated" : " phim được đánh giá cao nhất");
            else
                if (IsTotalComment)
                    TextTitleRank = "Top " + TopSelected.ToString() + (IsEnglish ? " movies have the least reviews" : " phim có lượt đánh giá ít nhất");
                else
                    TextTitleRank = "Top " + TopSelected.ToString() + (IsEnglish ? " movies are underestimated" : " phim được đánh giá thấp nhất");
        }
    }
}
