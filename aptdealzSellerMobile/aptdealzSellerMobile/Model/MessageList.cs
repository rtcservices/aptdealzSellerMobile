using aptdealzSellerMobile.Utility;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model
{
    public class MessageList
    {
        public string Message { get; set; }
        public LayoutOptions MessagePosition { get; set; }
        public Thickness MessageMargin { get; set; }
        public Color MessageBackgroundColor { get; set; } = (Color)App.Current.Resources["SilverColor"];
        public Color MessageTextColor { get; set; } = (Color)App.Current.Resources["DarkGrayColor"];
        public string UserName { get; set; }
        public string Time { get; set; }
        public string ContactImage { get; set; } = Constraints.Contact_Img;
        public string BuyerImage { get; set; } = Constraints.Buyer_Img;
        public bool IsContact { get; set; } = false;
        public bool IsBuyer { get; set; } = false;
    }
}
