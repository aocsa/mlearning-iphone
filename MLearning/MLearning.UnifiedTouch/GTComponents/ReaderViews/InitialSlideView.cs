using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;


namespace ComponentsApp
{
	public class InitialSlideView :UIView,SlideInterface
	{
		public InitialSlideView (nfloat pos) : base()
		{
			slidePos = pos;
			initLabels ();
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

		string fontName = "HelveticaNeue";
		UILabel titleLabel, contentLabel ;
		UIColor titleColor = UIColor.Black, contentColor = UIColor.Black ;
		nfloat separation = 18 ;


		void initLabels()
		{
			titleLabel = new UILabel (new CGRect(32,0, 270, 200));
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			titleLabel.TextColor = titleColor;
			titleLabel.Font = UIFont.FromName (fontName, 24); 

			Add (titleLabel);

			titleLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
			var titleHeight = ReaderConstants.ResizeHeigthWithText(titleLabel,maxHeight:960f); 


			contentLabel = new UILabel (new CGRect(32,titleHeight+separation, 270, 200));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = contentColor;
			contentLabel.Font = UIFont.FromName (fontName, 16); 
			Add (contentLabel);

			contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			var contentHeight = ReaderConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 

			slideHeight = titleHeight + contentHeight +separation +10 ;
			var frame = new CGRect (0,slidePos,320,slideHeight+10);
			Frame = frame;
		}
	}
}

