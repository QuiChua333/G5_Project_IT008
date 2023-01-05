using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using CinemaManagementProject.View.Staff.Trouble;

namespace CinemaManagementProject.ViewModel.StaffVM.TroubleStaffVM
{
    public partial class TroublePageViewModel : BaseViewModel
    {
        public static ObservableCollection<TroubleDTO> GetAllTrouble { get; set; }

        private ObservableCollection<TroubleDTO> _troubleList;
        public ObservableCollection<TroubleDTO> TroubleList
        {
            get => _troubleList;
            set { _troubleList = value; OnPropertyChanged(); }
        }

        private TroubleDTO _SelectedItem;
        public TroubleDTO SelectedItem
        {
            get => _SelectedItem;
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _filtercbbItem;
        public ComboBoxItem FiltercbbItem
        {
            get => _filtercbbItem;
            set { _filtercbbItem = value; OnPropertyChanged(); }
        }


        private TroubleDTO _ErrorDevice;
        public TroubleDTO ErrorDevice
        {
            get => _ErrorDevice;
            set { _ErrorDevice = value; OnPropertyChanged(); }
        }

        private ObservableCollection<StaffDTO> _ListStaff;
        public ObservableCollection<StaffDTO> ListStaff
        {
            get => _ListStaff;
            set { _ListStaff = value; OnPropertyChanged(); }
        }

        private string _troubleType;
        public string TroubleType
        {
            get => _troubleType;
            set { _troubleType = value; OnPropertyChanged(); }
        }

        private string _troubleStatus;
        public string TroubleStatus
        {
            get => _troubleStatus;
            set { _troubleStatus = value; OnPropertyChanged(); }
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set { _Description = value; OnPropertyChanged(); }
        }

        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _Level;
        public ComboBoxItem Level
        {
            get => _Level;
            set { _Level = value; OnPropertyChanged(); }
        }

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged(); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }


        public ICommand CancelCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand FilterListTroubleCommand { get; set; }
        public ICommand LoadDetailTroubleWindowCM { get; set; }
        public ICommand OpenAddTroubleCommand { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand MouseMoveCommand { get; set; }

        string filepath;
        bool IsImageChanged = false;

        public static Grid MaskName { get; set; }
        public TroublePageViewModel()
        {
            GetCurrentDate = System.DateTime.Today;
            CancelCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p != null)
                {
                    p.Close();
                }
            });
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await LoadTroubleList();
                await LoadListStaff();
                IsLoading = false;

            });
            FilterListTroubleCommand = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, (p) =>
            {
                FilterTroubleList();
            });
            LoadDetailTroubleWindowCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                if (SelectedItem is null) return;

                if (SelectedItem.TroubleStatus == Utils.STATUS.IN_PROGRESS || SelectedItem.TroubleStatus == Utils.STATUS.DONE)
                {
                    ViewTroubleInformation w = new ViewTroubleInformation();
                    w.ShowDialog();
                    return;
                }
                if (SelectedItem.TroubleStatus == Utils.STATUS.CANCLE)
                {
                    return;
                }
            });
            OpenAddTroubleCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                AddTroubleWindow w1 = new AddTroubleWindow();
                w1.StaffName.Text = StaffVM.currentStaff.StaffName;
                w1.ShowDialog();
            });
            SaveTroubleCM = new RelayCommand<AddTroubleWindow>((p) => { return !IsSaving; }, async (p) =>
            {
                IsSaving = true;
                await SaveTroubleFunc(p);
                IsSaving = false;
            });
            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    IsImageChanged = true;
                    filepath = openfile.FileName;
                    LoadImage();
                    return;
                }
                IsImageChanged = false;

            });
            LoadEditTroubleWindowCM = new RelayCommand<EditTroubleWindow>((p) => { return true; }, (p) =>
            {
                EditTroubleWindow w1 = new EditTroubleWindow();

                LoadEditTrouble(w1);
                w1.ShowDialog();
            });
            UpdateTroubleWindowCM = new RelayCommand<EditTroubleWindow>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateTroubleFunc(p);
                isSaving = false;
            });

           
            CloseCM = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, (p) =>
            {
                SelectedItem = null;
                p.Close();
            });
            MouseMoveCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });
        }

        public async Task LoadTroubleList()
        {
            //Khởi tạo danh sách lỗi và giá trị biến tham số
            TroubleList = new ObservableCollection<TroubleDTO>();
            GetAllTrouble = new ObservableCollection<TroubleDTO>();
            ErrorDevice = new TroubleDTO();
            IsImageChanged = false;

            //Lấy dữ liệu cho ListError
            await GetData();
        }
        public void FilterTroubleList()
        {
            TroubleList = new ObservableCollection<TroubleDTO>();
            
            if (FiltercbbItem.Tag.ToString() == "Toàn bộ")
            {   
                if (Properties.Settings.Default.isEnglish == false)
                    TroubleList = new ObservableCollection<TroubleDTO>(GetAllTrouble);
                else
                {
                    foreach (TroubleDTO troubleDTO in GetAllTrouble)
                    {
                        TroubleDTO newTB = new TroubleDTO
                        {
                            Id = troubleDTO.Id,
                            TroubleType = troubleDTO.TroubleType,
                            Description = troubleDTO.Description,
                            RepairCost = troubleDTO.RepairCost,
                            SubmittedAt = troubleDTO.SubmittedAt,
                            StartDate = troubleDTO.StartDate,
                            FinishDate = troubleDTO.FinishDate,
                            StaffId = troubleDTO.StaffId,
                            Image = troubleDTO.Image,
                            StaffName = troubleDTO.StaffName
                        };
                        if (troubleDTO.Level == "Bình thường") newTB.Level = "Normal";
                        if (troubleDTO.Level == "Nghiêm trọng") newTB.Level = "Serious";
                        if (troubleDTO.TroubleStatus == "Chờ tiếp nhận") newTB.TroubleStatus = "Waiting";
                        if (troubleDTO.TroubleStatus == "Đang giải quyết") newTB.TroubleStatus = "Solving";
                        if (troubleDTO.TroubleStatus == "Đã giải quyết") newTB.TroubleStatus = "Solved";
                        if (troubleDTO.TroubleStatus == "Đã hủy") newTB.TroubleStatus = "Cancelled";
                        TroubleList.Add(newTB);
                    }
                    
                }
            }
            else
            {
                if (Properties.Settings.Default.isEnglish == false)
                    TroubleList = new ObservableCollection<TroubleDTO>(GetAllTrouble.Where(tr => tr.TroubleStatus == FiltercbbItem.Tag.ToString()));
                else
                {
                    foreach (TroubleDTO troubleDTO in GetAllTrouble)
                    {
                        if (troubleDTO.TroubleStatus == FiltercbbItem.Tag.ToString())
                        {
                            TroubleDTO newTB = new TroubleDTO();
                            newTB.Id = troubleDTO.Id;
                            newTB.TroubleType = troubleDTO.TroubleType;
                            newTB.Description = troubleDTO.Description;
                            newTB.RepairCost = troubleDTO.RepairCost;
                            newTB.SubmittedAt = troubleDTO.SubmittedAt;
                            newTB.StartDate = troubleDTO.StartDate;
                            newTB.FinishDate = troubleDTO.FinishDate;
                            newTB.StaffId = troubleDTO.StaffId;
                            newTB.Image = troubleDTO.Image;
                            newTB.StaffName = troubleDTO.StaffName;
                            if (troubleDTO.Level == "Bình thường") newTB.Level = "Normal";
                            if (troubleDTO.Level == "Nghiêm trọng") newTB.Level = "Serious";
                            if (troubleDTO.TroubleStatus == "Chờ tiếp nhận") newTB.TroubleStatus = "Waiting";
                            if (troubleDTO.TroubleStatus == "Đang giải quyết") newTB.TroubleStatus = "Solving";
                            if (troubleDTO.TroubleStatus == "Đã giải quyết") newTB.TroubleStatus = "Solved";
                            if (troubleDTO.TroubleStatus == "Đã hủy") newTB.TroubleStatus = "Cancelled";
                            TroubleList.Add(newTB);

                        }
                    }
                }
            }
        }
        public async Task GetData()
        {
            GetAllTrouble = new ObservableCollection<TroubleDTO>(await Task.Run(() => TroubleService.Ins.GetAllTrouble()));
            TroubleList = new ObservableCollection<TroubleDTO>();
            if (Properties.Settings.Default.isEnglish == false)
                TroubleList = new ObservableCollection<TroubleDTO>(GetAllTrouble);
            else
            {
                foreach (TroubleDTO troubleDTO in GetAllTrouble)
                {
                    TroubleDTO newTB = new TroubleDTO
                    {
                        Id = troubleDTO.Id,
                        TroubleType = troubleDTO.TroubleType,
                        Description = troubleDTO.Description,
                        RepairCost = troubleDTO.RepairCost,
                        SubmittedAt = troubleDTO.SubmittedAt,
                        StartDate = troubleDTO.StartDate,
                        FinishDate = troubleDTO.FinishDate,
                        StaffId = troubleDTO.StaffId,
                        Image = troubleDTO.Image,
                        StaffName = troubleDTO.StaffName
                    };
                    if (troubleDTO.Level == "Bình thường") newTB.Level = "Normal";
                    if (troubleDTO.Level == "Nghiêm trọng") newTB.Level = "Serious";
                    if (troubleDTO.TroubleStatus == "Chờ tiếp nhận") newTB.TroubleStatus = "Waiting";
                    if (troubleDTO.TroubleStatus == "Đang giải quyết") newTB.TroubleStatus = "Solving";
                    if (troubleDTO.TroubleStatus == "Đã giải quyết") newTB.TroubleStatus = "Solved";
                    if (troubleDTO.TroubleStatus == "Đã hủy") newTB.TroubleStatus = "Cancelled";
                    TroubleList.Add(newTB);
                }
            }
        }
        public void RenewWindowData()
        {
            TroubleType = null;
            Description = null;
            ImageSource = null;
            Level = null;
            filepath = null;
        }

        public async Task LoadListStaff()
        {
            ListStaff = new ObservableCollection<StaffDTO>(await StaffService.Ins.GetAllStaff());
        }
        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(TroubleType)
                     && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Level.Tag.ToString());
        }
        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filepath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;
        }
        Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
    }
}
