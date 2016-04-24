using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace YComponents.YWidgets
{
	public class LoadingView : UIView
	{
		public LoadingView (nfloat xp, nfloat yp,nfloat ws, nfloat hs, string backimg)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;


			x = xp;
			y = yp;
			w = ws;
			h = hs;

			Frame = new CGRect (x,y,w,h);	
			backpath = backimg; 
			initControls ();
		}

		#region properties

		nfloat loadPercent = 60 ;
		public nfloat LoadPercent
		{
			get { return loadPercent;} 
			set { loadPercent = value; resetLoad (); }
		}


		public UIColor LoadColor
		{
			set{ loadView.BackgroundColor = value;}
		}

		public bool ActiveBackImage
		{
			set { 
				if (value)
					backImage.Alpha = 1;
				else {
					backImage.Alpha = 0;
					backView.Alpha = 1;
				}
			}
		}

		nfloat x,y,w,h ;
		string backpath ;

		UIImageView backImage ;
		UIView loadView , backView;
		#endregion 


		void initControls()
		{
			backView = new UIImageView (new CGRect(0,0,w,h)); 
			backView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);
			backView.Layer.CornerRadius = (h-8)/2 ;
			backView.Alpha = 0;
			Add (backView);


			backImage = new UIImageView (new CGRect(0,0,w,h));
			backImage.Image = UIImage.FromFile (backpath);
			Add (backImage);

			loadView = new UIView (new CGRect(6,4,w-12,h-8));
			loadView.BackgroundColor = UIColor.White;
			loadView.Layer.CornerRadius = (h-8)/2 ;
			Add (loadView);
		}

		void resetLoad()
		{
			nfloat load = (w - 12) * loadPercent / 100;
			loadView.Frame = new CGRect (6,4, load, h-8);
		}



	}
}

