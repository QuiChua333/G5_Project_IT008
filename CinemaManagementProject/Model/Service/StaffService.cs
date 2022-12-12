using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                              //where s.IsDeleted == false
                              select new StaffDTO
                              {
                                  Id = s.Id,
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
        public async Task<(bool, string, StaffDTO)> Login(string username, string password)
        {

            string hassPass = Helper.MD5Hash(password);

            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var staff = await (from s in context.Staffs
                                       where s.UserName == username && s.UserPass == hassPass
                                       select new StaffDTO
                                       {
                                           Id = s.Id,
                                           DateOfBirth = s.DateOfBirth,
                                           Gender = s.Gender,
                                           StaffName = s.StaffName,
                                           Position = s.Position,
                                           PhoneNumber = s.PhoneNumber,
                                           StartDate = s.DateOfBirth,
                                           Email = s.Email
                                       }).FirstOrDefaultAsync();



                    if (staff == null)
                    {
                        return (false, "Sai tài khoản hoặc mật khẩu", null);
                    }
                    return (true, "", staff);
                }

            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }


        }
        private string CreateNextStaffId(string maxId)
        {
            //NVxxx
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "NV" + newIdString.Substring(newIdString.Length - 3, 3);
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
                        return (false, "Số điện thoại đã tồn tại!", null);
                    }
                    if (usernameIsExist)
                    {
                        return (false, "Tài khoản đã tồn tại!", null);
                    }

                    if (newStaff.Email != null)
                    {
                        bool emailIsExist = await context.Staffs.AnyAsync(s => s.Email == newStaff.Email);
                        if (emailIsExist)
                        {
                            return (false, "Email đã được đăng kí!", null);
                        }
                    }

                    var maxId = await context.Staffs.MaxAsync(s => s.Id);
                    Staff st = Copy(newStaff);
                    st.Id = maxId + 1;
                    newStaff.Id = st.Id;
                    //st.UserPass = Helper.MD5Hash(newStaff.UserPass);

                    context.Staffs.Add(st);
                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
            return (true, "Thêm nhân viên mới thành công", newStaff);
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
                Email = s.Email
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
                        return (false, "Tài khoản đăng nhập đã tồn tại");
                    }

                    if (updatedStaff.Email != null)
                    {
                        bool emailIsExist = await context.Staffs.AnyAsync(s => s.Email == updatedStaff.Email && s.Id != updatedStaff.Id);
                        if (emailIsExist)
                        {
                            return (false, "Email đã được đăng kí!");
                        }
                    }

                    bool PhoneNumberIsExist = await context.Staffs.AnyAsync(s => s.PhoneNumber == updatedStaff.PhoneNumber && s.Id != updatedStaff.Id);

                    if (PhoneNumberIsExist)
                    {
                        return (false, "Số điện thoại đã tồn tại!");
                    }

                    Staff staff = await context.Staffs.FindAsync(updatedStaff.Id);
                    if (staff == null)
                    {
                        return (false, "Nhân viên không tồn tại");
                    }

                    staff.DateOfBirth = updatedStaff.DateOfBirth;
                    staff.Gender = updatedStaff.Gender;
                    staff.UserName = updatedStaff.UserName;
                    staff.StaffName = updatedStaff.StaffName;
                    staff.Position = updatedStaff.Position;
                    staff.PhoneNumber = updatedStaff.PhoneNumber;
                    staff.DateOfBirth = updatedStaff.DateOfBirth;
                    staff.Email = updatedStaff.Email;

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Error Server");
            }
            return (true, "Cập nhật thành công");

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
                        return (false, "Tài khoản không tồn tại");
                    }

                    //staff.UserPass = Helper.MD5Hash(newPassword);
                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, "Lỗi hệ thống");
            }
            return (true, "Cập nhật mật khẩu thành công");

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
                        return (false, "Nhân viên không tồn tại!");
                    }
                    staff.IsDeleted = true;
                    staff.UserName = null;
                    staff.Email = null;

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception)
            {
                return (false, $"Lỗi hệ thống.");
            }
            return (true, "Xóa nhân viên thành công");
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
                                   where p.UserName == username && !p.IsDeleted
                                   select p).FirstOrDefault();
                    if (staff is null || staff?.IsDeleted == true)
                    {
                        return ("Tài khoản đăng nhập không tồn tại!", null, null);
                    }

                    if (staff.Email is null)
                    {
                        return ("Tài khoản chưa đăng kí email. Vui lòng liên hệ quản lý để được hỗ trợ", null, null);
                    }

                    return (null, staff.Email, staff.Id.ToString());
                }
            }
            catch (Exception)
            {
                return ($"Lỗi hệ thống.", null, null);
            }
        }

    }
}

