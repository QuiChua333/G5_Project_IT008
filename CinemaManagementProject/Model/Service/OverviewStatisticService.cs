using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    public partial class OverviewStatisticService
    {
        private OverviewStatisticService() { }
        private static OverviewStatisticService _ins;
        public static OverviewStatisticService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new OverviewStatisticService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        #region Overview
        public async Task<int> GetBillQuantity(int year, int month = 0)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    if (month == 0)
                    {
                        return await context.Bills.Where(b => b.CreateDate.Value.Year == year).CountAsync();
                    }
                    else
                    {
                        return await context.Bills.Where(b => b.CreateDate.Value.Year == year && b.CreateDate.Value.Month == month).CountAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Revenue

        public async Task<(List<float>, float ProductRevenue, float TicketRevenue, string TicketRateStr)> GetRevenueByYear(int year)
        {
            List<float> MonthlyRevenueList = new List<float>(new float[12]);

            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var billList = context.Bills
                    .Where(b => b.CreateDate.Value.Year == year);

                    (float ProductRevenue, float TicketRevenue) = await GetFullRevenue(context, year);

                    var MonthlyRevenue = billList
                             .GroupBy(b => b.CreateDate.Value.Month)
                             .Select(gr => new
                             {
                                 Month = gr.Key,
                                 Income = gr.Sum(b => (float?)b.TotalPrize) ?? 0
                             }).ToList();

                    foreach (var re in MonthlyRevenue)
                    {
                        MonthlyRevenueList[re.Month - 1] = (float)re.Income;
                    }

                    (float lastProdReve, float lastTicketReve) = await GetFullRevenue(context, year - 1);
                    float lastRevenueTotal = lastProdReve + lastTicketReve;
                    string RevenueRateStr;
                    if (lastRevenueTotal == 0)
                    {
                        RevenueRateStr = "-2";
                    }
                    else
                    {
                        RevenueRateStr = Helper.ConvertDoubleToPercentageStr((double)((ProductRevenue + TicketRevenue) / lastRevenueTotal) - 1);
                    }

                    return (MonthlyRevenueList, (float)ProductRevenue, (float)TicketRevenue, RevenueRateStr);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<(List<float>, float ProductRevenue, float TicketRevenue, string RevenueRate)> GetRevenueByMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            List<float> DailyReveList = new List<float>(new float[days]);

            try
            {

                using (var context = new CinemaManagementProjectEntities())
                {
                    var billList = context.Bills
                     .Where(b => b.CreateDate.Value.Year == year && b.CreateDate.Value.Month == month);

                    (float ProductRevenue, float TicketRevenue) = await GetFullRevenue(context, year, month);

                    var dailyRevenue = await billList
                                .GroupBy(b => b.CreateDate.Value.Day)
                                 .Select(gr => new
                                 {
                                     Day = gr.Key,
                                     Income = gr.Sum(b => b.TotalPrize),
                                     DiscountPrice = gr.Sum(b => (float?)b.DiscountPrice) ?? 0,
                                 }).ToListAsync();

                    foreach (var re in dailyRevenue)
                    {
                        DailyReveList[re.Day - 1] = (float)re.Income;
                    }

                    if (month == 1)
                    {
                        year--;
                        month = 13;
                    }
                    (float lastProdReve, float lastTicketReve) = await GetFullRevenue(context, year, month - 1);
                    float lastRevenueTotal = lastProdReve + lastTicketReve;
                    string RevenueRateStr;
                    if (lastRevenueTotal == 0)
                    {
                        RevenueRateStr = "-2";
                    }
                    else
                    {
                        RevenueRateStr = Helper.ConvertDoubleToPercentageStr((double)((ProductRevenue + TicketRevenue) / lastRevenueTotal) - 1);
                    }

                    return (DailyReveList, (float)ProductRevenue, (float)TicketRevenue, RevenueRateStr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy doanh thu của sản phẩm và vé, truyền 1 tham số thì đó sẽ là tìm theo năm, 2 tham số là theo năm và tháng
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<(float, float)> GetFullRevenue(CinemaManagementProjectEntities context, int year, int month = 0)
        {
            try
            {

                if (month != 0)
                {

                    float ProductRevenue = await context.ProductBillInfoes.Where(pB => pB.Bill.CreateDate.Value.Year == year && pB.Bill.CreateDate.Value.Month == month)
                                                    .SumAsync(pB => (float?)(pB.PrizePerProduct * pB.Quantity)) ?? 0;
                    float TicketRevenue = await context.Tickets.Where(t => t.Bill.CreateDate.Value.Year == year && t.Bill.CreateDate.Value.Month == month)
                                                    .SumAsync(t => (float?)t.Price) ?? 0;

                    return (ProductRevenue, TicketRevenue);

                }
                else
                {
                    float ProductRevenue = await context.ProductBillInfoes.Where(pB => pB.Bill.CreateDate.Value.Year == year)
                                                    .SumAsync(pB => (float?)(pB.PrizePerProduct * pB.Quantity)) ?? 0;
                    float TicketRevenue = await context.Tickets.Where(t => t.Bill.CreateDate.Value.Year == year)
                                                    .SumAsync(t => (float?)t.Price) ?? 0;
                    return (ProductRevenue, TicketRevenue);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion



        #region Expense
        private async Task<float> GetFullExpenseLastTime(CinemaManagementProjectEntities context, int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    //Product Receipt
                    float LastYearProdExpense = await context.ProductReceipts
                             .Where(pr => pr.CreatedAt.Value.Year == year)
                             .SumAsync(pr => (float?)pr.ImportPrice) ?? 0;

                    //Repair Cost
                    var LastYearRepairCost = await context.Troubles
                             .Where(tr => tr.FinishDate != null && tr.FinishDate.Value.Year == year)
                             .SumAsync(tr => (float?)tr.RepairCost) ?? 0;
                    return (LastYearProdExpense + LastYearRepairCost);
                }
                else
                {
                    //Product Receipt
                    float LastMonthProdExpense = await context.ProductReceipts
                             .Where(pr => pr.CreatedAt.Value.Year == year && pr.CreatedAt.Value.Month == month)
                             .SumAsync(pr => (float?)pr.ImportPrice) ?? 0;
                    //Repair Cost
                    var LastMonthRepairCost = await context.Troubles
                             .Where(tr => tr.FinishDate != null && tr.FinishDate.Value.Year == year && tr.FinishDate.Value.Month == month)
                             .SumAsync(tr => (float?)tr.RepairCost) ?? 0;
                    return (LastMonthProdExpense + LastMonthRepairCost);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<(List<float> MonthlyExpense, float ProductExpense, float RepairCost, string ExpenseRate)> GetExpenseByYear(int year)
        {
            List<float> MonthlyExpense = new List<float>(new float[12]);
            float ProductExpenseTotal = 0;
            float RepairCostTotal = 0;

            //Product Receipt
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var MonthlyProdExpense = await context.ProductReceipts
                     .Where(b => b.CreatedAt.Value.Year == year)
                     .GroupBy(b => b.CreatedAt.Value.Month)
                     .Select(gr => new
                     {
                         Month = gr.Key,
                         Outcome = gr.Sum(b => (float?)b.ImportPrice) ?? 0
                     }).ToListAsync();

                    //Repair Cost
                    //var MonthlyRepairCost = MonthlyProdExpense.Select(p => new { Month = p.Month, Outcome = p.Outcome * 2 }).ToList();
                    var MonthlyRepairCost = await context.Troubles
                         .Where(t => t.FinishDate != null && t.FinishDate.Value.Year == year)
                         .GroupBy(t => t.FinishDate.Value.Month)
                         .Select(gr =>
                         new
                         {
                             Month = gr.Key,
                             Outcome = gr.Sum(t => (float?)t.RepairCost) ?? 0
                         }).ToListAsync();


                    //Accumulate
                    foreach (var ex in MonthlyProdExpense)
                    {
                        MonthlyExpense[ex.Month - 1] += (float)ex.Outcome;
                        ProductExpenseTotal += ex.Outcome;
                    }

                    foreach (var ex in MonthlyRepairCost)
                    {
                        MonthlyExpense[ex.Month - 1] += (float)ex.Outcome;
                        RepairCostTotal += ex.Outcome;
                    }

                    float lastProductExpenseTotal = await GetFullExpenseLastTime(context, year - 1);

                    string ExpenseRateStr;
                    //check mẫu  = 0
                    if (lastProductExpenseTotal == 0)
                    {
                        ExpenseRateStr = "-2";
                    }
                    else
                    {
                        ExpenseRateStr = Helper.ConvertDoubleToPercentageStr(((double)(ProductExpenseTotal / lastProductExpenseTotal) - 1));
                    }


                    return (MonthlyExpense, (float)ProductExpenseTotal, (float)RepairCostTotal, ExpenseRateStr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(List<float> DailyExpense, float ProductExpense, float RepairCost, string RepairRateStr)> GetExpenseByMonth(int year, int month)
        {

            int days = DateTime.DaysInMonth(year, month);
            List<float> DailyExpense = new List<float>(new float[days]);
            float ProductExpenseTotal = 0;
            float RepairCostTotal = 0;

            try
            {

                using (var context = new CinemaManagementProjectEntities())
                {
                    //Product Receipt
                    var MonthlyProdExpense = await context.ProductReceipts
                         .Where(b => b.CreatedAt.Value.Year == year && b.CreatedAt.Value.Month == month)
                         .GroupBy(b => b.CreatedAt.Value.Day)
                         .Select(gr => new
                         {
                             Day = gr.Key,
                             Outcome = gr.Sum(b => (float?)b.ImportPrice) ?? 0
                         }).ToListAsync();
                    //Repair Cost
                    var MonthlyRepairCost = await context.Troubles
                        .Where(t => t.FinishDate != null && t.FinishDate.Value.Year == year && t.FinishDate.Value.Month == month)
                        .GroupBy(t => t.FinishDate.Value.Day)
                        .Select(gr =>
                        new
                        {
                            Day = gr.Key,
                            Outcome = gr.Sum(t => (float?)t.RepairCost) ?? 0
                        }).ToListAsync();
                    //context.
                    //Accumulate
                    foreach (var ex in MonthlyProdExpense)
                    {
                        DailyExpense[ex.Day - 1] += (float)ex.Outcome;
                        ProductExpenseTotal += ex.Outcome;
                    }

                    foreach (var ex in MonthlyRepairCost)
                    {
                        DailyExpense[ex.Day - 1] += (float)ex.Outcome;
                        RepairCostTotal += ex.Outcome;
                    }
                    if (month == 1)
                    {
                        year--;
                        month = 13;
                    }


                    float lastProductExpenseTotal = await GetFullExpenseLastTime(context, year, month - 1);
                    string ExpenseRateStr;
                    //check mẫu  = 0
                    if (lastProductExpenseTotal == 0)
                    {
                        ExpenseRateStr = "-2";
                    }
                    else
                    {
                        ExpenseRateStr = Helper.ConvertDoubleToPercentageStr(((double)(ProductExpenseTotal / lastProductExpenseTotal) - 1));
                    }

                    return (DailyExpense, ProductExpenseTotal, RepairCostTotal, ExpenseRateStr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
