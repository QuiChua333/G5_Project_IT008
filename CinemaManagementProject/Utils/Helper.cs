using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Utils
{
    public class Helper
    {
        public static string FormatVNMoney(float money)
        {
            if (money == 0)
                return "0 đ";
            return String.Format(CultureInfo.InvariantCulture,
                                "{0:#,#} ₫", money);
        }
        public static string GetEmailTemplatePath(string fileName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resource\EmailTemplate", $"{fileName}" /*SelectedItem.Image*/);
        }
        public static bool CheckEmailStaff(string CurrentEmail)
        {
            using (CinemaManagementProjectEntities db = new CinemaManagementProjectEntities())
            {
                foreach (var staff in db.Staffs)
                    if (CurrentEmail == staff.Email)
                        return true;
                return false;
            }
        }

    }
}
