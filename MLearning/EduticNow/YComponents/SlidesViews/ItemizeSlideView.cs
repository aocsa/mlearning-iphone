using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using MLearning.UnifiedTouch;

namespace YComponents
{
	/// <summary>
	/// Itemize slide view.
	/// type = 3
	/// </summary>
	public class ItemizeSlideView : UIView,ISlideView
	{
		public ItemizeSlideView (nfloat pos) : base()
		{
			slidePos = pos;
			//initView ();
		}

		nfloat slideHeight = 0 ;
		nfloat slidePos =0 ;


		#region ISlideView implementation

		public nfloat GetHeight ()
		{
			return slideHeight;
		}

		public nfloat GetPosition ()
		{
			return slidePos;
		}

		#endregion


		LOSlideSource source ;
		public LOSlideSource SlideSource
		{
			get { return source;}
			set 
			{
				source = value;
				initView ();
			}
		}


		string fontName = "HelveticaNeue" ;
		UIView mainView ;
		UILabel titleLabel ;
		nfloat contentHeight=0 ,titleHeight, borderHeight , separation = 18; 

		void initView()
		{
			//content
			initResizableText ();

			mainView = new UIView (new CGRect(112,40,800,borderHeight ));
			//mainView.Layer.BorderColor = new CGColor (100, 100, 100);
			mainView.Layer.BorderWidth = 1;
			//mainView.Layer.MasksToBounds = false;
			Add (mainView);

			//set frame
			slideHeight = mainView.Frame.Size.Height  + 80;
			var frame = new CGRect(0,slidePos,YConstants.DeviceWidht, slideHeight);
			Frame = frame;


			//init controls
			mainView.Add(titleLabel);
			for (int i = 0; i < elements.Count; i++) {
				mainView.Add (elements[i]);
			}

			//BackgroundColor = UIColor.Yellow;
		}

		List<ItemizeElement> elements = new List<ItemizeElement>();
		void initResizableText()
		{
			titleLabel = new UILabel (new CGRect(46,20, 800, 32));
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			titleLabel.TextColor = UIColor.Purple;
			titleLabel.Font = UIFont.FromName (fontName, 36);   
			//titleLabel.Text = "Aves Tipicas"; 
			titleLabel.Text = source.Title;
			titleHeight = YConstants.ResizeHeigthWithText(titleLabel,maxHeight:960f); 
 
			contentHeight = 82;
			for (int i = 0; i < source.Itemize.Count; i++) {
				ItemizeElement el = new ItemizeElement ( contentHeight);
				el.BackColor = source.Color;
				el.Content = source.Itemize [i].Text;
				contentHeight += el.GetHeight();
				elements.Add (el);
			}

			borderHeight = contentHeight + separation;
		}
	}




	public class ItemizeElement : UIView
	{
		public ItemizeElement(nfloat pos) : base()
		{
			position = pos;
			//initElement ();
		}

		string fontName = "HelveticaNeue" ;

		public nfloat GetPosition()
		{
			return position;
		}

		nfloat cheight = 0 ;
		public nfloat GetHeight()
		{
			return cheight;
		}

		string content = "" ;
		public string Content
		{
			get { return ""; }
			set 
			{
				content = value;
				initElement ();
			}
		}

		public UIColor BackColor = UIColor.Purple; 
		UIView circleView;
		UILabel textLabel ;

		nfloat textWidth = 680, textHeight, position ;

		void initElement()
		{
			circleView = new UIView (new CGRect(0,10,10,10)){BackgroundColor = BackColor};//posy=10
			circleView.Layer.CornerRadius = 5;
			circleView.Layer.MasksToBounds = true;
			Add (circleView);

			textLabel = new UILabel (new CGRect(24,0, textWidth, 18));
			textLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			textLabel.TextColor = UIColor.Gray;
			textLabel.Font = UIFont.FromName (fontName, 24);   
			//textLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. End m ipsum dolor sit amet, consectetur adipiscing elit. End ";
			textLabel.Text = content ;
			textHeight = YConstants.ResizeHeigthWithText(textLabel,maxHeight:960f);
			Add (textLabel);


			cheight = textHeight + 12;
			var frame = new CGRect (30, position, 70, textHeight);
			Frame = frame;

			//BackgroundColor = UIColor.Red;
		}

	}


}

