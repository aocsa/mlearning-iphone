using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class SingleTextSlideView : UIView,SlideInterface
	{
		public SingleTextSlideView (nfloat pos) : base()
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

			UIView mainView ;
			UILabel contentLabel ;
			nfloat   contentHeight; 

			void initView()
			{
			//content
			initResizableText (); 

			//set frame
			var frame = new CGRect(0,slidePos,320, contentHeight + 40);
			Frame = frame;

			//init controls 
			Add(contentLabel);

		}

		void initResizableText()
		{ 

			contentLabel = new UILabel (new CGRect(20,20, 280, 100));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = UIColor.Gray;
			contentLabel.Font = UIFont.FromName (ReaderConstants.FontName, 16);   
			contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			contentHeight = ReaderConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 
			slideHeight = contentHeight + 40;
		}
	}
}

