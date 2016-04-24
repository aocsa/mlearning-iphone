using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Collections.ObjectModel; 

namespace MLearning.iPhone
{
	public delegate void DoOpenLOEventHandler(object sender, int lo_index);

	public class LOsScrollView : UIView
	{ 
		string fontname = "HelveticaNeue";
		nfloat CHeight = 198 , CWidth = 320 ;
		List<string> lo_names = new List<string> () { };//{"Literatura", "Humanidades", "Computacion", "Algebra", "Filosofia"};

		public event DoOpenLOEventHandler DoOpenLO ;

		public LOsScrollView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions; 

			initScroll ();
			addLOName ();
		}


		UIScrollView _scroll ;
		int lo_position = 0 ;

		private void initScroll()
		{
			_scroll = new UIScrollView (new CGRect(0,0,CWidth,CHeight));
			_scroll.MaximumZoomScale = 1.0f;
			_scroll.MinimumZoomScale = 1.0f;
			_scroll.PagingEnabled = true;
			_scroll.Scrolled += (s, e) => {
				var pos = (int)Math.Floor(_scroll.ContentOffset.X/320) ;
				if(pos!=lo_position && (pos>=0 || pos<lo_names.Count))
				{
					lo_position = pos ;
					if(lo_position<0) lo_position =0 ;
					if(lo_position >= lo_names.Count) lo_position = lo_names.Count -1 ;
					lo_label.Text = lo_names[lo_position].ToUpper();
				}
			};
			Add (_scroll);

			//initdefault elements
//			for (int i = 0; i < lo_names.Count; i++) {
//				addElement (i);
			//}
		}


		ObservableCollection<MLearning.Core.ViewModels.MainViewModel.lo_by_circle_wrapper> _learningObjectsList;
		public ObservableCollection<MLearning.Core.ViewModels.MainViewModel.lo_by_circle_wrapper> LearningOjectsList
		{
			get { return _learningObjectsList; }
			set {
				_learningObjectsList = value; 

				if(_learningObjectsList!=null) 
					addNewElements (0);

				if (_learningObjectsList == null) {
					return ;
				}
				_learningObjectsList.CollectionChanged += (sender, e) => {
					addNewElements(e.NewStartingIndex);
				};
			} 
		}


		void addNewElements(int idx)
		{
			
			for (int i = idx; i < _learningObjectsList.Count; i++) {
				lo_names.Add(_learningObjectsList [i].lo.name);
				var img = new UIImageView (new CGRect(CWidth*i, 0 , CWidth, CHeight));
				img.ContentMode = UIViewContentMode.ScaleToFill;
				//img.Image = UIImage.FromFile ("MLResources/default_img.png");  
				if (_learningObjectsList [i].cover_bytes != null)
					img.Image = MLConstants.BytesToUIImage (_learningObjectsList [i].cover_bytes);
					_learningObjectsList [i].PropertyChanged += (sender, e) => {
					img.Image = MLConstants.BytesToUIImage(
						(sender as MLearning.Core.ViewModels.MainViewModel.lo_by_circle_wrapper).cover_bytes);
				};
				_scroll.Add (img);
				lo_label.Text = lo_names [0].ToUpper ();
				_scroll.ContentSize= new CGSize (CWidth * _learningObjectsList.Count, CHeight);
			}
		}


		void addElement(int index)
		{
			var img = new UIImageView (new CGRect(CWidth*index, 0 , CWidth, CHeight));
			img.ContentMode = UIViewContentMode.ScaleToFill;
			img.Image = UIImage.FromFile ("MLResources/default_img.png");  
			_scroll.Add (img);
			_scroll.ContentSize= new CGSize (CWidth * lo_names.Count, CHeight);
		}

		UIView dataview ;
		UILabel lo_label ;
		UIButton open_button ;

		void addLOName()
		{
			dataview = new UIView (new CGRect(0,158,320,40));
			dataview.BackgroundColor = UIColor.FromRGBA (0.0f,0.0f,0.0f,0.3f);
			Add (dataview);

			lo_label = new UILabel (new CGRect( 20,11,200,20));
			lo_label.TextColor = UIColor.White;
			lo_label.Font = UIFont.FromName (fontname, 12);
			dataview.Add (lo_label);

			//open button
			var btimg = new UIImageView(new CGRect(245,11,62,18)){ContentMode = UIViewContentMode.ScaleToFill};
			btimg.Image = UIImage.FromFile ("MLResources/default_open.png");
			dataview.Add (btimg);

			open_button = new UIButton (new CGRect(245,0,64,40));
			open_button.TouchUpInside += delegate {
				if(DoOpenLO!=null)
					DoOpenLO(this,0);
			};
			dataview.Add (open_button);
		}
	}
}

