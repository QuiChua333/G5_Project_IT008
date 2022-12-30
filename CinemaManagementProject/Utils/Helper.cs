using CinemaManagementProject.DTOs;
using CinemaManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CinemaManagementProject.Utils
{
    public class Helper
    {

        public static (string, List<string>) GetListCode(int quantity, int length, string firstChars, string lastChars, VoucherReleaseDTO voucherRelease)
        {
            List<string> ListCode = new List<string>();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            int randomLength = length - firstChars.Length - lastChars.Length;
            int minimumLength = (int)Math.Ceiling(Math.Log(quantity, 36));
            if (randomLength < minimumLength)
            {
                return ("Không thể tạo được đúng số lượng voucher với yêu cầu trên!", null);

            }
            for (int i = 0; i < quantity; i++)
            {

                var stringChars = new char[randomLength];
                for (int j = 0; j < stringChars.Length; j++)
                {
                    stringChars[j] = chars[random.Next(chars.Length)];
                }
                string newCode = new String(stringChars);
                var isExist = ListCode.Any(code => code == newCode);
                if (isExist)
                {
                    i--;
                    continue;
                }
                var isExist2 = false;
                foreach (var item in voucherRelease.Vouchers)
                {
                    if (ListCode.Any(code => code == item.VoucherCode))
                    {
                        isExist2 = true;
                        break;
                    }
                }
                if (isExist2)
                {
                    i--;
                    continue;
                }
                ListCode.Add(firstChars + newCode + lastChars);
            }

            return (null, ListCode);
        }
        public static string ConvertDoubleToPercentageStr(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("P", CultureInfo.InvariantCulture);
        }
        public static string MD5Hash(string str)
        {
            StringBuilder hash = new StringBuilder();
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(str));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("X2"));
            }
            return hash.ToString();
        }
        public static bool IsPhoneNumber(string number)
        {
            if (number is null) return false;
            return Regex.Match(number, @"(([03+[2-9]|05+[6|8|9]|07+[0|6|7|8|9]|08+[1-9]|09+[1-4|6-9]]){3})+[0-9]{7}\b").Success;
        }
        public static string GetHourMinutes(TimeSpan t)
        {
            return t.ToString(@"hh\:mm");
        }

        public static string GetImagePath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resource\Images", $"{imageName}" /*SelectedItem.Image*/);
        }



        public static string FormatVNMoney(float money)
        {
            if (money == 0)
            {
                return "0 ₫";
            }
            return String.Format(CultureInfo.InvariantCulture,
                                "{0:#,#} ₫", money);
        }
        public static string FormatDecimal(decimal n)
        {
            if (n == 0)
            {
                return "0";
            }
            return String.Format(CultureInfo.InvariantCulture,
                                "{0:#,#}", n);
        }
        public static string GetEmailTemplatePath(string fileName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resource\EmailTemplate", $"{fileName}" /*SelectedItem.Image*/);
        }
        public static string GetSidebarTemplate()
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resource\Styles\MenuButtonUI.xaml");
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


