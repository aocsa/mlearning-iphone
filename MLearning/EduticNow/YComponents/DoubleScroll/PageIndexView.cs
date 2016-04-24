using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;


namespace YComponents
{

	public delegate void PageIndexTappedEventHandler(object sender , int idx) ;

	public class PageIndexView : UIView
	{
		public PageIndexView (nfloat x, nfloat y )
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			xPos = x;
			yPos = y;

			Frame = new CGRect (xPos,yPos,_width,_height);
 
			//init the controls 
			initcontrols() ;
		}

 
		#region Properties

		nfloat _height  =  768 , _width = 390 ,_deltax = 150;
		nfloat xPos = 0 , yPos = 0 ;
		nfloat skew = (nfloat)0.19 ;

		public event SlideButtonTappedEventHandler SlideButtonTapped;
		public event PageIndexTappedEventHandler PageIndexTapped ; 

		string _descriptionText = ""; //LOL Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
		public string DescriptionText
		{
			get { return _descriptionText;}
			set 
			{
				_descriptionText = value;
				addDescription ();
			}
		}

		int _index = 0 ;
		public int Index
		{
			get { return _index;}
			set { _index = value;}
		}

		int _number = 0 ;
		public int Number
		{
			get { return _number;}
			set { _number = value;}
		}

		int _section = 0 ;
		public int Section {
			get{ return _section;}
			set{ _section = value;}
		}

		#endregion

		UIScrollView slideScroll ;
		UIView colorLine ;
		void initcontrols()
		{
			BackgroundColor = UIColor.White;  

			skew = _deltax / _height;
			Transform = CGAffineTransformMakeSkew (skew, _height); 

		

			//Initialization of he scroll
			slideScroll = new UIScrollView (new CGRect (60 , 560 , 276, 38)); 
			slideScroll.ShowsHorizontalScrollIndicator = false;
			slideScroll.Layer.ZPosition = 100;
			slideScroll.Transform =   CGAffineTransformMakeSkew (-1 * skew, 40);
			Add (slideScroll);

			//color line
			colorLine =  new UIView(new CGRect(0,0,1,_height));
			Add (colorLine);

			///Gesture to tap -> navigate into de item to other view 
			UITapGestureRecognizer tapped =  new UITapGestureRecognizer(indexTapped){NumberOfTapsRequired =  1 } ;
			AddGestureRecognizer (tapped);


			//temporal -> static => must change
			//addDescription() ;
			//setNumber ("9",UIColor.Purple);
			//setName (titleText, UIColor.Purple); 
		}


		/// <summary>
		/// Indexs the tapped.
		/// For navigation into the current item tapped
		/// </summary>
		private void indexTapped()
		{
			if (PageIndexTapped != null)
				PageIndexTapped (this, _index);
		}



		void addDescription()
		{ 
			//Text of the description
			nfloat deltax = 55;
			var descriptionpath = new CGPath ();
			descriptionpath.AddLines (new CGPoint[]{ 
				new CGPoint (0, 280),
				new CGPoint (260, 280),
				new CGPoint (260+deltax , 0),
				new CGPoint (deltax, 0)
			}); 
			descriptionpath.CloseSubpath ();
 

			nfloat descriptionHeight = 280; 
			CGRect frame = new CGRect (60, 256, 310, 280);
			//eview2.Bounds
			var descriptionview = new UIFormatedText (){Frame = frame, Text = _descriptionText, Foreground = UIColor.Black, 
				FontSize = 28, FontName = "HelveticaNeue-Light", TextHeight = descriptionHeight ,  
				TextAligment= CoreText.CTTextAlignment.Left, Path = descriptionpath };
 
			descriptionview.Transform = CGAffineTransformMakeSkew (-1 * skew, descriptionHeight);
			descriptionview.BackgroundColor = UIColor.Clear; 
			Add (descriptionview);

		}


		UIView circleView ;
		UILabel numberLabel ; 
		public void setNumber(string number, UIColor color)
		{ 

			//make the view circle
			circleView = new UIView (new CGRect(60,164,50,50));
			circleView.Layer.CornerRadius = 25;
			circleView.Layer.BorderWidth = 1;
			circleView.Layer.BorderColor = color.CGColor;
			circleView.BackgroundColor = color;
			circleView.Transform  = CGAffineTransformMakeSkew (-1 * skew, 50);

			//set the  number 
			numberLabel =  new UILabel(new CGRect(5,12,40,26)){
				Font = UIFont.FromName("HelveticaNeue",30),
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White,
				Text = number
			};
			circleView.Add (numberLabel);

			Add (circleView);


			//Colorline
			colorLine.BackgroundColor = color; 
		}


		UIView titleView ;
		UILabel titleLabel ;
		string titleText = "";//LOL THIS IS  A TITLE " ;
		public void setName(string name, UIColor foreground)
		{
			//view for dinamic title
			titleView = new UIView (new CGRect(120,164,200,50)) ;
			titleView.Transform = CGAffineTransformMakeSkew (-1 * skew, 44);
			Add (titleView);

			//text  title
			titleLabel = new UILabel(new CGRect(0,0,196,22)) ;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Text = name;
			titleLabel.TextColor = foreground;
			titleLabel.Font = UIFont.FromName ("HelveticaNeue",20);

			var text_height = YConstants.ResizeHeigthWithText (titleLabel, 100);
			titleLabel.Frame =  new CGRect(0, (55 - text_height) / 2, 196 , text_height ) ;

			titleView.Add (titleLabel);
		}

		/// <summary>
		/// The slides position.
		/// flag to control the positon of the buttons for nagivate into especific slide
		/// </summary>
		nfloat slidesPosition =  0 ;
		/// <summary>
		/// Adds the slide button.
		/// 
		/// </summary>
		/// <param name="button">Button.</param>
		public void addSlideButton(UISlideButton button)
		{
			slideScroll.Add (button);
			slidesPosition += 40; //36 + 4 for contentWidth  -4
			slideScroll.ContentSize =  new CGSize(slidesPosition -4 ,36 ) ;
			button.SlideButtonTapped +=	(sender, idx) => {
				if(SlideButtonTapped!=null)
					SlideButtonTapped(this,idx) ;
			};
		}


		CGAffineTransform CGAffineTransformMakeSkew(nfloat skewAmount , nfloat height)
		{
			CGAffineTransform skewTransform = CGAffineTransform.MakeIdentity();
			skewTransform = CGAffineTransform.MakeTranslation ((nfloat)skewAmount * height /2 , 0);
			skewTransform.xy = skewAmount;
			return skewTransform;
		}

 

	}
}

