using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Storage;

namespace WPCore.Platform
{
    public class InAppHelper
    {
        #region Func
        private Func<string, string, Task<bool>> inAppPurchase;
        #endregion

        #region Fields
        private LicenseInformation licenseInformation;
        #endregion

        #region Methods
        /// <summary>
        /// Simulate a mock store for testing
        /// </summary>
        public async void SetupIAP()
        {
#if DEBUG
            StorageFile setupFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/test-purchase.xml", UriKind.RelativeOrAbsolute));
            await CurrentAppSimulator.ReloadSimulatorAsync(setupFile);
            licenseInformation = CurrentAppSimulator.LicenseInformation;
#else
            licenseInformation = CurrentApp.LicenseInformation;
#endif
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
                string receipt = string.Empty;
#if DEBUG
                var result = await CurrentAppSimulator.RequestProductPurchaseAsync(productID);
#else
                var result = await CurrentApp.RequestProductPurchaseAsync(productID);
#endif
                string xmlReceipt = result.ReceiptXml;

                if (!string.IsNullOrEmpty(xmlReceipt))
                {
                    CompleteFulfillMent(productID, xmlReceipt, transactionId, result.TransactionId);
                }
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
        private async void CompleteFulfillMent(string productID, string receiptData, string transactionId, Guid storeTransactionId)
        {
            ProductLicense tokenLicense = licenseInformation.ProductLicenses[productID];

            if (
#if WINDOWS_PHONE_APP
                tokenLicense.IsConsumable &&
#endif
                tokenLicense.IsActive)
            {
#if DEBUG
                await CurrentAppSimulator.ReportConsumableFulfillmentAsync(tokenLicense.ProductId, storeTransactionId);
#else

                if (inAppPurchase != null)
                {
                    bool result = await inAppPurchase(receiptData, transactionId);

                    if (result)
                    {
                        await CurrentApp.ReportConsumableFulfillmentAsync(tokenLicense.ProductId, storeTransactionId);
                    }
                }
                else
                {
                    await CurrentApp.ReportConsumableFulfillmentAsync(tokenLicense.ProductId, storeTransactionId);
                }
#endif
            }
        }
#endregion
    }
}
