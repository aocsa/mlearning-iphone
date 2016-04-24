using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace YComponents
{
	public delegate void SlideButtonTappedEventHandler(object sender, int idx) ;

	public class UISlideButton : UIView
	{
		public UISlideButton (nfloat x, nfloat y , nfloat w, nfloat h)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions; 
			_width = w;
			_height = h;
			_x = x;
			_y = y;

			Frame = new CGRect (_x,_y,_width,_height );

			//init
			initControls() ;
		}


		#region Constants

		nfloat _width = 0, _height = 0, _x = 0, _y = 0 ;
		UIImageView iconImage ;


		#endregion


		#region Properties

		public event SlideButtonTappedEventHandler SlideButtonTapped;

		string _imageUrl = "assets/icons/contenido6.png" ;
		public string ImageUrl
		{
			get { return _imageUrl; }
			set 
			{
				_imageUrl = value;
				iconImage.Image = UIImage.FromFile (_imageUrl);
			}
		}


		int _index = 0  ;
		public int Index
		{
			get { return _index;}
			set { _index = value;}
		}


		bool _selected =  false  ;
		public bool Selected
		{
			get { return _selected; }
			set { _selected = value; }
		}

		#endregion


		void initControls()
		{
					
			iconImage =  new UIImageView(new CGRect(0,0,_width,_height)) ;
			iconImage.ContentMode = UIViewContentMode.ScaleAspectFit;
			iconImage.Image = UIImage.FromFile (_imageUrl);
			Add (iconImage);
		
			//for tap
			UITapGestureRecognizer tapped =  new UITapGestureRecognizer(buttonTapped){NumberOfTapsRequired =  1 } ;
			AddGestureRecognizer (tapped);

		}


		private void buttonTapped()
		{
			if (SlideButtonTapped != null)
				SlideButtonTapped (this, _index);
		}


	}
}

