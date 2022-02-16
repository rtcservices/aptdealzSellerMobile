namespace aptdealzSellerMobile.Utility
{
    public class Constraints
    {
        #region [ String ]
        public const string Str_DateFormate = "dd/MM/yyyy";
        public const string Str_TokenExpired = "TokenExpired";
        public const string Str_NotificationCount = "NotificationCount";
        public const string Str_AccountDeactivated = "account is deactivated";
        public const string Str_Duplicate = "duplicate";

        public const string Str_RevealContact = "Reveal Contact";
        public const string Str_NotRevealContact = "Contact was not revealed";

        public const string Str_Right = "✓";
        public const string Str_Wrong = "✕";

        public const string Str_ExpectedPickupDate = "Expected Pickup Date";
        public const string Str_ExpectedDeliveryDate = "Expected Delivery Date";

        public const string Str_PickupPINCode = "Product Pickup PIN Code";
        public const string Str_ShippingPINCode = "Shipping PIN Code";

        #region [ Dev RazorPay ]
        //public const string RP_UserName = "rzp_test_PUJtE9p3XLuGe8";
        //public const string RP_Password = "42HIrjeUTXOHNC84Ldl3aDzL";
        #endregion

        #region [ Production RazorPay ]
        public const string RP_MerchantId = "HbW0qOiAlCsSMi";
        public const string RP_UserName = "rzp_test_vo5zGl2YTwdLjk";
        public const string RP_Password = "bXvtQZ4HdiVjwtMc3b7z8Zoe";
        #endregion

        public const string RP_RevealPayNow = "RevealPayNow";
        public const string RP_PaidRevealResponse = "PaidRevealResponse";

        public const string Invalid_URL = "Invalid URL: The format of the URL could not be determined!";
        public const string URL_Not_Available = "Tracking URL not available!";
        #endregion

        #region [ Images ]
        public const string Password_Hide = "iconHidePass.png";
        public const string Password_Visible = "iconVisiblepass.png";

        public const string Arrow_Up = "iconUpArrow.png";
        public const string Arrow_Right = "iconRightArrow.png";
        public const string Arrow_Down = "iconDownArrow.png";

        public const string Sort_ASC = "iconSortASC.png";
        public const string Sort_ASC_Dark = "iconSortASCWhite.png";
        public const string Sort_DSC = "iconSortDSC.png";
        public const string Sort_DSC_Dark = "iconSortDSCWhite.png";

        public const string CheckBox_UnChecked = "iconUncheck.png";
        public const string CheckBox_Checked = "iconCheck.png";

        public const string Redio_UnSelected = "iconRadioUnselect.png";
        public const string Redio_Selected = "iconRadioSelect.png";

        public const string Img_Show = "iconShowGray.png";
        public const string Img_Hide = "iconHide.png";
        public const string Img_Sad = "iconSad.png";
        public const string Img_Smile = "iconSmile.png";
        public const string Img_Contact = "imgContact.jpg";

        public const string ImgHome = "iconHome.png";
        public const string ImgHomeDark = "iconHomeWhite.png";
        public const string ImgHomeActive = "iconHomeActive.png";

        public const string ImgQuote = "iconQuotes.png";
        public const string ImgQuoteDark = "iconQuotesWhite.png";
        public const string ImgQuoteActive = "iconQuotesActive.png";

        public const string ImgOrder = "iconOrders.png";
        public const string ImgOrderDark = "iconOrdersWhite.png";
        public const string ImgOrderActive = "iconOrdersActive.png";

        public const string ImgAccount = "iconAccount.png";
        public const string ImgAccountDark = "iconAccountWhite.png";
        public const string ImgAccountActive = "iconAccountActive.png";

        public const string ImgProductBanner = "iconProductBanner.png";
        public const string ImgProductBannerWhite = "iconProductBannerWhite.png";

        public const string Img_GreenArrowDown = "iconGreenDownArrow.png";
        public const string Img_GreenArrowUp = "iconGreenUpArrow.png";

        public const string Img_SwitchOff = "iconSwitchOff.png";
        public const string Img_SwitchOn = "iconSwitchOn.png";
        #endregion

        #region [ Alert Title ]
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Ok = "OK";
        public const string TryAgain = "Try Again";
        public const string Cancel = "Cancel";
        public const string UploadPicture = "Upload Picture";
        public const string TakePhoto = "Take Photo";
        public const string ChooseFromLibrary = "Choose From Library";
        public const string NoCamera = "No Camera";
        public const string Alert = "Alert!";
        public const string Loading = "Loading...";
        public const string Logout = "Logout";
        #endregion

        #region [ Error Message ]
        public static string Session_Expired = "Session expired, Please login again!";
        public static string Something_Wrong = "Something went wrong!";
        public static string Something_Wrong_Server = "Something went wrong from server!";
        public static string ServiceUnavailable = "Service Unavailable, Try again later!";
        public static string Number_was_null = "Number was null or white space";
        public static string Phone_Dialer_Not_Support = "Phone Dialer is not supported on this device";
        #endregion

        #region [ Alert Message ]   
        public const string CouldNotSentOTP = "Could not send Verification Code to the given number!";
        public const string DoYouWantToExit = "Do you really want to exit?";
        public const string AreYouSureWantDeactivateAccount = "Are you sure want to deactivate account?";
        public const string AreYouSureWantDelete = "Are you sure want to delete?";
        public const string AreYouSureWantLogout = "Are you sure want to logout?";

        public const string NoInternetConnection = "No internet connection!";
        public const string NoCameraAwailable = "No camera awailable!";
        public const string UnableTakePhoto = "Unable to take photo!";
        public const string PermissionDenied = "Permission denied!";
        public const string PhotosNotSupported = "The Photo is not supported!";
        public const string PermissionNotGrantedPhotos = "Permission is not granted to photos!";
        public const string NeedStoragePermissionAccessYourPhotos = "Need storage permission to access your photos!";
        public const string PlsCheckInternetConncetion = "Please check internet connection to use App.";

        public const string InValid_Email = "Please enter valid email address!";
        public const string InValid_PhoneNumber = "Please enter valid phone number!";
        public const string InValid_AltNumber = "Please enter valid alternate phone number!";
        public const string InValid_NewPassword = "Please enter valid new password!";
        public const string InValid_ConfirmPassword = "Please enter valid confirm password!";
        public const string InValid_Pincode = "Please enter valid pincode!";
        public const string InValid_Nationality = "Please enter valid nationality!";
        public const string InValid_CountryOfOrigin = "Please enter valid Product Made In!";
        public const string InValid_GST = "Please enter valid GST number!";
        public const string InValid_PAN = "Please enter valid PAN number!";
        public const string InValid_PAN_GSTIN = "Please enter valid GST number or PAN number!";

        public const string Same_New_Confirm_Password = "New and Confirm password should be same!";
        public const string Same_Phone_AltPhone_Number = "Please enter different alternative number!";
        public const string Agree_T_C = "Please check box for Terms & Conditions.";
        public const string ContactRevealed = "Buyer Contacts Revealed!";

        public const string Required_BillingAddress = "Please enter billing address!";
        public const string Required_Email_Phone = "Please enter email address or phone number!";
        public const string Required_VerificationCode = "Please enter verification code!";
        public const string Required_Email = "Please enter email address!";
        public const string Required_Password = "Please enter password!";
        public const string Required_FullName = "Please enter full name!";
        public const string Required_PhoneNumber = "Please enter phone number!";
        public const string Required_AlterntePhoneNumber = "Please enter alternate phone number!";
        public const string Required_BuildingNumber = "Please enter building number!";
        public const string Required_Street = "Please enter street!";
        public const string Required_City = "Please enter city!";
        public const string Required_State = "Please enter state!";
        public const string Required_Nationality = "Please enter nationality!";
        public const string Required_CountryofOrigin = "Please enter Product Made In!";
        public const string Required_PinCode = "Please enter pincode!";
        public const string Required_Landmark = "Please enter landmark!";
        public const string Required_Description = "Please enter description!";
        public const string Required_Experience = "Please enter experience!";
        public const string Required_SupplyArea = "Please enter Area of supply!";
        public const string Required_GST = "Please enter GST number!";
        public const string Required_PAN = "Please enter PAN number!";
        public const string Required_Bank_Account = "Please enter bank Account number!";
        public const string Required_Bank_Name = "Please enter bank name!";
        public const string Required_IFSC = "Please enter IFSC code!";
        public const string Required_Category = "Please enter category!";
        public const string Required_Subcategory = "Please enter sub category!";

        public const string Required_Documents = "Please upload documents!";

        public const string Required_NewPassword = "Please enter new password!";
        public const string Required_ConfirmPassword = "Please enter confirm password!";

        public const string Required_UnitPrice = "Please enter unit price!";
        public const string Required_QuoteValidityDate = "Please enter quote validity date!";
        public const string Required_OrderStatus = "Please select order status!";
        public const string Required_ShippingNumber = "Please enter shipping number!";
        public const string Required_LRNumber = "Please enter LR number!";
        public const string Required_EWayBillNumber = "Please enter EWay bill number!";
        public const string Required_TrackingLink = "Please enter tracking link!";
        public const string Required_PickUp_Product = "Please enter Product Pickup PIN Code!";

        public const string Required_Response = "Please enter your message!";
        public const string Required_ComplainType = "Please select complaint type!";
        public const string Required_Reason = "Please enter the reason for rejection!";

        public const string Invalid_Tracking_link = "Please enter valid Tracking link";

        #endregion

        #region [ Success Message ]
        public const string OTPSent = "OTP Verification Code Sent Successfully";
        public const string InstantVerification = "you don't get any code, it is instant verification. Please try to login with email address";
        #endregion

        #region [ Copy Message ]
        public const string CopiedBuyerId = "Copied Buyer Id!";
        public const string CopiedSellerId = "Copied Seller Id!";
        public const string CopiedRequirementId = "Copied Requirement ID!";
        public const string CopiedOrderId = "Copied Order Id!";
        public const string CopiedQuoteRefNo = "Copied Quote Id!";
        public const string CopiedGrievanceId = "Copied Grievance Id!";
        #endregion
    }
}
