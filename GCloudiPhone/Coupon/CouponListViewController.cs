using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GCloud.Shared.Dto.Domain;
using GCloudiPhone.Extensions;
using GCloudiPhone.Shared;
using GCloudShared.Service;
using GCloudShared.Shared;
using Refit;
using UIKit;
using GCloudiPhone.Caching;
using Optional.Collections;
using GCloudShared.Repository;

namespace GCloudiPhone
{
    public partial class CouponListViewController : UITableViewController, IUISearchResultsUpdating
    {
        readonly IUserCouponService _couponService;
        List<CouponDto> _coupons;
        private UISearchController search;
        private UIRefreshControl refreshControl;
        private LoadingOverlay loadingView;
        private NoElementsFoundOverlay noElementsFound;
        public string CouponType;
        //public StoreLocationDto Store { get; set; }

        private readonly UserRepository _userRepository;
        private readonly IAuthService _authService;

        public CouponListViewController(IntPtr handle) : base(handle)
        {
            _coupons = new List<CouponDto>();
            _couponService = RestService.For<IUserCouponService>(HttpClientContainer.Instance.HttpClient);

            _userRepository = new UserRepository(DbBootstraper.Connection);
            _authService = RestService.For<IAuthService>(HttpClientContainer.Instance.HttpClient);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Dodato jer ukidamo tab bar
            this.NavigationController.SetNavigationBarHidden(false, true);

            refreshControl = new UIRefreshControl
            {
                BackgroundColor = UIColor.Clear
            };
            refreshControl.ValueChanged += RefreshControl_ValueChanged;

            loadingView = new LoadingOverlay(View.Frame);

            noElementsFound = new NoElementsFoundOverlay(View.Frame);

            CouponList.Add(refreshControl);
            CouponList.Source = new CouponTableSource(_coupons);
            CouponList.RowHeight = 75;
            search = new UISearchController(searchResultsController: null)
            {
                DimsBackgroundDuringPresentation = false
            };
            //search.SearchResultsUpdater = this;
            //search.SearchBar.Placeholder = "Gutscheine durchsuchen";
            //search.SearchBar.SetValueForKey(new NSString("Abbrechen"), new NSString("_cancelButtonText"));
            //search.SearchBar.ScopeButtonTitles = new string[] { "Alles", "Tags" };
            //if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            //{
            //    NavigationItem.SearchController = search;
            //    NavigationItem.HidesSearchBarWhenScrolling = false;
            //}
            //else
            //{
            //    search.SearchBar.SearchBarStyle = UISearchBarStyle.Default;
            //    search.SearchBar.SizeToFit();
            //    CouponList.TableHeaderView = search.SearchBar;
            //}

            //Sklanjamo linije izmedju redova u tabeli
            CouponList.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            //this.NavigationController.SetNavigationBarHidden(true, true);
            NavigationController.NavigationBar.BackgroundColor = UIColor.FromRGB(255, 205, 103);
            View.BackgroundColor = UIColor.FromRGB(255, 205, 103);

            //this.NavigationController.View.BackgroundColor = UIColor.Clear;
            //this.NavigationController.NavigationBar.BackgroundColor = UIColor.FromRGB(255, 205, 103);
            this.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(255, 205, 103);

            CouponList.BackgroundColor = UIColor.FromRGB(255, 205, 103);
            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.SystemFontOfSize(18.0f, UIFontWeight.Bold)
            };

            //NavigationItem.Title = "Zurück";
            NavigationItem.BackButtonTitle = "Zurück";

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //Fiksni naziv radnje
            //NavigationItem.Title = "Eisenstadt";
            NavigationItem.BackButtonTitle = "Zurück";

            var user = _userRepository.GetCurrentUser();
           
            var totalPoints = _authService.GetTotalPointsByUserID(user.UserId).Result;
            var totalPointsNew = totalPoints.Replace("\"", "");



            //NavigationItem.Title = "Punkte einlösen" + " " + totalPointsNew + " " + "Punkte";
            NavigationItem.Title = "Punkte einlösen";
            //add new line
            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.SystemFontOfSize(18.0f, UIFontWeight.Bold)
            };

            //var cashbackBtn = new UIBarButtonItem(UIImage.FromBundle("CashbackIcon"), UIBarButtonItemStyle.Plain, (sender, e) => PerformSegue("CashbackSegue", this));
            //NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[] { cashbackBtn }, true);
            //NavigationItem.SetLeftBarButtonItem(null, true);


            if (((AppDelegate)UIApplication.SharedApplication.Delegate).AuthState == AuthState.Unauthorized)
            {
                var loginBtn = new UIBarButtonItem(UIImage.FromBundle("LoginIcon"), UIBarButtonItemStyle.Plain, (sender, e) => TabBarController.PerformSegue("LoginSegue", this));
                NavigationItem.RightBarButtonItem = loginBtn;
            }

            LoadCoupons();
        }

        private async Task<bool> LoadCoupons()
        {
            if (((AppDelegate)UIApplication.SharedApplication.Delegate).AuthState == AuthState.Unauthorized)
            {
                CouponList.Source = new CouponTableSource(new List<CouponDto>());
                CouponList.ReloadData();
                TableViewHelper.EmptyMessage("Bitte melde Dich an, damit wir Dir Deine Gutscheine anzeigen können.", new WeakReference<UITableViewController>(this));
                CouponList.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
                return false;
            }

            View.Add(loadingView);

            try
            {
                //Dodat fiksni GUID
                Guid storeGuid = new Guid("1F526AA2-621F-EC11-901F-48F17F295823");
                //if (CouponType == "WithoutSpecialProducts")
                if (CouponType == "WithoutSpecialProducts")
                {
                    _coupons = CacheHolder.Instance.GetCouponsByStore(storeGuid).Values().ToList().Where(s => s.CouponType != CouponTypeDto.SpecialProductPoints).ToList();
                }
                //else if (CouponType == "WithSpecialProducts")
                else if (true)
                {
                    _coupons = CacheHolder.Instance.GetCouponsByStore(storeGuid).Values().ToList().Where(s => s.CouponType == CouponTypeDto.SpecialProductPoints).ToList();
                }
                   
               
            }
            catch (ApiException)
            {
                _coupons = new List<CouponDto>();
            }
            CouponList.Source = new CouponTableSource(_coupons);
            CouponList.ReloadData();
            CouponList.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));

            if (_coupons.Count <= 0)
            {
                TableViewHelper.EmptyMessage("Keine Gutscheine verfügbar", new WeakReference<UITableViewController>(this));
            }
            else
            {
                TableView.BackgroundView = null;
            }

            loadingView.Hide();
            return true;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "CouponDetailsSegue")
            {
                var couponDetailController = segue.DestinationViewController as CouponDetailController;
                var couponListItem = sender as CouponListItem;
                couponDetailController.Coupon = couponListItem.Coupon;

                if (search.Active)
                {
                    search.Active = false;
                }
            }
            else if (segue.Identifier == "CashbackSegue")
            {
                var cashbackViewController = segue.DestinationViewController as CashbackViewController;
                cashbackViewController.StoreGuid = "1F526AA2-621F-EC11-901F-48F17F295823";
            }
            base.PrepareForSegue(segue, sender);
        }

        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            if (((AppDelegate)UIApplication.SharedApplication.Delegate).AuthState == AuthState.Unauthorized)
            {
                return;
            }

            var searchString = searchController.SearchBar.Text;

            //scope is an array of strings, where 0 means all attributes (name and tags) and 1 means tags-only
            var scope = searchController.SearchBar.SelectedScopeButtonIndex;

            List<CouponDto> searchResult;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                searchResult = _coupons.Where(c => c.Name.ToLower().Contains(searchString.ToLower())).Select(c => c).ToList();
            }
            else
            {
                searchResult = _coupons;
            }

            if (searchResult.Count <= 0)
            {
                TableViewHelper.EmptyMessage("Keine Gutscheine gefunden", new WeakReference<UITableViewController>(this));
            }
            else
            {
                CouponList.BackgroundView = null;
            }
            CouponList.Source = new CouponTableSource(searchResult);
            CouponList.ReloadData();
        }

        async void RefreshControl_ValueChanged(object sender, EventArgs e)
        {
            await LoadCoupons();
            refreshControl.EndRefreshing();
        }
    }
}