using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Collections.ObjectModel; 

namespace MLearning.iPhone
{
	public class UsersScrollView : UIView
	{
		string fontname = "HelveticaNeue";

		public UsersScrollView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			init_view ();
			//addElements ();
		}



		UIScrollView _scroll ;
		void init_view()
		{
			var label = new UILabel (new CGRect(20, 22, 80,12)){TextColor = UIColor.White};
			label.Font = UIFont.FromName (fontname,11);
			label.Text = "Personas";
			Add (label);

			_scroll = new UIScrollView (new CGRect(22,42,276, 44)); 
			_scroll.BackgroundColor = UIColor.Clear;
			Add (_scroll);
		}


		ObservableCollection<MLearning.Core.ViewModels.MainViewModel.user_by_circle_wrapper> _usersList;
		public ObservableCollection<MLearning.Core.ViewModels.MainViewModel.user_by_circle_wrapper> UsersList
		{
			get { return _usersList; }
			set { _usersList = value; 
				addNewElements (0);
				_usersList.CollectionChanged += (sender, e) => {
					addNewElements(e.NewStartingIndex);
				};
			}
		}


		void addNewElements(int idx)
		{ 
			for (int i = idx; i < _usersList.Count; i++) {
				var img = new SingleRoundImageView (new CGRect(i*(44+14),0,44,44)); 
				if (_usersList [i].userImage != null)
					img.ImageBytes = _usersList [i].userImage;
				_usersList [i].PropertyChanged += (sender, e) => {
					img.ImageBytes = (sender as MLearning.Core.ViewModels.MainViewModel.user_by_circle_wrapper).userImage ;
				};
				_scroll.Add(img);
			}

			_scroll.ContentSize = new CGSize (_usersList.Count*(44+14), 44);
		}


		void addElements()
		{
			_scroll.ContentSize = new CGSize (9*(44+14), 44);
			for (int i = 0; i < 9; i++) {
				var img = new SingleRoundImageView (new CGRect(i*(44+14),0,44,44));//44+14 width+espacio
				_scroll.Add(img);
			}
		}
	}
}

