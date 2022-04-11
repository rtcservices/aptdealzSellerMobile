using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.OtherPage;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Services
{
    public class RequirementRepository : IRequirementRepository
    {
        RequirementAPI requirementAPI = new RequirementAPI();

        public async Task<Requirement> GetRequirementById(string RequirmentId)
        {
            Requirement mRequirement = new Requirement();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await requirementAPI.GetRequirementById(RequirmentId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mRequirement = jObject.ToObject<Requirement>();
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementRepository/GetRequirementById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return mRequirement;
        }

        public async Task<string> RevealContact(RevealBuyerContact mRevealBuyerContact)
        {
            string PhoneNumber = Constraints.Str_RevealContact;
            try
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                catch (Exception ex)
                {
                }
                var mResponse = await requirementAPI.RevealBuyerContact(mRevealBuyerContact);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mBuyerContact = jObject.ToObject<RevealBuyerContact>();
                        if (mBuyerContact != null)
                        {
                            await PopupNavigation.Instance.PushAsync(new Views.Popup.SuccessPopup(Constraints.ContactRevealed));
                            PhoneNumber = mBuyerContact.PhoneNumber;
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
            catch (ArgumentNullException)
            {
                Common.DisplayErrorMessage(Constraints.Number_was_null);
            }
            catch (FeatureNotSupportedException)
            {
                Common.DisplayErrorMessage(Constraints.Phone_Dialer_Not_Support);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteRepository/RevealContact: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return PhoneNumber;
        }
    }
}
