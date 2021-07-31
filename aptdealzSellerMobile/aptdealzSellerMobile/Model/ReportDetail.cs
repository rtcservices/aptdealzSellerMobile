using aptdealzSellerMobile.Utility;
using System.ComponentModel;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model
{
    public class ReportDetail : INotifyPropertyChanged
    {
        public string InvoiceId { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public double EarningAmount { get; set; }
        public string InvoiceDate { get; set; }

        public Color StatusColor
        {
            get
            {
                if (Status == "Cleared")
                {
                    return (Color)App.Current.Resources["Green"];
                }
                else if (Status == "Pending")
                {
                    return (Color)App.Current.Resources["Red"];
                }
                else
                {
                    return (Color)App.Current.Resources["Black"];
                }
            }
        }
        private string _ArrowImage { get; set; } = Constraints.Arrow_Right;
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }

        private Color _GridBg { get; set; } = Color.Transparent;
        public Color GridBg
        {
            get { return _GridBg; }
            set { _GridBg = value; PropertyChangedEventArgs("GridBg"); }
        }

        private bool _MoreDetail { get; set; } = false;
        public bool MoreDetail
        {
            get { return _MoreDetail; }
            set { _MoreDetail = value; PropertyChangedEventArgs("MoreDetail"); }
        }

        private bool _OldDetail { get; set; } = true;
        public bool OldDetail
        {
            get { return _OldDetail; }
            set { _OldDetail = value; PropertyChangedEventArgs("OldDetail"); }
        }
        private bool _ShowCategory { get; set; } = false;
        public bool ShowCategory
        {
            get { return _ShowCategory; }
            set { _ShowCategory = value; PropertyChangedEventArgs("ShowCategory"); }
        }

        private bool _ShowDelete { get; set; } = true;
        public bool ShowDelete
        {
            get { return _ShowDelete; }
            set { _ShowDelete = value; PropertyChangedEventArgs("ShowDelete"); }
        }

        private LayoutOptions _Layout { get; set; } = LayoutOptions.CenterAndExpand;
        public LayoutOptions Layout
        {
            get { return _Layout; }
            set { _Layout = value; PropertyChangedEventArgs("Layout"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
