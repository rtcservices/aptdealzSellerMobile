using aptdealzSellerMobile.Utility;
using System.ComponentModel;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model
{
    public class QuoteDetail : INotifyPropertyChanged
    {
        public string QuoteId { get; set; }
        public string ReqId { get; set; }
        public string Description { get; set; }
        public string QuoteCode { get; set; }
        public string QuoteDate { get; set; }
        public double QuoteAmount { get; set; }

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

        private string _ArrowImage { get; set; } = Constraints.Arrow_Right;
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
