using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class GTReaderView :UIView
	{

		nfloat DeviceHeight = 568, DeviceWidth =320 ;
		public GTReaderView () : base()
		{
			Frame = new CGRect (0, 0, DeviceWidth, DeviceHeight);
			initViews ();
		}

		//GTVerticalScrollView indexScroll;
		GTHorizontalScrollView contentScroll ,indexScroll;
		UIButton backButton ;
		bool isIndexSelected = false ;

		public void initViews()
		{
			

			indexScroll = new GTHorizontalScrollView (new CGRect(0,0,320,568));   
			Add (indexScroll);

			contentScroll = new GTHorizontalScrollView (new CGRect(320,0,320,568));
			Add (contentScroll);
 

			backButton = new UIButton(new CGRect(20 + 320,20,40,40)){BackgroundColor = UIColor.Red} ;
			Add(backButton);
			backButton.TouchUpInside+=(sender, e) => { 	
				UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn, () => {
					indexScroll.Transform = CGAffineTransform.MakeScale (1.0f, 1.0f);
				}, null);
				UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn ,
					() => { contentScroll.Center =  new PointF ((float)contentScroll.Center.X + 320,
						(float)contentScroll.Center.Y);}, null );

				UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn ,
					() => { backButton.Center =  new PointF ((float)backButton.Center.X + 320,
						(float)backButton.Center.Y);}, null );
			};

			//init
			initIndex();
			initContent ();
		}


		void initIndex()
		{
			for (int i = 0; i < 8; i++) {
				VerticalIndexContainer vc = new VerticalIndexContainer(); 
				vc.PageIndexSelected += (sender, id) => {
					contentScroll.SetToIndex(id) ;
					UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn, () => {
						indexScroll.Transform = CGAffineTransform.MakeScale (0.9f, 0.9f);
					}, null);
					UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn ,
						() => { contentScroll.Center =  new PointF ((float)contentScroll.Center.X - 320,
							(float)contentScroll.Center.Y);}, null );

					UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn ,
						() => { backButton.Center =  new PointF ((float)backButton.Center.X - 320,
							(float)backButton.Center.Y);}, null );
				}; 

				indexScroll.AddVerticalIndexContainer (vc, i);
			}
		}

		void initContent()
		{
			for (int i = 0; i < 8; i++) {
				VerticalContainer vc = new VerticalContainer ();  
				contentScroll.AddVerticalContainer (vc, i);
			}
		}
 
	}
}

