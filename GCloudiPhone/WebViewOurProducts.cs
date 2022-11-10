 using Foundation;
using System;
using UIKit;
using WebKit;

namespace GCloudiPhone
{
    public partial class WebViewOurProducts : UIViewController
    {

        private readonly NSUrl url = new NSUrl("https://myschnitzel.at/apppart/speisekarten");

        public WebViewOurProducts (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Dodato jer ukidamo tab bar
            this.NavigationController.SetNavigationBarHidden(false, true);

            NavigationController.NavigationBar.BackgroundColor = UIColor.FromRGB(255, 205, 103);


            var webView = new WKWebView(View.Frame, new WKWebViewConfiguration());
            View.AddSubview(webView);

            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.SystemFontOfSize(18.0f, UIFontWeight.Bold)
            };
            //

           
            webView.ScrollView.SetContentOffset(CoreGraphics.CGPoint.Empty, true);

            //webView.ScrollView.ContentOffset = CGPointMake(0, 100);

            webView.LoadRequest(new NSUrlRequest(url));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationItem.Title = "Speisekarte";

            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.SystemFontOfSize(18.0f, UIFontWeight.Bold)
            };
        }
    }
}