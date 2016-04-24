using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public delegate void PageIndexSelectedEventHandler(object sender, int id);

	public class PageIndexView :UIView
	{
		
		public  event PageIndexSelectedEventHandler PageIndexSelected;
		public PageIndexView (nfloat pos) : base ()
		{  
			
			initTopView ();
			initLabel ();
			initBottonView ();  

			ElementHeight = topHeight + centerHeight + bottonHeight;
			var frame = new CGRect (0,pos,320,ElementHeight);
			Frame = frame;
			setTapp ();
		}

		UITapGestureRecognizer tapGesture = null;

		public nfloat ElementHeight {
			get;
			set;
		}

		int _index ;
		public int Index {
			get{ return _index;}
			set{ _index = value;
				indexLabel.Text = "" + (_index+1);
			}
		}

		string fontName = "HelveticaNeue";
		UIView topView, bottonView ;
		nfloat topHeight=32, centerHeight=20, bottonHeight = 42 ;

		UILabel indexLabel, titleLabel ;
		UIColor itemColor = UIColor.Purple ;

		void initTopView()
		{
			topView = new UIView (new CGRect(0,0,320,32));
			//topView.BackgroundColor = UIColor.Purple;
			Add (topView);
			//index label
			indexLabel = new UILabel (new CGRect(8,10,20,20)){Text = "4"};
			indexLabel.BackgroundColor = itemColor;
			indexLabel.Layer.CornerRadius = 10;
			indexLabel.TextColor = UIColor.White;
			indexLabel.TextAlignment = UITextAlignment.Center;
			indexLabel.Font = UIFont.FromName (fontName, 11);
			indexLabel.Layer.MasksToBounds = true;
			topView.Add (indexLabel);
			//tileLabel
			titleLabel = new UILabel(new CGRect(32,12,250,14)){Text = "Orquideas"};  
			titleLabel.TextColor = itemColor; 
			titleLabel.Font = UIFont.FromName (fontName, 12);
			topView.Add (titleLabel);
		}


		void setTapp()
		{
			tapGesture = new UITapGestureRecognizer(  () => {  
				if(PageIndexSelected!=null)
					PageIndexSelected(this,Index);
			});
			tapGesture.NumberOfTapsRequired = 1;
			AddGestureRecognizer (tapGesture);
		}


		public string ContentText {
			get{ return "";}
			set{ textLabel.Text = value;}
		}

		public string TitleText {
			get{ return "";}
			set{ titleLabel.Text = value;}
		}

		public UIColor ItemColor {
			get{ return null;}
			set{
				itemColor = value;
				indexLabel.BackgroundColor = itemColor;
				titleLabel.TextColor = itemColor; 
			}
		}

		UILabel textLabel ;
		void initLabel()
		{
			textLabel = new UILabel (new CGRect(32,topHeight, 270, 200));
			textLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			textLabel.TextColor = UIColor.Gray;
			textLabel.Font = UIFont.FromName (fontName, 16); 
			Add (textLabel);

			textLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			centerHeight = ReaderConstants.ResizeHeigthWithText(textLabel,maxHeight:960f); 
		}


		UILabel authorLabel , likeLabel, shareLabel, commentLabel;

		void initBottonView()
		{
			bottonView = new UIView (new CGRect(0,topHeight+centerHeight, 320,bottonHeight));
			//bottonView.BackgroundColor = UIColor.Purple;
			Add (bottonView);

			authorLabel = new UILabel (new CGRect (32,4,150,12));
			authorLabel.TextColor = UIColor.Gray;
			authorLabel.Font = UIFont.FromName (fontName, 9);
			authorLabel.Text = "Author : Jose Herrera";
			bottonView.Add (authorLabel);


			var likeImg = new UIImageView (new CGRect(32,20,16,16)){ContentMode = UIViewContentMode.ScaleToFill};
			likeImg.Image = UIImage.FromFile ("assets/003settings.png");
			bottonView.Add (likeImg);
			var shareImg = new UIImageView (new CGRect(72,20,16,16)){ContentMode = UIViewContentMode.ScaleToFill};
			shareImg.Image = UIImage.FromFile ("assets/003settings.png");
			bottonView.Add (shareImg);
			var comImg = new UIImageView (new CGRect(112,20,16,16)){ContentMode = UIViewContentMode.ScaleToFill};
			comImg.Image = UIImage.FromFile ("assets/003settings.png");
			bottonView.Add (comImg);

			likeLabel = new UILabel (new CGRect(50,22,16,12)){Text="10"};
			likeLabel.TextColor = UIColor.Gray;
			likeLabel.Font = UIFont.FromName (fontName, 9);
			bottonView.Add (likeLabel);
			shareLabel = new UILabel (new CGRect(90,22,16,12)){Text="10"};
			shareLabel.TextColor = UIColor.Gray;
			shareLabel.Font = UIFont.FromName (fontName, 9);
			bottonView.Add (shareLabel);
			commentLabel = new UILabel (new CGRect(130,22,16,12)){Text="10"};
			commentLabel.TextColor = UIColor.Gray;
			commentLabel.Font = UIFont.FromName (fontName, 9);
			bottonView.Add (commentLabel);

			ElementHeight = topHeight + centerHeight + bottonHeight;
 
		}



		void ResizeHeigthWithText(UILabel label,float maxHeight = 960f) 
		{
			nfloat width =  label.Frame.Width;  
			CGSize size = ((NSString)label.Text).StringSize(label.Font,constrainedToSize:new CGSize(width,maxHeight),
				lineBreakMode:UILineBreakMode.WordWrap);
			var labelFrame = label.Frame;
			label.Lines = (int)(size.Height / label.Font.CapHeight);
			labelFrame.Size = new CGSize(width,size.Height);
			centerHeight = size.Height;
			label.Frame = labelFrame;
		}

 
	}
}

