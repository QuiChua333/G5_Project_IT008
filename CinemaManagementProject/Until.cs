using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagementProject
{
  
        internal static class Util
        {
            internal static ImageSource ToImageSource(this Icon icon)
            {
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                return imageSource;
            }

        
            internal static string TryAddKeyboardAccellerator(this string input)
            {
                const string accellerator = "_";            
                if (input.Contains(accellerator)) return input;

                return accellerator + input;
            }
        }
    
}
