using CinemaManagementProject.DTOs;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    public class SettingService
    {
        private static SettingService _ins;
        public static SettingService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new SettingService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private SettingService() { }
        public async Task<(bool, string)> EditName(string StaffName, int Id)
        {
            try
            {
                using(var context = new CinemaManagementProjectEntities())
                {
                    Staff staff = await context.Staffs.FindAsync(Id);
                    if (staff == null)
                        return (false, "Lỗi hệ thống");
                    staff.StaffName = StaffName;
                    await context.SaveChangesAsync();
                    return (true, "Lưu thông tin thành công");
                }    
            }
            catch(EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch(Exception e)
            {
                return (false, "lỗi hệ thống");
            }
        }
        public async Task<(bool, string)> EditEmail(string StaffEmail, int Id)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Staff staff = await context.Staffs.FindAsync(Id);
                    if (staff == null)
                        return (false, "Lỗi hệ thống");
                    staff.Email = StaffEmail;
                    await context.SaveChangesAsync();
                    return (true, "Lưu thông tin thành công");
                }
            }
            catch (EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "lỗi hệ thống");
            }
        }
    }
}
