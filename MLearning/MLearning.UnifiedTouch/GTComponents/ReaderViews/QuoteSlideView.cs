using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class QuoteSlideView : UIView,SlideInterface
	{
		public QuoteSlideView (nfloat pos) : base()
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


		UIView mainView , barView;
		UILabel cornerLabel, contentLabel , authorLabel;
		nfloat contentHeight  , authorHeight, borderHeight; 

		void initView()
		{
			//content
			initResizableText ();

			mainView = new UIView (new CGRect(15,10,290,borderHeight)); 
			Add (mainView);

			//set frame
			var frame = new CGRect(0,slidePos,320, mainView.Frame.Size.Height +20);
			Frame = frame;
			slideHeight = mainView.Frame.Size.Height + 20;

			//init controls
			mainView.Add(authorLabel);
			mainView.Add(contentLabel);

			//other
			cornerLabel = new UILabel(new CGRect( 0,0,16,14)){Text = "\""};
			cornerLabel.Font = UIFont.FromName (ReaderConstants.FontName,20);
			mainView.Add (cornerLabel);

			barView = new UIView(new CGRect(4,18,3,borderHeight-18));
			barView.BackgroundColor = UIColor.Purple;
			mainView.Add (barView);

		}


		void initResizableText()
		{ 

			contentLabel = new UILabel (new CGRect(20,0, 260, 100));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = UIColor.Purple;
			contentLabel.Font = UIFont.FromName (ReaderConstants.FontName, 16);   
			contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			contentHeight = ReaderConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 

			authorLabel = new UILabel (new CGRect(20,contentHeight + 12, 200 , 20));
			authorLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			authorLabel.TextColor = UIColor.Gray;
			authorLabel.Font = UIFont.FromName (ReaderConstants.FontName, 12);   
			authorLabel.Text = "Author de la frase"; 
			authorHeight = ReaderConstants.ResizeHeigthWithText(authorLabel,maxHeight:960f); 

			borderHeight = authorHeight + contentHeight + 12;
		}

	}
}

