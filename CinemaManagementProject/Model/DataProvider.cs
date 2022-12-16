using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementProject.Model
{
    public class DataProvider
    {
        private static DataProvider _ins;
        private static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }
        public CinemaManagementProjectEntities DB  { get; set; }
        private DataProvider()
        {
            DB = new CinemaManagementProjectEntities();
        }
    }
}
