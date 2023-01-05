using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utils;
using CinemaManagementProject.ViewModel.StaffVM.TicketVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagementProject.Model.Service
{
    public class BookingService
    {
        private bool IsEnglish = false;
        private static BookingService _ins;
        public static BookingService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BookingService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private BookingService()
        {
        }
        private string CreateNextBillId(string maxId)
        {
            //NVxxx
            if (maxId is null)
            {
                return "HD0001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "HD" + newIdString.Substring(newIdString.Length - 4, 4);
        }
        /// <summary>
        ///  Đặt những vé xem phim khi biết danh sách ghế => tạo list ticket
        /// </summary>
        /// <param name="newTicketList"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string message)> CreateTicketBooking(BillDTO bill, List<TicketDTO> newTicketList)
        {
            IsEnglish = TicketWindowViewModel.IsEnglish;
            if (newTicketList.Count() == 0)
            {
                return (false, IsEnglish?"Please choose a seat!":"Vui lòng chọn ghế!");
            }
            try
            {

                using (var context = new CinemaManagementProjectEntities())
                {
                    //Update seat 
                    string error = await UpdateSeatsBooked(context, newTicketList);
                    if (error != null)
                    {
                        return (false, error);
                    }

                    //Bill
                    string billCode = await CreateNewBill(context, bill);

                    //Ticket
                    AddNewTickets(context, billCode, newTicketList);

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    string error = await UpdateSeatsBooked(context, newTicketList);
                    if (error != null)
                    {
                        return (false, error);
                    }
                }
                return (false, /*"Danh sách ghế vừa đặt có chứa ghế đã được đặt. Vui lòng quay lại!"*/ e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, IsEnglish?"System Error":"Lỗi hệ thống");
            }

            return (true, IsEnglish?"Ticket booking successful!":"Đặt vé thành công!");
        }

        /// <summary>
        /// (Dành cho cả mua vé và đặt hàng) Tạo hóa đơn khi biết Bill (chứa CustomerId, StaffId , totalPrice) , danh sách vé, danh sách các sản phẩm đc đặt
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="newTicketList"></param>
        /// <param name="orderedProductList"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string message)> CreateFullOptionBooking(BillDTO bill, List<TicketDTO> newTicketList, List<ProductBillInfoDTO> orderedProductList)
        {
            IsEnglish = TicketWindowViewModel.IsEnglish;
            if (newTicketList.Count() == 0)
            {
                return (false, IsEnglish ? "Please choose a seat!" : "Vui lòng chọn ghế!");
            }
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    //Update seat 
                    string error = await UpdateSeatsBooked(context, newTicketList);

                    if (error != null)
                    {
                        return (false, error);
                    }

                    //Bill
                    string billCode = await CreateNewBill(context, bill);

                    //Ticket

                    AddNewTickets(context, billCode, newTicketList);

                    //Product
                    bool addSuccess = await AddNewProductBills(context, billCode, orderedProductList);
                    if (!addSuccess)
                    {
                        return (false, IsEnglish ? "The number of products is not enough to satisfy!" : "Số lượng sản phẩm không đủ để đáp ứng!");
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    string error = await UpdateSeatsBooked(context, newTicketList);
                    if (error != null)
                    {
                        return (false, error);
                    }
                }
                return (false, IsEnglish ? "The recently booked seats list contains already booked seats. Please come back!" : "Danh sách ghế vừa đặt có chứa ghế đã được đặt. Vui lòng quay lại!");
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                return (false, e.Message);
            }
            return (true, IsEnglish?"Successful transaction execution!":"Thực hiện giao dịch thành công!");
        }


        private async Task<string> UpdateSeatsBooked(CinemaManagementProjectEntities context, List<TicketDTO> newTicketList)
        {
            IsEnglish = TicketWindowViewModel.IsEnglish;
            var idSeatList = new List<int>();
            newTicketList.ForEach(s => idSeatList.Add(s.SeatId));

            //Make seat of showtime status = true
            int showtimeId = newTicketList[0].ShowtimeId;
            var seatSets = await context.SeatSettings.Where(s => s.ShowTimeId == showtimeId && idSeatList.Contains(s.SeatId)).ToListAsync();
            List<string> bookedSeats = new List<string>();
            foreach (var s in seatSets)
            {
                if (s.SeatStatus == true)
                {
                    var seat = s.Seat;
                    bookedSeats.Add($"{seat.SeatRow}{seat.SeatNumber}");
                }
                else
                {
                    s.SeatStatus = true;
                }
            }
            if (bookedSeats.Count > 0)
            {
                return (IsEnglish?$"Seat list {string.Join(", ", bookedSeats)} already booked!" :$"Ghế {string.Join(", ", bookedSeats)} đã được đặt!");
            }

            return null;
        }
        /// <summary>
        /// (Dành cho chỉ đặt sản phẩm) Tạo hóa đơn đặt hàng
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="orderedProductList"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string message)> CreateProductOrder(BillDTO bill, List<ProductBillInfoDTO> orderedProductList)
        {
            IsEnglish = TicketWindowViewModel.IsEnglish;
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    //Bill
                    string billId = await CreateNewBill(context, bill);

                    //Product
                    bool addSuccess = await AddNewProductBills(context, billId, orderedProductList);
                    if (!addSuccess)
                    {
                        return (false, IsEnglish ? "The number of products is not enough to satisfy!" : "Số lượng sản phẩm không đủ để đáp ứng!");
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return (false, IsEnglish ? "System Error" : "Lỗi hệ thống");
            }
            return (true,IsEnglish?"Successful product purchase!!": "Mua sản phẩm thành công!");
        }


        private async Task<string> CreateNewBill(CinemaManagementProjectEntities context, BillDTO bill)
        {

            string maxBillCode = await context.Bills.MaxAsync(b => b.BillCode);
            maxBillCode = CreateNextBillId(maxBillCode);
            Bill newBill = null;
            if (bill.CustomerId != 0)
            {
                newBill = new Bill
                {
                    BillCode = maxBillCode,
                    DiscountPrice = bill.DiscountPrice,
                    TotalPrize = bill.TotalPrice,
                    CustomerId= bill.CustomerId,
                    CreateDate = DateTime.Now,
                    StaffId = bill.StaffId,
                };
            }
            else
            {
                newBill = new Bill
                {
                    BillCode = maxBillCode,
                    DiscountPrice = bill.DiscountPrice,
                    TotalPrize = bill.TotalPrice,
                    CustomerId = null,
                CreateDate = DateTime.Now,
                StaffId = bill.StaffId
            };
            }

            context.Bills.Add(newBill);

            if (bill.VoucherIdList != null && bill.VoucherIdList.Count > 0)
            {
                string voucherIds = string.Join(",", bill.VoucherIdList);
                var sql = $@"Update [Voucher] SET VoucherStatus = N'{VOUCHER_STATUS.USED}', CustomerId = '{newBill.CustomerId}' , UsedAt = GETDATE()  WHERE Id IN ({voucherIds})";
                await context.Database.ExecuteSqlCommandAsync(sql);
            }
            return maxBillCode;
        }
        private void AddNewTickets(CinemaManagementProjectEntities context, string billCode, List<TicketDTO> newTicketList)
        {
            List<Ticket> ticketList = new List<Ticket>();

            for (int i = 0; i < newTicketList.Count; i++)
            {
                ticketList.Add(new Ticket
                {
                    BillCode = billCode,
                    Price = newTicketList[i].Price,
                    SeatId = newTicketList[i].SeatId,
                    ShowTimeId = newTicketList[i].ShowtimeId,
                }); ;
            }
            context.Tickets.AddRange(ticketList);
        }

        private async Task<bool> AddNewProductBills(CinemaManagementProjectEntities context, string billCode, List<ProductBillInfoDTO> orderedProductList)
        {
            List<ProductBillInfo> prodBillList = new List<ProductBillInfo>();

            List<int> prodIdList = new List<int>();

            for (int i = 0; i < orderedProductList.Count; i++)
            {
                prodBillList.Add(new ProductBillInfo
                {
                    BillCode = billCode,
                    ProductId = (int)orderedProductList[i].ProductId,
                    PrizePerProduct = (int)orderedProductList[i].PrizePerProduct,
                    Quantity = orderedProductList[i].Quantity
                });
                var Product = await context.Products.FindAsync(orderedProductList[i].ProductId);
                Product.ProductStorage.Quantity -= orderedProductList[i].Quantity;

                if (Product.ProductStorage.Quantity < 0)
                {
                    return false;
                }
            }

            context.ProductBillInfoes.AddRange(prodBillList);
            return true;

        }

    }
}
