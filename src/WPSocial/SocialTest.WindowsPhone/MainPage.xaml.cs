using System;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WPSocial.Facebook;
using WPSocial.Google;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SocialTest.WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            FacebookClient.Init("145634995501895");
            GoogleClient.Instance.Init("699153824643-utpktrshk2a717d46c0ldgepp35ju67k.apps.googleusercontent.com", "HKG9Teouhtj_Zho6tQz8wzBP");
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FacebookClient client = new FacebookClient();
            client.OnActionCompleted += Instance_OnActionCompleted;
            client.OnFacebookLoginCompleted += Instance_OnFacebookLoginCompleted;
            //FacebookClient.Instance.Login(new List<string>()
            //{
            //    "publish_actions",
            //    "publish_stream",
            //    "user_about_me",
            //    "manage_pages",
            //    "user_birthday",
            //    "user_friends",
            //    "user_status",
            //    "user_likes",
            //    "user_location",
            //    "user_posts"
            //});
            client.Login();
        }

        private void Instance_OnFacebookLoginCompleted(FacebookOauthResult result)
        {
            if (result.IsSuccess)
            {
                Debug.WriteLine("Login Facebook Sucessfully: " + result.AccessToken);
            }
            else
            {
                Debug.WriteLine("Login Error: " + result.ErrorDescription);
            }
        }

        private void google_Click(object sender, RoutedEventArgs e)
        {
            GoogleClient.Instance.GoogleLoginCompleted += Instance_GoogleLoginCompleted;

            string scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";
            GoogleClient.Instance.Login(scope);
        }

        private void Instance_GoogleLoginCompleted(GoogleOauthResult result)
        {
            if (result.IsSuccess)
            {
                Debug.WriteLine("Login Facebook Sucessfully: " + result.AccessToken);
            }
            else
            {
                Debug.WriteLine("Login Error: " + result.Error);
            }
        }

        private void shareFb_Click(object sender, RoutedEventArgs e)
        {
            FacebookClient client = new FacebookClient();
            client.OnActionCompleted += Instance_OnActionCompleted;
            client.Share(@"http://vnexpress.net/tin-tuc/thoi-su/tong-thong-obama-roi-tp-hcm-ket-thuc-chuyen-tham-viet-nam-3408809.html");
        }

        private void postFb_Click(object sender, RoutedEventArgs e)
        {
            FacebookClient client = new FacebookClient();
            client.OnActionCompleted += Instance_OnActionCompleted;

            client.Post(@"http://vnexpress.net/tin-tuc/thoi-su/tong-thong-obama-roi-tp-hcm-ket-thuc-chuyen-tham-viet-nam-3408809.html",
                "Test description",
                "Test caption",
                "Test name",
                "http://img.f1.thethao.vnecdn.net/2016/05/25/Mou-5308-1464147531.jpg");
        }

        private void Instance_OnActionCompleted(FacebookActionResult result)
        {
            if (result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(result.PostId))
                {
                    Debug.WriteLine("Action Ok: " + result.PostId);
                }

                if (!string.IsNullOrEmpty(result.RequestId))
                {
                    Debug.WriteLine("Action Ok: " + result.RequestId);
                }
            }
            else
            {
                Debug.WriteLine("Action Failed: " + result.ErrorCode + "-" + result.ErrorMessage);
            }
        }

        private void appRequestFb_Click(object sender, RoutedEventArgs e)
        {
            FacebookClient client = new FacebookClient();
            client.OnActionCompleted += Instance_OnActionCompleted;
            client.ShowAppRequest("request app test");
        }

        private void likePageFb_Click(object sender, RoutedEventArgs e)
        {
            FacebookClient client = new FacebookClient();
            client.OnActionCompleted += Instance_OnActionCompleted;
            client.LikePage("900160633407437");
        }
    }
}
