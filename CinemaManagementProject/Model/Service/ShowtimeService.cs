using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaManagementProject.Model.Service
{
    public class ShowtimeService
    {
        private static ShowtimeService _ins;
        public static ShowtimeService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ShowtimeService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private ShowtimeService() { }


        public async Task<(bool IsSuccess, string message)> AddShowtime(ShowtimeDTO newShowtime)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    //Uncomment when release
                    //if (newShowtime.ShowDate < DateTime.Today)
                    //{
                    //    return (false,"Thời gian này đã qua không thể thêm suất chiếu" ,null);
                    //}
                    var showtimeSet = await context.ShowTimeSettings
                    .Where(s => DbFunctions.TruncateTime(s.ShowDate) == newShowtime.ShowDate.Date
                    && s.RoomId == newShowtime.RoomId).FirstOrDefaultAsync();

                    if (showtimeSet == null)
                    {
                        showtimeSet = new ShowTimeSetting
                        {
                            RoomId = newShowtime.RoomId,
                            ShowDate = newShowtime.ShowDate.Date,
                        };
                        context.ShowTimeSettings.Add(showtimeSet);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        ShowTime show = null;

                        Film m = await context.Films.FindAsync(newShowtime.FilmId);
                        var newStartTime = newShowtime.StartTime;
                        var newEndTime = newShowtime.StartTime + new TimeSpan(0, (int)m.Duration, 0);
                        show = showtimeSet.ShowTimes.AsEnumerable().Where(s =>
                        {
                            var endTime = new TimeSpan(0, (int)s.Film.Duration, 0) + s.StartTime;
                            return TimeBetwwenIn((TimeSpan)newStartTime, (TimeSpan)newEndTime, (TimeSpan)s.StartTime, (TimeSpan)(endTime + TIME.BreakTime));
                        }).FirstOrDefault();

                        if (show != null)
                        {
                            var endTime = new TimeSpan(0, (int)show.Film.Duration, 0) + show.StartTime;
                            return (false, $"Khoảng thời gian từ {Helper.GetHourMinutes((TimeSpan)show.StartTime)} đến {Helper.GetHourMinutes((TimeSpan)(endTime + TIME.BreakTime))} đã có phim chiếu tại phòng {showtimeSet.RoomId}");
                        }
                    }

                    ShowTime showtime = new ShowTime
                    {
                        FilmId = newShowtime.FilmId,
                        ShowTimeSettingId = showtimeSet.Id,
                        StartTime = newShowtime.StartTime,
                        Price = newShowtime.Price
                    };
                    context.ShowTimes.Add(showtime);

                    //setting seats in room for new showtime 
                    var seatIds = await (from s in context.Seats
                                         where s.RoomId == showtimeSet.RoomId
                                         select s.Id
                           ).ToListAsync();
                    List<SeatSetting> seatSetList = new List<SeatSetting>();
                    foreach (var seatId in seatIds)
                    {
                        seatSetList.Add(new SeatSetting
                        {
                            SeatId = seatId,
                            ShowTimeId = showtime.Id
                        });
                    }
                    context.SeatSettings.AddRange(seatSetList);


                    await context.SaveChangesAsync();
                    newShowtime.Id = showtime.Id;
                    return (true, "Thêm suất chiếu thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống" + e.Message);

            }
        }
        public async Task<(bool IsSuccess, string message)> DeleteShowtime(int showtimeId)
        {

            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    ShowTime show = await context.ShowTimes.FindAsync(showtimeId);
                    if (show is null)
                    {
                        return (false, "Suất chiếu không tồn tại!");
                    }
                    context.ShowTimes.Remove(show);
                    await context.SaveChangesAsync();
                }


            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Xóa suất chiếu thành công!");
        }
        public async Task<(bool IsSuccess, string message)> UpdateTicketPrice(int showtimeId, float price)
        {

            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    ShowTime show = await context.ShowTimes.FindAsync(showtimeId);
                    if (show is null)
                    {
                        return (false, "Suất chiếu không tồn tại!");
                    }
                    show.Price = price;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Cập nhật giá thành công!");
        }
        public async Task<bool> CheckShowtimeHaveBooking(int showtimeId)
        {

            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var IsExist = await context.SeatSettings.AnyAsync(s => s.ShowTimeId == showtimeId && s.SeatStatus==true);
                    return IsExist;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Check (t1,t2) vs (a1,a2)
        bool TimeBetwwenIn(TimeSpan t1, TimeSpan t2, TimeSpan a1, TimeSpan a2)
        {

            if ((t1 >= a1 && t1 <= a2) || (t2 >= a1 && t2 <= a2))
                return true;
            if (t1 <= a1 && t2 >= a2)
            {
                return true;
            }
            // t2 > t1;
            return false;
        }

    }
}
