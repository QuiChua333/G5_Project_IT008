using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaManagementProject.Component.TroubleItem
{
    /// <summary>
    /// Interaction logic for TroubleItem.xaml
    /// </summary>
    public partial class TroubleItem : UserControl
    {
        public TroubleItem()
        {
            InitializeComponent();
            ShadowWhite = new DropShadowEffect();
            ShadowBlur = new DropShadowEffect();
            ShadowWhite1 = new DropShadowEffect();
            ShadowWhite2 = new DropShadowEffect();
            ShadowDark1 = new DropShadowEffect();
            ShadowDark2 = new DropShadowEffect();

            ShadowBlur.BlurRadius = 20;
            ShadowBlur.Color = Colors.Black;
            ShadowBlur.Opacity = 0.3;
            ShadowBlur.Direction = 270;

            ShadowWhite.BlurRadius = 10;
            ShadowWhite.Color = Colors.Black;
            ShadowWhite.Opacity = 0.25;
            ShadowWhite.Direction = 270;

            ShadowWhite1.BlurRadius = 6;
            ShadowWhite1.Color = Colors.Black;
            ShadowWhite1.Opacity = 0.2;
            ShadowWhite1.ShadowDepth = 0.5;
            ShadowWhite1.Direction = 315;

            ShadowWhite2 = ShadowWhite1;
            ShadowWhite2.Direction = 135;

            ShadowDark1.BlurRadius = 30;
            ShadowDark1.Color = Colors.Black;
            ShadowDark1.Opacity = 0.2;
            ShadowDark1.ShadowDepth = 0.5;
            ShadowDark1.Direction = 315;

            ShadowDark2 = ShadowDark1;
            ShadowDark2.Direction = 135;
        }
        DropShadowEffect ShadowWhite;
        DropShadowEffect ShadowWhite1;
        DropShadowEffect ShadowWhite2;
        DropShadowEffect ShadowDark1;
        DropShadowEffect ShadowDark2;
        DropShadowEffect ShadowBlur;
        public event EventHandler EditButtonClick;
        public event EventHandler TroubleItemClick;

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
           BackLayer.Effect = ShadowDark1;
           Back2Layer.Effect = ShadowDark2;
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            BackLayer.Effect = ShadowWhite1;
            Back2Layer.Effect = ShadowWhite2;
        }
        private void EditButton_MouseMove(object sender, MouseEventArgs e)
        {
            EditBackground.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 230, 231, 232));
            EditBackground.Effect = ShadowBlur;
            e.Handled = true;
        }

        private void EditButton_MouseLeave(object sender, MouseEventArgs e)
        {
            EditBackground.Fill = new SolidColorBrush(Colors.White);
            EditBackground.Effect = ShadowWhite;
        }
        private void RemoveButton_MouseMove(object sender, MouseEventArgs e)
        {
            RemoveBackground.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 230, 231, 232));
            RemoveBackground.Effect = ShadowBlur;
            e.Handled = true;
        }
        private void RemoveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RemoveBackground.Fill = new SolidColorBrush(Colors.White);
            EditBackground.Effect = ShadowWhite;
        }


        private void EditButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EditButtonClick?.Invoke(this, e);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TroubleItemClick?.Invoke(this, e);
        }

    }
}
