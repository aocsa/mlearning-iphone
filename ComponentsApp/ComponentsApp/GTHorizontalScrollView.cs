using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace ComponentsApp
{
	public class GTHorizontalScrollView : UIView
	{
		public GTHorizontalScrollView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			initScroll ();
		}

		public void SetToIndex(int id)
		{
			_currentOffSet = id * _width;
			_scroll.SetContentOffset (new CGPoint (_currentOffSet,0),false);
		}



		nfloat _width = 320 , _height = 568 ;
		UIScrollView _scroll ;
		int _actualIndex = 0, _numberofElements= 8 ;
		nfloat _currentOffSet =0 , _lastContentOffset =0;

		void initScroll()
		{
			_scroll = new UIScrollView (new CGRect (0,0,_width, _height));
			_scroll.PagingEnabled = true;
			Add (_scroll);


			_scroll.Scrolled += (sender, e) => {

				_currentOffSet = _scroll.ContentOffset.X - _lastContentOffset ;
				if(_currentOffSet>=0 && _actualIndex + 1< _numberofElements){
					ScrollList[_actualIndex+1].SetHorizontalOffset((_width -_currentOffSet)/2);
					ScrollList[_actualIndex].SetHorizontalOffset(-1*(_currentOffSet)/2);
				}
				if(_currentOffSet<0 && _actualIndex-1>=0){
					ScrollList[_actualIndex-1].SetHorizontalOffset(-1*(_width +_currentOffSet)/2);
					ScrollList[_actualIndex].SetHorizontalOffset(-1*(_currentOffSet)/2);	
				}
			};

			_scroll.DraggingStarted += (sender, e) => {
				_lastContentOffset = _scroll.ContentOffset.X;
				_currentOffSet =0 ;
				var pos =  (int)Math.Floor((_scroll.ContentOffset.X + _width/2 )/_width);
				if(pos>=0 && pos < _numberofElements)
					_actualIndex = pos ;
			};

			_scroll.DraggingEnded+= (sender, e) => {  
			};

			//
			//tmp_init();
		}


		List<GTVerticalScrollView> _scrollList = new List<GTVerticalScrollView>() ;

		public List<GTVerticalScrollView> ScrollList = new List<GTVerticalScrollView>() ;



		public void AddVerticalContainer(VerticalContainer vc, int idx)
		{
			GTVerticalScrollView vscroll = new GTVerticalScrollView (new CGRect(_width * idx , 0, _width,_height));
			vscroll.Index = idx;
			if(idx%2 ==0) vscroll.MainImage = UIImage.FromFile("assets/imgtest.jpg");
			else vscroll.MainImage = UIImage.FromFile("assets/luna.jpg");
			//set content
			vscroll.SetContent (vc, vc.getHeight());
			_scroll.Add (vscroll);
			_scroll.ContentSize = new CGSize ((idx+1)*_width ,_height);

			ScrollList.Add (vscroll);
		}


		public void AddVerticalIndexContainer(VerticalIndexContainer vc, int idx)
		{
			GTVerticalScrollView vscroll = new GTVerticalScrollView (new CGRect(_width * idx , 0, _width,_height));
			vscroll.Index = idx;
			if(idx%2 ==0) vscroll.MainImage = UIImage.FromFile("assets/imgtest.jpg");
			else vscroll.MainImage = UIImage.FromFile("assets/luna.jpg");
			//set content
			vscroll.SetContent (vc, vc.getHeight());
			_scroll.Add (vscroll);
			_scroll.ContentSize = new CGSize ((idx+1)*_width ,_height);

			ScrollList.Add (vscroll);
		}

		void tmp_init()
		{
			for (int i = 0; i < _numberofElements; i++) {
				GTVerticalScrollView vscroll = new GTVerticalScrollView (new CGRect(_width * i , 0, _width,_height));
				vscroll.Index = i;
				if(i%2 ==0) vscroll.MainImage = UIImage.FromFile("assets/imgtest.jpg");
				else vscroll.MainImage = UIImage.FromFile("assets/luna.jpg");
				VerticalContainer vc = new VerticalContainer ();
				//VerticalIndexContainer vc = new VerticalIndexContainer();
				vscroll.SetContent (vc, vc.getHeight());
				_scroll.Add (vscroll);
				_scroll.ContentSize = new CGSize ((i+1)*_width ,_height);

				//if (i > 0)
				///	vscroll.SetHorizontalOffset (_width/2);
				vscroll.GTVerticalScrollSelected+=(s,id)=>{ 
				};
				vscroll.GTVerticalScrollReleased+=(s,id)=>{};
				vscroll.GTVerticalScrollTapped+=(s,id)=>{};

				_scrollList.Add (vscroll);
			}
		}
	}
}

