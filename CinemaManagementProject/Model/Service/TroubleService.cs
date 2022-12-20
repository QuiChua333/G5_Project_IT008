using CinemaManagementProject.DTOs;
using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CinemaManagementProject.Model.Service
{
    public class TroubleService
    {
        private static TroubleService _ins;
        public static TroubleService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new TroubleService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private TroubleService()
        {
        }
        public async Task<List<TroubleDTO>> GetAllTrouble()
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    List<TroubleDTO> troubleList = await (  from trou in context.Troubles
                                                            
                                                            select new TroubleDTO
                                                            {
                                                                Id = trou.Id,
                                                                TroubleType = trou.TroubleType,
                                                                Description = trou.Description,
                                                                RepairCost = (float)trou.RepairCost,
                                                                SubmittedAt= (DateTime)trou.SubmittedAt,
                                                                StartDate = (DateTime)trou.StartDate,
                                                                FinishDate = (DateTime)trou.FinishDate,
                                                                TroubleStatus=trou.TroubleStatus,
                                                                StaffId= (int)trou.StaffId,
                                                                Level=trou.Level,
                                                                Image=trou.Image,
                                                                StaffName=trou.Staff.StaffName
                                                            }).ToListAsync();
                    
                    
                    return troubleList; 
                }

            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string)> UpdateStatusTrouble(TroubleDTO updatedTrouble)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {

                    var trouble = await context.Troubles.FindAsync(updatedTrouble.Id);

                    if (updatedTrouble.TroubleStatus == STATUS.IN_PROGRESS)
                    {
                        trouble.StartDate = updatedTrouble.StartDate;
                    }
                    else if (updatedTrouble.TroubleStatus == STATUS.DONE)
                    {
                        if (trouble.TroubleStatus == STATUS.WAITING)
                        {
                            trouble.StartDate = DateTime.Now;
                        }
                        trouble.FinishDate = updatedTrouble.FinishDate;
                        trouble.RepairCost = updatedTrouble.RepairCost;
                    }
                    else if (updatedTrouble.TroubleStatus == STATUS.CANCLE)
                    {
                        trouble.FinishDate = DateTime.Now;
                        trouble.RepairCost = 0;
                    }

                    trouble.TroubleStatus = updatedTrouble.TroubleStatus;

                    await context.SaveChangesAsync();

                    return (true, "Cập nhật thành công");
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }
        public async Task<(bool, string, TroubleDTO)> CreateNewTrouble(TroubleDTO newTrouble)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var maxId = await context.Troubles.MaxAsync(t => t.Id);
                    Trouble tr = new Trouble()
                    {
                        //Id = maxId + 1,
                        RepairCost = 0,
                        TroubleType = newTrouble.TroubleType,
                        Description = newTrouble.Description,
                        Image = newTrouble.Image,
                        TroubleStatus = STATUS.WAITING,
                        Level = newTrouble.Level ?? LEVEL.NORMAL,
                        SubmittedAt = DateTime.Now,
                        StaffId = newTrouble.StaffId,
                    };
                    context.Troubles.Add(tr);

                    await context.SaveChangesAsync();

                    newTrouble.Id = tr.Id;
                    return (true, null, newTrouble);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<(bool, string)> UpdateTroubleInfo(TroubleDTO updatedTrouble)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {

                    var trouble = await context.Troubles.FindAsync(updatedTrouble.Id);

                    trouble.TroubleType = updatedTrouble.TroubleType;
                    trouble.Description = updatedTrouble.Description;

                    trouble.Image = updatedTrouble.Image;
                    trouble.SubmittedAt = DateTime.Now;
                    trouble.StaffId = updatedTrouble.StaffId;
                    trouble.Level = updatedTrouble.Level ?? trouble.Level;

                    await context.SaveChangesAsync();

                    return (true, null);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
