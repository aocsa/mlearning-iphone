using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;


namespace ComponentsApp
{
	public class MultilineTextView : UIView
	{
		public MultilineTextView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
		}

		UILabel textlabel ;

		void initText()
		{
			BackgroundColor = UIColor.Yellow;		
		}
	}

}

