using CinemaManagementProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.DTOs
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {
        }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime FirstDate { get; set; }

        //Expense
        public bool IsDeleted { get; set; }
        public float Expense { get; set; }
        public string ExpenseStr
        {
            get
            {
                return Helper.FormatVNMoney(Expense);
            }
        }
    }
}
