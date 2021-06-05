using aptdealzSellerMobile.Utility;
using System.ComponentModel;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model
{
    public class CurrentlyShipping : INotifyPropertyChanged
    {
        public string InvoceId { get; set; }
        public string Description { get; set; }
        public string ShippingDate { get; set; }
        public double Amount { get; set; }
        public string ShippingStatus { get; set; }
        private string _ArrowImage { get; set; } = Constraints.Right_Arrow;
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

    //public class ViewRequirementLister
    //{
    //    public ViewRequirementLister()
    //    {
    //        mViewRequirement = new List<ViewRequirement>();
    //        SearchCriteria = new ViewRequirement();
    //    }
    //    public List<ViewRequirement> mViewRequirement { get; set; }
    //    public ViewRequirement SearchCriteria { get; set; }
    //}
}

