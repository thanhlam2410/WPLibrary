using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace WPCore.Platform
{
    public class InAppHelper
    {
        #region Func
        private Func<string, string, Task<bool>> inAppPurchase;
        #endregion

        #region Methods
        /// <summary>
        /// Simulate a mock store for testing
        /// </summary>
        public void SetupIAP()
        {

        }

        /// <summary>
        /// Purchase product
        /// </summary>
        /// <param name="productID"></param>
        public async void Purchase(string productID, string transactionId, Func<string, string, Task<bool>> inAppCallback)
        {
            try
            {
                inAppPurchase = inAppCallback;

                string xmlReceipt = await CurrentApp.RequestProductPurchaseAsync(productID, true);
                CompleteFulfillMent(productID, xmlReceipt, transactionId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Purchase " + ex.Message);
            }
        }

        /// <summary>
        /// Complete in-app purchase
        /// </summary>
        /// <param name="productID"></param>
        private async void CompleteFulfillMent(string productID, string receiptData, string transactionId)
        {
            ProductLicense tokenLicense = CurrentApp.LicenseInformation.ProductLicenses[productID];

            if (tokenLicense.IsConsumable && tokenLicense.IsActive)
            {
                if (string.IsNullOrEmpty(receiptData))
                {
                    receiptData = await CurrentApp.GetProductReceiptAsync(productID);
                }

                if (inAppPurchase != null)
                {
                    bool result = await inAppPurchase(receiptData, transactionId);

                    if (result)
                    {
                        CurrentApp.ReportProductFulfillment(tokenLicense.ProductId);
                    }
                }
                else
                {
                    CurrentApp.ReportProductFulfillment(tokenLicense.ProductId);
                }
            }
        }
        #endregion
    }
}
