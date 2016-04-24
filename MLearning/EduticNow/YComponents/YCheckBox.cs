using System;
using UIKit;
using CoreGraphics;
using System.Drawing;
using Foundation;
using CoreText;

namespace MLearning.UnifiedTouch
{
	public delegate void YCheckBoxSelectedEventHandler(object sender);
	public delegate void YCheckBoxReleasedEventHandler(object sender);


	public class YCheckBox : UIView
	{
		public YCheckBox (nfloat x, nfloat y, nfloat w, nfloat h)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			cheight = h;
			cwidth = w;
			Frame = new CGRect (x,y,w,h);

			initControls ();

		}


		#region events

		public event YCheckBoxReleasedEventHandler YCheckBoxReleased;
		public event YCheckBoxSelectedEventHandler YCheckBoxSelected;

		#endregion

		bool isChecked = false ;
		public bool IsChecked {
			get{ return isChecked; }
			set{  isChecked = true;}
		}


		nfloat cheight, cwidth ;
		UIImageView backImage, checkImage ;

		void initControls()
		{

			backImage = new UIImageView (new CGRect (0,0,cwidth, cheight));
			backImage.Image = UIImage.FromFile ("efiles/login/cuadro.png"); 
			Add (backImage);

			checkImage = new UIImageView (new CGRect(0,0,cwidth,cheight));
			checkImage.Image = UIImage.FromFile ("efiles/login/cuadrocheck.png"); 
			Add (checkImage);

			checkImage.Alpha = 0.0f; 

			var tap = new UITapGestureRecognizer ( ()=> {
				if(isChecked){
					isChecked = false ;
					checkImage.Alpha = 0.0f ;
					if(YCheckBoxReleased!=null)
						YCheckBoxReleased(this);
				}	
				else{
					isChecked = true ;
					checkImage.Alpha = 1.0f ;
					if(YCheckBoxSelected!=null)
						YCheckBoxSelected(this);
				}
			});

			AddGestureRecognizer (tap);
		}

	}
}

