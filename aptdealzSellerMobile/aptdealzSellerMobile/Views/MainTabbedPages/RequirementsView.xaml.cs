using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using aptdealzSellerMobile.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequirementsView : ContentView
    {
        #region Objects
        public event EventHandler isRefresh;
        private List<ViewRequirement> mRequirements = new List<ViewRequirement>();
        #endregion

        #region Constructor
        public RequirementsView()
        {
            InitializeComponent();
            BindRequirements();
        } 
        #endregion

        #region Methods
        private void BindRequirements()
        {
            lstRequirements.ItemsSource = null;
            string reqdesc = "Need 5 Canon A210 All In one Printer.";
            string catdesc = "Lorem iosum dolor sit amet,consectetur adipiscing elit.Integer nec odio..";

            mRequirements = new List<ViewRequirement>()
            {
                new ViewRequirement
                {
                    RequirementNo="REQ#123",
                    RequirementDes=reqdesc,
                    ReqDate="12-05-2021",
                    CatDescription=catdesc,
                    ReqStatus="Open",
                    Price=15000,
                    Code="B852"
                },
                new ViewRequirement
                {
                    RequirementNo="REQ#128",
                    RequirementDes=reqdesc,
                    CatDescription=catdesc,
                    ReqDate="10-05-2021",
                    ReqStatus="Rejected",
                     Price=15000,
                     Code="B852"
                },
                new ViewRequirement
                {
                    RequirementNo="REQ#132",
                    RequirementDes=reqdesc,
                    CatDescription=catdesc,
                    ReqDate="18-05-2021",
                    ReqStatus="Open",
                     Price=15000,
                     Code="B852"
                },
                new ViewRequirement
                { RequirementNo="REQ#141",
                    RequirementDes=reqdesc,
                    CatDescription=catdesc,
                    ReqDate="22-05-2021",
                    ReqStatus="Open",
                    Price=15000,
                     Code="B852"
                },
                new ViewRequirement
                {
                    RequirementNo="REQ#149",
                    RequirementDes=reqdesc,
                    CatDescription=catdesc,
                    ReqDate="27-05-2021",
                    ReqStatus="Rejected",
                     Price=15000,
                     Code="B852"
                },
                new ViewRequirement
                {
                    RequirementNo="REQ#155",
                    RequirementDes=reqdesc,
                    CatDescription=catdesc,
                    ReqDate="03-06-2021",
                    ReqStatus="Open",
                    Price=15000,
                     Code="B852"
                },
                new ViewRequirement
                {
                    RequirementNo="REQ#163",
                    RequirementDes=reqdesc,
                    CatDescription=catdesc,
                    ReqDate="11-06-2021",
                    ReqStatus="Open",
                     Price=15000,
                     Code="B852"
                },
            };

            foreach (var color in mRequirements)
            {
                if (color.ReqStatus == "Open")
                {
                    color.StatusColor = Color.FromHex("#006027");
                }
                else if (color.ReqStatus == "Rejected")
                {
                    color.StatusColor = Color.FromHex("#E50019");
                }
                else
                {
                    color.StatusColor = Color.FromHex("#000000");
                }
            }

            lstRequirements.ItemsSource = mRequirements.ToList();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            isRefresh?.Invoke(true, EventArgs.Empty);
        }

        private async void ImgSearch_Tapped(object sender, EventArgs e)
        {
            try
            {
                SearchPopup searchPopup = new SearchPopup();
                searchPopup.isRefresh += (s1, e1) =>
                {
                    lstRequirements.ItemsSource = mRequirements.ToList();
                };
                await PopupNavigation.Instance.PushAsync(searchPopup);
            }
            catch (Exception ex)
            {

            }
        }

        private async void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                SortByPopup sortByPopup = new SortByPopup();
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
            }
        }

        private async void FrmStatus_Tapped(object sender, EventArgs e)
        {
            try
            {
                StatusPopup statusPopup = new StatusPopup();
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
            }
        }

        private void BtnRequerments_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectGrid = (ImageButton)sender;
                var setHight = (ViewCell)selectGrid.Parent.Parent.Parent;
                if (setHight != null)
                {
                    setHight.ForceUpdateSize();
                }

                var response = (ViewRequirement)selectGrid.BindingContext;
                if (response != null)
                {
                    foreach (var selectedImage in mRequirements)
                    {
                        if (selectedImage.ArrowImage == Constraints.Right_Arrow)
                        {
                            selectedImage.ArrowImage = Constraints.Right_Arrow;
                            selectedImage.GridBg = Color.Transparent;
                            selectedImage.MoreDetail = false;
                            selectedImage.OldDetail = true;
                        }
                        else
                        {
                            selectedImage.ArrowImage = Constraints.Down_Arrow;
                            selectedImage.GridBg =(Color)App.Current.Resources["LightGray"];
                            selectedImage.MoreDetail = true;
                            selectedImage.OldDetail = false;
                        }
                    }
                    if (response.ArrowImage == Constraints.Right_Arrow)
                    {
                        response.ArrowImage = Constraints.Down_Arrow;
                        response.GridBg =(Color)App.Current.Resources["LightGray"];
                        response.MoreDetail = true;
                        response.OldDetail = false;
                    }
                    else
                    {
                        response.ArrowImage = Constraints.Right_Arrow;
                        response.GridBg = Color.Transparent;
                        response.MoreDetail = false;
                        response.OldDetail = true;
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void lstRequirements_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstRequirements.SelectedItem = null;
            Navigation.PushAsync(new RequirementDetailPage());
        } 
        #endregion
    }
}