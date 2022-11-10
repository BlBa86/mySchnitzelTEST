using Foundation;
using GCloudiPhone.Extensions;
using GCloudShared.Repository;
using GCloudShared.Shared;
using System;
using UIKit;
using WebKit;
using Xamarin.Essentials;

namespace GCloudiPhone
{
    public partial class WebShopViewController : UIViewController
    {
        //ovde treba ucitati username i password sa login stranice
        //customer/ValidationExample?email=perica@gmail.com&password=test12345

        //private readonly NSUrl url1 = new NSUrl("https://esterhazystrasse.myschnitzel.at/customer/ValidationExample?email=");
        //private readonly NSUrl url2 = new NSUrl("https://fischauergasse.myschnitzel.at/customer/ValidationExample?email=");
        //private readonly NSUrl url3 = new NSUrl("https://grazerstrasse.myschnitzel.at/customer/ValidationExample?email=");
        //private readonly NSUrl url4 = new NSUrl("https://neunkirchen.myschnitzel.at/customer/ValidationExample?email=");

        public StoreLocationDto Store { get; set; }

        private readonly UserRepository _userRepository;
       // NSUserDefaults storevalues = new NSUserDefaults();

        public WebShopViewController (IntPtr handle) : base (handle)
        {
            _userRepository = new UserRepository(DbBootstraper.Connection);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var user = _userRepository.GetCurrentUser();
            var username = user.Username;
            //var password = storevalues.StringForKey("stringvalue");
            //var password = CommonClass.value;
            var password = Preferences.Get("password", "00000");

            string url1 = "https://esterhazystrasse.myschnitzel.at/customer/ValidationExample?email=" + username + "&password=" + password;
            string url2 = "https://fischauergasse.myschnitzel.at/customer/ValidationExample?email=" + username + "&password=" + password;
            string url3 = "https://grazerstrasse.myschnitzel.at/customer/ValidationExample?email=" + username + "&password=" + password;
            string url4 = "https://neunkirchen.myschnitzel.at/customer/ValidationExample?email=" + username + "&password=" + password;

            NSUrl urlEisenstadt = new NSUrl(url1);
            NSUrl urlFischauerGasse = new NSUrl(url2);
            NSUrl urlGrazerStrasse = new NSUrl(url3);
            NSUrl urlNeukirchen = new NSUrl(url4);


            var storeName = Store.Name;
            var webView = new WKWebView(View.Frame, new WKWebViewConfiguration());
            //webView.ScrollView.ScrollEnabled = false;
            View.AddSubview(webView);

            //Dodato jer se javljala bela boja iznad webView-a kada se otvori stranica.
            webView.Opaque = false;
            webView.BackgroundColor = UIColor.Clear;

            if (storeName == "Eisenstadt")
            {
                webView.LoadRequest(new NSUrlRequest(urlEisenstadt));
            }
            else if (storeName == "Fischauer Gasse")
            {
                webView.LoadRequest(new NSUrlRequest(urlFischauerGasse));
            }
            else if (storeName == "Grazer Straße")
            {
                webView.LoadRequest(new NSUrlRequest(urlGrazerStrasse));
            }
            else
            {
                webView.LoadRequest(new NSUrlRequest(urlNeukirchen));
            }
        }
    }
}