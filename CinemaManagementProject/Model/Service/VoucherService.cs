using CinemaManagementProject.DTOs;
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
                                            Id = vr.Id,
                                            VoucherReleaseName = vr.VoucherReleaseName,
                                            StartDate = (DateTime)vr.StartDate,
                                            EndDate = (DateTime)vr.EndDate,
                                            MinimizeTotal = (double)vr.MinimizeTotal,
                                            Price = (double)vr.Price,
                                            TypeObject = vr.TypeObject,
                                            VoucherReleaseStatus = (bool)vr.VoucherReleaseStatus,
                                            VCount = vr.Vouchers.Count(),
                                            UnusedVCount = vr.Vouchers.Count(v => v.VoucherStatus == false),
                                        }).ToListAsync();
                    return VrList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
