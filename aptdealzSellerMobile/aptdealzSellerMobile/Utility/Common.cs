using Acr.UserDialogs;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Views.MasterData;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Utility
{
    public static class Common
    {
        #region [ Properties ]
        public static MasterDataPage MasterData { get; set; }
        public static Model.Request.SellerDetails mSellerDetails { get; set; }
        public static List<Country> mCountries { get; set; }
        public static string Token { get; set; }
        public static string NotificationCount { get; set; }
        #endregion

        #region [ Regex Properties ]
        private static Regex PhoneNumber { get; set; } = new Regex(@"^[0-9]{10}$");
        private static Regex RegexPassword { get; set; } = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$");
        private static Regex RegexPincode { get; set; } = new Regex(@"^[0-9]{6}$");
        private static Regex RegexGST { get; set; } = new Regex(@"[0-9]{2}[A-Z]{3}[ABCFGHLJPTF]{1}[A-Z]{1}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}");
        private static Regex RegexPAN { get; set; } = new Regex("([A-Z]){5}([0-9]){4}([A-Z]){1}$");
        #endregion       

        #region [ Display Messages ]
        public static void DisplayErrorMessage(string errormessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(errormessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = (Color)App.Current.Resources["ErrorBackground"],
                MessageTextColor = (Color)App.Current.Resources["White"],
                Duration = new TimeSpan(0, 0, 5),
            });
        }

        public static void DisplaySuccessMessage(string successmessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(successmessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = (Color)App.Current.Resources["SuccessBackground"],
                MessageTextColor = (Color)App.Current.Resources["White"],
                Duration = new TimeSpan(0, 0, 5)
            });
        }

        public static void DisplayWarningMessage(string warningmessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(warningmessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = (Color)App.Current.Resources["WarningBackground"],
                MessageTextColor = (Color)App.Current.Resources["White"],
                Duration = new TimeSpan(0, 0, 5),
            });
        }
        #endregion

        #region [ Methods ]
        public static async void BindAnimation(ImageButton imageButton = null, Button button = null, Grid grid = null, StackLayout stackLayout = null, Label label = null, Image image = null, Frame frame = null)
        {
            try
            {
                if (imageButton != null)
                {
                    await imageButton.ScaleTo(0.9, 100, Easing.Linear);
                    await imageButton.ScaleTo(1, 100, Easing.Linear);
                }

                if (button != null)
                {
                    await button.ScaleTo(0.9, 100, Easing.Linear);
                    await button.ScaleTo(1, 100, Easing.Linear);
                }

                if (grid != null)
                {
                    await grid.ScaleTo(0.9, 100, Easing.Linear);
                    await grid.ScaleTo(1, 100, Easing.Linear);
                }

                if (stackLayout != null)
                {
                    await stackLayout.ScaleTo(0.9, 100, Easing.Linear);
                    await stackLayout.ScaleTo(1, 100, Easing.Linear);
                }

                if (label != null)
                {
                    await label.ScaleTo(0.9, 100, Easing.Linear);
                    await label.ScaleTo(1, 100, Easing.Linear);
                }

                if (image != null)
                {
                    await image.ScaleTo(0.9, 100, Easing.Linear);
                    await image.ScaleTo(1, 100, Easing.Linear);
                }

                if (frame != null)
                {
                    await frame.ScaleTo(0.9, 100, Easing.Linear);
                    await frame.ScaleTo(1, 100, Easing.Linear);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Common/BindAnimation: " + ex.Message);
            }
        }

        public static string ToCamelCase(this string str)
        {
            return Regex.Replace(str, "(\\B[A-Z])", " $1");
        }

        public static bool EmptyFiels(string extEntry)
        {
            if (string.IsNullOrEmpty(extEntry) || string.IsNullOrWhiteSpace(extEntry))
            {
                return true;
            }
            else
            {
                extEntry = extEntry.Trim();
                return false;
            }
        }

        public async static Task<bool> InternetConnection()
        {
            bool result = await App.Current.MainPage.DisplayAlert(Constraints.NoInternetConnection, Constraints.PlsCheckInternetConncetion, Constraints.TryAgain, Constraints.Cancel);
            return result;
        }

        public static void OpenMenu()
        {
            try
            {
                if (Common.MasterData != null)
                {
                    Common.MasterData.IsGestureEnabled = true;
                    Common.MasterData.IsPresented = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Common/OpenMenu: " + ex.Message);
            }
        }

        public static bool IsValidPhone(this string value)
        {
            value = value.Trim();
            return PhoneNumber.IsMatch(value);
        }

        public static bool IsValidEmail(this string value)
        {
            try
            {
                value = value.Trim();
                var addr = new System.Net.Mail.MailAddress($"{value}");
                return addr.Address == $"{value}";
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(this string value)
        {
            value = value.Trim();
            return (RegexPassword.IsMatch($"{value}"));
        }

        public static bool IsValidPincode(this string value)
        {
            value = value.Trim();
            return (RegexPincode.IsMatch($"{value}"));
        }

        public static bool IsValidGSTPIN(this string value)
        {
            value = value.Trim();
            if (value.Length == 15)
                return (RegexGST.IsMatch($"{value}"));
            else
                return false;
        }

        public static bool IsValidPAN(this string value)
        {
            value = value.Trim();
            return (RegexPAN.IsMatch($"{value}"));
        }

        public static int GetOrderStatus(string orderStatus)
        {
            orderStatus = orderStatus.Replace(" ", "");

            switch (orderStatus)
            {
                case "Pending":
                    return (int)OrderStatus.Pending;
                case "Accepted":
                    return (int)OrderStatus.Accepted;
                case "ReadyForPickup":
                    return (int)OrderStatus.ReadyForPickup;
                case "Shipped":
                    return (int)OrderStatus.Shipped;
                case "Delivered":
                    return (int)OrderStatus.Delivered;
                case "Completed":
                    return (int)OrderStatus.Completed;
                case "CancelledFromBuyer":
                    return (int)OrderStatus.CancelledFromBuyer;
                case "All":
                    return (int)OrderStatus.All;
                default:
                    return 0;
            }
        }

        public static int GetOrderIndex(int orderStatus)
        {

            switch (orderStatus)
            {
                case (int)OrderStatus.Pending:
                    return 0;
                case (int)OrderStatus.Accepted:
                    return 1;
                case (int)OrderStatus.ReadyForPickup:
                    return 2;
                case (int)OrderStatus.Shipped:
                    return 3;
                case (int)OrderStatus.Delivered:
                    return 4;
                case (int)OrderStatus.Completed:
                    return 5;
                case (int)OrderStatus.CancelledFromBuyer:
                    return 6;
                case (int)OrderStatus.All:
                    return 7;
                default:
                    return 0;
            }
        }

        public static int GetQuoteStatus(string quoteStatus)
        {
            switch (quoteStatus)
            {
                case "Submitted":
                    return (int)QuoteStatus.Submitted;
                case "Accepted":
                    return (int)QuoteStatus.Accepted;
                case "Rejected":
                    return (int)QuoteStatus.Rejected;
                case "All":
                    return (int)QuoteStatus.All;
                default:
                    return 0;
            }
        }

        public static int GetGrievanceStatus(string grievanceStatus)
        {
            switch (grievanceStatus)
            {
                case "Pending":
                    return (int)GrievancesStatus.Pending;
                case "Open":
                    return (int)GrievancesStatus.Open;
                case "Closed":
                    return (int)GrievancesStatus.Closed;
                case "All":
                    return (int)GrievancesStatus.All;
                default:
                    return 0;
            }
        }

        public static void CopyText(Label copyLabel, string message)
        {
            try
            {
                Clipboard.SetTextAsync(copyLabel.Text);
                if (Clipboard.HasText)
                {
                    Common.BindAnimation(label: copyLabel);
                    UserDialogs.Instance.Toast(message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Common/CopyText: " + ex.Message);
            }
        }
        #endregion
    }

    #region [ Enum ]
    public enum FileUploadCategory
    {
        ProfilePicture = 0,
        ProfileDocuments = 1,
        RequirementImages = 2
    }

    public enum RequirementStatus
    {
        Active = 1,
        Completed = 2,
        Rejected = 3,
        Inactive = 4,
        Cancelled = 5
    }

    public enum SortByField
    {
        ID = 1,
        Date = 2,
        Quotes = 3,
        Validity = 4,
        Amount = 5,
        TotalPriceEstimation = 6
    }

    public enum QuoteStatus
    {
        Submitted = 1,
        Accepted = 2,
        Rejected = 3,
        All = 4
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Success = 2,
        Failed = 3
    }

    public enum OrderStatus
    {
        Pending = 1,
        Accepted = 2,
        ReadyForPickup = 3,
        Shipped = 4,
        Delivered = 5,
        Completed = 6,
        CancelledFromBuyer = 7,
        All = 8
    }

    public enum GrievancesStatus
    {
        Pending = 1,
        Open = 2,
        Closed = 3,
        All = 4
    }

    public enum GrievancesType
    {
        OrderRelated,
        DelayedDelivery,
        PaymentRelated
    }

    public enum GrievancesFrom
    {
        Buyer,
        Seller,
        MarkettingExecutive
    }

    public enum NavigationScreen
    {
        System,
        RequirementDetails,
        QuoteDetails,
        OrderDetails,
        GrievanceDetails,
    }
    #endregion
}