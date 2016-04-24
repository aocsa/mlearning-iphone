using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using MLearning.UnifiedTouch;


namespace YComponents
{
	public class SinglePartSlideView : UIView, ISlideView
	{
		/// <summary>
		/// type = 2
		/// </summary>
		/// <param name="pos">Position.</param>
		public SinglePartSlideView (nfloat pos) : base()
		{
			slidePos = pos;
			//initView ();

			//BackgroundColor = UIColor.Red;
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



		UIView mainView ;
		UILabel titleLabel, contentLabel ;
		nfloat contentHeight  , titleHeight, borderHeight; 

		void initView()
		{
			//content
			initResizableText ();

			mainView = new UIView (new CGRect(112,40, 800,borderHeight));
			//mainView.Layer.BorderColor = new CGColor (100, 100, 100);
			//mainView.Layer.BorderWidth = 2;
			//mainView.Layer.MasksToBounds = false;
			Add (mainView);

			//set frame
			slideHeight = mainView.Frame.Size.Height + 80;
			var frame = new CGRect(0,slidePos,YConstants.DeviceHeight, slideHeight);
			Frame = frame;


			//init controls
			mainView.Add(titleLabel);
			mainView.Add(contentLabel);

		}

		void initResizableText()
		{
			//ATT fpr HTML
			var attr = new NSAttributedStringDocumentAttributes();
			var nsError = new NSError();
			attr.DocumentType = NSDocumentType.HTML;

			titleLabel = new UILabel (new CGRect(0,0, 800, 20));
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			titleLabel.TextColor = UIColor.Black;
			titleLabel.Font = UIFont.FromName (YConstants.FontName, 32);   
			//titleLabel.Text = "Aves Tipicas"; 
			//titleLabel.Text = source.Title ;
			var myHtmlTitle = source.Title; //"<b>Hello</b> <i>Everyone</i> ÆØÅ";
			var myHtmlDataT = NSData.FromString(myHtmlTitle, NSStringEncoding.Unicode);
			titleLabel.AttributedText = new NSAttributedString(myHtmlDataT, attr, ref nsError);
			titleLabel.Font = UIFont.FromName (YConstants.FontName, 32);  

			titleHeight = YConstants.ResizeHeigthWithText(titleLabel,maxHeight:960f); 

			contentLabel = new UILabel (new CGRect(0,0+titleHeight+30, 800, 200));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = UIColor.Gray;
			contentLabel.Font = UIFont.FromName (YConstants.FontName, 24);   
			//contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			//contentLabel.Text =  source.Paragraph ;

			//Next text from HTML

			//var myHtmlText = source.Paragraph; //"<b>Hello</b> <i>Everyone</i>";
			//contentLabel.AttributedText =  new  NSAttributedString(myHtmlText, attr, ref nsError);


			var myHtmlText = source.Paragraph; //"<b>Hello</b> <i>Everyone</i> ÆØÅ";
			var myHtmlData = NSData.FromString(myHtmlText, NSStringEncoding.Unicode);
			contentLabel.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
			contentLabel.Font = UIFont.FromName (YConstants.FontName, 24);  	

			contentHeight = YConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 

			borderHeight = titleHeight + contentHeight;
		}
	}
}

