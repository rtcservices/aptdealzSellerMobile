using aptdealzSellerMobile.Utility;
using System;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model
{
    public class Grievances
    {
        public string GrievanceId { get; set; }
        public string GrievanceStatus { get; set; }
        public string Description { get; set; }


        public Color StatusColor
        {
            get
            {
                if (GrievanceStatus == "Open")
                {
                    return (Color)App.Current.Resources["Green"];
                }
                else if (GrievanceStatus == "Closed")
                {
                    return (Color)App.Current.Resources["DarkRedColor"];
                }
                else
                {
                    return (Color)App.Current.Resources["BlackColor"];
                }
            }
        }
    }
}
