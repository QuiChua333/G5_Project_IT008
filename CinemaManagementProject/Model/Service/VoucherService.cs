using CinemaManagementProject.DTOs;
using CinemaManagementProject.Ultis;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



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
            //NVxxx
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
                            UsedAt = (DateTime)vR.UsedAt,
                            CustomerName = vR.Customer?.CustomerName,
                            ReleaseAt = (DateTime)vR.ReleaseAt,
                        }).ToList()
                    }, haveAnyUsedVoucher); ;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
