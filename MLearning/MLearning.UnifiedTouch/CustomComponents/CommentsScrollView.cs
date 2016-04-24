using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Collections.ObjectModel; 

namespace MLearning.iPhone
{
	public class CommentsScrollView : UIView
	{
		public CommentsScrollView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions; 

			initCommentScroll ();
		}
		string fontname = "HelveticaNeue";

		UILabel _titlelabel ;
		UIScrollView _scroll;

		private void initCommentScroll()
		{
			_titlelabel = getTextLabel (20, 10, 120, 14, "Comentarios >", 12);
			Add (_titlelabel);

			//scroll
			_scroll = new UIScrollView (new CGRect(0,27,320,140)); 
			_scroll.BackgroundColor = UIColor.Clear;
			Add (_scroll);

			//only temporal
			//addComments();
		}


		ObservableCollection<MLearning.Core.ViewModels.MainViewModel.post_with_username_wrapper> _postsList;
		public ObservableCollection<MLearning.Core.ViewModels.MainViewModel.post_with_username_wrapper> PostsList
		{
			get { return _postsList; }
			set {
				_postsList = value; 
				if (_postsList != null)
					addMoreComments (0);
				_postsList.CollectionChanged += (sender, e) => {
					addMoreComments(e.NewStartingIndex);
				};
			}
		}

 

		void addMoreComments(int idx)
		{
			if(_postsList!=null)
				for (int i = idx; i < _postsList.Count; i++) {
					MLSingleComment com = new MLSingleComment (new CGRect (0,i*68, 320,68));
					com.CommentText = _postsList [i].post.text;
					com.NameText = _postsList [i].post.name + " " + _postsList [i].post.lastname;
					com.TimeText = _postsList [i].post.created_at.ToString();
					if (_postsList [i].userImage != null)
						com.ImageBytes = _postsList [i].userImage;
					_postsList [i].PropertyChanged += (sender, e) => {
						com.ImageBytes = (sender as MLearning.Core.ViewModels.MainViewModel.post_with_username_wrapper).userImage ;
					};
					_scroll.Add (com);
				}
 
			//resize scroll
			_scroll.ContentSize = new SizeF (320,68*_postsList.Count);
		}



		private void addComments()
		{
			_scroll.ContentSize = new SizeF (320,68*5);
			for (int i = 0; i < 5; i++) {
				MLSingleComment com = new MLSingleComment (new CGRect (0,i*68, 320,68));
				_scroll.Add (com);
			}
		}

		UILabel getTextLabel(nfloat x , nfloat y, nfloat width , nfloat height, string text, int font )
		{
			var label = new UILabel (new CGRect(x,y,width,height)); 
			label.TextColor = UIColor.White;
			label.Font = UIFont.FromName (fontname, font);
			label.LineBreakMode = UILineBreakMode.WordWrap;
			label.TextAlignment = UITextAlignment.Left;
			label.Text = text;
			return label;
		}
	}
}

