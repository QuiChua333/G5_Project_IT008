using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagementProject.Model.Service
{
    public class VoucherService
    {
        private static VoucherService _ins;
        public static VoucherService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new VoucherService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private VoucherService()
        {
        }
        public async Task<List<VoucherReleaseDTO>> GetAllVoucherReleases()
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var VrList = await (from vr in context.VoucherReleases
                                        where vr.IsDeleted==false
                                        orderby vr.Id descending
                                        select new VoucherReleaseDTO
                                        {
                                            VoucherReleaseCode = vr.VoucherReleaseCode,
                                            VoucherReleaseName = vr.VoucherReleaseName,
                                            StartDate = (DateTime)vr.StartDate,
                                            EndDate = (DateTime)vr.EndDate,
                                            MinimizeTotal = (double)vr.MinimizeTotal,
                                            Price = (double)vr.Price,
                                            TypeObject = vr.TypeObject,
                                            VoucherReleaseStatus = (bool)vr.VoucherReleaseStatus,
                                            VCount = vr.Vouchers.Count(),
                                            UnusedVCount = vr.Vouchers.Count(v => v.VoucherStatus == VOUCHER_STATUS.UNRELEASED),
                                        }).ToListAsync();
                    return VrList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private string CreateNextVoucherReleaseId(string maxCode)
        {
            
            if (maxCode is null)
            {
                return "FatFimFoo0000";
            }
            int index = (int.Parse(maxCode.Substring(9)) + 1);
            string CodeID = index.ToString();
            while (CodeID.Length < 4) CodeID = "0" + CodeID;

            return "FatFimFoo" + CodeID;
        }
        public async Task<(bool, string, VoucherReleaseDTO newVR)> CreateVoucherRelease(VoucherReleaseDTO newVR)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    string maxCode = await context.VoucherReleases.MaxAsync(vR => vR.VoucherReleaseCode);
                    VoucherRelease voucherRelease = new VoucherRelease
                    {
                        VoucherReleaseCode = CreateNextVoucherReleaseId(maxCode),
                        VoucherReleaseName = newVR.VoucherReleaseName,
                        StartDate = newVR.StartDate,
                        EndDate = newVR.EndDate,
                        EnableMerge = newVR.EnableMerge,
                        MinimizeTotal = (float)newVR.MinimizeTotal,
                        Price = (float)newVR.Price,
                        TypeObject = newVR.TypeObject,
                        VoucherReleaseStatus = newVR.VoucherReleaseStatus,
                        IsDeleted = false
                    };

                    context.VoucherReleases.Add(voucherRelease);
                    await context.SaveChangesAsync();

                    newVR.VoucherReleaseCode = voucherRelease.VoucherReleaseCode;
                    return (true, "Thêm đợt phát hành mới thành công", newVR);
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public async Task<(VoucherReleaseDTO, bool haveAnyUsedVoucher)> GetVoucherReleaseDetails(string Code)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var voucherRelease =  await context.VoucherReleases.FirstOrDefaultAsync(x => x.VoucherReleaseCode==Code);
                    bool haveAnyUsedVoucher = voucherRelease.Vouchers.Any(v => v.VoucherStatus == VOUCHER_STATUS.USED);
                    return (new VoucherReleaseDTO
                    {
                        Id= voucherRelease.Id,
                        VoucherReleaseCode = voucherRelease.VoucherReleaseCode,
                        VoucherReleaseName = voucherRelease.VoucherReleaseName,
                        StartDate = (DateTime)voucherRelease.StartDate,
                        EndDate = (DateTime)voucherRelease.EndDate,
                        EnableMerge = (bool)voucherRelease.EnableMerge,
                        MinimizeTotal = (double)voucherRelease.MinimizeTotal,
                        Price = (double)voucherRelease.Price,
                        TypeObject = voucherRelease.TypeObject,
                        VoucherReleaseStatus = (bool)voucherRelease.VoucherReleaseStatus,
                        VCount = voucherRelease.Vouchers.Count(),
                        UnusedVCount = voucherRelease.Vouchers.Count(v => v.VoucherStatus == VOUCHER_STATUS.UNRELEASED),
                        Vouchers = voucherRelease.Vouchers.Select(vR => new VoucherDTO
                        {
                            Id = vR.Id,
                            VoucherCode = vR.VoucherCode,
                            VoucherStatus = vR.VoucherStatus,
                            VoucherReleaseId = (int)vR.VoucherReleaseId,
                            UsedAt = (DateTime)vR.UsedAt.GetValueOrDefault(),
                            CustomerName = vR.Customer?.CustomerName,
                            ReleaseAt = (DateTime)vR.ReleaseAt.GetValueOrDefault(),
                        }).ToList()
                    }, haveAnyUsedVoucher); ;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string)> UpdateVoucherRelease(VoucherReleaseDTO upVoucherR)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {

                    VoucherRelease voucherRelease = await context.VoucherReleases.FirstOrDefaultAsync(x => x.VoucherReleaseCode == upVoucherR.VoucherReleaseCode);

                    voucherRelease.VoucherReleaseName = upVoucherR.VoucherReleaseName;
                    voucherRelease.StartDate = upVoucherR.StartDate;
                    voucherRelease.EndDate = upVoucherR.EndDate;
                    voucherRelease.EnableMerge = upVoucherR.EnableMerge;
                    voucherRelease.MinimizeTotal = (float)upVoucherR.MinimizeTotal;
                    voucherRelease.Price = (float)upVoucherR.Price;
                    voucherRelease.TypeObject = upVoucherR.TypeObject;
                    voucherRelease.VoucherReleaseStatus = upVoucherR.VoucherReleaseStatus;

                    await context.SaveChangesAsync();

                    return (true, "Cập nhật đợt phát hành thành công!");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi");
            }
        }
        public async Task<(bool, string)> DeteleVoucherRelease(string VrCode)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    VoucherRelease voucherRelease = (await context.VoucherReleases.FirstOrDefaultAsync(x => x.VoucherReleaseCode == VrCode));
                    voucherRelease.IsDeleted= true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa đợt phát hành thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string)> DeteleVouchers(List<int> ListCodeId)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    context.Vouchers.RemoveRange(context.Vouchers.Where(v => ListCodeId.Contains(v.Id)));
                    await context.SaveChangesAsync();
                    return (true, "Xóa danh sách voucher thành công");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string, List<VoucherDTO> voucherList)> CreateVoucher(int voucherReleaseId, List<VoucherDTO> ListVoucher)
        {
            try
            {
                List<string> ListCode = ListVoucher.Select(v => v.VoucherCode).ToList();
               
                using (var context = new CinemaManagementProjectEntities())
                {
                    var IsExist = context.Vouchers.Any(v => ListCode.Contains(v.VoucherCode));
                    
                    if (IsExist)
                    {
                        return (false, "Mã voucher đã tồn tại!", null);
                    }
                    VoucherRelease vl = await context.VoucherReleases.FindAsync(voucherReleaseId);
                    List<Voucher> vouchers = ListCode.Select(c => new Voucher
                    {
                        VoucherCode = c,
                        VoucherReleaseId = voucherReleaseId,
                        VoucherStatus = VOUCHER_STATUS.UNRELEASED,
                        EnableMerge=vl.VoucherReleaseStatus,
                    }).ToList();

                    context.Vouchers.AddRange(vouchers);
                    await context.SaveChangesAsync();
                    return (true, "Thêm voucher thành công", vouchers.Select(v => new VoucherDTO
                    {
                        VoucherReleaseId = (int)v.VoucherReleaseId,
                        Id = v.Id,
                        VoucherCode = v.VoucherCode,
                        VoucherStatus=VOUCHER_STATUS.UNRELEASED,       
                    }).ToList());
                }
            }
            catch (Exception e) 
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public async Task<(bool, string, List<VoucherDTO> voucherList)> CreateRandomVoucherList(VoucherReleaseDTO voucherRelease, List<string> ListCode)
        {
            try
            {
                List<Voucher> vouchers = ListCode.Select(c => new Voucher
                {
                    VoucherCode = c,
                    VoucherReleaseId = voucherRelease.Id,
                    VoucherStatus = VOUCHER_STATUS.UNRELEASED,
                    EnableMerge = voucherRelease.VoucherReleaseStatus,
                    
                }).ToList();

                using (var context = new CinemaManagementProjectEntities())
                {
                    context.Vouchers.AddRange(vouchers);
                    await context.SaveChangesAsync();
                    return (true, "Thêm danh sách voucher thành công", vouchers.Select(v => new VoucherDTO
                    {
                        VoucherReleaseId = (int)v.VoucherReleaseId,
                        Id = v.Id,
                        VoucherCode = v.VoucherCode,
                        VoucherStatus = v.VoucherStatus,
                        
                    }).ToList());
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public async Task<(bool, string)> ReleaseMultiVoucher(List<int> ListCodeId)
        {
            try
            {
                string idList = string.Join(",", ListCodeId);
                using (var context = new CinemaManagementProjectEntities())
                {
                    var sql = $@"Update [Voucher] SET VoucherStatus = '{VOUCHER_STATUS.REALEASED}', ReleaseAt = GETDATE()  WHERE Id IN ({idList})";
                    await context.Database.ExecuteSqlCommandAsync(sql);
                }
                return (true, "Phát hành thành công");
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(string error, VoucherDTO)> GetVoucherInfo(string Code)
        {
            using (var context = new CinemaManagementProjectEntities())
            {
                try
                {
                    var voucher = await context.Vouchers.Where(v => v.VoucherCode == Code).Select(v => new VoucherDTO
                    {
                        Id = v.Id,
                        VoucherCode = v.VoucherCode,
                        VoucherStatus = v.VoucherStatus,
                        VoucherReleaseId = (int)v.VoucherReleaseId,
                        UsedAt = (DateTime)v.UsedAt,
                        CustomerName = v.Customer != null ? v.Customer.CustomerName : null,
                        ReleaseAt = (DateTime)v.ReleaseAt,
                        VoucherInfo = new VoucherReleaseDTO
                        {
                            Id = v.VoucherRelease.Id,
                            VoucherReleaseCode = v.VoucherRelease.VoucherReleaseCode,
                            VoucherReleaseName = v.VoucherRelease.VoucherReleaseName,
                            StartDate = (DateTime)v.VoucherRelease.StartDate,
                            EndDate = (DateTime)v.VoucherRelease.EndDate,
                            MinimizeTotal = (double)v.VoucherRelease.MinimizeTotal,
                            Price = (double)v.VoucherRelease.Price,
                            TypeObject = v.VoucherRelease.TypeObject,
                            VoucherReleaseStatus = (bool)v.VoucherRelease.VoucherReleaseStatus,
                            EnableMerge = (bool)v.VoucherRelease.EnableMerge,
                        }
                    }).FirstOrDefaultAsync();

                    if (voucher is null || !voucher.VoucherInfo.VoucherReleaseStatus || voucher.VoucherStatus == VOUCHER_STATUS.UNRELEASED)
                    {
                        return ("Mã giảm giá không tồn tại", null);
                    }

                    if (voucher.VoucherInfo.EndDate < DateTime.Now)
                    {
                        return ("Mã giảm giá đã hết hạn sử dụng", null);
                    }

                    if (voucher.VoucherStatus == VOUCHER_STATUS.USED)
                    {
                        return ("Mã giảm giá đã sử dụng", null);
                    }

                    voucher.Price = voucher.VoucherInfo.Price;
                    voucher.TypeObject = voucher.VoucherInfo.TypeObject;
                    voucher.EnableMerge = voucher.VoucherInfo.EnableMerge;

                    voucher.VoucherInfoStr = $"Giảm {String.Format(CultureInfo.InvariantCulture, "{0:#,#}", voucher.Price)} đ ({voucher.TypeObject})";

                    return (null, voucher);
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    return ("Mất kết nối cơ sở dữ liệu", null);
                }
                catch (Exception)
                {
                    return ("Lỗi hệ thống", null);
                }
            }
        }


    }
}
