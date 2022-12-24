using CinemaManagementProject.DTOs;
using LiveCharts.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    public partial class StatisticsService
    {
        private StatisticsService() { }
        private static StatisticsService _ins;
        public static StatisticsService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new StatisticsService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        #region Customer

        public async Task<(List<CustomerDTO>, float TicketExpenseOfTop1, float ProductExpenseOfTop1)> GetTop5CustomerExpenseByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var cusStatistic = await context.Bills.Where(b => b.CreateDate.Value.Year == year && b.CustomerId != null)
                       .GroupBy(b => b.CustomerId)
                       .Select(grC => new
                       {
                           CustomerId = grC.Key,
                           Expense = grC.Sum(c => (float?)(c.TotalPrize + c.DiscountPrice)) ?? 0
                       })
                       .OrderByDescending(b => b.Expense).Take(5)
                       .Join(
                       context.Customers,
                       statis => statis.CustomerId,
                       cus => cus.Id,
                       (statis, cus) => new CustomerDTO
                       {
                           Id = cus.Id,
                           CustomerName = cus.CustomerName,
                           PhoneNumber = cus.PhoneNumber,
                           Expense = (float)statis.Expense
                       }).ToListAsync();

                    float TicketExpense = 0, ProductExpense = 0;
                    if (cusStatistic.Count >= 1)
                    {
                        string cusId = cusStatistic.First().Id.ToString();
                        TicketExpense = context.Tickets.Where(b => b.Bill.Id.ToString() == cusId).Sum(t => (float?)t.Price) ?? 0;
                        ProductExpense = context.ProductBillInfoes.Where(b => b.Bill.Id.ToString() == cusId).Sum(t => (float?)(t.PrizePerProduct * t.Quantity)) ?? 0;
                    }
                    return (cusStatistic, TicketExpense, ProductExpense);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<(int NewCustomerQuanity, int TotalCustomerQuantity, int WalkinGuestQuantity)> GetDetailedCustomerStatistics(int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        int NewCustomerQuanity = await context.Customers.CountAsync(c => c.FirstDate.Value.Year == year);
                        int TotalCustomerQuantity = await context.Customers.CountAsync(c => c.Bills.Any(b => b.CreateDate.Value.Year == year) || c.FirstDate.Value.Year == year);
                        int WalkinGuestQuantity = await context.Bills.Where(b => b.CustomerId == null && b.CreateDate.Value.Year == year).CountAsync();
                        return (NewCustomerQuanity, TotalCustomerQuantity, WalkinGuestQuantity);
                    }
                }
                else
                {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        int NewCustomerQuanity = await context.Customers.CountAsync(c => c.FirstDate.Value.Year == year && c.FirstDate.Value.Month == month);
                        int TotalCustomerQuantity = await context.Customers
                            .CountAsync(c => c.Bills.Any(b => b.CreateDate.Value.Year == year && b.CreateDate.Value.Month == month) || (c.FirstDate.Value.Year == year && c.FirstDate.Value.Month == month));
                        int WalkinGuestQuantity = await context.Bills.Where(b => b.CustomerId == null && b.CreateDate.Value.Year == year && b.CreateDate.Value.Month == month).CountAsync();
                        return (NewCustomerQuanity, TotalCustomerQuantity, WalkinGuestQuantity);
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<(List<CustomerDTO>, float TicketExpenseOfTop1, float ProductExpenseOfTop1)> GetTop5CustomerExpenseByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    List<CustomerDTO> cusStatistic = await context.Bills.Where(b => b.CreateDate.Value.Year == DateTime.Now.Year && b.CreateDate.Value.Month == month && b.CustomerId != null)
                        .GroupBy(b => b.CustomerId)
                        .Select(grC => new
                        {
                            CustomerId = grC.Key,
                            Expense = grC.Sum(c => (Decimal?)(c.TotalPrize + c.DiscountPrice)) ?? 0
                        })
                        .OrderByDescending(b => b.Expense).Take(5)
                        .Join(
                        context.Customers,
                        statis => statis.CustomerId,
                        cus => cus.Id,
                        (statis, cus) => new CustomerDTO
                        {
                            Id = cus.Id,
                            CustomerName = cus.CustomerName,
                            PhoneNumber = cus.PhoneNumber,
                            Expense = (float)statis.Expense

                        }).ToListAsync();

                    float TicketExpense = 0, ProductExpense = 0;
                    if (cusStatistic.Count >= 1)
                    {
                        string cusId = cusStatistic.First().Id.ToString();
                        TicketExpense = context.Tickets.Where(b => b.Bill.CustomerId.ToString() == cusId).Sum(t => (float?)t.Price) ?? 0;
                        ProductExpense = context.ProductBillInfoes.Where(b => b.Bill.CustomerId.ToString() == cusId).Sum(t => (float?)(t.PrizePerProduct * t.Quantity)) ?? 0;
                    }
                    return (cusStatistic, TicketExpense, ProductExpense);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        #region Staff

        public async Task<List<StaffDTO>> GetTop5ContributionStaffByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var staffStatistic = context.Bills.Where(b => b.CreateDate.Value.Year == year)
                    .GroupBy(b => b.StaffId)
                    .Select(grB => new
                    {
                        StaffId = grB.Key,
                        BenefitContribution = grB.Sum(b => (float?)b.TotalPrize) ?? 0
                    })
                    .OrderByDescending(b => b.BenefitContribution).Take(5)
                    .Join(
                    context.Staffs,
                    statis => statis.StaffId,
                    staff => staff.Id,
                    (statis, staff) => new StaffDTO
                    {
                        Id = staff.Id,
                        StaffName = staff.StaffName,
                        BenefitContribution = statis.BenefitContribution
                    }).ToListAsync();

                    return await staffStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<StaffDTO>> GetTop5ContributionStaffByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var staffStatistic = context.Bills.Where(b => b.CreateDate.Value.Year == DateTime.Today.Year && b.CreateDate.Value.Month == month)
                   .GroupBy(b => b.StaffId)
                   .Select(grB => new
                   {
                       StaffId = grB.Key,
                       BenefitContribution = grB.Sum(b => (float?)b.TotalPrize) ?? 0
                   })
                   .OrderByDescending(b => b.BenefitContribution).Take(5)
                   .Join(
                   context.Staffs,
                   statis => statis.StaffId,
                   staff => staff.Id,
                   (statis, staff) => new StaffDTO
                   {
                       Id = staff.Id,
                       StaffName = staff.StaffName,
                       BenefitContribution = statis.BenefitContribution
                   }).ToListAsync();

                    return await staffStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<float> GetTotalBenefitContributionOfStaffs(int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        float TotalBenefitContribution = await context.Bills.Where(b => b.CreateDate.Value.Year == year).SumAsync(b => (float?)b.TotalPrize) ?? 0;
                        return TotalBenefitContribution;
                    }
                }
                else
                {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        float TotalBenefitContribution = await context.Bills.Where(b => b.CreateDate.Value.Year == year && b.CreateDate.Value.Month == month).SumAsync(b => (float?)b.TotalPrize) ?? 0;
                        return TotalBenefitContribution;
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region Movie
        public async Task<List<FilmDTO>> GetTop5BestMovieByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var movieStatistic = context.ShowTimes.Where(s => s.ShowTimeSetting.ShowDate.Value.Year == year)
                    .GroupBy(s => s.FilmId)
                    .Select(gr => new
                    {
                        MovieId = gr.Key,
                        Revenue = gr.Sum(s => (s.Tickets.Count() * s.Price)),
                        TicketCount = gr.Sum(s => s.Tickets.Count())
                    })
                    .OrderByDescending(m => m.Revenue).Take(5)
                    .Join(
                    context.Films,
                    statis => statis.MovieId,
                    movie => movie.Id,
                    (statis, movie) => new FilmDTO
                    {
                        Id = movie.Id,
                        FilmName = movie.FilmName,
                        Revenue = (float)statis.Revenue,
                        TicketCount = statis.TicketCount
                    }).ToListAsync();

                    return await movieStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<FilmDTO>> GetTop5BestMovieByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var movieStatistic = context.ShowTimes.Where(s => s.ShowTimeSetting.ShowDate.Value.Year == DateTime.Today.Year && s.ShowTimeSetting.ShowDate.Value.Month == month)
                    .GroupBy(s => s.FilmId)
                    .Select(gr => new
                    {
                        MovieId = gr.Key,
                        Revenue = gr.Sum(s => (s.Tickets.Count() * s.Price)),
                        TicketCount = gr.Sum(s => s.Tickets.Count())
                    })
                    .OrderByDescending(m => m.Revenue).Take(5)
                    .Join(
                    context.Films,
                    statis => statis.MovieId,
                    movie => movie.Id,
                    (statis, movie) => new FilmDTO
                    {
                        Id = movie.Id,
                        FilmName = movie.FilmName,
                        Revenue = (float)statis.Revenue,
                        TicketCount = statis.TicketCount
                    }).ToListAsync();
                    return await movieStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Product
        public async Task<List<ProductDTO>> GetTop5BestProductByYear(int year)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var prodStatistic = context.ProductBillInfoes.Where(b => b.Bill.CreateDate.Value.Year == year)
                    .GroupBy(pBill => pBill.ProductId)
                    .Select(gr => new
                    {
                        ProductId = gr.Key,
                        Revenue = gr.Sum(pBill => (Decimal?)(pBill.Quantity * pBill.PrizePerProduct)) ?? 0,
                        SalesQuantity = gr.Sum(pBill => (int?)pBill.Quantity) ?? 0
                    })
                    .OrderByDescending(m => m.Revenue).Take(5)
                    .Join(
                    context.Products,
                    statis => statis.ProductId,
                    prod => prod.Id,
                    (statis, prod) => new ProductDTO
                    {
                        Id = prod.Id,
                        ProductName = prod.ProductName,
                        Revenue = (float)statis.Revenue,
                        SalesQuantity = statis.SalesQuantity
                    }).ToListAsync();

                    return await prodStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<ProductDTO>> GetTop5BestProductByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var prodStatistic = context.ProductBillInfoes.Where(b => b.Bill.CreateDate.Value.Year == DateTime.Today.Year && b.Bill.CreateDate.Value.Month == month)
                    .GroupBy(pBill => pBill.ProductId)
                    .Select(gr => new
                    {
                        ProductId = gr.Key,
                        Revenue = gr.Sum(pBill => (float?)(pBill.Quantity * pBill.PrizePerProduct)) ?? 0,
                        SalesQuantity = gr.Sum(pBill => (int?)pBill.Quantity) ?? 0
                    })
                    .OrderByDescending(m => m.Revenue).Take(5)
                    .Join(
                    context.Products,
                    statis => statis.ProductId,
                    prod => prod.Id,
                    (statis, prod) => new ProductDTO
                    {
                        Id = prod.Id,
                        ProductName = prod.ProductName,
                        Revenue = statis.Revenue,
                        SalesQuantity = statis.SalesQuantity
                    }).ToListAsync();
                    return await prodStatistic;
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
