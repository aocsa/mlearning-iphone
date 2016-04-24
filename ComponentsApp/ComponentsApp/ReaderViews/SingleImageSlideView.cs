using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;


namespace ComponentsApp
{
	public class SingleImageSlideView:UIView,SlideInterface
	{
		public SingleImageSlideView (nfloat pos) : base()
		{
			slidePos = pos;
			initView ();
			//BackgroundColor = UIColor.Yellow; 
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
		UILabel titleLabel, subtitleLabel, authorLabel,contentLabel ;
		nfloat contentHeight = 0 ;
		UIImageView image ;

		void initView()
		{
			//content
			initResizableText ();

			mainView = new UIView (new CGRect(15,10,290, 126+  contentHeight+20));
			//mainView.Layer.BorderColor = new CGColor (48, 48, 48);
			mainView.Layer.BorderWidth = 2;
			//mainView.Layer.MasksToBounds = false;
			Add (mainView);

			//set frame
			slideHeight = mainView.Frame.Size.Height +20;
			var frame = new CGRect(0,slidePos,320, slideHeight);
			Frame = frame;

			//init controls
			mainView.Add(contentLabel);

			titleLabel = ReaderConstants.GetNewTextLabel(20,20,256,22,22,UIColor.Black,1);
			titleLabel.Text = "Aves Tipicas";
			mainView.Add (titleLabel);

			subtitleLabel = ReaderConstants.GetNewTextLabel (94, 46, 176, 48,18, UIColor.Purple, 2);
			subtitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			subtitleLabel.Text = "Diferentes tipos de Aves en el peru";
			mainView.Add (subtitleLabel);

			authorLabel = ReaderConstants.GetNewTextLabel (94, 96, 176, 14, 12, UIColor.Gray, 1);
			authorLabel.Text = "Autor del Articulo";
			mainView.Add (authorLabel);

			//image
			image = new UIImageView(new CGRect(20,46,62,62)){ContentMode = UIViewContentMode.ScaleToFill};
			image.Image = UIImage.FromFile ("assets/imgtest.jpg");
			mainView.Add (image);

		}

		void initResizableText()
		{
			contentLabel = new UILabel (new CGRect(20,126, 250, 200));
			contentLabel.LineBreakMode = UILineBreakMode.WordWrap; 
			contentLabel.TextColor = UIColor.Gray;
			contentLabel.Font = UIFont.FromName (ReaderConstants.FontName, 16); 
			//mainView.Add (contentLabel);

			contentLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. quis nostrud exercitation END";
			contentHeight = ReaderConstants.ResizeHeigthWithText(contentLabel,maxHeight:960f); 
		}

	}
}

