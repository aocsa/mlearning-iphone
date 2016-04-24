using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using System.Collections.ObjectModel;
using MLearning.Core.ViewModels;
using YComponents.YWidgets;
using System.Collections.Generic;
using MLearning.UnifiedTouch;

namespace YComponents
{
	public class YPageReaderView : UIView
	{
		public YPageReaderView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

		}


		nfloat _width = 1024 , _height = 768 ; 
		int _actualIndex = 0, _numberofElements= 8 ;
		nfloat _currentOffSet =0 , _lastContentOffset =0;


		LOPageSource source ;
		public LOPageSource PageSource
		{
			get { return source; }
			set 
			{
				source = value;
				if (source != null)
					LoadPageSource (); 
			}
		}

		public int PageIndex {
			get;
			set;
		}


		public void SetHorizontalOffset(nfloat offset)
		{
			parallax.backScroll.SetContentOffset(new CGPoint (offset, 0), false);
		}


		GTParallaxScroll parallax ;
		UIView contentView, mainView , backView ;
		nfloat position = 530 ;
		public void LoadPageSource()
		{
			parallax = new GTParallaxScroll (0,0,_width, _height , GTScrollOrientation.Vertical);
			Add (parallax);

			contentView = new UIView (new CGRect(0,0,_width, _height));

			mainView = new UIView (new CGRect(0,position,_width, _height));

			addTopView ();


			nfloat ypos = 0;

			for (int i = 0; i < source.Slides.Count; i++) {
				switch (source.Slides[i].Type) {
				case 0:
					titleLabel.TextColor = source.Slides [0].Color;
					titleLabel.Text = source.Slides [i].Title;
					numberView.BackgroundColor = source.Slides [0].Color;
					numberLabel.Text = "" + (PageIndex + 1);
					addTitle (source.Slides [i].Title, source.Slides [i].Paragraph);
					break;
				case 1:
					SingleImageSlideView slide1 = new SingleImageSlideView (ypos);
					slide1.SlideSource = source.Slides [i];
					ypos += slide1.GetHeight ();
					slide1.BackgroundColor = UIColor.White;
					mainView.Add (slide1);
					break;

				case 2:
					SinglePartSlideView slide2 = new SinglePartSlideView (ypos);
					slide2.SlideSource = source.Slides [i];
					ypos += slide2.GetHeight ();
					slide2.BackgroundColor = UIColor.White;
					mainView.Add (slide2);
					break;

				case 3:
					ItemizeSlideView slide3 = new ItemizeSlideView (ypos);
					slide3.SlideSource = source.Slides [i];
					ypos += slide3.GetHeight ();
					slide3.BackgroundColor = UIColor.White;
					mainView.Add (slide3);
					break;

				case 4:
					MultiImageSlideView slide4 = new MultiImageSlideView (ypos);
					slide4.SlideSource = source.Slides [i];
					ypos += slide4.GetHeight ();
					slide4.BackgroundColor = UIColor.White;
					mainView.Add (slide4);
					break;

				case 5:
					QuoteSlideView slide5 = new QuoteSlideView (ypos);
					slide5.SlideSource = source.Slides [i];
					ypos += slide5.GetHeight ();
					slide5.BackgroundColor = UIColor.White;
					mainView.Add (slide5);
					break;

				case 6:
					BackImageSlideView slide6 = new BackImageSlideView (ypos);
					slide6.SlideSource = source.Slides [i];
					slide6.BackgroundColor = UIColor.White;
					ypos += slide6.GetHeight ();
					mainView.Add (slide6);
					break;

				default:
					break;
				}
			}

			mainView.Frame = new CGRect(0,position,_width, ypos +50 );
			mainView.BackgroundColor = UIColor.White ;

			contentView.Frame = new CGRect(0,0,_width, position + ypos +50 );
			contentView.Add (mainView);

			backView = new UIView (new CGRect(0,0,_width, (position + ypos +50)/2));

			var backImage = new UIImageView (new CGRect (0,0, _width,_height));
			backImage.ContentMode = UIViewContentMode.ScaleAspectFill; 
			backImage.Layer.MasksToBounds = true;
			backImage.BackgroundColor = UIColor.Gray;
			backImage.Image = source.Cover;
			backView.Add (backImage);

			parallax.SetContent (contentView, position + ypos + 50);
			parallax.SetBackContent (backView, (position +ypos + 50) / 2);



			var lineView = new UIView (new CGRect(1023,0,1,768)){BackgroundColor = UIColor.Gray};
			Add (lineView);

		}


		UIView topView ;
		UIView numberView ;
		UILabel titleLabel , numberLabel;
		void addTopView()
		{
			//top
			topView = new UIView (new CGRect(0, 410, 1024, 122));
			//topView.BackgroundColor = UIColor.Red;
			contentView.Add (topView);

			var topImage = new UIImageView (new CGRect(0,0,1024, 122));
			topView.ContentMode = UIViewContentMode.ScaleToFill;
			topView.Layer.MasksToBounds = true;
			topImage.Image = UIImage.FromFile ("efiles/topwhite.png");
			topView.Add (topImage);

			numberView = new UIView (new CGRect(54,76, 40,40));
			numberView.Layer.CornerRadius = 20;
			numberView.Layer.MasksToBounds = true;
			topView.Add (numberView);

			numberLabel = new UILabel (new CGRect (0, 3, 40, 36)){ 
				Text  = "1" ,
				TextColor = UIColor.White,
				Font = UIFont.FromName("HelveticaNeue",24),
				TextAlignment = UITextAlignment.Center
			};
			numberView.Add (numberLabel);

			titleLabel = new UILabel (new CGRect (104,90,600, 24)){ 
				Text  = "FAUNA AMAZONICA" ,
				TextColor = UIColor.White,
				Font = UIFont.FromName("HelveticaNeue",22),
				TextAlignment = UITextAlignment.Left
			};
			topView.Add (titleLabel);

		}

		UIView descriptionView ;
		UILabel pageTitle , pageDescription ;
		void addTitle(string t, string st)
		{
			//ATT fpr HTML
			var attr = new NSAttributedStringDocumentAttributes();
			var nsError = new NSError();
			attr.DocumentType = NSDocumentType.HTML;

			nfloat xpos = 474, ypos = 380, delta  = 74 , pw = 486;
			//description
			pageDescription = new UILabel (new CGRect (xpos, ypos, pw, 70))
			{
				//Text = "SUBTITULOOOOO" ,//st ,
				Font = UIFont.FromName("HelveticaNeue", 24),
				TextColor = UIColor.White,
				LineBreakMode = UILineBreakMode.WordWrap ,
				TextAlignment = UITextAlignment.Right
			};
			//Adding text HTML
			var myHtmlText = st; //"<b>Hello</b> <i>Everyone</i> ÆØÅ";
			var myHtmlData = NSData.FromString(myHtmlText, NSStringEncoding.Unicode);
			pageDescription.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
			pageDescription.Font = UIFont.FromName("HelveticaNeue", 24);  	
			pageDescription.TextColor = UIColor.White;
			pageDescription.LineBreakMode = UILineBreakMode.WordWrap;
			pageDescription.TextAlignment = UITextAlignment.Right;

			var d_h = YConstants.ResizeHeigthWithText (pageDescription, 300);
			ypos = position - d_h - delta;
			pageDescription.Frame = new CGRect (xpos, ypos , pw, d_h);
			contentView.Add (pageDescription);

			//line
			ypos -= 20 ;
			var line = new UIView(new CGRect(xpos , ypos , pw, 1)) {BackgroundColor =  UIColor.White} ;
			contentView.Add (line);
			ypos -= 20 ;

			//title
			pageTitle =  new UILabel (new CGRect (xpos, ypos, pw, 70))
			{
				Text = t ,
				Font = UIFont.FromName("HelveticaNeue", 42),
				TextColor = UIColor.White,
				LineBreakMode = UILineBreakMode.WordWrap ,
				TextAlignment = UITextAlignment.Right
			};
			var t_h = YConstants.ResizeHeigthWithText (pageTitle, 200);
			ypos -= t_h;

			pageTitle.Frame = new CGRect (xpos ,ypos , pw, t_h);
			contentView.Add (pageTitle);

			 
		}

	}
}

