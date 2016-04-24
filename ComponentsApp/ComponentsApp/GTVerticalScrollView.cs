using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace ComponentsApp
{

	public delegate void GTVerticalScrollSelectedEventHandler(object sender, int index) ;
	public delegate void GTVerticalScrollReleasedEventHandler(object sender, int index) ;
	public delegate void GTVerticalScrollTappedEventHandler(object sender, int index) ;

	public class GTVerticalScrollView :UIView
	{

		public event GTVerticalScrollSelectedEventHandler GTVerticalScrollSelected ;
		public event GTVerticalScrollReleasedEventHandler GTVerticalScrollReleased ;
		public event GTVerticalScrollTappedEventHandler GTVerticalScrollTapped ;

		public GTVerticalScrollView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			initScroll ();

			//temporal
			addtmpcontent ();
		}


		public void ResetPositions()
		{
			_scroll.SetContentOffset (new CGPoint(0,0), false);
		}
  

		nfloat _width = 320 , _height = 568 ;
		nfloat _deltah = 62, _defaultheight = 458 ;//334 ;
		UIScrollView _scroll , _backscroll;
		UIImageView _mainimage , _topimage;
		UIView _topview, _tapview ;
		nfloat _lastContentOffset ;
		bool _isvertical = true, _orientationsetted =  false ;

		private void initScroll()
		{
			_backscroll = new UIScrollView (new CGRect(0,0,_width,_height));
			_backscroll.BackgroundColor = UIColor.White;
			_backscroll.MinimumZoomScale = 1.0f;
			_backscroll.MaximumZoomScale = 10.0f;
			Add (_backscroll); 

			_mainimage = new UIImageView (new CGRect(0,0,_width, _defaultheight)){ContentMode = UIViewContentMode.ScaleAspectFill};
			_backscroll.ContentSize = new CGSize (_width, _height);
			_backscroll.ViewForZoomingInScrollView += (UIScrollView sv) => { return _mainimage; };
			_backscroll.Add (_mainimage);

			_scroll = new UIScrollView (new CGRect(0,0,_width,_height));
			_scroll.BackgroundColor = UIColor.Clear;
			_scroll.ContentSize = new CGSize (320, 1024);
			_scroll.DecelerationRate = 8;
			Add (_scroll); 

			_topview = new UIView (new CGRect(0, _defaultheight - _deltah,_width,_deltah)); 
			_scroll.Add (_topview);


			_tapview = new UIView (new CGRect (0, 0, _width, _defaultheight - _deltah));
			_scroll.Add (_tapview);
			UITapGestureRecognizer tap1 = new UITapGestureRecognizer(OnTap1	) {
				NumberOfTapsRequired = 1
			};
			_tapview.AddGestureRecognizer(tap1); 

			_scroll.Scrolled += (s, e) => {  
					
					var off_set =_scroll.ContentOffset.Y ;
					if(off_set <= 0 )
					{
					_backscroll.SetZoomScale(1+ (nfloat) Math.Abs(off_set/280) , false);
					}
					else
					{
						_backscroll.SetContentOffset(new CGPoint(0, _scroll.ContentOffset.Y / 2), false);  
					} 
			};
 
			_scroll.WillEndDragging += (s, e) =>{
				//_isvertical = false ; 
			} ;

			_scroll.DraggingStarted += (sender, e) => {
				//if(!_orientationsetted && GTVerticalScrollSelected!=null)
				if(_isvertical&& GTVerticalScrollSelected!=null)
					GTVerticalScrollSelected(this, Index);
				/*_lastContentOffset =  _scroll.ContentOffset.Y ;
				_orientationsetted = false ;
				_isvertical = false ;*/
			};

			_scroll.DraggingEnded+= (sender, e) => {
				if(GTVerticalScrollReleased!=null)
					GTVerticalScrollReleased(this, Index) ;
			};
 		}

		private void OnTap1 (UIGestureRecognizer gesture) {
			if (GTVerticalScrollTapped != null)
				GTVerticalScrollTapped (this, Index);
		}

		UIView contentView ;
		void addtmpcontent()
		{
			_mainimage.Image = UIImage.FromFile("assets/imgtest.jpg");

			var topimg = new UIImageView (new CGRect(0,0,320, 62)){ContentMode = UIViewContentMode.ScaleToFill};
			topimg.Image = UIImage.FromFile ("assets/topwhite1.png");
			_topview.Add (topimg);

			//temp background
			contentView = new UIView(new CGRect(0,_defaultheight, 320,568)){BackgroundColor=UIColor.White};
			_scroll.Add (contentView);

		}


		#region Public Functions and Properties

		public void SetContent(UIView content, nfloat height)
		{ 
			var frame = new CGRect(0,_defaultheight,320, height);
			contentView.Frame = frame; 
			contentView.Add (content); 
			_scroll.ContentSize = new CGSize (320, height + _defaultheight);
		}
		 
		public void Lock()
		{
			_isvertical = false;
		}

		public void Unlock()
		{
			_isvertical = false;
		}

		public void SetHorizontalOffset(nfloat xoffset)
		{
			_backscroll.SetContentOffset (new CGPoint (xoffset, 0), false);
		}

		public UIImage MainImage {
			get{ return null;}
			set
			{
				_mainimage.Image = value;
			}
		}



		public int Index {
			get;
			set;
		}

		#endregion
	}
}

