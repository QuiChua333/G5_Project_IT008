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
        public async Task<List<CustomerDTO>> GetAllCustomer()
        {
            List<CustomerDTO> customerlist;
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    customerlist = (from s in context.Customers
                                    //where s.IsDeleted == false
                                    select new CustomerDTO
                                    {
                                        Id = s.Id,
                                        Name = s.CustomerName,
                                        Email = s.Email,
                                        PhoneNumber = s.PhoneNumber,
                                        FirstDate = (DateTime)s.FirstDate
                                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return customerlist;
      
        }
        public async Task<CustomerDTO> FindCustomerInfo(string phoneNumber)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var customer = await context.Customers.Where(c => /*!c.IsDeleted && */c.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
                    if (customer is null)
                    {
                        return null;
                    }
                    return new CustomerDTO
                    {
                        Id = customer.Id,
                        Name = customer.CustomerName,
                        PhoneNumber = customer.PhoneNumber,
                        Email = customer.Email,
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<CustomerDTO>> GetAllCustomerByTime(int year, int month = 0)
        {
            try
            {
                if (month == 0)
                {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        var customer = await context.Customers.Where(c =>/* !c.IsDeleted &&*/ ((DateTime)c.FirstDate).Year == year).Select(c => new CustomerDTO
                        {
                            Id = c.Id,
                            Name = c.CustomerName,
                            PhoneNumber = c.PhoneNumber,
                            Email = c.Email,
                            FirstDate = (DateTime)c.FirstDate,
                            Expense = (float)c.Bill.Where(b => ((DateTime)c.FirstDate).Year == year).Sum(b => b.TotalPrize)
                        }).ToListAsync();

                        return customer;
                    }
                }
                else
                {
                    using (var context = new CinemaManagementProjectEntities())
                    {
                        var customer = await context.Customers.Where(c => /*!c.IsDeleted && */((DateTime)c.FirstDate).Year == DateTime.Today.Year && ((DateTime)c.FirstDate).Month == month)
                            .Select(c => new CustomerDTO
                            {
                                Id = c.Id,
                                Name = c.CustomerName,
                                PhoneNumber = c.PhoneNumber,
                                Email = c.Email,
                                FirstDate = (DateTime)c.FirstDate,
                                Expense = c.Bill.Where(b => ((DateTime)c.FirstDate).Year == year && ((DateTime)c.FirstDate).Month == month).Sum(b => (float?)b.TotalPrize) ?? 0
                            }).ToListAsync();

                        return customer;
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public async Task<(bool, string, string CustomerId)> CreateNewCustomer(CustomerDTO newCus)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    if (newCus.Email != null)
                    {
                        bool isExistEmail = await context.Customers.AnyAsync(c => c.Email == newCus.Email);
                        if (isExistEmail)
                        {
                            return (false, "Email này đã tồn tại", null);
                        }
                    }

                    var cus = await context.Customers.Where(c => c.PhoneNumber == newCus.PhoneNumber).FirstOrDefaultAsync();
                    if (cus != null)
                    {
                        if (!cus.IsDeleted)
                        {
                            return (false, "Số điện thoại này đã tồn tại", null);
                        }
                        else
                        {
                            cus.CustomerName = newCus.Name;
                            cus.Email = newCus.Email;
                            cus.FirstDate = DateTime.Now;
                            cus.IsDeleted = false;
                        }

                        await context.SaveChangesAsync();
                        return (true, "Đăng ký thành công", cus.Id.ToString());
                    }
                    //string currentMaxId = await context.Customers.MaxAsync(c => c.Id.ToString());
                    Customer newCusomer = new Customer
                    {                        
                        CustomerName = newCus.Name,
                        PhoneNumber = newCus.PhoneNumber,
                        Email = newCus.Email,
                        FirstDate = DateTime.Now,
                    };

                    context.Customers.Add(newCusomer);
                    await context.SaveChangesAsync();
                    return (true, "Đăng ký thành công", newCusomer.Id.ToString());
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

        public async Task<(bool, string)> UpdateCustomerInfo(CustomerDTO updatedCus)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    bool isExistPhone = await context.Customers.AnyAsync(c => c.Id != updatedCus.Id && c.PhoneNumber == updatedCus.PhoneNumber);

                    if (isExistPhone)
                    {
                        return (false, "Số điện thoại này đã tồn tại");
                    }

                    if (!string.IsNullOrEmpty(updatedCus.Email))
                    {
                        bool isExistEmail = await context.Customers.AnyAsync(c => c.Id != updatedCus.Id && c.Email == updatedCus.Email);
                        if (isExistEmail)
                        {
                            return (false, "Email này đã tồn tại");
                        }
                    }
                    var cus = await context.Customers.FindAsync(updatedCus.Id);

                    cus.CustomerName = updatedCus.Name;
                    cus.PhoneNumber = updatedCus.PhoneNumber;
                    cus.Email = updatedCus.Email;

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
        }

        public async Task<(bool, string)> DeleteCustomer(int id)
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var cus = await context.Customers.FindAsync(id);
                    if (cus is null || cus.IsDeleted)
                    {
                        return (false, "Khách hàng không tồn tại!");
                    }
                    cus.IsDeleted = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
        }

        public async Task<List<CustomerDTO>> GetTop5CustomerEmail()
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var cusStatistic = await context.Bills.Where(b => ((DateTime)b.CreateDate).Year == DateTime.Now.Year && ((DateTime)b.CreateDate).Month == DateTime.Now.Month)
                        .GroupBy(b => b.CustomerId)
                        .Select(grC => new
                        {
                            CustomerId = grC.Key,
                            Expense = grC.Sum(c => (float?)(c.TotalPrize + c.DiscountPrice)) ?? 0
                        })
                        .OrderByDescending(b => b.Expense).Take(5)
                        .Join(
                        context.Customers,
                        statis => statis.CustomerId,
                        cus => cus.Id,
                        (statis, cus) => new CustomerDTO
                        {
                            Id = cus.Id,
                            Name = cus.CustomerName,
                            PhoneNumber = cus.PhoneNumber,
                            Email = cus.Email
                        }).ToListAsync();
                    return cusStatistic;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<CustomerDTO>> GetNewCustomer()
        {
            try
            {
                using (var context = new CinemaManagementProjectEntities())
                {
                    var customers = await context.Customers.Where(c => ((DateTime)c.FirstDate).Year == DateTime.Today.Year && DbFunctions.DiffDays(c.FirstDate, DateTime.Now) <= 30)
                        .Select(c => new CustomerDTO
                        {
                            Id = c.Id,
                            Name = c.CustomerName,
                            Email = c.Email
                        }).ToListAsync();
                    return customers;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
