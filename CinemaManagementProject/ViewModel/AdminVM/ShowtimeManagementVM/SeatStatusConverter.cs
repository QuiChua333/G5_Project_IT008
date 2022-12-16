using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CinemaManagementProject.ViewModel.AdminVM.ShowtimeManagementVM
{
    public class SeatStatusConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.


            if ((bool)value == true)
                return new SolidColorBrush(Colors.Brown);
            else if ((bool)value == false)
                return new SolidColorBrush(Colors.White);
            else
                return new SolidColorBrush(Colors.White);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ForeGroundConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.


            if ((bool)value == true)
                return new SolidColorBrush(Colors.White);
            else if ((bool)value == false)
                return new SolidColorBrush(Colors.Black);
            else
                return new SolidColorBrush(Colors.Black);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
