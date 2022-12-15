using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Drawing;
using System.Windows.Media.Effects;
using CinemaManagementProject.View.Admin.FoodManagement;
using CinemaManagementProject.DTOs;
using System.ComponentModel;

namespace CinemaManagementProject.Component.FoodItem
{
    /// <summary>
    /// Interaction logic for FoodItem.xaml
    /// </summary>
    public partial class FoodItem : UserControl
    {
        public FoodItem()
        {
            InitializeComponent();
            
            dropShadowEffectWhite = new DropShadowEffect();
            dropShadowEffectBlur = new DropShadowEffect();

            dropShadowEffectBlur.BlurRadius = 20;
            dropShadowEffectBlur.Color = Colors.Black;
            dropShadowEffectBlur.Opacity = 0.3;
            dropShadowEffectBlur.Direction = 270;

            dropShadowEffectWhite.BlurRadius = 10;
            dropShadowEffectWhite.Color = Colors.Black;
            dropShadowEffectWhite.Opacity = 0.25;
            dropShadowEffectWhite.Direction = 270;

            this.DataContext = this;
        }
        DropShadowEffect dropShadowEffectWhite;
        DropShadowEffect dropShadowEffectBlur;
        public event EventHandler EditButtonClick;
        public event EventHandler FoodItemClick;

        public static readonly DependencyProperty DisplayNameProperty =
        DependencyProperty.Register("DisplayName", typeof(string), typeof(FoodItem), new PropertyMetadata(null));

        public string DisplayName
        {
            get
            {
                return GetValue(DisplayNameProperty) as string;
            }
            set
            {
                SetValue(DisplayNameProperty, value);
            }
        }
        public string DisplayImage { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            BackLayer.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,243,245,247));
            BackLayer.Effect = dropShadowEffectBlur;
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            BackLayer.Fill = new SolidColorBrush(Colors.White);
            BackLayer.Effect = dropShadowEffectWhite;
        }

        private void EditButton_MouseMove(object sender, MouseEventArgs e)
        {
            EditBackground.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 230, 231, 232));
            EditBackground.Effect = dropShadowEffectBlur;
            e.Handled = true;
        }

        private void EditButton_MouseLeave(object sender, MouseEventArgs e)
        {
            EditBackground.Fill = new SolidColorBrush(Colors.White);
            BackLayer.Effect = dropShadowEffectWhite;
        }

        private void RemoveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RemoveBackground.Fill = new SolidColorBrush(Colors.White);
            BackLayer.Effect = dropShadowEffectWhite;
        }

        private void RemoveButton_MouseMove(object sender, MouseEventArgs e)
        {
            RemoveBackground.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 230, 231, 232));
            RemoveBackground.Effect = dropShadowEffectBlur;
            e.Handled = true;
        }

        private void EditButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EditButtonClick?.Invoke(this, e);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FoodItemClick?.Invoke(this, e);
        }
    }
}
