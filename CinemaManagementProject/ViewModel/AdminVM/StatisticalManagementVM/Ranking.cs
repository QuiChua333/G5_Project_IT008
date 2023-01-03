using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model.Service;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CinemaManagementProject.Model;

namespace CinemaManagementProject.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementVM : BaseViewModel
    {
        private List<CustomerDTO> top5Customer;
        public List<CustomerDTO> Top5Customer
        {
            get { return top5Customer; }
            set { top5Customer = value; OnPropertyChanged(); }
        }

        private List<StaffDTO> top5Staff;
        public List<StaffDTO> Top5Staff
        {
            get { return top5Staff; }
            set { top5Staff = value; OnPropertyChanged(); }
        }

        private SeriesCollection _CustomerExpe;
        public SeriesCollection CustomerExpe
        {
            get { return _CustomerExpe; }
            set { _CustomerExpe = value; OnPropertyChanged(); }
        }

        private SeriesCollection _NewCusPie;
        public SeriesCollection NewCusPie
        {
            get { return _NewCusPie; }
            set { _NewCusPie = value; OnPropertyChanged(); }
        }

        private SeriesCollection _StaffContributePie;
        public SeriesCollection StaffContributePie
        {
            get { return _StaffContributePie; }
            set { _StaffContributePie = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedRankingPeriod;
        public ComboBoxItem SelectedRankingPeriod
        {
            get { return _SelectedRankingPeriod; }
            set { _SelectedRankingPeriod = value; OnPropertyChanged(); }
        }

        private string _SelectedRankingTime;
        public string SelectedRankingTime
        {
            get { return _SelectedRankingTime; }
            set { _SelectedRankingTime = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedRankingPeriod2;
        public ComboBoxItem SelectedRankingPeriod2
        {
            get { return _SelectedRankingPeriod2; }
            set { _SelectedRankingPeriod2 = value; OnPropertyChanged(); }
        }

        private string _SelectedRankingTime2;
        public string SelectedRankingTime2
        {
            get { return _SelectedRankingTime2; }
            set { _SelectedRankingTime2 = value; OnPropertyChanged(); }
        }

        private int walkingGuest;
        public int WalkingGuest
        {
            get { return walkingGuest; }
            set { walkingGuest = value; OnPropertyChanged(); }
        }


        public async Task ChangeRankingPeriod()
        {
            if (SelectedRankingPeriod != null)
            {
                switch (SelectedRankingPeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedRankingTime != null)
                            {
                                await LoadRankingByYear();
                            }
                            return;
                        }
                    case "By Year":
                        {
                            if (SelectedRankingTime != null)
                            {
                                await LoadRankingByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedRankingTime != null)
                            {
                                await LoadRankingByMonth();
                            }
                            return;
                        }
                    case "By Month":
                        {
                            if (SelectedRankingTime != null)
                            {
                                await LoadRankingByMonth();
                            }
                            return;
                        }
                }
            }
        }
        public async Task LoadRankingByYear()
        {
            if (SelectedRankingTime.Length != 4) return;
            try
            {
                (List<CustomerDTO> Top5Cus, float TicketExpenseOfTop1, float ProductExpenseOfTop1) = await Task.Run(() => StatisticsService.Ins.GetTop5CustomerExpenseByYear(int.Parse(SelectedRankingTime)));
                (int NewCustomerQuanityInYear, int TotalCustomerQuantityInYear, int WalkinGuestQuantityInYear) = await Task.Run(() => StatisticsService.Ins.GetDetailedCustomerStatistics(int.Parse(SelectedRankingTime)));
                Top5Customer = Top5Cus;

                CustomerExpe = new SeriesCollection
                {
                    new PieSeries
                    {
                        Values = new ChartValues<float>{TicketExpenseOfTop1 },
                        Title = Properties.Settings.Default.isEnglish ? "Ticket fare" : "Tiền vé",
                    },
                    new PieSeries
                    {
                        Values = new ChartValues<float>{ProductExpenseOfTop1 },
                        Title = Properties.Settings.Default.isEnglish ? "Product" : "Sản phẩm",
                    }
                };
                NewCusPie = new SeriesCollection
                {
                    new PieSeries
                    {
                        Values = new ChartValues<int>{NewCustomerQuanityInYear},
                        Title = Properties.Settings.Default.isEnglish ? "New customer" : "Khách hàng mới",
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Values = new ChartValues<int>{TotalCustomerQuantityInYear-NewCustomerQuanityInYear},
                        Title = Properties.Settings.Default.isEnglish ? "Old customer" : "Khách hàng cũ",
                        DataLabels = true
                    },
                };
                WalkingGuest = WalkinGuestQuantityInYear;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("System error", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }
        public async Task LoadRankingByMonth()
        {
            if (SelectedRankingTime.Length == 4) return;
            try
            {
                (List<CustomerDTO> Top5Cus, float TicketExpenseTop1Cus, float ProductExpenseTop1Cus) = await Task.Run(() => StatisticsService.Ins.GetTop5CustomerExpenseByMonth(int.Parse(SelectedRankingTime.Remove(0, 6))));
                (int NewCustomerQuanity, int TotalCustomerQuantity, int WalkinGuestQuantity) = await Task.Run(() => StatisticsService.Ins.GetDetailedCustomerStatistics(DateTime.Now.Year, int.Parse(SelectedRankingTime.Remove(0, 6))));
                Top5Customer = Top5Cus;


                CustomerExpe = new SeriesCollection
                {
                    new PieSeries
                    {
                        Values = new ChartValues<float>{TicketExpenseTop1Cus },
                        Title = Properties.Settings.Default.isEnglish ? "Ticket fare" : "Tiền vé",
                    },
                    new PieSeries
                    {
                        Values = new ChartValues<float>{ProductExpenseTop1Cus },
                        Title = Properties.Settings.Default.isEnglish ? "Product" : "Sản phẩm",
                    }
                };
                NewCusPie = new SeriesCollection
                {
                    new PieSeries
                    {
                        Values = new ChartValues<int>{NewCustomerQuanity},
                        Title = Properties.Settings.Default.isEnglish ? "New customer" : "Khách hàng mới",
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Values = new ChartValues<int>{TotalCustomerQuantity - NewCustomerQuanity},
                        Title = Properties.Settings.Default.isEnglish ? "Old customer" : "Khách hàng cũ",
                        DataLabels = true
                    },
                };
                WalkingGuest = WalkinGuestQuantity;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("System error", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }

        public async Task ChangeRankingPeriod2()
        {
            if (SelectedRankingPeriod2 != null)
            {
                switch (SelectedRankingPeriod2.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                await LoadRankingByYear2();
                            }
                            return;
                        }
                    case "By Year":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                await LoadRankingByYear2();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                await LoadRankingByMonth2();
                            }
                            return;
                        }
                    case "By Month":
                        {
                            if (SelectedRankingTime2 != null)
                            {
                                await LoadRankingByMonth2();
                            }
                            return;
                        }
                }
            }
        }
        public async Task LoadRankingByYear2()
        {
            if (SelectedRankingTime2.Length != 4) return;
            try
            {
                Top5Staff = await StatisticsService.Ins.GetTop5ContributionStaffByYear(int.Parse(SelectedRankingTime2));
                float TotalBenefitByYear = await Task.Run(() => StatisticsService.Ins.GetTotalBenefitContributionOfStaffs(int.Parse(SelectedRankingTime2)));
                float totaltop5 = 0;
                foreach (var item in top5Staff)
                {
                    totaltop5 += item.BenefitContribution;
                }
                StaffContributePie = new SeriesCollection();
                for (int i = 0; i < Top5Staff.Count; i++)
                {
                    PieSeries p = new PieSeries
                    {
                        Values = new ChartValues<float> { Top5Staff[i].BenefitContribution },
                        Title = Top5Staff[i].Id.ToString(),
                    };
                    StaffContributePie.Add(p);
                }
                StaffContributePie.Add(new PieSeries
                {
                    Values = new ChartValues<float> { TotalBenefitByYear - totaltop5 },
                    Title = Properties.Settings.Default.isEnglish ? "The rest of the staff" : "Các nhân viên còn lại",
                });
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("System error", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }
        public async Task LoadRankingByMonth2()
        {
            if (SelectedRankingTime2.Length == 4) return;
            try
            {
                Top5Staff = await StatisticsService.Ins.GetTop5ContributionStaffByMonth(int.Parse(SelectedRankingTime2.Remove(0, 6)));
                float TotalBenefitByMonth = await Task.Run(() => StatisticsService.Ins.GetTotalBenefitContributionOfStaffs(DateTime.Now.Year, int.Parse(SelectedRankingTime2.Remove(0, 6))));
                float totaltop5 = 0;
                foreach (var item in top5Staff)
                {
                    totaltop5 += item.BenefitContribution;
                }
                StaffContributePie = new SeriesCollection();
                for (int i = 0; i < Top5Staff.Count; i++)
                {
                    PieSeries p = new PieSeries
                    {
                        Values = new ChartValues<float> { Top5Staff[i].BenefitContribution },
                        Title = Top5Staff[i].Id.ToString(),
                    };
                    StaffContributePie.Add(p);
                }
                StaffContributePie.Add(new PieSeries
                {
                    Values = new ChartValues<float> { TotalBenefitByMonth - totaltop5 },
                    Title = Properties.Settings.Default.isEnglish ? "The rest of the staff" : "Các nhân viên còn lại",
                });
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("Unable to connect to database", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (Properties.Settings.Default.isEnglish) CustomMessageBox.ShowOk("System error", "Error", "OK", Views.CustomMessageBoxImage.Error);
                else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", Views.CustomMessageBoxImage.Error);
            }
        }
    }
}
