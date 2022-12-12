using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime FirstDate { get; set; }
        public double Expense { get; set; }
        public string ExpenseStr
        {
            get
            {
                return Helper.FormatVNMoney(Expense);
            }
        }
    }
}
