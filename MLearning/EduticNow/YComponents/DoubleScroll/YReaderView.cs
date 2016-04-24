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
	public class YReaderView : UIView
	{
		public YReaderView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
		}

		List<LOPageSource> pages ;
		public List<LOPageSource> PagesList
		{
			get { return pages; }
			set 
			{
				pages = value;  
				initView ();
			}
		}

		public UIButton BackButton;

		UIScrollView scroll ;
		List<YPageReaderView> pagesList = new List<YPageReaderView>();
		void initView()
		{

			BackgroundColor = UIColor.White;

			scroll = new UIScrollView (new CGRect(0,0,1024,768));
			scroll.PagingEnabled = true;
			Add (scroll);

			for (int i = 0; i < pages.Count; i++) {
				YPageReaderView page = new YPageReaderView (new CGRect (1024*i,0,1024,768));
				page.PageIndex = i;
				page.PageSource = pages [i];
				scroll.Add (page);
				pagesList.Add (page); 
			}
			scroll.ContentSize = new CGSize (1024 * pages.Count, 768);

			scroll.Scrolled+= Scroll_Scrolled;
			scroll.DraggingStarted+= Scroll_DraggingStarted;

			BackButton = new UIButton (UIButtonType.Custom);
			BackButton.Frame = new CGRect (56,40,40,40);
			BackButton.SetImage (UIImage.FromFile ("efiles/reader/back.png"), UIControlState.Normal);
			Add (BackButton);
		}


		nfloat _currentOffSet =0,  _lastContentOffset =0;
		void Scroll_Scrolled (object sender, EventArgs e)
		{
			_currentOffSet =  scroll.ContentOffset.X - _lastContentOffset ;
			if(_currentOffSet>=0 && currentPage + 1< pagesList.Count){
				pagesList[currentPage+1].SetHorizontalOffset((1024 -_currentOffSet)/2);
				pagesList[currentPage].SetHorizontalOffset(-1*(_currentOffSet)/2);
			}
			if(_currentOffSet<0 && currentPage-1>=0){
				pagesList[currentPage-1].SetHorizontalOffset(-1*(1024 +_currentOffSet)/2);
				pagesList[currentPage].SetHorizontalOffset(-1*(_currentOffSet)/2);	
			}
		}


		void Scroll_DraggingStarted (object sender, EventArgs e)
		{ 
			_lastContentOffset = scroll.ContentOffset.X;
			_currentOffSet =0 ;
			var pos =  (int)Math.Floor((scroll.ContentOffset.X + 1024/2 )/1024);
			if(pos>=0 && pos < pagesList.Count)
				currentPage = pos ; 
		}



		//start at 0
		int currentPage  = 0;
		public int CurrentPage
		{
			get { return currentPage; }
			set { currentPage = value;  set2page (currentPage);}
		}

		void set2page(int i)
		{
			scroll.SetContentOffset ( new CGPoint (1024 * i,0),false);
			_currentOffSet = i * 1024;
		}


		public void ResetPositions()
		{
			for (int i = 0; i < pagesList.Count; i++) {
				pagesList [i].SetHorizontalOffset (0);
			}	
		}

	}
}

