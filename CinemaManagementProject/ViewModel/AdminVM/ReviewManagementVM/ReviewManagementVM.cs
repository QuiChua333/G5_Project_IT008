using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.Utils;
using CinemaManagementProject.View.Admin;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Json;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using MaterialDesignThemes.Wpf;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagementProject.ViewModel.AdminVM.ReviewManagementVM
{
    public partial class ReviewManagementVM:BaseViewModel
    {
        private ObservableCollection<ReviewDTO> _reviewFilmList;
        public ObservableCollection<ReviewDTO> ReviewFilmList
        {
            get { return _reviewFilmList; }
            set { _reviewFilmList = value; OnPropertyChanged(nameof(ReviewFilmList)); }
        }
        private ReviewDTO _selectedItem;
        public ReviewDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ImageBrush> _imageList;
        public ObservableCollection<ImageBrush> ImageList
        {
            get { return _imageList; }
            set { _imageList = value; OnPropertyChanged(); }
        }
        private ObservableCollection<TextBlock> _fileList;
        public ObservableCollection<TextBlock> FileList
        {
            get { return _fileList; }
            set { _fileList = value; OnPropertyChanged(); }
        }
        private TextBlock _fileSelection;
        public TextBlock FileSelection
        {
            get { return _fileSelection; }
            set { _fileSelection = value; OnPropertyChanged(); }
        }
        private ImageBrush _selectedImage;
        public ImageBrush SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; OnPropertyChanged(); }
        }
        
        private string _nameCustomerSelected;
        public string NameCustomerSelected
        {
            get { return _nameCustomerSelected; }
            set { _nameCustomerSelected = value; OnPropertyChanged(); }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }
        System.Windows.Controls.Border SendMailBox;
        private string EmailCustomerSelected;
        private string BillCodeCustomerSelected;
        private bool IsEnglish = Properties.Settings.Default.isEnglish;
        // Bien bool de xet theo thanh loc
        private bool IsDes = true;
        private bool IsTotalComment = true;
        private int TopSelected = 5;
        public ICommand FirstLoadCM { get; set; }
        public ICommand ShowMailBoxCustomerCommand { get; set; }
        public ICommand LoadImageToSendBoxCM { get; set; }
        public ICommand RemoveImageCM { get; set; }
        public ICommand DeleteReviewCM { get; set; }
        public ICommand SendEmailCM { get; set; }
        public ICommand HideMailBoxCM { get; set; }
        public ICommand LoadFileCM { get; set; }
        public ICommand RemoveFileCM { get; set; }
        public ICommand ReloadPageCM { get; set; }

        public ReviewManagementVM()
        {
            ItemClickCommand = new RelayCommand<RichTextBox>((p) => { return true; }, (p) =>
            {
                IsLoading = true;
                LoadPieChar();
                LoadReviewFilm();
                ResetValues(p);
                IsLoading = false;
            });
            FirstLoadCM = new RelayCommand<ListView>((p) => { return true; },async (p) =>
            {
                TopSelected = 5;
                IsEnglish = Properties.Settings.Default.isEnglish;
                if (IsEnglish)
                    TextTitleRank = "Top 5 movies have the most reviews";
                else
                    TextTitleRank = "Top 5 phim có lượt đánh giá nhiều nhất";
                SendMailBox = new System.Windows.Controls.Border();
                ReviewFilmList = new ObservableCollection<ReviewDTO>();
                ImageList = new ObservableCollection<ImageBrush>();
                FileList = new ObservableCollection<TextBlock>();
                FilmStarPie = new LiveCharts.SeriesCollection();
                IsLoading = true;
                await LoadTop5Film();
                IsLoading = false;
            });
            ReloadPageCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                TopSelected = 5;
                IsEnglish = Properties.Settings.Default.isEnglish;
                if (IsEnglish)
                    TextTitleRank = "Top 5 movies have the most reviews";
                else
                    TextTitleRank = "Top 5 phim có lượt đánh giá nhiều nhất";
                SendMailBox = new System.Windows.Controls.Border();
                ReviewFilmList = new ObservableCollection<ReviewDTO>();
                ImageList = new ObservableCollection<ImageBrush>();
                FileList = new ObservableCollection<TextBlock>();
                FilmStarPie = new LiveCharts.SeriesCollection();
                IsLoading = true;
                await LoadTop5Film();
                IsLoading = false;
            });
            ShowMailBoxCustomerCommand = new RelayCommand<System.Windows.Controls.Border>((p) => { return true; }, (p) =>
            {
                if(SelectedItem != null)
                {
                    using(CinemaManagementProjectEntities db = new CinemaManagementProjectEntities())
                    {
                        Bill bill = db.Bills.Find(SelectedItem.BillCode);
                        if (bill != null)
                        {
                            NameCustomerSelected = bill.Customer.CustomerName;
                            EmailCustomerSelected = bill.Customer.Email;
                            BillCodeCustomerSelected = bill.BillCode;
                        }    
                        else
                        {
                            NameCustomerSelected = "Unknow";
                            EmailCustomerSelected = "21522402@gm.uit.edu.vn";
                        }
                    }
                    SendMailBox = p;
                    p.Visibility = System.Windows.Visibility.Visible;
                }    
                
            });
            LoadImageToSendBoxCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == true)
                {
                    ImageBrush imageAdd = new ImageBrush();
                    imageAdd.ImageSource = LoadImage(openfile.FileName);
                    ImageList.Add(imageAdd);
                }
            });
            RemoveImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if(SelectedImage != null)
                    ImageList.Remove(SelectedImage);
            });
            DeleteReviewCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if(SelectedItem != null)
                {
                    string billCode = SelectedItem.BillCode;
                    ReviewFilmList.Remove(SelectedItem);
                    using(CinemaManagementProjectEntities db = new CinemaManagementProjectEntities())
                    {
                        Review rv = db.Reviews.Find(billCode);
                        if (rv == null)
                            return;
                        rv.IsDeleted = true;
                        db.SaveChanges();
                    }
                }    
            });
            SendEmailCM = new RelayCommand<RichTextBox>((p) => { return true; }, (p) =>
            {
                TextRange obj = new TextRange(p.Document.ContentStart, p.Document.ContentEnd);
                if (SendMailToCustomer(obj.Text))
                {
                    ReviewDTO rvdto = ReviewFilmList.FirstOrDefault(item => item.BillCode == BillCodeCustomerSelected);
                    rvdto.IsRespond = true;
                    using (CinemaManagementProjectEntities db = new CinemaManagementProjectEntities())
                    {
                        Review rv = db.Reviews.Find(BillCodeCustomerSelected);
                        if (rv == null)
                            return;
                        rv.IsRespond = true;
                        db.SaveChanges();
                    }
                    CustomMessageBox.ShowOk(IsEnglish ? "Mail sent successfully" : "Gửi mail thành công", IsEnglish ? "Success" : "Thành công", "Ok", Views.CustomMessageBoxImage.Success);
                }
            });
            HideMailBoxCM = new RelayCommand<RichTextBox>((p) => { return true; }, (p) =>
            {
                ResetValues(p);
            });
            LoadFileCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an file";
                openfile.Filter = "File (*.pdf;*.txt;*.doc;*.docx;*.mp3;*.mp4;*.xls;*.xlsx;*.ppt;*.pptx;)|*.pdf;*.txt;*.doc;*.docx;*.mp3;*.mp4;*.xls;*.xlsx;*.ppt;*.pptx";
                if (openfile.ShowDialog() == true)
                {
                    TextBlock tb = new TextBlock();
                    tb.Tag = openfile.FileName;
                    tb.Text = openfile.FileName.Substring(openfile.FileName.LastIndexOf('\\') + 1);
                    FileList.Add(tb);
                }
            });
            RemoveFileCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (FileSelection != null)
                    FileList.Remove(FileSelection);
            });
            ChangeOrderTypeCM = new RelayCommand<RadioButton>((p) => { return true; }, async (p) =>
            {
                if(p.Tag.ToString() == "Dsc")
                {
                    IsDes = true;
                    SetTextTitle();
                    IsLoading = true;
                    await LoadTop5FilmHasFilter();
                    IsLoading = false;
                }
                else
                {
                    IsDes = false;
                    SetTextTitle();
                    IsLoading = true;
                    await LoadTop5FilmHasFilter();
                    IsLoading = false;
                }    
            });
            ChangeReviewFormatCM = new RelayCommand<RadioButton>((p) => { return true; }, async (p) =>
            {
                if(p.Tag.ToString() == "LuotBinhLuan")
                {
                    IsTotalComment = true;
                    SetTextTitle();
                    IsLoading = true;
                    await LoadTop5FilmHasFilter();
                    IsLoading = false;
                }
                else
                {
                    IsTotalComment = false;
                    SetTextTitle();
                    IsLoading = true;
                    await LoadTop5FilmHasFilter();
                    IsLoading = false;
                }
            });
            ChangeSelectTopCM = new RelayCommand<RadioButton>((p) => { return true; },async (p) =>
            {
                TopSelected = int.Parse(p.Tag.ToString());
                SetTextTitle();
                IsLoading = true;
                await LoadTop5FilmHasFilter();
                IsLoading = false;
            });
        }
        public ImageSource LoadImage(string filePath)
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            return _image;
        }
        public bool SendMailToCustomer(string textBody)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string APP_EMAIL = appSettings["APP_EMAIL"];
                string APP_PASSWORD = appSettings["APP_PASSWORD"];

                //Tạo mail
                MailMessage mail = new MailMessage(APP_EMAIL, EmailCustomerSelected);
                mail.To.Add(EmailCustomerSelected);
                mail.Subject = IsEnglish ? "Thank you for your submittion" : "Cảm ơn vì đã đánh giá phim";
                //Attach file
                mail.IsBodyHtml = true;

                string htmlBody = GetHTMLTemplate(textBody);

                mail.Body = htmlBody;

                foreach(ImageBrush img in ImageList)
                {
                    string imagePath = img.ImageSource.ToString().Substring(8);
                    mail.Attachments.Add(new Attachment(imagePath));
                }
                foreach (TextBlock file in FileList)
                {
                    string FilePath = file.Tag.ToString();
                    mail.Attachments.Add(new Attachment(FilePath));
                }

                //tạo Server

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(APP_EMAIL, APP_PASSWORD);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "Ok", Views.CustomMessageBoxImage.Error);
                return false;
            }
        }

        public void LoadReviewFilm()
        {
            IsLoading = true;
            ReviewFilmList = ReviewService.Ins.GetAllReviewOf(FilmSelected);
            IsLoading = false;
        }
        public void ResetValues(RichTextBox p)
        {
            p.Document.Blocks.Clear();
            NameCustomerSelected = String.Empty;
            FileList.Clear();
            ImageList.Clear();
            SendMailBox.Visibility = Visibility.Collapsed;
        }
        public string GetHTMLTemplate(string TextSend)
        {
            string resetPasswordTemplate = Helper.GetEmailTemplatePath(THANKYOU_FILE);
            string HTML = File.ReadAllText(resetPasswordTemplate).Replace("{Content_Here}", TextSend);
            return HTML;
        }
        const string THANKYOU_FILE = "ThankYouTemplate.txt";
    }
}
