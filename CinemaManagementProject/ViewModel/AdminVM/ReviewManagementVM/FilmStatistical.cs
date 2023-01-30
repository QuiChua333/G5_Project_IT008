using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.ViewModel.AdminVM.ReviewManagementVM
{
    public class FilmStatistical
    {
        public int FilmId { get; set; }
        public string Name { get; set; }
        public int TotalReview { get; set; }
        public float AverageStar { get; set; }
        public int TotalStar { get; set; }
        public List<string> BillCodes{ get; set; }
        public List<int> StarList { get; set; }

        public void SetValues(IList<IList<Object>> values)
        {
            StarList = new List<int>();
            foreach (var item in BillCodes)
            {
                var review = values.FirstOrDefault(i => i[1].ToString() == item);
                if(review != null)
                    StarList.Add(int.Parse(review[2].ToString()));
            }
            TotalStar = StarList.Sum();
            AverageStar = (float)TotalStar / TotalReview;
        }
        public List<int> CountStar()
        {
            List<int> starCount = new List<int>(5);
            for(int i = 1; i <= 5; i++)
                starCount.Add(StarList.Where(item => item == i).Count());
            return starCount;
        }
    }
}
