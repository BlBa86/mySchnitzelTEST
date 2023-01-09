using CoreGraphics;
using Foundation;
using System;
using System.Threading.Tasks;
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

            //NavigationItem.BackButtonTitle = "Zurück";

            //Dodato jer ukidamo tab bar
            this.NavigationController.SetNavigationBarHidden(false, true);

            NavigationController.NavigationBar.BackgroundColor = UIColor.FromRGB(255, 205, 103);
            View.BackgroundColor = UIColor.FromRGB(255, 205, 103);


            var webView = new WKWebView(View.Frame, new WKWebViewConfiguration());
            //webView.ScrollView.ContentOffset = new CGPoint(200, 200);
            View.AddSubview(webView);

            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.SystemFontOfSize(18.0f, UIFontWeight.Bold)
            };
            //


            //webView.ScrollView.SetContentOffset(CoreGraphics.CGPoint.Empty, true);

            //webView.ScrollView.ContentOffset = CGPointMake(0, 100);

            //webView.Eval("window.scrollTo(200, 200)");

            //webView.ScrollView.ScrollEnabled = false;

            //webView.Bounds.Offset

            // webView.ScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);

            //var scrollPoint = this.View.ConvertPointFromCoordinateSpace(new CGPoint(x: 0, y: 0), webView.ScrollView);
            //scrollPoint = new CGPoint(x: scrollPoint.X, y: webView.ScrollView.ContentSize.Height - webView.Frame.Size.Height);
            //webView.ScrollView.SetContentOffset(scrollPoint, animated: true);

            //webView.ScrollView.SetContentOffset(new CGPoint(0,500), animated: true);
            //webView.ScrollView.Bounces = false;

            //Dodato jer se javljala bela boja iznad webView-a kada se otvori stranica.
            webView.Opaque = false;
            webView.BackgroundColor = UIColor.Clear;

            webView.LoadRequest(new NSUrlRequest(url));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //NavigationItem.BackButtonTitle = "Zurück";
            //var webView = new WKWebView(View.Frame, new WKWebViewConfiguration());
            //webView.ScrollView.ContentOffset = new CGPoint(200, 200);
            //View.AddSubview(webView);

            NavigationController.NavigationBar.BackgroundColor = UIColor.FromRGB(255, 205, 103);
            View.BackgroundColor = UIColor.FromRGB(255, 205, 103);

            NavigationItem.Title = "Speisekarte";

            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.SystemFontOfSize(18.0f, UIFontWeight.Bold)
            };
        }
    }
}