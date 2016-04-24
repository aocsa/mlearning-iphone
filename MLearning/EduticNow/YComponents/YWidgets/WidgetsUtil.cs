using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace YComponents.YWidgets
{
	public class WidgetsUtil
	{ 

		public static UIImageView getImageFromBytes(nfloat x , nfloat y, nfloat width , nfloat height, nfloat corad ,  byte[] bytes )
		{
			var image = new UIImageView (new CGRect(x,y,width,height));
			var img = ToUIImage (bytes);
			if (img != null)
				image.Image = img;
			image.Layer.CornerRadius = corad ;
			//image.Layer.BorderWidth = 0.0f ;
			//image.Layer.BorderColor = UIColor.White.CGColor;
			image.SetNeedsDisplay ();
			return image;
		}

		public static UIImage ToUIImage (byte[] bytes)
		{
			if (bytes == null)
				return null;

			using (var data = NSData.FromArray(bytes)) {
				return UIImage.LoadFromData (data);
			}

		}


		public static UIImage ImageFromBytes(byte[] bytes, nfloat width, nfloat height)
		{
			try {
				NSData data = NSData.FromArray(bytes);
				UIImage image = UIImage.LoadFromData(data);
				CGSize scaleSize = new CGSize(width, height);
				UIGraphics.BeginImageContextWithOptions(scaleSize, false, 0);
				image.Draw(new CGRect(0,0, scaleSize.Width, scaleSize.Height));
				UIImage resizedImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
				return resizedImage;
			} catch (Exception) {
				return null;
			}
		}
   

		public static List<UIColor> themes = new List<UIColor> (){
			UIColor.FromRGBA(34,141,224,250),
			UIColor.FromRGBA(192,50,242,250),
			UIColor.FromRGBA(0,201,0,250),
			UIColor.FromRGBA(234,162,3,250),
			UIColor.FromRGBA(255,127,0,250),
			UIColor.FromRGBA(255,53,148,250)
		};


		public static List<UIColor> bthemes = new List<UIColor> (){
			UIColor.FromRGBA(34,141,224,100),
			UIColor.FromRGBA(192,50,242,100),
			UIColor.FromRGBA(0,201,0,100),
			UIColor.FromRGBA(234,162,3,100),
			UIColor.FromRGBA(255,127,0,100),
			UIColor.FromRGBA(255,53,148,100)
		};


		public static UIButton getDownMenuButton(nfloat x, nfloat y , UIColor bc , string content)
		{
			UIButton bt = new UIButton (UIButtonType.Custom);
			bt.Frame = new CGRect (x,y,90,30);
			bt.Layer.CornerRadius = 6;
			bt.Layer.MasksToBounds = true;
			bt.BackgroundColor = bc;
			bt.SetTitleColor (UIColor.White, UIControlState.Normal);
			bt.SetTitle (content, UIControlState.Normal); 
			bt.Font = UIFont.FromName ("HelveticaNeue" , 16);
			return bt;
		}
	

	}


	public delegate void LeftCircleItemViewTappedEventHandler(object sender);
	public class LeftCircleItemView : UIView
	{
		public LeftCircleItemView(nfloat px, nfloat py)
		{
			x = px;
			y = py;

			Frame = new CGRect (x,y,w,h); 
			BackgroundColor = UIColor.Clear;
			initView ();
		}

		nfloat x, y , w = 190, h= 58;

		UILabel circleLabel, numberLabel ;
		UIView board ;
		void initView()
		{
			circleLabel = new UILabel (new CGRect (66, 30, 174, 20)){ 
				Font = UIFont.FromName("HelveticaNeue",14),
				TextColor = UIColor.White
			};
			Add (circleLabel);

			var label  = new UILabel (new CGRect (66, 10, 174, 20)){ 
				Font = UIFont.FromName("HelveticaNeue",14),
				TextColor = UIColor.White,
				Text = "Curso :"
			};
			Add (label);

			board = new UIView (new CGRect(254, 16,26,26)){
				BackgroundColor = UIColor.DarkGray
			};
			board.Layer.CornerRadius = 4;
			Add (board);

			numberLabel = new UILabel (new CGRect (0, 0, 26, 26)){ 
				Font = UIFont.FromName("HelveticaNeue",14),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				Text = "3"
			};
			board.Add (numberLabel);

			UITapGestureRecognizer gesture = new UITapGestureRecognizer (() => {
				if(LeftCircleItemViewTapped != null)
					LeftCircleItemViewTapped(this);
			});
			AddGestureRecognizer (gesture);
		}

	 


		public string Title
		{
			set { circleLabel.Text = value;}
		}
			
		public string Number
		{
			set { numberLabel.Text = value;}
		}

		int idx = 0 ;
		public int Index
		{
			get { return idx;}
			set { idx = value;}
		}

		public event LeftCircleItemViewTappedEventHandler LeftCircleItemViewTapped ;
	}

	public class LeftMenuButton : UIView	
	{
		public LeftMenuButton(nfloat _x, nfloat _y, UIColor bcolor)
		{
			x = _x;
			y = _y;
			backColor = bcolor; 
			Frame = new CGRect (x,y, w,h);
			initView ();
		}


		nfloat x, y, w =  300 , h = 46 ;

		UIColor backColor ;
		UILabel titleLabel ;
		UIImageView image ;
		void initView()
		{
			BackgroundColor = backColor;

			image = new UIImageView (new CGRect(30, 10 , 20, 24));
			image.ContentMode = UIViewContentMode.ScaleAspectFit;
			Add (image);

			titleLabel = new UILabel (new CGRect(68,16,208,18)){
				Font = UIFont.FromName("HelveticaNeue",15),
				TextColor = UIColor.White	
			};
			Add (titleLabel);
		}


		#region public properties and methods


		public UIImage Image
		{
			set { image.Image = value;}
		}

		public string Title
		{
			set { titleLabel.Text = value;}
		}


		public void SetVisible(bool animated )
		{
			if (animated) {
				UIView.Animate (0.25, () => {
					BackgroundColor = backColor;
					titleLabel.Alpha = 1;
				});
			} else {
				BackgroundColor = backColor;
				titleLabel.Alpha = 1 ;
			}
		}


		public void SetInvisible(bool animated )
		{
			if (animated) {
				UIView.Animate (0.25, () => {
					BackgroundColor = UIColor.Clear;
					titleLabel.Alpha = 0;
				});
			} else {
				BackgroundColor = UIColor.Clear;
				titleLabel.Alpha = 0 ;
			}
		}

		#endregion
	}


	//class for the list of view of the users on te right
	public class UserElementView : UIView
	{
		public UserElementView(nfloat px, nfloat py)
		{
			x = px;
			y = py;

			Frame = new CGRect (x,y,w,h); 
			BackgroundColor = UIColor.Clear;
			initView ();
		}

		nfloat x, y , w = 190, h= 46;

		UIImageView userImage ;
		UILabel nameLabel, stateLabel ,userView;
		void initView()
		{

			userView = new UILabel (new CGRect (14, 3, 30, 30))
			{
				BackgroundColor = UIColor.Gray,
				Font = UIFont.FromName("HelveticaNeue" , 32 ),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center
			};
			userView.Layer.CornerRadius = 4;
			userView.Layer.MasksToBounds = true;
			Add (userView);

			userImage = new UIImageView (new CGRect(14,3,30,30 ));
			userImage.Layer.CornerRadius = 4;
			userImage.Layer.MasksToBounds = true;
			Add (userImage);

			nameLabel = new UILabel (new CGRect( 58, 6, 150, 20)) {
				Text = "Alexander Ocsa",
				Font = UIFont.FromName ("HelveticaNeue", 12),
				TextColor = UIColor.Black 
			};
			Add (nameLabel);
			stateLabel = new UILabel (new CGRect(58, 18, 150, 20)) {
				Text = "Online Now",
				Font = UIFont.FromName ("HelveticaNeue", 12),
				TextColor = ThemeColor  
			};
			Add (stateLabel);
		}


		#region Public Properties

		public UIImage UserImage
		{
			set{ userImage.Image = value; userView.Text = ""; SetNeedsDisplay (); }
		}


		public string Name
		{
			set
			{ 
				nameLabel.Text = value;
				userView.Text = (value[0] + "").ToUpper();
			}
		}

		bool isOnline ;
		public bool IsOnline
		{
			get { return isOnline;}
			set{
				isOnline = value;
				if (isOnline) {
					stateLabel.TextColor = ThemeColor;  
					stateLabel.Text = "Online Now";
				} else {
					stateLabel.TextColor = UIColor.Gray;
					stateLabel.Text = "OffLine";
				}
					
			}
		}

		UIColor themeColor  = UIColor.Yellow ;
		public UIColor ThemeColor
		{
			get { return themeColor;}
			set 
			{
				themeColor = value;
				if (isOnline)
					stateLabel.TextColor = themeColor;
			}
		}

		#endregion
	}

	public class NotificationCounter : UIView
	{
		public NotificationCounter(nfloat xpos, nfloat ypos , int fsize)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			x = xpos;
			y = ypos;
			fontsize = fsize;
			Frame = new CGRect (xpos, ypos, 30,20);

			initView ();
		}


		UIColor backColor = UIColor.White ;

		public UIColor BackColor
		{
			get { return backColor;}
			set { backColor = value;}
		}

		int numberNot = 100 ;
		public int NotificationsNumber
		{
			get { return  numberNot; }
			set { numberNot = value; }
		}


		nfloat w, h,x, y , maxwidth = 100, fontsize ;
		UILabel aLabel ;

		void initView()
		{
			aLabel = new UILabel (new CGRect(0,0,30,20));
			aLabel.Font = UIFont.FromName ("HelveticaNeue" , fontsize);
			Add (aLabel);
			resetValue ();
		}

		void resetValue()
		{
			BackgroundColor = BackColor;
			aLabel.Text = "" + numberNot;

			nfloat height = aLabel.Frame.Height;
			CGSize size = ((NSString)aLabel.Text).StringSize(aLabel.Font,constrainedToSize:new CGSize(30,maxwidth),
				lineBreakMode:UILineBreakMode.WordWrap);
			Frame = new CGRect (x,y,size.Width +8 , size.Height+4);
			aLabel.Frame = new CGRect (3,2,size.Width,size.Height);
			Layer.CornerRadius = (size.Height +4 )/ 2; 

		}


	}
}

