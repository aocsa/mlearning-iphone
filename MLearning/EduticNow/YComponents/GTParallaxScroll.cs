using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace YComponents
{
	public enum GTScrollOrientation
	{
		Horizontal, Vertical
	};

	public class GTParallaxScroll : UIView
	{
		public GTParallaxScroll (nfloat x, nfloat y,nfloat width, nfloat height, GTScrollOrientation orientation) 
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			DeviceWidth = width;
			DeviceHeight = height;

			Frame = new CGRect (0, 0, DeviceWidth, DeviceHeight);

			ScrollOrientation = orientation;
			xPos = x;
			yPos = y;
			initControls ();

		}

		#region Properties

		public UIScrollView scroll, backScroll ; 
		GTScrollOrientation ScrollOrientation = GTScrollOrientation.Horizontal ;
		nfloat DeviceWidth = 1024, DeviceHeight = 768 ;
		nfloat ContentSize = 0 ;
		nfloat xPos= 0, yPos = 0;

		#endregion


		void initControls()
		{
			BackgroundColor = UIColor.Clear;

			backScroll = new UIScrollView (new CGRect(xPos,yPos,DeviceWidth,DeviceHeight));
			backScroll.MinimumZoomScale = 1.0f;
			backScroll.MaximumZoomScale = 10.0f;
			backScroll.BouncesZoom = false;
			backScroll.BackgroundColor = UIColor.Clear;
			Add (backScroll);

			scroll = new UIScrollView (new CGRect(xPos,yPos,DeviceWidth,DeviceHeight));
			scroll.DecelerationRate = 8;  
			Add (scroll);

			scroll.Scrolled += (sender, e) => {
				nfloat off_set ;
				if (ScrollOrientation == GTScrollOrientation.Horizontal)
					off_set =scroll.ContentOffset.X ;
				else off_set = scroll.ContentOffset.Y ;

				if(off_set <= 0 ) 
				{
					backScroll.SetZoomScale(1+ (nfloat)Math.Abs(off_set*0.0002) , false ); 
					backScroll.SetContentOffset(new CGPoint(0, 0), false); 
				}
				else 
					backScroll.SetContentOffset(new CGPoint(scroll.ContentOffset.X / 2, scroll.ContentOffset.Y / 2), false);  
			};
 
		}


		#region Public Methods

		public void SetContent(UIView content, nfloat size)
		{
			ContentSize = size; 
			scroll.Add (content); 
			if (ScrollOrientation == GTScrollOrientation.Horizontal)
				scroll.ContentSize = new CGSize (ContentSize, DeviceHeight);
			else
				scroll.ContentSize = new CGSize (DeviceWidth,ContentSize); 
		}

		public void SetBackContent(UIView content, nfloat size)
		{
			backScroll.Add (content); 
			if (ScrollOrientation == GTScrollOrientation.Horizontal)
				backScroll.ContentSize = new CGSize (size, DeviceHeight);
			else
				backScroll.ContentSize = new CGSize (DeviceWidth,size); 
			backScroll.ViewForZoomingInScrollView += (UIScrollView sv) => { return content; };
		}

		public void SetSize(nfloat size)
		{
			ContentSize = size;
			if (ScrollOrientation == GTScrollOrientation.Horizontal)
				scroll.ContentSize = new CGSize (ContentSize, DeviceHeight);
			else
				scroll.ContentSize = new CGSize (DeviceWidth,ContentSize); 
		}

		public void SetContentOffset(nfloat offset)
		{
			if (ScrollOrientation == GTScrollOrientation.Vertical)
				backScroll.SetContentOffset (new CGPoint (offset, backScroll.ContentOffset.Y), false);
			else
				backScroll.SetContentOffset (new CGPoint (backScroll.ContentOffset.X, offset), false);
		}

		#endregion


	}
}

