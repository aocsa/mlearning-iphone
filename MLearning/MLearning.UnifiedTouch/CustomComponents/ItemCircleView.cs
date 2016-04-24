using System; 
using System.Drawing;
using UIKit;
using CoreGraphics; 

namespace MLearning.iPhone
{
	public delegate void ItemCircleSelectedEventHandler(object sender, int index);

	public class ItemCircleView : UIView
	{

		string fontname = "HelveticaNeue";

		public ItemCircleView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions; 

			init_item ();

			addTapGestureRecognizer ();
		}

		public event ItemCircleSelectedEventHandler ItemCircleSelected ;

		int _index = 0  ;
		public int Index
		{
			get{ return _index;}
			set{ _index = value;}
		}

		string _name = string.Empty ;
		public string Name
		{
			get{ return _name; }
			set{
				_name = value;  
				_circlename.Text = _name;
			}
		}

		UIColor _circlecolor  ;
		public UIColor CircleColor
		{
			get{ return _circlecolor; }
			set{
				_circlecolor = value;  
				_circlebullet.BackgroundColor = _circlecolor;
			}
		}

		UIColor _backcolor  ;
		public UIColor BackColor
		{
			get{ return _backcolor; }
			set{
				_backcolor = value;  
				_back_view.BackgroundColor = _backcolor;
			}
		}

		UIImageView _imagebullet ;
		UIView _circlebullet , _back_view;
		UILabel _circlename ;

		private void init_item()
		{
			_back_view = new UIView (new CGRect(0,0,192,38)){Alpha = 0} ;
			Add (_back_view);

			_imagebullet = new UIImageView (new CGRect(28,15,8,8)){ContentMode = UIViewContentMode.ScaleToFill};
			_imagebullet.Image = UIImage.FromFile ("MLResources/IconsMuro/icon_circle.png");

			Add (_imagebullet);

			_circlebullet = new UIView (new CGRect(28,15,8,8)){BackgroundColor = UIColor.Clear};
			_circlebullet.Layer.CornerRadius = 4;
			_circlebullet.Alpha = 0;
			Add (_circlebullet);

			_circlename = new UILabel (new CGRect(46,14,130,12)){TextColor =UIColor.White};
			_circlename.Font = UIFont.FromName (fontname, 11);
			Add (_circlename);

		}

		private void addTapGestureRecognizer()
		{
			// Create a new tap gesture
			UITapGestureRecognizer tapGesture = null;
 
			tapGesture = new UITapGestureRecognizer(() => { 
				if(ItemCircleSelected!=null)
					ItemCircleSelected(this, _index);
			}); 
			// Configure it
			tapGesture.NumberOfTapsRequired = 1; 
			// Add the gesture recognizer to the view
			AddGestureRecognizer(tapGesture);
		}

		bool is_on = false ;

		public void SetOn()
		{
			if (!is_on) {
				is_on = true;
  
				UIView.Animate(0.3,0, UIViewAnimationOptions.CurveEaseIn , () =>
					{                
						_back_view.Alpha = 1;                
					}, null);

				UIView.Animate(0.3,0, UIViewAnimationOptions.CurveEaseIn , () =>
					{                
						_circlebullet.Alpha = 1;                
					}, null);

				UIView.Animate(0.3,0, UIViewAnimationOptions.CurveEaseIn , () =>
					{                
						_imagebullet.Alpha = 0;                
					}, null);
				 
			}
		}

		public void SetOff()
		{
			if (is_on) {
				is_on = false;
				UIView.Animate(0.3,0, UIViewAnimationOptions.CurveEaseOut , () =>
					{                
						_back_view.Alpha = 0;                
					}, null);

				UIView.Animate(0.3,0, UIViewAnimationOptions.CurveEaseOut , () =>
					{                
						_circlebullet.Alpha = 0;                
					}, null);

				UIView.Animate(0.3,0, UIViewAnimationOptions.CurveEaseOut , () =>
					{                
						_imagebullet.Alpha = 1;                
					}, null);
				
			}
		}
	}
}

