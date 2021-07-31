using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GrievanceDetailPage : ContentPage
    {
        #region Objects
        private Grievance mGrievance;
        private string ErrorMessage = string.Empty;
        private string GrievanceId;
        #endregion

        #region Constructor
        public GrievanceDetailPage(string GrievanceId)
        {
            InitializeComponent();
            this.GrievanceId = GrievanceId;

            MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
            {
                if (!Common.EmptyFiels(Common.NotificationCount))
                {
                    lblNotificationCount.Text = count;
                    frmNotification.IsVisible = true;
                }
                else
                {
                    frmNotification.IsVisible = false;
                    lblNotificationCount.Text = string.Empty;
                }
            });
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetGrievancesDetails();
        }

        private async Task GetGrievancesDetails()
        {
            try
            {
                mGrievance = await DependencyService.Get<IGrievanceRepository>().GetGrievancesDetails(GrievanceId);
                if (mGrievance != null)
                {
                    lblGrievanceId.Text = mGrievance.GrievanceNo;
                    lblOrderId.Text = mGrievance.OrderNo;
                    lblOrderDate.Text = mGrievance.OrderDate.ToString("dd/MM/yyyy");
                    lblGrievanceDate.Text = mGrievance.Created.ToString("dd/MM/yyyy");
                    lblBuyeName.Text = mGrievance.GrievanceFromUserName;
                    lblGrievanceType.Text = mGrievance.GrievanceTypeDescr;
                    lblStatus.Text = mGrievance.StatusDescr;
                    AttachDocumentList();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/GetGrievancesDetails: " + ex.Message);
            }
        }

        private async Task SubmitGrievanceResponse()
        {
            try
            {
                GrievanceAPI grievanceAPI = new GrievanceAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);


                if (!Common.EmptyFiels(txtMessage.Text))
                {
                    var mResponse = await grievanceAPI.SubmitGrievanceResponseFromSeller(GrievanceId, txtMessage.Text);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        txtMessage.Text = string.Empty;
                        if ((bool)mResponse.Data)
                            Common.DisplaySuccessMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(mResponse.Message);
                        await GetGrievancesDetails();
                    }
                    else
                    {
                        if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
                else
                {
                    BoxMessage.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    Common.DisplayErrorMessage(Constraints.Required_Response);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/SubmitGrievanceResponse: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

        //private RaiseGrievance FillGrievance()
        //{
        //    try
        //    {
        //        RaiseGrievance mRaiseGrievance = new RaiseGrievance();
        //        mRaiseGrievance.OrderId = OrderId;
        //        if (pkType.SelectedIndex != -1)
        //        {
        //            mRaiseGrievance.GrievanceType = GetGrievanceType(pkType.SelectedItem.ToString());
        //        }
        //        else
        //        {
        //            FrmType.BorderColor = (Color)App.Current.Resources["LightRed"];
        //            ErrorMessage = Constraints.Required_ComplainType;
        //            return null;
        //        }

        //        if (documentList != null && documentList.Count > 0)
        //        {
        //            mRaiseGrievance.Documents = documentList;
        //        }
        //        if (!Common.EmptyFiels(txtDescription.Text))
        //        {
        //            mRaiseGrievance.IssueDescription = txtDescription.Text;
        //        }
        //        if (!Common.EmptyFiels(txtSolution.Text))
        //        {
        //            mRaiseGrievance.PreferredSolution = txtSolution.Text;
        //        }
        //        return mRaiseGrievance;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message != null)
        //        {
        //            ErrorMessage = ex.Message;
        //        }
        //        return null;
        //    }
        //}

        //public async Task CreateGrievance()
        //{
        //    try
        //    {
        //        GrievanceAPI grievanceAPI = new GrievanceAPI();
        //        UserDialogs.Instance.ShowLoading(Constraints.Loading);

        //        var mRaiseGrievance = FillGrievance();
        //        if (mRaiseGrievance != null)
        //        {
        //            var mResponse = await grievanceAPI.CreateGrievanceFromBuyer(mRaiseGrievance);
        //            if (mResponse != null && mResponse.Succeeded)
        //            {
        //                Common.DisplaySuccessMessage(mResponse.Message);
        //                await Navigation.PushAsync(new GrievancesPage());
        //            }
        //            else
        //            {
        //                if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
        //                    Common.DisplayErrorMessage(mResponse.Message);
        //                else
        //                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
        //            }
        //        }
        //        else
        //        {
        //            if (ErrorMessage == null)
        //            {
        //                ErrorMessage = Constraints.Something_Wrong;
        //            }
        //            Common.DisplayErrorMessage(ErrorMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.DisplayErrorMessage("RaiseGrievancePage/CreateGrievance: " + ex.Message);
        //    }
        //    finally
        //    {
        //        UserDialogs.Instance.HideLoading();
        //    }
        //}

        private void AttachDocumentList()
        {
            try
            {
                if (mGrievance.Documents != null && mGrievance.Documents.Count > 0)
                {
                    lblAttachDocument.IsVisible = false;
                    lstDocument.ItemsSource = mGrievance.Documents.ToList();
                    lstDocument.IsVisible = true;
                }
                else
                {
                    lblAttachDocument.IsVisible = true;
                    lstDocument.ItemsSource = null;
                    lstDocument.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/AttachDocumentList: " + ex.Message);
            }
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Dashboard.NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private async void BtnSubmit_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnSubmit);
            await SubmitGrievanceResponse();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            rfView.IsRefreshing = true;
            await GetGrievancesDetails();
            rfView.IsRefreshing = false;
        }

        private void lstResponse_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstResponse.SelectedItem = null;
        }

        private void txtMessage_Unfocused(object sender, FocusEventArgs e)
        {
            if (!Common.EmptyFiels(txtMessage.Text))
            {
                BoxMessage.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            }
        }
        #endregion

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                var stackLayout = (StackLayout)sender;
                if (!Common.EmptyFiels(stackLayout.ClassId))
                {
                    if (stackLayout.ClassId == "GrievanceId")
                    {
                        string message = Constraints.CopiedGrievanceId;
                        Common.CopyText(lblGrievanceId, message);
                    }
                    else if (stackLayout.ClassId == "OrderId")
                    {
                        string message = Constraints.CopiedOrderId;
                        Common.CopyText(lblOrderId, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/CopyString_Tapped: " + ex.Message);
            }
        }
    }
}