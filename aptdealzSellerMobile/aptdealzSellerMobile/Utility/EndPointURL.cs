namespace aptdealzSellerMobile.Utility
{
    public class EndPointURL
    {
        #region [ Register API ]
        public const string Register = "api/v{0}/SellerAuth/Register";
        public const string IsUniquePhoneNumber = "api/Account/IsUniquePhoneNumber";
        public const string IsUniqueEmail = "api/Account/IsUniqueEmail";
        #endregion

        #region [ Authentication API ]
        public const string SellerAuth = "api/v{0}/SellerAuth/authenticate";
        public const string RefreshToken = "api/Account/refresh-token";
        public const string Logout = "api/Account/logout";
        public const string ActivateUser = "api/Account/ActivateUser";
        public const string SendOtpByEmail = "api/Account/SendOtpByEmail";
        public const string ValidateOtpForResetPassword = "api/Account/ValidateOtpForResetPassword";
        public const string ResetPassword = "api/Account/ResetPassword";
        public const string CheckPhoneNumber = "api/v{0}/SellerAuth/CheckPhoneNumber?phoneNumber={1}";
        #endregion

        #region [ Profile API ]       
        public const string Country = "api/v{0}/Country/Get";
        public const string Category = "api/v{0}/Category/Get";
        public const string SubCategory = "api/v{0}/SubCategory/Get?CategoryId={1}";
        public const string CreateCategory = "/api/v{0}/Category/Create";
        public const string CreateSubCategory = "api/v{0}/SubCategory/Create";
        public const string FileUpload = "api/FileUpload";
        public const string GetMyProfileData = "api/v{0}/SellerManagement/GetMyProfileData";
        public const string GetSellerDataById = "api/v{0}/SellerManagement/Get/{1}";
        public const string SaveProfile = "api/v{0}/SellerManagement/Update";
        public const string ValidatePincode = "pincode/{0}";
        #endregion

        #region [ Requirement API ]
        public const string GetAllActiveRequirements = "api/v{0}/Requirement/GetAllActiveRequirements";
        public const string GetRequirementById = "api/v{0}/Requirement/Get/{1}";
        public const string RevealBuyerContact = "api/v{0}/Requirement/RevealBuyerContact?requirementId={1}";
        #endregion

        #region [ Quote API ]
        public const string CreateQuote = "api/v{0}/Quote/Create";
        public const string GetSubmittedQuotesByMe = "api/v{0}/Quote/GetSubmittedQuotesByMe";
        #endregion
    }
}
