using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace CinemaManagementProject.Views
{
    
    public enum CustomMessageBoxImage
    {

        None = 0,

        Error = 1,

        Question = 2,


        Warning = 3,

        Success = 4,

        Information = 5
    }


    public enum CustomMessageBoxResult
    {

        None = 0,

        OK = 1,

        Cancel = 2,
    }
    internal partial class CustomMessageBoxWindow : Window
    {
        Color color = new Color();

       

        internal string Caption
        {
            get
            {
                return MessageBoxTitle.Text;
            }
            set
            {
                MessageBoxTitle.Text = value;
            }
        }

        internal string Message
        {
            get
            {
                return TextBlock_Message.Text;
            }
            set
            {
                TextBlock_Message.Text = value;
            }
        }

        internal string OkButtonText
        {
            get
            {
                return Label_Ok.Content.ToString();
            }
            set => Label_Ok.Content = value.TryAddKeyboardAccellerator();
        }

        internal string CancelButtonText
        {
            get
            {
                return Label_Cancel.Content.ToString();
            }
            set
            {
                Label_Cancel.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string OkButtonSingleText
        {
            get
            {
                return Label_Ok_Single.Content.ToString();
            }
            set
            {
                Label_Ok_Single.Content = value.TryAddKeyboardAccellerator();
            }
        }


        public CustomMessageBoxResult Result { get; set; }

      

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button)
        {
            InitializeComponent();
            color = (Color)ColorConverter.ConvertFromString("#2196F3");
            TitleBody.Background = new SolidColorBrush(color);
            Message = message;
            Caption = caption;
            Image_MessageBox.Visibility = System.Windows.Visibility.Collapsed;
            Button_OK_Single.Background = new SolidColorBrush(color);
            DisplayButtons(button);
        }

        
        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button, CustomMessageBoxImage image)
        {
            InitializeComponent();

            switch (image)
            {
                case CustomMessageBoxImage.Warning:
                    color = (Color)ColorConverter.ConvertFromString("#FFC107");
                    break;
                case CustomMessageBoxImage.Success:
                    color = (Color)ColorConverter.ConvertFromString("#4CAF50");
                    break;
                case CustomMessageBoxImage.Error:
                    color = (Color)ColorConverter.ConvertFromString("#F44336");
                    break;
                case CustomMessageBoxImage.Question:
                    color = (Color)ColorConverter.ConvertFromString("#3383CC");
                    break;
                case CustomMessageBoxImage.Information:
                    color = (Color)ColorConverter.ConvertFromString("#2196F3");
                    break;
                default:
                    color = (Color)ColorConverter.ConvertFromString("#2196F3");
                    break;
            }
            TitleBody.Background = new SolidColorBrush(color);
            Message = message;
            Caption = caption;
            Button_OK.Background = new SolidColorBrush(color);
            Button_OK_Single.Background = new SolidColorBrush(color);
            Image_MessageBox.Visibility = System.Windows.Visibility.Collapsed;

            DisplayButtons(button);
            DisplayImage(image);
        }

        private void DisplayButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    // Hide all but OK, Cancel
                    Button_OK.Visibility = System.Windows.Visibility.Visible;
                    Button_OK.Focus();
                    Button_Cancel.Visibility = System.Windows.Visibility.Visible;
                    Button_OK_Single.Visibility = System.Windows.Visibility.Collapsed;
                    break;
               
                default:
                    // Hide all but OK
                    Button_OK_Single.Visibility = System.Windows.Visibility.Visible;
                    Button_OK_Single.Focus();
                    Button_OK.Visibility = System.Windows.Visibility.Collapsed;
                    Button_Cancel.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        private void DisplayImage(CustomMessageBoxImage image)
        {
            BitmapImage bitmapImage;

            switch (image)
            {
               
                case CustomMessageBoxImage.Warning:
                    System.Media.SystemSounds.Hand.Play();
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/icon_Warning.png"));
                    break;
                case CustomMessageBoxImage.Success:
                    System.Media.SystemSounds.Beep.Play();
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/icon_Succes.png"));
                    break;
                case CustomMessageBoxImage.Error:
                    System.Media.SystemSounds.Hand.Play();
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/icon_Error.png"));
                    break;
                case CustomMessageBoxImage.Question:
                    System.Media.SystemSounds.Beep.Play();
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/icon_Question.png"));
                    break;
                case CustomMessageBoxImage.Information:
                    System.Media.SystemSounds.Beep.Play();
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/icon_Info.png"));
                    break;
                default:
                    System.Media.SystemSounds.Beep.Play();
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/CinemaManagementProject;component/Resource/Images/icon_Info.png"));
                    break;
            }

            Image_MessageBox.Source = bitmapImage;
            Image_MessageBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.OK;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.Cancel;
            Close();
        }

        private void Button_OK_Single_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.OK;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
