using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Security;
using System.Net;
using System.IO;

namespace CinemaManagementProject.Utils
{
    public class CloudinaryService
    {
        private static CloudinaryService _ins;
        public static CloudinaryService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new CloudinaryService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private Account account;
        private Cloudinary cloudinary;
        private CloudinaryService()
        {
            account = new Account("dcdjan0oo", "512949436267937", "JFaFGzZ8BmR4uzsxxrSPQFmRasI");
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
        }



        public async Task<string> UploadImage(string filePath)
        {
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath),
                    Folder = "FoodManagement"
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                return uploadResult.SecureUrl.AbsoluteUri;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task DeleteImage(string imageURL)
        {
            try
            {
                string publicId = GetPublicIdFromURL(imageURL);
                var deletionParams = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Image,
                };

                var deletionResult = await cloudinary.DestroyAsync(deletionParams);
            }
            catch (System.Exception)
            {
                return;
            }
        }

        public async Task<BitmapImage> LoadImageFromURL(string imageURL)
        {
            if (string.IsNullOrEmpty(imageURL))
            {
                return null;
            }
            WebRequest request = WebRequest.Create(imageURL);
            HttpWebResponse response;
            try
            {
                response = (await request.GetResponseAsync()) as HttpWebResponse;
            }
            catch (WebException)
            {
                return null;
            }

            Stream responseStream =
                response.GetResponseStream();

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = responseStream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return bitmap;
        }
        private string GetPublicIdFromURL(string url)
        {
            string strStart = "squadinImages";
            string strEnd = ".";
            if (url.Contains("squadinImages") && url.Contains("."))
            {
                int Start, End;
                Start = url.IndexOf(strStart, 0);
                End = url.IndexOf(strEnd, Start);
                return url.Substring(Start, End - Start);
            }
            return null;
        }
    }
}
