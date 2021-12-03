namespace aptdealzSellerMobile.Utility
{
    public class EndPointURL
    {
        #region [ Register API ]
        public const string Register = "api/v{0}/SellerAuth/Register";
        public const string IsUniquePhoneNumber = "api/v{0}/SellerAuth/IsUniquePhoneNumber";
        public const string IsUniqueEmail = "api/v{0}/SellerAuth/IsUniqueEmail";
        #endregion

        #region [ Authentication API ]
        public const string SellerAuth = "api/v{0}/SellerAuth/authenticate";
        public const string RefreshToken = "api/Account/refresh-token";
        public const string Logout = "api/Account/logout";
        public const string ActivateUser = "api/Account/ActivateUser";
        public const string SendOtpByEmail = "api/v{0}/SellerAuth/SendOtpByEmail";
        public const string ValidateOtpForResetPassword = "api/v{0}/SellerAuth/ValidateOtpForResetPassword";
        public const string ResetPassword = "api/v{0}/SellerAuth/ResetPassword";
        public const string CheckPhoneNumberExists = "api/v{0}/SellerAuth/CheckPhoneNumberExists?phoneNumber={1}";
        #endregion

        #region [ Profile API ]       
        public const string DeactivateUser = "api/Account/DeactivateUser";
        public const string Country = "api/v{0}/Country/Get";
        public const string FileUpload = "api/FileUpload";
        public const string GetMyProfileData = "api/v{0}/SellerManagement/GetMyProfileData";
        public const string SaveProfile = "api/v{0}/SellerManagement/Update";
        public const string GetPincodeInfo = "api/IndianPincode/GetPincodeInfo/{0}";
       
        public const string UpdateUserMuteNotification = "api/Account/UpdateUserMuteNotification/{0}";
        #endregion

        #region [ Category API ]
        public const string Category = "api/v{0}/Category/Get";
        public const string SubCategory = "api/v{0}/SubCategory/Get?CategoryId={1}";
        public const string CreateCategory = "/api/v{0}/Category/Create";
        public const string CreateSubCategory = "api/v{0}/SubCategory/Create";
        #endregion

        #region [ Requirement API ]
        public const string GetAllActiveRequirements = "api/v{0}/Requirement/GetAllActiveRequirements";
        public const string GetRequirementById = "api/v{0}/Requirement/Get/{1}";
        public const string GetRequirementDetailsWithoutQuoteDetails = "api/v{0}/Requirement/GetRequirementDetailsWithoutQuoteDetails/{1}";
        public const string RevealBuyerContact = "api/v{0}/Requirement/RevealBuyerContact";
        #endregion

        #region [ Quote API ]
        public const string CreateQuote = "api/v{0}/Quote/Create";
        public const string GetSubmittedQuotesByMe = "api/v{0}/Quote/GetSubmittedQuotesByMe";
        public const string GetQuotesById = "api/v{0}/Quote/Get/{1}";
        public const string UpdateQuote = "api/v{0}/Quote/Update";
        #endregion

        #region [ Order API ]
        public const string GetOrdersForSeller = "api/v{0}/Order/GetOrdersForSeller";
        public const string GetOrderDetailsForSeller = "api/v{0}/Order/GetOrderDetailsForSeller/{1}";
        public const string UpdateOrder = "api/v{0}/Order/Update";
        public const string GetShippedOrdersForSeller = "api/v{0}/Order/GetShippedOrdersForSeller";
        public const string ScanQRCodeAndUpdateOrder = "api​/v{0}​/Order​/ScanQRCodeAndUpdateOrder";
        public const string GetCompletedOrdersAgainstSeller = "api​/v{0}​/Order/GetCompletedOrdersAgainstSeller";
        #endregion

        #region [ RateAndReview API ]
        public const string ReviewSeller = "api/v{0}/RateAndReview/ReviewSeller";
        public const string ReviewSellerProduct = "api/v{0}/RateAndReview/ReviewSellerProduct";
        #endregion

        #region [ Grievance API ]
        public const string GetAllGrievancesByMe = "api/v{0}/Grievance/GetAllGrievancesBySeller";
        public const string GetGrievancesDetailsForSeller = "api/v{0}/Grievance/GetGrievancesDetailsForSeller/{1}";
        public const string SubmitGrievanceResponseFromSeller = "api/v{0}/Grievance/SubmitGrievanceResponseFromSeller";
        public const string CreateGrievanceFromSeller = "api/v{0}/Grievance/CreateGrievanceFromSeller";
        #endregion

        #region [ Notification API ]
        public const string GetAllNotificationsForUser = "api/v{0}/Notifications/GetAllNotificationsForUser";
        public const string GetNotificationsCountForUser = "api/v{0}/Notifications/GetNotificationsCountForUser";
        public const string SetUserNoficiationAsRead = "api/v{0}/Notifications/SetUserNoficiationAsRead/{1}";
        public const string SetUserNoficiationAsReadAndDelete = "/api/v{0}/Notifications/SetUserNoficiationAsReadAndDelete/{1}";
        #endregion

        #region [ SupportChat API ]
        public const string GetAllMyChat = "api/v{0}/SupportChat/GetAllMyChat";
        public const string SendChatSupportMessage = "api/v{0}/SupportChat/SendChatSupportMessage";
        #endregion

        #region [ Affiliations API ]
        public const string GetAllAffiliations = "api/v{0}/Affiliations/GetAllAffiliations";
        #endregion

        #region MyRegion
        public const string GetAmountToBePaidToRevealSellerContact = "api​/v{0}​/Requirement​/GetAmountToBePaidToRevealSellerContact";
        public const string GetPrivacyPolicyTermsAndConditions = "api/v{0}/AppSettings/GetPrivacyPolicyTermsAndConditions";
        public const string GetFAQ = "api/v{0}/AppSettings/GetFAQ";
        public const string GetAppPromoBar = "api/v{0}/AppSettings/GetAppPromoBar";
        public const string AboutAptdealzSellerApp = "api/v{0}/AppSettings/AboutAptdealzSellerApp";
        #endregion
    }
}
