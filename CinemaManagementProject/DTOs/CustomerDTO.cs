﻿using System;
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
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string StartDate { get; set; }

        //Expense
        public string Expense { get; set; }
        //public string ExpenseStr
        //{
        //    get
        //    {
        //        return Helper.FormatVNMoney(Expense);
        //    }
        //}
    }
}
