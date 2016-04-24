using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class MultiImageSlideView : UIView,SlideInterface
	{
		public MultiImageSlideView(nfloat pos) : base()
		{
			slidePos = pos;
			initView ();
		}

		nfloat slideHeight = 0 ;
		public nfloat GetHeight()
		{
			return slideHeight;
		}

		nfloat slidePos =0 ;
		public nfloat GetPosition()
		{
			return slidePos;
		}

 
		UIScrollView scroll ;
		nfloat separation = 8, imageWidth =192 ,imageHeight = 196 ;
		int numberofItems = 3 ;
		void initView()
		{
			scroll = new UIScrollView (new CGRect(0,15,320,196));
			Add (scroll);

			for (int i = 0; i < numberofItems; i++) {
				var img = new UIImageView (new CGRect(i*(imageWidth + separation),0,imageWidth,imageHeight));
				img.ContentMode = UIViewContentMode.ScaleToFill;
				img.Image = UIImage.FromFile ("MLResources/default_img.png");
				scroll.Add (img);
			}
			scroll.ContentSize = new CGSize (numberofItems * (imageWidth + separation) - separation, imageHeight);

			//set frame
			var frame = new CGRect(0,slidePos,320, imageHeight + 30);
			Frame = frame;
			slideHeight = imageHeight + 30;
		}
	}
}

