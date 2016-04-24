using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace YComponents.YWidgets
{
	public delegate void LOThumbSelectedEventHandler(object sender,int color);

	public class LOThumbView : UIView
	{
		
		public LOThumbView (nfloat px, nfloat py)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			x = px;
			y = py;
			Frame = new CGRect (x,y,w,h);
			initView ();

			UITapGestureRecognizer gesture = new UITapGestureRecognizer (() => {
				if(LOThumbSelected != null)
					LOThumbSelected(this, LOColorID);
			});
			AddGestureRecognizer (gesture);
		}
			
 		
		public LOThumbSelectedEventHandler LOThumbSelected ;

		int idcolor = 0 ;
		public int LOColorID
		{
			get{ return idcolor;}
			set{ idcolor = value; }
		}


		nfloat x, y, w = 230, h = 164 ;
 
		UIImageView backImage , likeImage;
		UILabel nameLabel, likesLabel , npageLabel, pageLabel, percentLabel ;
		UIView downView, lineView, percentView ;

		void initView()
		{
			backImage = new UIImageView (new CGRect (0, 0, w, h));
			backImage.ContentMode = UIViewContentMode.ScaleAspectFill;
			backImage.Layer.MasksToBounds = true;
			backImage.BackgroundColor = UIColor.Gray;
			backImage.Image = UIImage.FromFile ("MyImage.png");
			Add (backImage);


			likeImage = new UIImageView (new CGRect (206, 34, 18, 16));
			likeImage.Image = UIImage.FromFile ("efiles/muro/like.png");
			Add (likeImage);

			likesLabel = new UILabel (new CGRect(206, 51, 18, 16))
			{
				TextColor = UIColor.White,
				Text = "10",
				Font = UIFont.FromName ("HelveticaNeue-Light",10),
				TextAlignment = UITextAlignment.Center
			} ;
			Add (likesLabel);

			likesLabel = new UILabel (new CGRect(206, 51, 18, 16))
			{
				TextColor = UIColor.White,
				Text = "10",
				Font = UIFont.FromName ("HelveticaNeue-Light",10),
				TextAlignment = UITextAlignment.Center
			} ;
			Add (likesLabel);


			npageLabel = new UILabel (new CGRect(206, 70, 18, 16))
			{
				TextColor = UIColor.White,
				Text = "7",
				Font = UIFont.FromName ("HelveticaNeue-Light",10),
				TextAlignment = UITextAlignment.Center
			} ;
			Add (npageLabel);

			pageLabel = new UILabel (new CGRect(186, 86 , 34, 16))
			{
				TextColor = UIColor.White,
				Text = "PAGES",
				Font = UIFont.FromName ("HelveticaNeue-Light",10),
				TextAlignment = UITextAlignment.Center
			} ;
			Add (pageLabel);

			initDown ();
		}

		nfloat downHeight =52 ;
		void initDown()
		{
			downView = new UIView (new CGRect(0, h-downHeight , w, downHeight));
			downView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 120);
			Add (downView);

			nameLabel = new UILabel (new CGRect(24, 18 , 154, 20))
			{
				TextColor = UIColor.White,
				Text = "Flora y Fauna",
				Font = UIFont.FromName ("HelveticaNeue",16),
				TextAlignment = UITextAlignment.Left
			} ;
			downView.Add (nameLabel);

			percentLabel = new UILabel (new CGRect(180, 18 , 34, 20))
			{
				TextColor = UIColor.White,
				Text = "30%",
				Font = UIFont.FromName ("HelveticaNeue",16),
				TextAlignment = UITextAlignment.Right
			} ;
			downView.Add (percentLabel); 

			lineView = new UIView (new CGRect (20, 39, 190, 2)){ BackgroundColor = UIColor.White };
			lineView.Layer.CornerRadius = 1;
			lineView.Layer.MasksToBounds = true;
			downView.Add (lineView);

			percentView = new UIView (new CGRect (20, 38, 190, 4)){BackgroundColor =  WidgetsUtil.themes[0]};
			nfloat p = 190 * 30 / 100;
			percentView.Frame = new CGRect (20, 38, p , 4); //30 is teh percent
			percentView.Layer.CornerRadius = 2 ;
			percentView.Layer.MasksToBounds = true;
			downView.Add (percentView);
		}

		public void SetValues( string name, int percent, int likes, int pages)
		{
			nameLabel.Text = name; 
			likesLabel.Text = "" + likes;
			npageLabel.Text = "" + pages;
		}


		public UIImage LOImage
		{
			set { backImage.Image = value; }
		}




		public UIColor ThemeColor
		{
			set
			{
				percentView.BackgroundColor = value;
			}
		}
	
	}
}

