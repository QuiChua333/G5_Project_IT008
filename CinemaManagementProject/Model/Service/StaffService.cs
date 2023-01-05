using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CinemaManagementProject.Model.Service
{
    internal class StaffService
    {
        private StaffService() { }
        private static StaffService _ins;
        public static StaffService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new StaffService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        public async Task<List<StaffDTO>> GetAllStaff()
        {
            using (var context = new CinemaManagementProjectEntities())
            {
                var staffs = (from s in context.Staffs
                              where s.IsDeleted == false
                              select new StaffDTO
                              {
                                  Id = s.Id,
                                  MaNV=s.MaNV,
                                  DateOfBirth = s.DateOfBirth,
                                  Gender = s.Gender,
                                  UserName = s.UserName,
                                  StaffName = s.StaffName,
                                  Position = s.Position,
                                  PhoneNumber = s.PhoneNumber,
                                  StartDate = s.StartDate,
                                  UserPass = s.UserPass,
                                  Email = s.Email
                              }).ToListAsync();
                return await staffs;
            }
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
                                                 MaNV= staff.MaNV,
                                                 StaffName = staff.StaffName,
                                                 Gender = staff.Gender,
                                                 DateOfBirth = (DateTime)staff.DateOfBirth,
                                                 Email = staff.Email,
                                                 PhoneNumber = staff.PhoneNumber,
                                                 Position = staff.Position,
                                                 StartDate = staff.StartDate,
                                                 UserName = staff.UserName,
                                                 UserPass = staff.UserPass,
                                                 Avatar = staff.Avatar
                                             }
                                            ).FirstOrDefaultAsync();
                    if (staffChosen == null)
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
        private string CreateNextStaffCode(string maxCode)
        {
            if (maxCode is null)
            {
                return "NV0001";
            }
            int index = (int.Parse(maxCode.Substring(2)) + 1);
            string CodeID = index.ToString();
            while (CodeID.Length < 4) CodeID = "0" + CodeID;

            return "NV" + CodeID;
        }
        public async Task<(bool, string, StaffDTO)> AddStaff(StaffDTO newStaff)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    bool usernameIsExist = await context.Staffs.AnyAsync(s => s.UserName == newStaff.UserName);
                    bool PhoneNumberIsExist = await context.Staffs.AnyAsync(s => s.PhoneNumber == newStaff.PhoneNumber);

                    if (PhoneNumberIsExist)
                    {
                        return (false, Properties.Settings.Default.isEnglish == false ? "Số điện thoại đã tồn tại!": "Phone number already exists!", null);
                    }
                    if (usernameIsExist)
                    {
                        return (false, Properties.Settings.Default.isEnglish == false ? "Tài khoản đã tồn tại!": "Account already exists!", null);
                    }

                    if (newStaff.Email != null)
                    {
                        bool emailIsExist = await context.Staffs.AnyAsync(s => s.Email == newStaff.Email);
                        if (emailIsExist)
                        {
                            return (false, Properties.Settings.Default.isEnglish == false ? "Email đã được đăng kí!": "The Email was registered!", null);
                        }
                    }

                    var maxId = await context.Staffs.MaxAsync(s => s.MaNV);
                    Staff st = new Staff
                    {
                        MaNV = CreateNextStaffCode(maxId),
                        StaffName = newStaff.StaffName,
                        PhoneNumber = newStaff.PhoneNumber,
                        Email = newStaff.Email,
                        Gender= newStaff.Gender,
                        DateOfBirth = newStaff.DateOfBirth,
                        Position=newStaff.Position,
                        IsDeleted = false,
                        StartDate=newStaff.StartDate,
                        UserName=newStaff.UserName,
                        UserPass = newStaff.UserPass
                    };

                    context.Staffs.Add(st);
                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, Properties.Settings.Default.isEnglish == false ? "Mất kết nối cơ sở dữ liệu": "Unable to connect to database", null);
            }
            catch (Exception e)
            {
                return (false, /*"Lỗi hệ thống"*/ e.Message, null);
            }
            return (true, Properties.Settings.Default.isEnglish == false ? "Thêm nhân viên mới thành công": "Add New Employee Successfully", newStaff);
        }
        private Staff Copy(StaffDTO s)
        {
            return new Staff
            {
                DateOfBirth = s.DateOfBirth,
                Gender = s.Gender,
                UserName = s.UserName,
                StaffName = s.StaffName,
                Position = s.Position,
                PhoneNumber = s.PhoneNumber,
                StartDate = s.StartDate,
                Email = s.Email,
                Avatar = s.Avatar,
            };
        }

        public async Task<(bool, string)> UpdateStaff(StaffDTO updatedStaff)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    bool usernameIsExist = await context.Staffs.AnyAsync(s => s.UserName == updatedStaff.UserName && s.Id != updatedStaff.Id);

                    if (usernameIsExist)
                    {
                        return (false, Properties.Settings.Default.isEnglish == false ? "Tài khoản đăng nhập đã tồn tại" : "Account already exists!" );
                    }

                    if (updatedStaff.Email != null)
                    {
                        bool emailIsExist = await context.Staffs.AnyAsync(s => s.Email == updatedStaff.Email && s.Id != updatedStaff.Id);
                        if (emailIsExist)
                        {
                            return (false, Properties.Settings.Default.isEnglish == false ? "Email đã được đăng kí!" : "The Email was registered!");
                        }
                    }

                    bool PhoneNumberIsExist = await context.Staffs.AnyAsync(s => s.PhoneNumber == updatedStaff.PhoneNumber && s.Id != updatedStaff.Id);

                    if (PhoneNumberIsExist)
                    {
                        return (false, Properties.Settings.Default.isEnglish == false ? "Số điện thoại đã tồn tại!": "Phone number already exists!");
                    }

                    Staff staff = await context.Staffs.FindAsync(updatedStaff.Id);
                    if (staff == null)
                    {
                        return (false, Properties.Settings.Default.isEnglish == false ? "Nhân viên không tồn tại": "Staff does not exist");
                    }

                    staff.DateOfBirth = updatedStaff.DateOfBirth;
                    staff.Gender = updatedStaff.Gender;
                    staff.UserName = updatedStaff.UserName;
                    staff.StaffName = updatedStaff.StaffName;
                    staff.Position = updatedStaff.Position;
                    staff.PhoneNumber = updatedStaff.PhoneNumber;
                    staff.DateOfBirth = updatedStaff.DateOfBirth;
                    staff.Email = updatedStaff.Email;
                    staff.Avatar = updatedStaff.Avatar;

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, Properties.Settings.Default.isEnglish == false ? "Mất kết nối cơ sở dữ liệu" : "Unable to connect to database");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Error Server");
            }
            return (true, Properties.Settings.Default.isEnglish == false ? "Cập nhật thành công": "Update successful");

        }

        public async Task<(bool, string)> UpdatePassword(int StaffId, string newPassword)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Staff staff = await context.Staffs.FindAsync(StaffId);
                    if (staff is null)
                    {
                        return (false, Properties.Settings.Default.isEnglish == false ? "Tài khoản không tồn tại": "Account does not exist");
                    }

                    //staff.UserPass = Helper.MD5Hash(newPassword);
                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, Properties.Settings.Default.isEnglish == false ? "Mất kết nối cơ sở dữ liệu" : "Unable to connect to database");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, Properties.Settings.Default.isEnglish == false ? "Lỗi hệ thống": "System error");
            }
            return (true, Properties.Settings.Default.isEnglish == false ? "Cập nhật mật khẩu thành công": "Password update successful");

        }
        public async Task<(bool, string)> DeleteStaff(string Id)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Staff staff = await (from p in context.Staffs
                                         where p.Id.ToString() == Id /*&& !p.IsDeleted*/
                                         select p).FirstOrDefaultAsync();
                    if (staff is null || staff?.IsDeleted == true)
                    {
                        return (false, Properties.Settings.Default.isEnglish == false ? "Nhân viên không tồn tại" : "Staff does not exist");
                    }
                    staff.IsDeleted = true;
                    staff.UserName = null;
                    staff.Email = null;

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, Properties.Settings.Default.isEnglish == false ? "Mất kết nối cơ sở dữ liệu" : "Unable to connect to database");
            }
            catch (Exception)
            {
                return (false, Properties.Settings.Default.isEnglish == false ? "Lỗi hệ thống" : "System error");
            }
            return (true, Properties.Settings.Default.isEnglish == false ? "Xóa nhân viên thành công": "Delete employee successfully");
        }

        /// <summary>
        /// Dùng để tìm email của staff và gửi mail cho chức năng quên mật khẩu
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public (string error, string email, string Id) GetStaffEmail(string username)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    Staff staff = (from p in context.Staffs
                                   where p.UserName == username && !(bool)p.IsDeleted
                                   select p).FirstOrDefault();
                    if (staff is null || staff?.IsDeleted == true)
                    {
                        return (Properties.Settings.Default.isEnglish == false ? "Tài khoản không tồn tại" : "Account does not exist", null, null);
                    }

                    if (staff.Email is null)
                    {
                        return (Properties.Settings.Default.isEnglish == false ? "Tài khoản chưa đăng kí email. Vui lòng liên hệ quản lý để được hỗ trợ": "The account has not registered email. Please contact the manager for support", null, null);
                    }

                    return (null, staff.Email, staff.Id.ToString());
                }
            }
            catch (Exception)
            {
                return (Properties.Settings.Default.isEnglish == false ? "Lỗi hệ thống" : "System error", null, null);
            }
        }

    }
}

