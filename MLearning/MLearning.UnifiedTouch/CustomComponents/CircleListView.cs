using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MLearningDB; 

namespace MLearning.iPhone
{
	public class CircleListView : UIView
	{
		nfloat item_width = 192, item_height = 38 ;
		UIScrollView _scroll ;
		List<string> _names = new List<string>() {"Matematica", "Historia","Geografia","Literatura", "Humanidades", "Computacion", "Algebra", "Filosofia"};
		List<ItemCircleView> _itemslist = new List<ItemCircleView> ();

		//event for selection
		public event ItemCircleSelectedEventHandler ItemCircleSelected ;

		public CircleListView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions; 

			BackgroundColor = UIColor.Clear;

			_scroll = new UIScrollView (new CGRect(0,0,item_width, 228)); 
			_scroll.BackgroundColor = UIColor.Clear;
			Add (_scroll);
			//only for initial
			//addAll_tmp ();

		}

		void addAll_tmp()
		{
			for (int i = 0; i < _names.Count; i++) {
				addElement (i);
			}
			_itemslist [0].SetOn ();
		}

		void addElement(int index)
		{
			var ele = new ItemCircleView (new CGRect(0, item_height*index , item_width , item_height));
			ele.Index = index;
			ele.BackColor = UIColor.Red;
			ele.CircleColor = UIColor.Yellow;
			ele.Name = _names [index];
			ele.ItemCircleSelected += (s, id) => {
				for (int i = 0; i < _names.Count; i++) {
					if(i==id)
						_itemslist[id].SetOn();
					else _itemslist[i].SetOff();
				}
			};
			_itemslist.Add (ele);
			_scroll.Add (ele);
			_scroll.ContentSize= new CGSize (item_width, item_height * _names.Count);
		}



		ObservableCollection<circle_by_user> _circlesList;
		public ObservableCollection<circle_by_user> CirclesList
		{
			get { return _circlesList; }
			set 
			{
				_circlesList = value; 
				populateCircleScroll(0);
				_circlesList.CollectionChanged += (sender, e) => {
					populateCircleScroll(e.NewStartingIndex);
				};
			}
		}

		void populateCircleScroll(int index)
		{ 
			if (_circlesList != null) {
				for (int i = 0; i < _circlesList.Count; i++) {
					var ele = new ItemCircleView (new CGRect(0, item_height*i , item_width , item_height));
					ele.Index = i;
					ele.BackColor = UIColor.Red;
					ele.CircleColor = UIColor.Yellow;
					ele.Name = _circlesList [i].name; 
					ele.ItemCircleSelected += (s, id) => {
						for (int j = 0; j < _itemslist.Count; j++) {
							if(j==id)
								_itemslist[id].SetOn();
							else _itemslist[j].SetOff();
							//event for selected
							if(ItemCircleSelected!=null)
								ItemCircleSelected(this, id);
						}
					};
					_itemslist.Add (ele);
					_scroll.Add (ele);
					_scroll.ContentSize= new CGSize (item_width, item_height * _itemslist.Count);
				}
			}
		}

	}
}

