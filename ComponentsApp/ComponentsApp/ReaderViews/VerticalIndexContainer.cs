using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class VerticalIndexContainer : UIView
	{
		public  event PageIndexSelectedEventHandler PageIndexSelected;

		public VerticalIndexContainer () : base()
		{
			initContainer ();
		}

		public nfloat getHeight()
		{
			return containerHeight;
		}

		int numberofItems =  8 ;
		public int NumberOfItems {
			get{ return numberofItems;}
			set{ numberofItems = value;}
		}


		nfloat containerHeight = 0, itemHeight = 120  ;
		void initContainer()
		{ 
			
			for (int i = 0; i < numberofItems; i++) {
				PageIndexView item = new PageIndexView (containerHeight);
				item.Index = i;
				item.PageIndexSelected += (sender, id) => {
					if(PageIndexSelected!=null)
						PageIndexSelected(this,id);
				};
				Add (item);
				containerHeight += item.ElementHeight;
			}

			//set frame
			var frame = new CGRect(0,0,320, containerHeight);
			Frame = frame; 
		}

	}
}

