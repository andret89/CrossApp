
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace CrossApp.iOS
{
    public partial class ViewController1 : UIViewController
    {
        public UIBarButtonItem shareButton;

        public ViewController1(IntPtr handle) : base(handle)
        {
       
        }

        private void ShareEventHandler(object sender, EventArgs e)
        {
            UIImage image = new UIImage();
            NSObject[] activityItems = { image };
            UIActivityViewController activityViewController = new UIActivityViewController(activityItems, null);
            activityViewController.ExcludedActivityTypes = new NSString[] { };
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                activityViewController.PopoverPresentationController.SourceView = View;
                activityViewController.PopoverPresentationController.SourceRect = new CoreGraphics.CGRect((View.Bounds.Width / 2), (View.Bounds.Height / 4), 0, 0);
            }
            this.PresentViewController(activityViewController, true, null);
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("OpenMyFile"), OpenFileVoid);

            shareButton = new UIBarButtonItem(UIBarButtonSystemItem.Action, ShareEventHandler);
            this.NavigationItem.RightBarButtonItem = shareButton;

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public void OpenFileVoid(NSNotification notification)
        {
            NSUrl _filePath = (NSUrl)notification.Object;
            // Do what you need with this file path
        }

        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
    }
}