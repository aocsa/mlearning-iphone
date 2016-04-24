using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;


namespace YComponents.YWidgets
{

	public delegate void IPostDoCommentEventHandler(object sender) ;
	public delegate void IPostSizeChangedEventHandler(object sender);

	public interface IPost
	{
		nfloat GetHeight();
		nfloat GetPosition();
	}

	public class InputPostView : UIView , IPost
	{
		public InputPostView (nfloat xpos, nfloat ypos , nfloat cw , nfloat ch)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			x = xpos;
			y = ypos;
			w = cw;
			h = ch;

			Frame = new CGRect (x,y,w,h);
			//BackgroundColor = UIColor.Yellow;
			initView ();
		}


		#region properties

		nfloat x, y, w, h ;
		nfloat max_height = 340 ;

		#endregion
	

		#region interface methods

		nfloat currentHeight = 0 , currentWidth = 0 ;

		public nfloat GetHeight()
		{
			return currentHeight;
		}
		public nfloat GetPosition()
		{
			return currentWidth;
		}


		public event IPostDoCommentEventHandler IPostDoComment ;
		public event IPostSizeChangedEventHandler IPostSizeChanged;
		#endregion


		public UITextView InputTextView
		{
			get { return input;}
			set { }
		}

		public UIButton DoSendButton
		{
			get { return sendButton;}
			set { }
		}

		UIView borderView  ;
		UITextView input ;
		UIButton sendButton ;
		void initView()
		{
			currentHeight = h;

			borderView = new UIView (new CGRect (0, 0, w, h));
			borderView.Layer.BorderColor = UIColor.Gray.CGColor;
			borderView.Layer.BorderWidth = 1;
			borderView.Layer.CornerRadius = 8;
			Add (borderView);

			input = new UITextView (new CGRect(10,6,w-100,h-12)){
				BackgroundColor = UIColor.Clear ,
				Editable = true ,
				TextColor = UIColor.Black ,
				Font = UIFont.FromName("HelveticaNeue-Light", 14),
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin
			};
			Add (input);

			sendButton = new UIButton (UIButtonType.Custom)
			{
				Frame  = new CGRect(528 ,6,75, 30),
				BackgroundColor =  WidgetsUtil.themes[0],
				Font = UIFont.FromName("HelveticaNeue",16)
			};
			sendButton.SetTitle ("Post", UIControlState.Normal);
			sendButton.Layer.CornerRadius = 6;
			sendButton.Layer.MasksToBounds = true;
			Add (sendButton);

			input.Changed+= (object sender, EventArgs e) => {
				CGSize size = ((NSString)input.Text).StringSize(input.Font,constrainedToSize:new CGSize(w-100,max_height),
					lineBreakMode:UILineBreakMode.WordWrap);
				nfloat n_h = max_height/2 ;
				if(size.Height < max_height/2)
					if(size.Height > h-12)
						n_h =  size.Height + 12 ;
					else n_h = h ;
							
				currentHeight = n_h + 12 ;
				if(IPostSizeChanged!= null)
					IPostSizeChanged(this);

				UIView.Animate(0.2 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
					{
						borderView.Frame =  new CGRect(0,0, w , n_h + 12); 
						input.Frame = new CGRect(10,6 , w-100 ,n_h) ;
						sendButton.Frame = new CGRect(528 ,(n_h-12)/2,75, 30);
						Frame = new CGRect(x,y , w , n_h + 12); 
					}, null );

			};

			sendButton.TouchUpInside+= (sender, e) => {
			

				if(input.HasText)
				{
					currentHeight = h ;
					if(IPostDoComment!= null)
						IPostDoComment(this);

					UIView.Animate(0.2 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
						{
							nfloat n_h = h-12 ;
							borderView.Frame =  new CGRect(0,0, w , n_h + 12); 
							input.Frame = new CGRect(10,6 , w-100 ,n_h) ;
							sendButton.Frame = new CGRect(528 ,12/2,75, 30);
							Frame = new CGRect(x,y , w , n_h + 12); 
						}, null );

				}
			};
		}

	}


	public class OutputPostView : UIView , IPost
	{
		public OutputPostView(nfloat px, nfloat py , nfloat cw )
		{
			x = px;
			y = py;
			w = cw;

			Frame = new CGRect (x, y , w, 100);
			initView ();

			//BackgroundColor = UIColor.Yellow;
		}


		#region interface methods

		nfloat currentHeight = 0 , currentWidth = 0 ;

		public nfloat GetHeight()
		{
			return currentHeight;
		}
		public nfloat GetPosition()
		{
			return currentWidth;
		}


		public event IPostDoCommentEventHandler IPostDoComment ;
		public event IPostSizeChangedEventHandler IPostSizeChanged;
		#endregion

		#region properties and functions

		public UIImage UserImage
		{
			get { return null;}
			set { userImage.Image = value;  SetNeedsDisplay (); }
		}
			
		public void SetText(string name, string post, string t )
		{
			userName.Text = name;
			time.Text = t;
			userView.Text = (name[0] + "").ToUpper();

			CGSize size1 = ((NSString)userName.Text).StringSize(userName.Font,constrainedToSize:new CGSize(commWidth,300),
				lineBreakMode:UILineBreakMode.WordWrap);
			nfloat name_width = size1.Width;
			userName.Frame = new CGRect (72,20, name_width ,20);

			time.Frame = new CGRect (72 + margin + name_width, 20, 200, 20);

			comment.Text = post;
			CGSize size = ((NSString)comment.Text).StringSize(comment.Font,constrainedToSize:new CGSize(commWidth,300),
				lineBreakMode:UILineBreakMode.WordWrap);
			var labelFrame = comment.Frame;
			comment.Lines = (int)(size.Height / comment.Font.CapHeight);
			labelFrame.Size = new CGSize(commWidth,size.Height); 
			comment.Frame = labelFrame; 
			currentHeight = size.Height + 46 + margin;
			Frame = new CGRect (x,y,w,currentHeight);


		}

		#endregion

		nfloat x, y , w , margin = 12;
		nfloat commWidth = 550;

		UIImageView userImage  ;
		UILabel userName, time , comment ; 
		UILabel userView ;

		void initView()
		{

			userView = new UILabel (new CGRect (30, 12, 30, 30))
			{
				BackgroundColor = UIColor.Gray,
				Font = UIFont.FromName("HelveticaNeue" , 32 ),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center
			};
			userView.Layer.CornerRadius = 4;
			userView.Layer.MasksToBounds = true;
			Add (userView);

			userImage = new UIImageView (new CGRect(30,12,30,30));
			userImage.Image = UIImage.FromFile ("MyImage.png");
			userImage.Layer.CornerRadius = 3;
			userImage.Layer.MasksToBounds = true ; 
			Add (userImage);


			userName = new UILabel (new CGRect(72,20 , 200 , 20)) 
			{
				Text = "Ryan Elliot",
				TextAlignment = UITextAlignment.Left,
				TextColor = UIColor.Black,
				Font = UIFont.FromName("HelveticaNeue-Bold",15)
			};
			Add (userName);

			time = new UILabel (new CGRect(72 +  210 ,20 , 200 , 20)) 
			{
				Text = "Ryan Elliot",
				TextAlignment = UITextAlignment.Left,
				TextColor = UIColor.Gray,
				Font = UIFont.FromName("HelveticaNeue-Light",14)
			};
			Add (time);


			comment = new UILabel (new CGRect(72,46 , commWidth , 100)) 
			{
				Text = "Hola",
				TextAlignment = UITextAlignment.Left,
				TextColor = UIColor.Black,
				Font = UIFont.FromName("HelveticaNeue-Light",15)
			};

			Add (comment);
		}


	}

}

