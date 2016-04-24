using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public partial class ComponentsAppViewController : UIViewController
	{
		public ComponentsAppViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.

			//initscroll ();
			//testElement();

			GTReaderView rview = new GTReaderView ();
			View.Add (rview);

		}


		void testElement()
		{
			BackImageSlideView slide = new BackImageSlideView (45);
			View.Add (slide);
		}



		GTVerticalScrollView _scroll ;
		GTHorizontalScrollView hscroll ;

		void initscroll()
		{
			View.BackgroundColor = UIColor.Red;

			//_scroll = new GTVerticalScrollView (new CGRect(0,0,320,568));
			//View.Add (_scroll);
  
			hscroll = new GTHorizontalScrollView(new CGRect(0,0,320,568));
			View.Add (hscroll);
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

