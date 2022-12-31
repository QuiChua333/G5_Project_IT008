using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaManagementProject.View
{
    public static class LanguageManager
    {
        public static void SetLanguageDictionary(ELanguage lang)
        {
            ResourceDictionary dict = new ResourceDictionary();
            ResourceDictionary oldReSource = new ResourceDictionary();
            switch (lang)
            {
                case ELanguage.English:
                    dict.Source = new Uri("..\\..\\..\\Resource\\Language\\ResourceString.en-US.xaml",
                             UriKind.Relative);
                    oldReSource.Source = new Uri("..\\..\\..\\Resource\\Language\\ResourceString.vi-VN.xaml",
                                   UriKind.Relative);
                    break;
                case ELanguage.VietNamese:
                    dict.Source = new Uri("..\\..\\..\\Resource\\Language\\ResourceString.vi-VN.xaml",
                                   UriKind.Relative);
                    oldReSource.Source = new Uri("..\\..\\..\\Resource\\Language\\ResourceString.en-US.xaml",
                             UriKind.Relative);
                    break;
                default:

                    dict.Source = new Uri("..\\..\\..\\Resource\\Language\\ResourceString.vi-VN.xaml",
                              UriKind.Relative);
                    oldReSource.Source = new Uri("..\\..\\..\\Resource\\Language\\ResourceString.en-US.xaml",
                            UriKind.Relative);
                    break;
            }
            Cons.CurrentLanguage = lang;
            
            Application.Current.Resources.MergedDictionaries.Remove(oldReSource);
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
        public enum ELanguage
        {
            English,
            VietNamese
        }

        public static class Cons
        {
            public static ELanguage CurrentLanguage = ELanguage.VietNamese;
        }
    }
}
