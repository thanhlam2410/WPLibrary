using System.Windows;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using WPSocial.Facebook;
using WPSocial.Google;

namespace SocialTest.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            FacebookClient.Init("1405576279680868");
            GoogleClient.Instance.Init("699153824643-utpktrshk2a717d46c0ldgepp35ju67k.apps.googleusercontent.com", "HKG9Teouhtj_Zho6tQz8wzBP");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FacebookClient client = new FacebookClient();
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

        private void Instance_OnActionCompleted(FacebookActionResult result)
        {
            if (result.IsSuccess)
            {
                Debug.WriteLine("Action Ok: " + result.PostId);
            }
            else
            {
                Debug.WriteLine("Action Failed: " + result.ErrorCode + ": " + result.PostId);
            }
        }

        private void postFb_Click(object sender, RoutedEventArgs e)
        {
            FacebookClient client = new FacebookClient();
            client.OnActionCompleted += Instance_OnActionCompleted;

            client.Post(@"http://vnexpress.net/tin-tuc/thoi-su/tong-thong-obama-roi-tp-hcm-ket-thuc-chuyen-tham-viet-nam-3408809.html",
                "Test description",
                "Test caption",
                "Test post");
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