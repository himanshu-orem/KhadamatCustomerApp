﻿using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Services.ApiService;
using Khadamat_CustomerApp.ViewModels;
using System;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.Views
{
    public partial class ExpressServicePage : ContentPage
    {
        private WebApiRestClient _webApiRestClient;
        ExpressServicePageViewModel expressServicePageViewModel;
        public ExpressServicePage()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            {
                var languageculture = Application.Current.Properties["AppLocale"].ToString();
                if (languageculture == "ar-AE")
                {
                    this.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    this.FlowDirection = FlowDirection.LeftToRight;
                }
            }
            else
            {
                this.FlowDirection = FlowDirection.LeftToRight;
            }
            expressServicePageViewModel = this.BindingContext as ExpressServicePageViewModel;
            _webApiRestClient = new WebApiRestClient();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            expressServicePageViewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            expressServicePageViewModel.OnDisappearing();
        }

        #region GetSubServiceData
        private async void GetSubServiceData(string service_category_name, long service_category_id)
        {
            try
            {
                if (Common.CheckConnection())
                {
                    SubCategoryResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<SubCategoryResponseModel>(string.Format(ApiUrl.GetSubCategories, service_category_id));
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        await MaterialDialog.Instance.SnackbarAsync(message: ex.Message, msDuration: 3000);
                        return;
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            //Common.CustomNavigation(Navigation, new ServiceDetailPage(service_category_name, response.SubCategoryData));
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
                        }
                    }
                }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                            msDuration: 3000);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
        }
        #endregion

        private void HomeTab_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send("HomeTab", "TabSelected");
        }
    }
}
