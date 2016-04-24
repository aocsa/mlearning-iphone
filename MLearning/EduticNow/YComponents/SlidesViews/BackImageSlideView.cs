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
	/// Type 6
	/// </summary>
	public class BackImageSlideView : UIView, ISlideView
	{
		
		public BackImageSlideView (nfloat pos) : base()
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

		UIView mainView ;
		UILabel titleLabel, contentLabel ;
		nfloat contentHeight  , titleHeight, borderHeight; 
		UIImageView backImage ;

		void initView()
		{
			//content
			initResizableText ();

			backImage = new UIImageView (new CGRect(112,40,800,borderHeight+ 2*32)); 
			backImage.ContentMode = UIViewContentMode.ScaleAspectFill;
			backImage.Layer.MasksToBounds = true;
			//backImage.Image = UIImage.FromFile ("MyImage.png");

			YConstants.DownloadImageAsync (source.ImageUrl).ContinueWith ((task) => InvokeOnMainThread (() => {
				try { backImage.Image = task.Result; }
				catch{ }
			}));


			Add (backImage);
			mainView = new UIView (new CGRect(112 + 32,40 + 32 ,800 -  2*32,borderHeight)); //32->separacion 
			mainView.BackgroundColor = UIColor.FromRGBA(0,0,0,75);
			Add (mainView); 

			//BackgroundColor = UIColor.Red;

			//set frame
			slideHeight = mainView.Frame.Size.Height + 80 + 2*32;
			var frame = new CGRect(0,slidePos,YConstants.DeviceWidht, slideHeight);
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


			titleLabel = new UILabel (new CGRect(30,30, 676 , 20));
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			titleLabel.TextColor = UIColor.White;
			titleLabel.Font = UIFont.FromName (YConstants.FontName, 32);   
			//titleLabel.Text = "Aves Tipicas"; 
			//titleLabel.Text = source.Title ;
			var myHtmlTitle = source.Title; //"<b>Hello</b> <i>Everyone</i> ÆØÅ";
			var myHtmlDataT = NSData.FromString(myHtmlTitle, NSStringEncoding.Unicode);
			titleLabel.AttributedText = new NSAttributedString(myHtmlDataT, attr, ref nsError);
			titleLabel.Font = UIFont.FromName (YConstants.FontName, 32);  
			titleHeight = YConstants.ResizeHeigthWithText(titleLabel,maxHeight:960f); 

			contentLabel = new UILabel (new CGRect(30,30+titleHeight+28, 676, 200));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = UIColor.White;
			contentLabel.Font = UIFont.FromName (YConstants.FontName, 24);   
			//contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			//contentLabel.Text = source.Paragraph ;
			var myHtmlText = source.Paragraph; //"<b>Hello</b> <i>Everyone</i> ÆØÅ";
			var myHtmlData = NSData.FromString(myHtmlText, NSStringEncoding.Unicode);
			contentLabel.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
			contentLabel.Font = UIFont.FromName (YConstants.FontName, 24);

			contentHeight = YConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 

			borderHeight = titleHeight + contentHeight + 80;
		}
	}
}

