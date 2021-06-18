using aptdealzSellerMobile.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Model
{
    public class ViewRequirement : INotifyPropertyChanged
    {
        public string RequirementNo { get; set; }
        public string RequirementDes { get; set; }
        public string CatDescription { get; set; }
        public string ReqDate { get; set; }
        public string ReqStatus { get; set; }
        public int Price { get; set; }
        public string Code { get; set; }
        public Color StatusColor { get; set; }

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

    public class ViewRequirementLister
    {
        public ViewRequirementLister()
        {
            mViewRequirement = new List<ViewRequirement>();
            SearchCriteria = new ViewRequirement();
        }
        public List<ViewRequirement> mViewRequirement { get; set; }
        public ViewRequirement SearchCriteria { get; set; }
    }
}
