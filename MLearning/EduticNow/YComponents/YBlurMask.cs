using System;
using UIKit;
using CoreGraphics;
using System.Drawing;
using Foundation;
using CoreText;

namespace MLearning.UnifiedTouch
{
	public class YBlurMask : UIView
	{
		public YBlurMask (string img, nfloat x, nfloat y, nfloat w, nfloat h )
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
		
			bwidth = w;
			bheight = h;
			Frame = new CGRect (x,y,w,h);
			imagePath = img;

			initComponent ();
		}


		#region properties

		nfloat bwidth, bheight ;
		string imagePath ;
		UIImageView bimage ;
		UIScrollView bscroll ;
		#endregion


		void initComponent()
		{
			bscroll = new UIScrollView (new CGRect (0,0,bwidth,bheight));
			Add (bscroll);

			bimage = new UIImageView (new CGRect(0,0,bwidth,bheight)){ContentMode= UIViewContentMode.ScaleAspectFill};
			bimage.Image = UIImage.FromFile (imagePath);
			bscroll.Add (bimage);

			//set blur 
   
			UIImage effectimage = UIImageEffects.ApplyBlur (UIImage.FromFile (imagePath), 90, 
				UIColor.FromRGBA(0,0,0,120), 0.6f, null);
			//UIImage effectimage = UIImage.FromFile(imagePath).ApplyTintEffect( UIColor.FromRGBA(0,0,0,100));
			bimage.Image = effectimage; 
		}



		#region public methods

		public void Open()
		{
			
		}



		public void Close()
		{
			
		}

		#endregion
	}
}

