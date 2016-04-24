using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using MLearning.UnifiedTouch;


namespace YComponents
{
	public class QuoteSlideView :  UIView,ISlideView
	{
	
		/// <summary>
		/// Type = 5
		/// </summary>
		/// <param name="pos">Position.</param>
		public QuoteSlideView (nfloat pos) : base()
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
		UIView mainView , barView;
		UILabel cornerLabel, contentLabel , authorLabel;
		nfloat contentHeight  , authorHeight, borderHeight; 

		void initView()
		{
			//content
			initResizableText ();

			mainView = new UIView (new CGRect(112,40,800,borderHeight)); 
			Add (mainView);

			//set frame
			slideHeight = mainView.Frame.Size.Height + 40*2;//40 up-down
			var frame = new CGRect(0,slidePos,YConstants.DeviceWidht , slideHeight);
			Frame = frame;

			//BackgroundColor = UIColor.Yellow;

			//init controls
			mainView.Add(authorLabel);
			mainView.Add(contentLabel);

			//other
			cornerLabel = new UILabel(new CGRect( 0,0,16,14)){Text = "\""};
			cornerLabel.Font = UIFont.FromName (fontName,20);
			mainView.Add (cornerLabel);

			barView = new UIView(new CGRect(4,18,3,borderHeight-18));
			barView.BackgroundColor = UIColor.Purple;
			mainView.Add (barView);

		}


		void initResizableText()
		{ 
			//ATT fpr HTML
			var attr = new NSAttributedStringDocumentAttributes();
			var nsError = new NSError();
			attr.DocumentType = NSDocumentType.HTML;


			contentLabel = new UILabel (new CGRect(24,0, 720, 100));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = source.Color; // UIColor.Purple;
			contentLabel.Font = UIFont.FromName (fontName, 24);   
			//contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			//contentLabel.Text = source.Paragraph ;

			var myHtmlText = source.Paragraph; //"<b>Hello</b> <i>Everyone</i> ÆØÅ";
			var myHtmlData = NSData.FromString(myHtmlText, NSStringEncoding.Unicode);
			contentLabel.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
			contentLabel.Font = UIFont.FromName (fontName, 24);

			contentHeight = YConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 

			authorLabel = new UILabel (new CGRect(24,contentHeight + 12, 500 , 24));
			authorLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			authorLabel.TextColor = UIColor.Gray;
			authorLabel.Font = UIFont.FromName (fontName, 16);   
			//authorLabel.Text = "Author de la frase"; 
			authorLabel.Text = source.Author ;
			authorHeight = YConstants.ResizeHeigthWithText(authorLabel,maxHeight:960f); 

			borderHeight = authorHeight + contentHeight + 12;
		}


	}
}

