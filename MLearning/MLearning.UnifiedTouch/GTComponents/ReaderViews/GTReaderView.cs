using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using DataSource;
using MLReader;

namespace ComponentsApp
{
	public class GTReaderView :UIView
	{

		nfloat DeviceHeight = 568, DeviceWidth =320 ;
		public GTReaderView () : base()
		{
			Frame = new CGRect (0, 0, DeviceWidth*2, DeviceHeight);
			initViews ();
		}

		//GTVerticalScrollView indexScroll;
		GTHorizontalScrollView contentScroll ,indexScroll; 
		UIButton backButton ;
		bool isIndexSelected = false ;

		public void initViews()
		{
			
			contentScroll = new GTHorizontalScrollView (new CGRect(320,0,320,568));
			Add (contentScroll);

			indexScroll = new GTHorizontalScrollView (new CGRect(0,0,320,568));   
			Add (indexScroll);


 

			backButton = new UIButton (new CGRect (60 + 320, 20, 40, 40));//{BackgroundColor = UIColor.Red} ;
			backButton.SetImage(UIImage.FromFile("MLResources/backbt.png"), UIControlState.Normal);
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

				indexScroll.Layer.ZPosition += 100 ;
				contentScroll.Layer.ZPosition -=100 ;
			};

			//init
			//initIndex();
			initContent ();
		}


		/*void initIndex()
		{
			for (int i = 0; i < 8; i++) {
				VerticalIndexContainer vc = new VerticalIndexContainer(); 
				vc.PageIndexSelected += (sender, id) => {
					InitContentReader(0);
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

				//indexScroll.AddVerticalIndexContainer (vc, i);
			}
		}*/

		void initContent()
		{
			for (int i = 0; i < 8; i++) {
				VerticalContainer vc = new VerticalContainer ();  
				contentScroll.AddVerticalContainer (vc, i);
			}
		}


		public BookDataSource booksource;


		public void InitIndexReader()
		{
			for (int i = 0; i < booksource.Chapters.Count; i++) {
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
					indexScroll.Layer.ZPosition -= 100 ;
					contentScroll.Layer.ZPosition +=100 ;
				}; 

				vc.SourceData = booksource.Chapters [i]; 
				vc.InitContent ();
				indexScroll.AddVerticalIndexContainer (vc, i, booksource.Chapters[i].BackgroundImage);
			}
		}


		public List<List<LOPageSource>> pagelistsource ;

		public void  InitContentReader(int idx)
		{
			var source = pagelistsource [idx];
			for (int i = 0; i < source.Count; i++) { 
				VerticalContainer vc = new VerticalContainer ();  
				//contentScroll.AddVerticalContainer (vc, i, source[i].Cover);
			}
		}
 
	}
}

