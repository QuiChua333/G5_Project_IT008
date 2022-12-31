using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using CinemaManagementProject.View.Admin.HistoryManagement;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace CinemaManagementProject.ViewModel.AdminVM.Import_ExportManagementVM
{
    public class Import_ExportManagementViewModel : BaseViewModel
    {
        private bool isGettingSource;
        public bool IsGettingSource
        {
            get { return isGettingSource; }
            set { isGettingSource = value; OnPropertyChanged(); }
        }

        private DateTime _getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return _getCurrentDate; }
            set { _getCurrentDate = value; }
        }
        private string _setCurrentDate;
        public string SetCurrentDate
        {
            get { return _setCurrentDate; }
            set { _setCurrentDate = value; }
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _SelectedItemFilter;
        public ComboBoxItem SelectedItemFilter
        {
            get { return _SelectedItemFilter; }
            set { _SelectedItemFilter = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _SelectedImportItemFilter;
        public ComboBoxItem SelectedImportItemFilter
        {
            get { return _SelectedImportItemFilter; }
            set { _SelectedImportItemFilter = value; OnPropertyChanged(); }
        }
        private BillDTO _selectedTicketBill;
        public BillDTO SelectedTicketBill
        {
            get { return _selectedTicketBill; }
            set { _selectedTicketBill = value; OnPropertyChanged(); }
        }
        private BillDTO _billDetail;
        public BillDTO BillDetail
        {
            get { return _billDetail; }
            set { _billDetail = value; OnPropertyChanged(); }
        }

        private int _SelectedMonth;
        public int SelectedMonth
        {
            get { return _SelectedMonth; }
            set { _SelectedMonth = value; OnPropertyChanged(); }
        }

        private int _SelectedImportMonth;
        public int SelectedImportMonth
        {
            get { return _SelectedImportMonth; }
            set { _SelectedImportMonth = value; OnPropertyChanged(); }
        }

        public int SelectedView = 0;
        public static Grid MaskName { get; set; }

        private System.Windows.Controls.Label _ResultName;
        public System.Windows.Controls.Label ResultName
        {
            get { return _ResultName; }
            set { _ResultName = value; OnPropertyChanged(); }
        }


        public ICommand LoadImportPageCM { get; set; }
        public ICommand LoadExportPageCM { get; set; }
        public ICommand ExportFileCM { get; set; }
        public ICommand SelectedDateExportListCM { get; set; }
        public ICommand LoadInforBillCM { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand CheckItemFilterCM { get; set; }
        public ICommand CheckImportItemFilterCM { get; set; }
        public ICommand SelectedImportMonthCM { get; set; }
        public ICommand SelectedMonthCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand SaveResultNameCM { get; set; }

        private ObservableCollection<ProductReceiptDTO> _ListProduct;
        public ObservableCollection<ProductReceiptDTO> ListProduct
        {
            get { return _ListProduct; }
            set { _ListProduct = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BillDTO> _ListBill;
        public ObservableCollection<BillDTO> ListBill
        {
            get { return _ListBill; }
            set { _ListBill = value; OnPropertyChanged(); }
        }

        public Import_ExportManagementViewModel()
        {
            GetCurrentDate = DateTime.Today;
            SelectedDate = GetCurrentDate;
            SelectedMonth = DateTime.Now.Month - 1;
            SelectedImportMonth = DateTime.Now.Month - 1;

            SelectedMonthCM = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, async (p) =>
            {
                await CheckMonthFilter();
            });
            SelectedImportMonthCM = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, async (p) =>
            {
                await CheckImportMonthFilter();
            });
            CheckImportItemFilterCM = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, async (p) =>
            {
                await CheckImportItemFilter();
            });
            CheckItemFilterCM = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, async (p) =>
            {
                await CheckItemFilter();
            });
            SelectedDateExportListCM = new RelayCommand<DatePicker>((p) => { return true; }, async (p) =>
            {
                await GetExportListSource("date");
            });
            LoadImportPageCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                SelectedView = 0;
                IsGettingSource = true;
                ListProduct = new ObservableCollection<ProductReceiptDTO>();
                await GetImportListSource();
                IsGettingSource = false;
                ImportHistoryPage page = new ImportHistoryPage();
                p.Content = page;
            });
            LoadExportPageCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                SelectedView = 1;
                IsGettingSource = true;
                ListBill = new ObservableCollection<BillDTO>();
                await GetExportListSource("date");
                IsGettingSource = false;
                ExportHistoryPage page = new ExportHistoryPage();
                p.Content = page;

            });
            ExportFileCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ExportToFileFunc();
            });
            LoadInforBillCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedTicketBill != null)
                {
                    try
                    {
                        IsGettingSource = true;
                        BillDetail = await Task.Run(() => BillService.Ins.GetBillDetails(SelectedTicketBill.BillCode));
                        IsGettingSource = false;
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        Console.WriteLine(e);
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi","OK", Views.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                    }

                    if (BillDetail.TicketInfo is null)
                    {
                        ProductDetail w = new ProductDetail();
                        int sum = 0;
                        foreach (var item in BillDetail.ProductBillInfoes)
                        {
                            sum +=(int)item.Quantity *(int)item.PrizePerProduct;
                        }
                        w._totalproduct.Content = sum;
                        //MaskName.Visibility = System.Windows.Visibility.Visible;
                        w.ShowDialog();
                    }
                    else if (BillDetail.ProductBillInfoes.Count == 0)
                    {
                        TicketDetailsWindow w = new TicketDetailsWindow();
                        w._moviename.Content = BillDetail.TicketInfo.movieName;
                        w._price.Content = BillDetail.TicketInfo.PricePerTicketStr;
                        w._time.Content = BillDetail.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                        w._totalticket.Content = BillDetail.TicketInfo.TotalPriceTicketStr;
                        //MaskName.Visibility = System.Windows.Visibility.Visible;

                        w.ShowDialog();

                    }
                    else if (BillDetail.TicketInfo != null && BillDetail.ProductBillInfoes.Count != 0)
                    {
                        ExportDetail w = new ExportDetail();
                        w._moviename.Content = BillDetail.TicketInfo.movieName;
                        w._price.Content = BillDetail.TicketInfo.PricePerTicketStr;
                        w._time.Content = BillDetail.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                        w._totalticket.Content = BillDetail.TicketInfo.TotalPriceTicketStr;
                        //MaskName.Visibility = System.Windows.Visibility.Visible;

                        w.ShowDialog();
                    }
                }
            });
            //MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            //{
            //    MaskName = p;
            //});
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                //MaskName.Visibility = Visibility.Collapsed;
                SelectedTicketBill = null;
                p.Close();
            });
            SaveResultNameCM = new RelayCommand<System.Windows.Controls.Label>((p) => { return true; }, (p) =>
            {
                ResultName = p;
            });
        }

        public void ExportToFileFunc()
        {
            switch (SelectedView)
            {
                case 0:
                    {
                        using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                                app.Visible = false;
                                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];


                                ws.Cells[1, 1] = "Mã đơn";
                                ws.Cells[1, 2] = "Tên đơn";
                                ws.Cells[1, 3] = "Số lượng";
                                ws.Cells[1, 4] = "Tổng giá";
                                ws.Cells[1, 5] = "Nhân viên";
                                ws.Cells[1, 6] = "Ngày nhập";

                                int i2 = 2;
                                foreach (var item in ListProduct)
                                {

                                    ws.Cells[i2, 1] = item.Id;
                                    ws.Cells[i2, 2] = item.ProductName;
                                    ws.Cells[i2, 3] = item.Quantity;
                                    ws.Cells[i2, 4] = item.ImportPrice;
                                    ws.Cells[i2, 5] = item.StaffName;
                                    ws.Cells[i2, 6] = item.CreatedAt;

                                    i2++;
                                }
                                ws.SaveAs(sfd.FileName);
                                wb.Close();
                                app.Quit();

                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                                CustomMessageBox.ShowOk("Xuất file thành công", "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                            }
                        }
                        break;
                    }
                case 1:
                    {

                        using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                                app.Visible = false;
                                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];


                                ws.Cells[1, 1] = "Mã đơn";
                                ws.Cells[1, 2] = "Ngày xuất";
                                ws.Cells[1, 3] = "Khách hàng";
                                ws.Cells[1, 4] = "Số điện thoại";
                                ws.Cells[1, 5] = "Tổng giá";
                                ws.Cells[1, 6] = "Giảm giá";
                                ws.Cells[1, 7] = "Sau giảm giá";

                                int i2 = 2;
                                foreach (var item in ListBill)
                                {

                                    ws.Cells[i2, 1] = item.BillCode;
                                    ws.Cells[i2, 2] = item.CreatedAt;
                                    ws.Cells[i2, 3] = item.CustomerName;
                                    ws.Cells[i2, 4] = item.PhoneNumber;
                                    ws.Cells[i2, 5] = item.OriginalTotalPriceStr;
                                    ws.Cells[i2, 6] = item.DiscountPriceStr;
                                    ws.Cells[i2, 7] = item.TotalPriceStr;

                                    i2++;
                                }
                                ws.SaveAs(sfd.FileName);
                                wb.Close();
                                app.Quit();

                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                                CustomMessageBox.ShowOk("Xuất file thành công", "Thông báo", "OK", Views.CustomMessageBoxImage.Success);
                            }
                        }
                        break;
                    }
            }
        }
        public async Task GetImportListSource(string s = "")
        {
            ListProduct = new ObservableCollection<ProductReceiptDTO>();
            switch (s)
            {
                case "":
                    {
                        try
                        {
                            IsGettingSource = true;

                            ListProduct = new ObservableCollection<ProductReceiptDTO>(await ProductReceiptService.Ins.GetProductReceipt());
                            if (ResultName != null)
                                //ResultName.Content = ListProduct.Count;
                            IsGettingSource = false;
                            return;
                        }
                        catch (System.Data.Entity.Core.EntityException e)
                        {
                            Console.WriteLine(e);
                            CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                            throw;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                            throw;
                        }

                    }
                case "month":
                    {
                        IsGettingSource = true;
                        await CheckImportMonthFilter();
                        IsGettingSource = false;
                        return;
                    }

            }
        }
        public async Task GetExportListSource(string s = "")
        {
            ListBill = new ObservableCollection<BillDTO>();
            switch (s)
            {
                case "date":
                    {
                        try
                        {
                            IsGettingSource = true;
                            ListBill = new ObservableCollection<BillDTO>(await BillService.Ins.GetBillByDate(SelectedDate));
                            //ResultName.Content = ListBill.Count;
                            IsGettingSource = false;
                            return;
                        }
                        catch (System.Data.Entity.Core.EntityException e)
                        {
                            Console.WriteLine(e);
                            CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                            throw;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                            throw;
                        }

                    }
                case "":
                    {
                        try
                        {
                            IsGettingSource = true;
                            ListBill = new ObservableCollection<BillDTO>(await BillService.Ins.GetAllBill());
                            //ResultName.Content = ListBill.Count;
                            IsGettingSource = false;
                            return;
                        }
                        catch (System.Data.Entity.Core.EntityException e)
                        {
                            Console.WriteLine(e);
                            CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                            throw;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
                            throw;
                        }

                    }
                case "month":
                    {
                        IsGettingSource = true;
                        await CheckMonthFilter();
                        //ResultName.Content = ListBill.Count;
                        IsGettingSource = false;
                        return;
                    }
            }
        }
        public async Task CheckItemFilter()
        {
            switch (SelectedItemFilter.Content.ToString())
            {
                case "Toàn bộ":
                    {
                        await GetExportListSource("");
                        return;
                    }
                case "Theo ngày":
                    {
                        await GetExportListSource("date");
                        return;
                    }
                case "Theo tháng":
                    {
                        await GetExportListSource("month");
                        return;
                    }
            }
        }
        public async Task CheckImportItemFilter()
        {
            switch (SelectedImportItemFilter.Content.ToString())
            {
                case "Toàn bộ":
                    {
                        await GetImportListSource("");
                        return;
                    }
                case "Theo tháng":
                    {
                        await GetImportListSource("month");
                        return;
                    }
            }
        }
        public async Task CheckMonthFilter()
        {
            try
            {
                ListBill = new ObservableCollection<BillDTO>(await BillService.Ins.GetBillByMonth(SelectedMonth + 1));
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }
        public async Task CheckImportMonthFilter()
        {
            try
            {
                ListProduct = new ObservableCollection<ProductReceiptDTO>(await ProductReceiptService.Ins.GetProductReceipt(SelectedImportMonth + 1));
                //ResultName.Content = ListProduct.Count;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }

        }
    }
}
