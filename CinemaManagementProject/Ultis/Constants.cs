using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Ultis
{
    public class VOUCHER_STATUS
    {
        public static readonly string REALEASED = "Đã phát hành";
        public static readonly string UNRELEASED = "Chưa phát hành";
        public static readonly string USED = "Đã sử dụng";
    }
    public class VOUCHER_OBJECT_TYPE
    {
        public static readonly string PRODUCT = "Sản phẩm";
        public static readonly string TICKET = "Vé xem phim";
        public static readonly string ALL = "Toàn bộ";
    }
}
