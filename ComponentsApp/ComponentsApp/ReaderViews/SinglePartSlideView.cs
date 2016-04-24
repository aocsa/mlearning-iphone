using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class SinglePartSlideView : UIView,SlideInterface
	{
		public SinglePartSlideView (nfloat pos) : base()
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
		UILabel titleLabel, contentLabel ;
		nfloat contentHeight  , titleHeight, borderHeight; 

		void initView()
		{
			//content
			initResizableText ();

			mainView = new UIView (new CGRect(15,10,290,borderHeight));
			//mainView.Layer.BorderColor = new CGColor (100, 100, 100);
			mainView.Layer.BorderWidth = 2;
			//mainView.Layer.MasksToBounds = false;
			Add (mainView);

			//set frame
			var frame = new CGRect(0,slidePos,320, mainView.Frame.Size.Height +20);
			Frame = frame;
			slideHeight = mainView.Frame.Size.Height + 20;

			//init controls
			mainView.Add(titleLabel);
			mainView.Add(contentLabel);
  
		}

		void initResizableText()
		{
			titleLabel = new UILabel (new CGRect(20,20, 250, 20));
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			titleLabel.TextColor = UIColor.Purple;
			titleLabel.Font = UIFont.FromName (ReaderConstants.FontName, 22);   
			titleLabel.Text = "Aves Tipicas"; 
			titleHeight = ReaderConstants.ResizeHeigthWithText(titleLabel,maxHeight:960f); 

			contentLabel = new UILabel (new CGRect(20,20+titleHeight+16, 250, 200));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = UIColor.Gray;
			contentLabel.Font = UIFont.FromName (ReaderConstants.FontName, 16);   
			contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			contentHeight = ReaderConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 

			borderHeight = titleHeight + contentHeight + 56;
		}
	}
}

