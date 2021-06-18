﻿using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SkiaSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Utility
{
    public class ImageConvertion
    {
        #region Properties
        public static string profileImageBase64 { get; set; }
        public static FFImageLoading.Forms.CachedImage SelectedImagePath { get; set; }
        public static string NullImagePath { get; set; }

        public static byte[] SelectedImageByte = null;
        #endregion

        #region Methods
        public static byte[] CompressImage(byte[] imgBytes)
        {
            byte[] CompressimageBytes = null;
            try
            {
                var image = SKImage.FromEncodedData(imgBytes);
                var data = image.Encode(SKEncodedImageFormat.Jpeg, 30);
                CompressimageBytes = data.ToArray();
            }
            catch (Exception)
            {
                //DisplayErrorMessage("ImageConvertion/CompressImage : " + ex.Message);
            }
            return CompressimageBytes;
        }

        public static String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return Convert.ToString(_sb);
        }

        private static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception)
            {
                buf = null;
                //DisplayErrorMessage("ImageConvertion/GetImage : " + ex.Message);
            }

            return (buf);
        }

        public static byte[] TakeCameraAsync(MediaFile file)
        {
            byte[] ImageBytes = null;

            try
            {
                //Convert image to Byte
                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();

                //Reduce Image size
                ImageBytes = ImageConvertion.CompressImage(ImageBytes);
                memoryStream.Dispose();
                Device.BeginInvokeOnMainThread(() =>
                {
                    SelectedImagePath.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                });

                SelectedImageByte = ImageBytes;

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ImageConvertion/TakeCameraAsync: " + ex.Message);
            }

            finally { UserDialogs.Instance.HideLoading(); }
            return ImageBytes;
        }

        public static void SetNullSource(int fileUploadCategory)
        {
            if (fileUploadCategory == 0)
            {
                ImageConvertion.NullImagePath = "iconUserAccount.png";
            }
            else
            {
                ImageConvertion.NullImagePath = "imgUploadImage.png";
            }
        }

        public static async Task SelectImage()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            try
            {
                MediaFile file = null;
                var result = await App.Current.MainPage.DisplayActionSheet(Constraints.UploadPicture, Constraints.Cancel, "", new string[] { Constraints.TakePhoto, Constraints.ChooseFromLibrary });

                if (result == Constraints.TakePhoto)
                {
                    try
                    {
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await App.Current.MainPage.DisplayAlert(Constraints.NoCamera, ":( " + Constraints.NoCameraAwailable, Constraints.Ok);
                            return;
                        }

                        var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                        var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                        if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                        {
                            var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                            cameraStatus = results[Permission.Camera];
                            storageStatus = results[Permission.Storage];
                        }

                        if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                        {
                            file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                            {
                                SaveToAlbum = true,
                                CompressionQuality = 50,
                                CustomPhotoSize = 50,
                                DefaultCamera = CameraDevice.Rear,
                                AllowCropping = true,
                            });

                            if (file == null)
                            {
                                return;
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Constraints.PermissionDenied, Constraints.UnableTakePhoto, Constraints.Ok);
                            //On iOS you may want to send your user to the settings screen.
                            //CrossPermissions.Current.OpenAppSettings();
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.DisplayErrorMessage("ImageConvertion/SelectImage/TakePhoto: " + ex.Message);
                    }
                    finally
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else if (result == Constraints.ChooseFromLibrary)
                {
                    try
                    {
                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            await App.Current.MainPage.DisplayAlert(Constraints.PhotosNotSupported, ":( " + Constraints.PermissionNotGrantedPhotos, Constraints.Ok);
                            return;
                        }

                        var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                        if (status != PermissionStatus.Granted)
                        {
                            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
                            {
                                await App.Current.MainPage.DisplayAlert(Constraints.NeedStoragePermissionAccessYourPhotos, "", Constraints.Ok);
                            }

                            var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Storage });
                            status = results[Plugin.Permissions.Abstractions.Permission.Storage];
                        }

                        if (status == PermissionStatus.Granted)
                        {
                            file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                            {
                                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                            });

                            if (file == null)
                            {
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.DisplayErrorMessage("ImageConvertion/SelectImage/ChooseFromLibrary: " + ex.Message);
                    }
                    finally { UserDialogs.Instance.HideLoading(); }
                }
                else if (SelectedImagePath.Source == null)
                {
                    SelectedImagePath.Source = NullImagePath;
                }

                try
                {
                    if (file != null)
                    {
                        SelectedImageByte = TakeCameraAsync(file);
                    }
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ImageConvertion/SelectImage/UploadPhoto: " + ex.Message);
                }
                finally { UserDialogs.Instance.HideLoading(); }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ImageConvertion/SelectImage: " + ex.Message);
            }
            finally { UserDialogs.Instance.HideLoading(); }
#pragma warning restore CS0618 // Type or member is obsolete
        }
        #endregion
    }
}
