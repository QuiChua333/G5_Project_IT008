using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    public class BillService
    {
        private static BillService _ins;
        public static BillService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BillService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private BillService()
        {
        }
        public async Task<List<BillDTO>> GetAllBill()
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var billList = (from b in context.Bills
                                    orderby b.CreateDate descending
                                    select new BillDTO
                                    {
                                        Id = b.Id,
                                        StaffId =(int)b.StaffId,
                                        StaffName = b.Staff.StaffName,
                                        TotalPrice = (float)b.TotalPrize,
                                        DiscountPrice = (float)b.DiscountPrice,
                                        CustomerId = (int)b.CustomerId,
                                        CustomerName = b.Customer.CustomerName,
                                        PhoneNumber = b.Customer.PhoneNumber,
                                        CreatedAt = (DateTime)b.CreateDate
                                    }).ToListAsync();
                    return await billList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get Bill by particular date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<BillDTO>> GetBillByDate(DateTime date)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var billList = (from b in context.Bills
                                    where DbFunctions.TruncateTime(b.CreateDate) == date.Date
                                    orderby b.CreateDate descending
                                    select new BillDTO
                                    {
                                        Id = b.Id,
                                        StaffId = (int)b.StaffId,
                                        StaffName = b.Staff.StaffName,
                                        TotalPrice = (float)b.TotalPrize,
                                        DiscountPrice = (float)b.DiscountPrice,
                                        CustomerId = (int)b.CustomerId,
                                        CustomerName = b.Customer.CustomerName,
                                        PhoneNumber = b.Customer.PhoneNumber,
                                        CreatedAt = (DateTime)b.CreateDate
                                    }).ToListAsync();
                    return await billList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy hóa đơn trong tháng nào đó
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<List<BillDTO>> GetBillByMonth(int month)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var billList = (from b in context.Bills
                                    where ((DateTime)b.CreateDate).Year == DateTime.Now.Year && ((DateTime)b.CreateDate).Month == month
                                    orderby b.CreateDate descending
                                    select new BillDTO
                                    {
                                        Id = b.Id,
                                        StaffId = (int)b.StaffId,
                                        StaffName = b.Staff.StaffName,
                                        TotalPrice = (float)b.TotalPrize,
                                        DiscountPrice = (float)b.DiscountPrice,
                                        CustomerId = (int)b.CustomerId,
                                        CustomerName = b.Customer.CustomerName,
                                        PhoneNumber = b.Customer.PhoneNumber,
                                        CreatedAt = (DateTime)b.CreateDate
                                    }).ToListAsync();
                    return await billList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của hóa đơn 
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public async Task<BillDTO> GetBillDetails(int billId)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var bill = await context.Bills.FindAsync(billId);

                    BillDTO billInfo = new BillDTO
                    {
                        Id = bill.Id,
                        StaffId = bill.Staff.Id,
                        StaffName = bill.Staff.StaffName,
                        DiscountPrice = (float)bill.DiscountPrice,
                        TotalPrice = (float)bill.TotalPrize,
                        CreatedAt = (DateTime)bill.CreateDate,
                        ProductBillInfoes = (from pi in bill.ProductBillInfo
                                             select new ProductBillInfoDTO
                                             {
                                                 BillId = pi.BillId,
                                                 ProductId = pi.ProductId,
                                                 ProductName = pi.Product.ProductName,
                                                 PrizePerProduct = pi.PrizePerProduct,
                                                 Quantity = pi.Quantity
                                             }).ToList(),
                    };
                    if (bill.CustomerId != null)
                    {
                        billInfo.CustomerId = bill.Customer.Id;
                        billInfo.CustomerName = bill.Customer.CustomerName;
                        billInfo.PhoneNumber = bill.Customer.PhoneNumber;
                    }

                    var tickets = bill.Ticket;
                    if (tickets.Count != 0)
                    {
                        var showtime = tickets.FirstOrDefault().ShowTime;
                        int roomId = 0;
                        List<string> seatList = new List<string>();
                        foreach (var t in tickets)
                        {
                            if (roomId == 0)
                            {
                                roomId = (int)t.Seat.RoomId;
                            }
                            seatList.Add($"{t.Seat.SeatRow}{t.Seat.SeatNumber}");
                        }
                        billInfo.TicketInfo = new TicketBillInfoDTO()
                        {
                            roomId = roomId,
                            movieName = showtime.Film.FilmName,
                            ShowDate = (DateTime)showtime.ShowTimeSetting.ShowDate,
                            StartShowTime = (TimeSpan)showtime.StartTime,
                            TotalPriceTicket = (float)(tickets.Count() * showtime.Price),
                            seats = seatList,
                        };
                    }
                    return billInfo;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
