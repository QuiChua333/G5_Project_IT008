using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CinemaManagementProject.Model.Service
{
    public class StaffService
    {
        private static StaffService _ins;
        public static StaffService Ins
        {
            get { if( _ins == null ) _ins = new StaffService(); return _ins; }
            private set { _ins = value; }
        }

        public StaffService()
        {

        }

        public async Task<(bool, string, StaffDTO)> Login(string userNameOrEmail, string password)
        {
            try 
            {
                using (var db = new CinemaManagementProjectEntities())
                {
                    var staffChosen = await (from staff in db.Staffs
                                             where (staff.UserName == userNameOrEmail || staff.Email == userNameOrEmail) && staff.UserPass == password
                                             select new StaffDTO
                                             {
                                                    Id = staff.Id,
                                                    StaffName = staff.StaffName,
                                                    Gender = staff.Gender,
                                                    DateOfBirth = (DateTime)staff.DateOfBirth,
                                                    Email = staff.Email,
                                                    PhoneNumber = staff.PhoneNumber,
                                                    Position = staff.Position,
                                                    StartDate = staff.StartDate,
                                                    UserName = staff.UserName,
                                                    UserPass = staff.UserPass
                                            }
                                            ).FirstOrDefaultAsync();
                    if(staffChosen == null)
                    {
                        return (false, "Sai tài khoản hoặc mật khẩu", null);
                    }
                    return (true, "", staffChosen);
                }
            }
            catch (EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
    }
}
