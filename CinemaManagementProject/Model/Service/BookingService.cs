using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utils;
using CinemaManagementProject.ViewModel.StaffVM.TicketVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.IO;

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
                    bill.BillCode = billCode;
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
                    bill.BillCode = billCode;
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
                return (false, IsEnglish ? "System Error" : "Lỗi hệ thống");
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
        //Gửi mail cho customer khi mua xong
        public async Task SendBillToCustomer(string email,BillDTO bill, string filmName, string showDate, string showTime, string showRoom, string seat, string ticketPrice,string discount, string total, List<ProductBillInfoDTO> listProductBillInfo = null)
        {
            IsEnglish = TicketWindowViewModel.IsEnglish;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string APP_EMAIL = appSettings["APP_EMAIL"];
                string APP_PASSWORD = appSettings["APP_PASSWORD"];

                //Tạo mail
                MailMessage mail = new MailMessage(APP_EMAIL, email);
                mail.To.Add(email);
                mail.Subject = IsEnglish ? "Thank you for choosing FatFilmFoo" : "Cảm ơn quý khách đã chọn xem phim tại FatFilmFoo";
                //Attach file
                mail.IsBodyHtml = true;
                string htmlBody = "";

                if (listProductBillInfo != null)
                {
                    htmlBody = GetHTMLTemplateMovieAndBill(bill.BillCode, filmName, showDate, showTime, showRoom, seat, ticketPrice, discount, total, listProductBillInfo);
                }
                else
                {
                    htmlBody = GetHTMLTemplateMovie(bill.BillCode, filmName, showDate, showTime, showRoom, seat, ticketPrice, discount, total);
                }    
                
                mail.Body = htmlBody;

                //tạo Server

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(APP_EMAIL, APP_PASSWORD);
                SmtpServer.EnableSsl = true;
                await SmtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
            }
        }
        public string GetHTMLTemplateMovie(string billCode, string filmName, string showDate, string showTime, string showRoom, string seat, string ticketPrice, string discount, string total)
        {
            string resetPasswordTemplate = Helper.GetEmailTemplatePath(E_Ticket_FILE);
            string HTML = File.ReadAllText(resetPasswordTemplate).Replace("{BillCode}", billCode).Replace("{FilmName}", filmName).Replace("{ShowDate}", showDate).Replace("{ShowTime}", showTime).Replace("{ShowRoom}", showRoom)
                                                                 .Replace("{Seats}", seat).Replace("{TicketPrice}", ticketPrice).Replace("{Total}", total).Replace("{DiscountPrice}", discount);
            return HTML;
        }
        public string GetHTMLTemplateMovieAndBill(string billCode, string filmName, string showDate, string showTime, string showRoom, string seat, string ticketPrice,string discount, string total, List<ProductBillInfoDTO> listProductBillInfo)
        {
            string resetPasswordTemplate = Helper.GetEmailTemplatePath(E_Ticket_FILE);
            string HTML = File.ReadAllText(resetPasswordTemplate).Replace("{BillCode}", billCode).Replace("{FilmName}", filmName).Replace("{ShowDate}", showDate).Replace("{ShowTime}", showTime).Replace("{ShowRoom}", showRoom)
                                                                 .Replace("{Seats}", seat).Replace("{TicketPrice}", ticketPrice).Replace("{Total}", total).Replace("{DiscountPrice}",discount);
            int length = listProductBillInfo.Count;
            string foodHTML = headerFood;
            for(int i = 0; i < length; i++)
            {
                if(i == 0)
                {
                    foodHTML += FirstFood.Replace("{ProductName}", listProductBillInfo[i].ProductName)
                                         .Replace("{Quantity}", listProductBillInfo[i].Quantity.ToString())
                                         .Replace("{PricePerProduct}", listProductBillInfo[i].PrizePerProductStr);
                }    
                else if(i == length - 1)
                    foodHTML += EndFood.Replace("{ProductName}", listProductBillInfo[i].ProductName)
                                         .Replace("{Quantity}", listProductBillInfo[i].Quantity.ToString())
                                         .Replace("{PricePerProduct}", listProductBillInfo[i].PrizePerProductStr);
                else
                    foodHTML += BodyFood.Replace("{ProductName}", listProductBillInfo[i].ProductName)
                                         .Replace("{Quantity}", listProductBillInfo[i].Quantity.ToString())
                                         .Replace("{PricePerProduct}", listProductBillInfo[i].PrizePerProductStr);

            }    
            return HTML.Replace("<!-- {FOOD_ORDER_HERE} -->", foodHTML);
        }
        const string E_Ticket_FILE = "BillAndReviewFilm.txt";
        const string headerFood = " <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-6\"\r\n                        role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td>\r\n                                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                        class=\"row-content stack\" role=\"presentation\"\r\n                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; border-radius: 0; width: 660px;\"\r\n                                        width=\"660\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td class=\"column column-1\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"100%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-1\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"padding-left:45px;padding-top:5px;width:100%;text-align:center;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #000000; font-size: 23px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: left; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    Foods</h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n";
        const string FirstFood = "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-7\"\r\n                        role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td>\r\n                                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                        class=\"row-content stack\" role=\"presentation\"\r\n                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; border-top: 0 solid #6d6060; border-right: 0px solid #6d6060; border-bottom: 0 solid #6d6060; border-left: 0 solid #6d6060; border-radius: 0; width: 660px;\"\r\n                                        width=\"660\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td class=\"column column-1\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"16.666666666666668%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 23px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: center; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\"></span></h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-2\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; border-left: 1px solid #000000; border-top: 1px solid #000000; vertical-align: middle; border-right: 0px; border-bottom: 0px;\"\r\n                                                    width=\"33.33333333333333%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"padding-left:30px;width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 18px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: left; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\">{ProductName} x{Quantity}</span>\r\n                                                                </h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-3\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; border-top: 1px solid #000000; border-right: 1px solid #000000; vertical-align: middle; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"33.33333333333333%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-right:30px;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 18px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: right; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\">{PricePerProduct}</span>\r\n                                                                </h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-4\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"16.666666666666668%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 23px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: center; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\"></span></h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>";
        const string BodyFood = "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-11\"\r\n                        role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td>\r\n                                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                        class=\"row-content stack\" role=\"presentation\"\r\n                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; border-top: 0 solid #6d6060; border-right: 0px solid #6d6060; border-bottom: 0 solid #6d6060; border-left: 0 solid #6d6060; border-radius: 0; width: 660px;\"\r\n                                        width=\"660\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td class=\"column column-1\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"16.666666666666668%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 23px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: center; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\"></span></h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-2\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; border-left: 1px solid #000000; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px;\"\r\n                                                    width=\"25%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"padding-left:30px;width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 18px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: left; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\">{ProductName} x{Quantity}</span>\r\n                                                                </h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-3\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; border-right: 1px solid #000000; vertical-align: middle; border-top: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"41.666666666666664%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-right:30px;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 18px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: right; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\">{PricePerProduct}</span>\r\n                                                                </h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-4\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"16.666666666666668%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 23px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: center; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\"></span></h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>";
        const string EndFood = "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-12\"\r\n                        role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td>\r\n                                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                        class=\"row-content stack\" role=\"presentation\"\r\n                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; border-top: 0 solid #6d6060; border-right: 0px solid #6d6060; border-bottom: 0 solid #6d6060; border-left: 0 solid #6d6060; border-radius: 0; width: 660px;\"\r\n                                        width=\"660\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td class=\"column column-1\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"16.666666666666668%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 23px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: center; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\"></span></h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-2\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; border-bottom: 1px solid #000000; border-left: 1px solid #000000; vertical-align: middle; border-top: 0px; border-right: 0px;\"\r\n                                                    width=\"25%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"padding-left:30px;width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 18px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: left; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\">{ProductName} x{Quantity}\r\n                                                                    </span></h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-3\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; border-right: 1px solid #000000; border-bottom: 1px solid #000000; vertical-align: middle; border-top: 0px; border-left: 0px;\"\r\n                                                    width=\"41.666666666666664%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"padding-right:30px;width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 18px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: right; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span\r\n                                                                        class=\"tinyMce-placeholder\">{PricePerProduct}</span>\r\n                                                                </h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                                <td class=\"column column-4\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n                                                    width=\"16.666666666666668%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                                        class=\"heading_block block-2\" role=\"presentation\"\r\n                                                        style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n                                                        width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\"\r\n                                                                style=\"width:100%;text-align:center;padding-top:5px;padding-bottom:5px;\">\r\n                                                                <h1\r\n                                                                    style=\"margin: 0; color: #555555; font-size: 23px; font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif; line-height: 120%; text-align: center; direction: ltr; font-weight: 700; letter-spacing: normal; margin-top: 0; margin-bottom: 0;\">\r\n                                                                    <span class=\"tinyMce-placeholder\"></span></h1>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>";
    }
}
