using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    public class SeatService
    {
        private static SeatService _ins;
        public static SeatService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new SeatService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private SeatService() { }

        public async Task<List<SeatSettingDTO>> GetSeatsByShowtime(int showtimeId)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var seatList = await (from s in context.SeatSettings
                                          where s.ShowTimeId == showtimeId
                                          select new SeatSettingDTO
                                          {
                                              SeatId = s.SeatId,
                                              ShowTimeId = s.ShowTimeId,
                                              SeatStatus = s.SeatStatus,
                                              Seat = new SeatDTO
                                              {
                                                  Id = s.Seat.Id,
                                                  RoomId = s.Seat.RoomId,
                                                  SeatRow = s.Seat.SeatRow,
                                                  SeatNumber = s.Seat.SeatNumber,
                                              },
                                          }
                               ).ToListAsync();
                    return seatList;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
