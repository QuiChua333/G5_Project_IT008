using CinemaManagementProject.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model.Service
{
    internal class CustomerService
    {
        private static CustomerService _ins;
        public static CustomerService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new CustomerService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private CustomerService()
        {
        }
        public async Task<List<CustomerDTO>> GetAllStaff()
        {
            using (var context = new CinemaManagementProjectEntities())
            {
                var staffs = (from s in context.Customers
                              where s.IsDeleted == false
                              select new CustomerDTO
                              {
                                  Id = s.Id,
                                  Name = s.CustomerName,           
                                  Email = s.Email
                              }).ToListAsync();
                return await staffs;
            }
        }
    }
}
