using CinemaManagementProject.Utils;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using CinemaManagementProject.Model.Service;

namespace CinemaManagementProject.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementVM : BaseViewModel
    {
        private SeriesCollection _InComeData;
        public SeriesCollection InComeData
        {
            get { return _InComeData; }
            set { _InComeData = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedIncomePeriod;
        public ComboBoxItem SelectedIncomePeriod
        {
            get { return _SelectedIncomePeriod; }
            set { _SelectedIncomePeriod = value; OnPropertyChanged(); }
        }

        private string _SelectedIncomeTime;
        public string SelectedIncomeTime
        {
            get { return _SelectedIncomeTime; }
            set { _SelectedIncomeTime = value; OnPropertyChanged(); }
        }

        private int selectedYear;
        public int SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; }
        }


        private string _TrueIncome;
        public string TrueIncome
        {
            get { return _TrueIncome; }
            set { _TrueIncome = value; OnPropertyChanged(); }
        }

        private string _TotalIn;
        public string TotalIn
        {
            get { return _TotalIn; }
            set { _TotalIn = value; OnPropertyChanged(); }
        }

        //MID CARD========================
        private string _TotalOut;
        public string TotalOut
        {
            get { return _TotalOut; }
            set { _TotalOut = value; OnPropertyChanged(); }
        }

        private float _TotalInPc;  //this is for the horizontial bar, just for displaying
        public float TotalInPc
        {
            get { return _TotalInPc; }
            set { _TotalInPc = value; OnPropertyChanged(); }
        }

        private float _TotalOutPc;
        public float TotalOutPc
        {
            get { return _TotalOutPc; }
            set { _TotalOutPc = value; OnPropertyChanged(); }
        }

        private int totalBill;
        public int TotalBill
        {
            get { return totalBill; }
            set { totalBill = value; OnPropertyChanged(); }
        }

        //=================================

        //TOP CARD==================
        private string ticketReve;
        public string TicketReve
        {
            get { return ticketReve; }
            set { ticketReve = value; OnPropertyChanged(); }
        }

        private string productReve;
        public string ProductReve
        {
            get { return productReve; }
            set { productReve = value; OnPropertyChanged(); }
        }

        private string productExpe;
        public string ProductExpe
        {
            get { return productExpe; }
            set { productExpe = value; OnPropertyChanged(); }
        }

        private string repairExpe;
        public string RepairExpe
        {
            get { return repairExpe; }
            set { repairExpe = value; OnPropertyChanged(); }
        }

        private string ticketPc;
        public string TicketPc
        {
            get { return ticketPc; }
            set { ticketPc = value; OnPropertyChanged(); }
        }

        private string productPc;
        public string ProductPc
        {
            get { return productPc; }
            set { productPc = value; OnPropertyChanged(); }
        }

        private string productExPc;
        public string ProductExPc
        {
            get { return productExPc; }
            set { productExPc = value; OnPropertyChanged(); }
        }

        private string repairPc;
        public string RepairPc
        {
            get { return repairPc; }
            set { repairPc = value; OnPropertyChanged(); }
        }

        private string reveRate;
        public string ReveRate
        {
            get { return reveRate; }
            set { reveRate = value; OnPropertyChanged(); }
        }

        private string expeRate;
        public string ExpeRate
        {
            get { return expeRate; }
            set { expeRate = value; OnPropertyChanged(); }
        }

        //==============================

        private int _LabelMaxValue;
        public int LabelMaxValue
        {
            get { return _LabelMaxValue; }
            set { _LabelMaxValue = value; OnPropertyChanged(); }
        }




        public async Task ChangeIncomePeriod()
        {
            if (SelectedIncomePeriod != null)
            {
                switch (SelectedIncomePeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedIncomeTime != null)
                            {
                                if (SelectedIncomeTime.Length == 4)
                                    SelectedYear = int.Parse(SelectedIncomeTime);
                                await LoadIncomeByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedIncomeTime != null)
                            {
                                await LoadIncomeByMonth();
                            }
                            return;
                        }
                }
            }
        }
        public async Task LoadIncomeByYear()
        {
            if (SelectedIncomeTime.Length != 4) return;
            LabelMaxValue = 12;
            try
            {

                TotalBill = await Task.Run(() => OverviewStatisticService.Ins.GetBillQuantity(int.Parse(SelectedIncomeTime)));
                (List<float> monthlyRevenue, float Productreve, float Ticketreve, string YearRevenueRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetRevenueByYear(int.Parse(SelectedIncomeTime)));
                (List<float> monthlyExpense, float ProductExpense, float RepairCost, string YearExpenseRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetExpenseByYear(int.Parse(SelectedIncomeTime)));


                TicketReve = Helper.FormatVNMoney(Ticketreve);
                ProductReve = Helper.FormatVNMoney(Productreve);
                ProductExpe = Helper.FormatVNMoney(ProductExpense);
                RepairExpe = Helper.FormatVNMoney(RepairCost);
                ReveRate = YearRevenueRateStr;
                ExpeRate = YearExpenseRateStr;

                monthlyRevenue.Insert(0, 0);
                monthlyExpense.Insert(0, 0);

                CalculateTrueIncome(monthlyRevenue, monthlyExpense);
                Calculate_RevExpPercentage(Ticketreve, Productreve, ProductExpense, RepairCost);

                for (int i = 1; i <= 12; i++)
                {
                    monthlyRevenue[i] /= 1000000;
                    monthlyExpense[i] /= 1000000;
                }


                InComeData = new SeriesCollection
            {
            new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<float>(monthlyRevenue),
                Fill = Brushes.Transparent
            },
            new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<float>(monthlyExpense),
                Fill = Brushes.Transparent
            }
            };
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
        public async Task LoadIncomeByMonth()
        {
            if (SelectedIncomeTime.Length == 4) return;
            LabelMaxValue = 30;
            try
            {
                TotalBill = await OverviewStatisticService.Ins.GetBillQuantity(2021, int.Parse(SelectedIncomeTime.Remove(0, 6)));
                (List<float> dailyRevenue, float MonthProductReve, float MonthTicketReve, string MonthRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetRevenueByMonth(SelectedYear, int.Parse(SelectedIncomeTime.Remove(0, 6))));
                (List<float> dailyExpense, float MonthProductExpense, float MonthRepairCost, string MonthExpenseRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetExpenseByMonth(SelectedYear, int.Parse(SelectedIncomeTime.Remove(0, 6))));
                TicketReve = Helper.FormatVNMoney(MonthTicketReve);
                ProductReve = Helper.FormatVNMoney(MonthProductReve);
                ProductExpe = Helper.FormatVNMoney(MonthProductExpense);
                RepairExpe = Helper.FormatVNMoney(MonthRepairCost);
                ReveRate = MonthRateStr;
                ExpeRate = MonthExpenseRateStr;

                dailyRevenue.Insert(0, 0);
                dailyExpense.Insert(0, 0);

                CalculateTrueIncome(dailyRevenue, dailyExpense);
                Calculate_RevExpPercentage(MonthTicketReve, MonthProductReve, MonthProductExpense, MonthRepairCost);

                for (int i = 1; i <= dailyRevenue.Count - 1; i++)
                {
                    dailyRevenue[i] /= 1000000;
                    dailyExpense[i] /= 1000000;
                }

                InComeData = new SeriesCollection
            {
            new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<float>(dailyRevenue),
                Fill = Brushes.Transparent,
            },
            new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<float>(dailyExpense),
                Fill = Brushes.Transparent,
            }
            };
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
        public void CalculateTrueIncome(List<float> l1, List<float> l2)
        {

            float totalin = 0, totalout = 0;

            foreach (float item in l1)
                totalin += item;
            foreach (float item in l2)
                totalout += item;

            float trueincome = totalin - totalout;
            FindMaxPercentage(totalin, totalout);


            TrueIncome = Helper.FormatVNMoney(trueincome);
            TotalIn = Helper.FormatVNMoney(totalin);
            TotalOut = Helper.FormatVNMoney(totalout);
        }
        public void FindMaxPercentage(float _in, float _out)
        {
            if (_in != 0 && _out != 0)
            {
                if (_in >= _out)
                {
                    TotalInPc = 100;
                    TotalOutPc = _out / _in * 100;
                }
                else
                {
                    TotalOutPc = 100;
                    TotalInPc = _in / _out * 100;
                }
            }
            else
            {
                if (_in == 0 && _out == 0)
                {
                    TotalInPc = 10;
                    TotalOutPc = 10;
                    return;
                }
                if (_in == 0)
                {
                    TotalInPc = 10;
                    TotalOutPc = 90;
                    return;
                }
                if (_out == 0)
                {
                    TotalInPc = 90;
                    TotalOutPc = 10;
                    return;
                }

            }

        }
        public void Calculate_RevExpPercentage(float a1, float a2, float a3, float a4)
        {
            Calculate_RevPercentage(a1, a2);
            Calculate_ExpPercentage(a3, a4);
        }
        public void Calculate_RevPercentage(float a1, float a2)
        {
            if (a1 == 0)
            {
                if (a2 == 0)
                    TicketPc = ProductPc = "0%";
                else
                {
                    TicketPc = "0%";
                    ProductPc = "100%";
                }
                return;
            }
            if (a2 == 0)
            {
                if (a1 == 0)
                    TicketPc = ProductPc = "0%";
                else
                {
                    TicketPc = "100%";
                    ProductPc = "0%";
                }
                return;
            }

            TicketPc = Math.Round(a1 / (a1 + a2) * 100,2).ToString() + "%";
            ProductPc = Math.Round(a2 / (a1 + a2) * 100,2).ToString() + "%";
        }
        public void Calculate_ExpPercentage(float a3, float a4)
        {
            if (a3 == 0)
            {
                if (a4 == 0)
                    ProductExPc = RepairPc = "0%";
                else
                {
                    ProductExPc = "0%";
                    RepairPc = "100%";
                }
                return;
            }
            if (a4 == 0)
            {
                if (a3 == 0)
                    ProductExPc = RepairPc = "0%";
                else
                {
                    ProductExPc = "100%";
                    RepairPc = "0%";
                }
                return;
            }
            ProductExPc = Math.Round(a3 / (a3 + a4) * 100, 2).ToString() + "%";
            RepairPc = Math.Round(a4 / (a3 + a4) * 100, 2).ToString() + "%";
        }
    }
}
