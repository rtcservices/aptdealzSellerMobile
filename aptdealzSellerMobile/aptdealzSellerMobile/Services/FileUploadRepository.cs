using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Services
{
    public class FileUploadRepository : IFileUploadRepository
    {
        public async Task<string> UploadFile(int fileUploadCategory)
        {
            string relativePath = string.Empty;
            string base64String;

            try
            {
                if (ImageConvertion.SelectedImageByte != null || FileSelection.fileByte != null)
                {
                    if (fileUploadCategory == 0)
                    {
                        base64String = Convert.ToBase64String(ImageConvertion.SelectedImageByte);
                    }
                    else
                    {
                        base64String = Convert.ToBase64String(FileSelection.fileByte);
                    }

                    var fileName = Guid.NewGuid().ToString() + ".png";

                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    ProfileAPI profileAPI = new ProfileAPI();
                    FileUpload mFileUpload = new FileUpload();

                    mFileUpload.Base64String = base64String;
                    mFileUpload.FileName = fileName;
                    mFileUpload.FileUploadCategory = fileUploadCategory;

                    var mResponse = await profileAPI.FileUpload(mFileUpload);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mBuyerFile = jObject.ToObject<Model.Reponse.SellerFileDocument>();
                            if (mBuyerFile != null)
                            {
                                relativePath = mBuyerFile.relativePath;
                            }
                        }
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
                else
                {
                    return relativePath;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FileUplodeRepository/UplodeFile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return relativePath;
        }
    }
}
