using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class ItemizeSlideView: UIView,SlideInterface
	{
		public ItemizeSlideView (nfloat pos) : base()
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
		UILabel titleLabel ;
		nfloat contentHeight=0  , titleHeight, borderHeight , separation = 18; 

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
			for (int i = 0; i < elements.Count; i++) {
				mainView.Add (elements[i]);
			}
		}

		List<ItemizeElement> elements = new List<ItemizeElement>();
		void initResizableText()
		{
			titleLabel = new UILabel (new CGRect(20,20, 250, 20));
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			titleLabel.TextColor = UIColor.Purple;
			titleLabel.Font = UIFont.FromName (ReaderConstants.FontName, 22);   
			titleLabel.Text = "Aves Tipicas"; 
			titleHeight = ReaderConstants.ResizeHeigthWithText(titleLabel,maxHeight:960f); 
  
			nfloat epositon = 54;
			for (int i = 0; i < 4; i++) {
				ItemizeElement el = new ItemizeElement (epositon + contentHeight);
				contentHeight = el.GetPosition ();
				elements.Add (el);
			}

			borderHeight = epositon + contentHeight + separation;
		}
	}
  

	public class ItemizeElement : UIView
	{
		public ItemizeElement(nfloat pos) : base()
		{
			position = pos;
			initElement ();
		}

		public nfloat GetPosition()
		{
			return position;
		}

		UIColor BackColor = UIColor.Purple; 
		UIView circleView;
		UILabel textLabel ;

		nfloat textWidth = 230, textHeight, position ;

		void initElement()
		{
			circleView = new UIView (new CGRect(0,4,8,8)){BackgroundColor = BackColor};
			circleView.Layer.CornerRadius = 4;
			circleView.Layer.MasksToBounds = true;
			Add (circleView);

			textLabel = new UILabel (new CGRect(18,0, textWidth, 10));
			textLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			textLabel.TextColor = UIColor.Gray;
			textLabel.Font = UIFont.FromName (ReaderConstants.FontName, 16);   
			textLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. End";
			textHeight = ReaderConstants.ResizeHeigthWithText(textLabel,maxHeight:960f);
			Add (textLabel);

			var frame = new CGRect (22, position, 250, textHeight);
			Frame = frame;
		}

	}


}

