using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using DataSource;

namespace ComponentsApp
{
	public class VerticalIndexContainer : UIView
	{
		public  event PageIndexSelectedEventHandler PageIndexSelected;

		public VerticalIndexContainer () : base()
		{
			//initContainer ();
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
 
		ChapterDataSource sourceData ;
		public ChapterDataSource SourceData {
			get{ return sourceData;  }
			set{ sourceData = value;}
		}

		public void InitContent()
		{
			for (int i = 0; i < sourceData.Sections.Count; i++) {

				for (int j = 0; j < sourceData.Sections[i].Pages.Count; j++) {

					PageIndexView item = new PageIndexView (containerHeight);
					item.Index = i;
					item.TitleText = sourceData.Sections [i].Pages [j].Name;
					item.ContentText = sourceData.Sections [i].Pages [j].Description;
					item.ItemColor = sourceData.Sections [i].Pages [j].BorderColor;
					item.PageIndexSelected += (sender, id) => {
						if(PageIndexSelected!=null)
							PageIndexSelected(this,id);
					};
					Add (item);
					containerHeight += item.ElementHeight;
				}
			}

			//set frame
			var frame = new CGRect(0,0,320, containerHeight);
			Frame = frame; 
		}
	}
}

