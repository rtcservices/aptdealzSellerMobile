using aptdealzSellerMobile.Utility;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model
{
    public class MessageList
    {
        public string Message { get; set; }
        public LayoutOptions MessagePosition { get; set; }
        public Thickness MessageMargin { get; set; }
        public Color MessageBackgroundColor { get; set; } = (Color)App.Current.Resources["LightGray"];
        public CornerRadius PancakeRadius { get; set; } = new CornerRadius(10, 0, 10, 10);
        public Color MessageTextColor { get; set; } = (Color)App.Current.Resources["Black"];
        public string UserName { get; set; }
        public string Time { get; set; }
        public string ContactImage { get; set; } = Constraints.Img_Contact;
        public string BuyerImage { get; set; } = Constraints.Img_Buyer;
        public bool IsContact { get; set; } = false;
        public bool IsBuyer { get; set; } = false;
    }
}
