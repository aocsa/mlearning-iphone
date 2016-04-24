using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class VerticalContainer : UIView
	{
		public VerticalContainer (): base()
		{
			initContainer ();
		}

		public nfloat getHeight()
		{
			return containerHeight;
		}



		nfloat containerHeight = 0  ;
		void initContainer()
		{
			InitialSlideView slide0 = new InitialSlideView (containerHeight);
			Add (slide0);
			containerHeight += slide0.GetHeight();

			SingleImageSlideView slide1 = new SingleImageSlideView (containerHeight);
			Add (slide1);
			containerHeight += slide1.GetHeight();

			SinglePartSlideView slide2 = new SinglePartSlideView (containerHeight);
			Add (slide2);
			containerHeight += slide2.GetHeight();

			SingleTextSlideView slide3 = new SingleTextSlideView (containerHeight);
			Add (slide3);
			containerHeight += slide3.GetHeight();

			ItemizeSlideView slide4 = new ItemizeSlideView (containerHeight);
			Add (slide4);
			containerHeight += slide4.GetHeight();

			MultiImageSlideView slide5 = new MultiImageSlideView (containerHeight);
			Add (slide5);
			containerHeight += slide5.GetHeight();

			QuoteSlideView slide6 = new QuoteSlideView (containerHeight);
			Add (slide6);
			containerHeight += slide6.GetHeight();

			BackImageSlideView slide7 = new BackImageSlideView (containerHeight);
			Add (slide7);
			containerHeight += slide7.GetHeight();

			//set frame
			var frame = new CGRect(0,0,320, containerHeight);
			Frame = frame; 
		}
	}
}

