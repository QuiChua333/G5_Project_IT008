using CinemaManagementProject.DTOs;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using CinemaManagementProject.ViewModel.AdminVM.ReviewManagementVM;
using System.CodeDom;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Controls;
using System.Net;

namespace CinemaManagementProject.Model.Service
{
    internal class ReviewService
    {
        String spreadsheetId = "1_0hkhetacAu0wtteSSKDZW7d0eWgW2lSTGCCft5fGrM";
        String range = "Review!A2:E";
        string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        string ApplicationName = "Read Data Google SpeadSheet";
        UserCredential credential;

        private ReviewService() {

            using (var stream = new FileStream("../../Resource/Json/client_secret_260965131938-ipbesmblrqmtvv0u92dcdq4r9icc4l7e.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
        }
        private static ReviewService _ins;

        public static ReviewService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new ReviewService();
                return _ins;
            }
            private set { _ins = value; }
        }

        public ObservableCollection<ReviewDTO> GetAllReviewOf(FilmStatistical filmSelected)
        {
            ObservableCollection<ReviewDTO> ReviewFilmList = new ObservableCollection<ReviewDTO>();
            try
            {
                using (CinemaManagementProjectEntities db = new CinemaManagementProjectEntities())
                {
                    if (filmSelected == null)
                        return ReviewFilmList;

                    foreach (var review in db.Reviews)
                    {
                        if (review.FilmId == filmSelected.FilmId)
                        {
                            ReviewDTO ItemReview = new ReviewDTO();
                            ItemReview.BillCode = review.BillCode.ToString();
                            ItemReview.FilmStar = review.Star.ToString();
                            ItemReview.FilmReview = review.Comment.ToString();
                            ItemReview.CustomerName = review.Bill.Customer.CustomerName;
                            ItemReview.ShortName = ToShortName(ItemReview.CustomerName);
                            ItemReview.IsDeleted = (bool)review.IsDeleted;
                            ItemReview.IsRespond = (bool)review.IsRespond;
                            for (int i = 0; i < int.Parse(ItemReview.FilmStar); i++)
                                ItemReview.StarList[i] = true;
                            if(!ItemReview.IsDeleted)
                                ReviewFilmList.Add(ItemReview);
                        }
                    }
                }
                if (ReviewFilmList.Count == 0)
                    CustomMessageBox.ShowOk("Không có dữ liệu đánh giá", "Cảnh báo", "Ok", Views.CustomMessageBoxImage.Warning);
                return ReviewFilmList;
            }
            catch(System.InvalidOperationException e )
            {
                CustomMessageBox.ShowOk("Dữ liệu đang được cập nhật trên máy chủ, vui lòng thử lại", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                return ReviewFilmList;
            }
            catch (EntityException e)
            {
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                return ReviewFilmList;
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                return ReviewFilmList;
            }
        }
        public async Task<(bool,List<FilmStatistical>)> GetTop5FilmReview()
        {
            List < FilmStatistical > listReviewFilmStatical = new List< FilmStatistical >();
            try
            {
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;
                if (values != null && values.Count > 0)
                {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        foreach (var row in values)
                        {
                            Bill bill = await context.Bills.FindAsync(row[1].ToString());
                            if (bill != null)
                            {
                                Review review = await context.Reviews.FindAsync(row[1].ToString());
                                if (review == null)
                                {
                                    review = new Review();
                                    review.BillCode = row[1].ToString();
                                    review.IsDeleted = false;
                                    review.IsRespond = false;
                                    review.Star = int.Parse(row[2].ToString());
                                    review.Comment = row[3].ToString();

                                    var item = context.ShowTimes.Find(context.Tickets.FirstOrDefault(tk => tk.BillCode == bill.BillCode).ShowTimeId).Film;
                                    review.FilmId = item.Id;
                                    review.FilmName = item.FilmName;

                                    context.Reviews.Add(review);
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                        listReviewFilmStatical = await Task.Run(()=> context.Reviews.GroupBy(b => new { b.FilmId, b.FilmName })
                                                                .Select(gr => new FilmStatistical
                                                                { 
                                                                    FilmId = gr.Key.FilmId,
                                                                    Name = gr.Key.FilmName,
                                                                    TotalReview = gr.Count(),
                                                                    BillCodes = gr.Select(item => item.BillCode).ToList(),
                                                                }).OrderByDescending(o => o.TotalReview).Take(5).ToList());
                        foreach (var item in listReviewFilmStatical)
                            item.SetValues(values);
                    }
                }
                    return (true,listReviewFilmStatical); 
            }
            catch (System.InvalidOperationException e)
            {
                CustomMessageBox.ShowOk("Dữ liệu đang được cập nhật trên máy chủ, vui lòng thử lại", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                return (false, listReviewFilmStatical);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return (false,listReviewFilmStatical);
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                return (false, null);
            }
        }
        public async Task<(bool, List<FilmStatistical>)> GetTop5FilmReview(bool IsDes, bool IsTotalComment, int TopSelected)
        {
            List<FilmStatistical> listReviewFilmStatical = new List<FilmStatistical>();
            try
            {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        listReviewFilmStatical = await Task.Run(() => context.Reviews.GroupBy(b => new { b.FilmId, b.FilmName })
                                                                .Select(gr => new FilmStatistical
                                                                {
                                                                    FilmId = gr.Key.FilmId,
                                                                    Name = gr.Key.FilmName,
                                                                    TotalReview = gr.Count(),
                                                                    BillCodes = gr.Select(item => item.BillCode).ToList(),
                                                                }).ToList());
                        foreach (var item in listReviewFilmStatical)
                            item.SetValues();
                        if (IsTotalComment)
                        {
                            if(IsDes)
                                if(TopSelected == 0)
                                    listReviewFilmStatical = listReviewFilmStatical.OrderByDescending(o => o.TotalReview).ToList();
                                else
                                    listReviewFilmStatical = listReviewFilmStatical.OrderByDescending(o => o.TotalReview).Take(TopSelected).ToList();
                            else
                                if (TopSelected == 0)
                                    listReviewFilmStatical = listReviewFilmStatical.OrderBy(o => o.TotalReview).ToList();
                                else
                                    listReviewFilmStatical = listReviewFilmStatical.OrderBy(o => o.TotalReview).Take(TopSelected).ToList();
                        }    
                        else
                        {
                            if (IsDes)
                                if (TopSelected == 0)
                                    listReviewFilmStatical = listReviewFilmStatical.OrderByDescending(o => o.AverageStar).ToList();
                                else
                                    listReviewFilmStatical = listReviewFilmStatical.OrderByDescending(o => o.AverageStar).Take(TopSelected).ToList();
                            else
                                if (TopSelected == 0)
                                    listReviewFilmStatical = listReviewFilmStatical.OrderBy(o => o.AverageStar).ToList();
                                else
                                    listReviewFilmStatical = listReviewFilmStatical.OrderBy(o => o.AverageStar).Take(TopSelected).ToList();
                        }    
                    
                }
                return (true,listReviewFilmStatical);
            }
            catch (System.InvalidOperationException e)
            {
                CustomMessageBox.ShowOk("Dữ liệu đang được cập nhật trên máy chủ, vui lòng thử lại", "Cảnh báo", "OK", Views.CustomMessageBoxImage.Warning);
                return (false, listReviewFilmStatical);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return (false, listReviewFilmStatical);
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                return (false, null);
            }
        }
        public DateTime toDateTime(string stringDT)
        {
            string[] dateAndTime = stringDT.Split(' ');
            string[] date = dateAndTime[0].Split('/');
            string[] time = dateAndTime[1].Split(':');
            int year = int.Parse(date[2]);
            int month = int.Parse(date[0]);
            int day = int.Parse(date[1]);
            int hour = int.Parse(time[0]);
            int minute = int.Parse(time[1]);
            int second = int.Parse(time[2]);
            DateTime DT = new DateTime(year, month, day, hour, minute, second);
            return DT;
        }
        public string ToShortName(string longName)
        {
            string[] trimNames = longName.Split(' ');
            return trimNames[trimNames.Length - 1][0].ToString() + trimNames[0][0].ToString();
        }
    }
}
