using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Services
{
    public class ProfileRepository : IProfileRepository
    {
        ProfileAPI profileAPI = new ProfileAPI();

        public async Task<List<Country>> GetCountry()
        {
            List<Country> mCountry = new List<Country>();
            try
            {
                mCountry = await profileAPI.GetCountry();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/GetCountry: " + ex.Message);
            }
            return mCountry;
        }

        public async Task<bool> ValidPincode(string pinCode)
        {
            bool isValid = false;
            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                if (Common.IsValidPincode(pinCode))
                {
                    var response = await profileAPI.GetPincodeInfo(pinCode);
                    if (response != null && response.Succeeded)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Pincode);
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Pincode);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/ValidPincode: " + ex.Message);
            }
            return isValid;
        }
    }
}
