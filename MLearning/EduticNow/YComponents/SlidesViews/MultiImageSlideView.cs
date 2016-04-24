using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using MLearning.UnifiedTouch;

namespace YComponents
{
	/// <summary>
	/// Multi image slide view.
	/// type = 4 
	/// </summary>
	public class MultiImageSlideView : UIView,ISlideView
	{
		public MultiImageSlideView (nfloat pos) : base()
		{
			slidePos = pos;
			//addTitle ();
			//initImages ();
		}

		nfloat slideHeight = 0 ;
		nfloat slidePos =0 ;

		#region ISlideView implementation

		public nfloat GetHeight ()
		{
			return slideHeight;
		}

		public nfloat GetPosition ()
		{
			return slidePos;
		}

		#endregion

		LOSlideSource source ;
		public LOSlideSource SlideSource
		{
			get { return source;}
			set 
			{
				source = value;
				addTitle ();
				initImages ();
			}
		}

		// title 

		UILabel titleLabel ;
		void addTitle()
		{
			titleLabel = new UILabel (new CGRect(112,30 , 800 , 40 ));
			//titleLabel.Text = "Galeria de Aves";
			titleLabel.Text =  source.Title ;
			titleLabel.TextColor = UIColor.Black;
			titleLabel.Font = UIFont.FromName ("HelveticaNeue", 32);
			Add (titleLabel);
		}


		//static size of images
		nfloat margin = 112 , separation = 4;
		nfloat width1 = 398, width2 = 264 ;
		nfloat height1 = 260, height2 = 260 ;

		int imageCount = 5 ; 

		void initImages()
		{ 

			imageCount = source.Itemize.Count;
			if (imageCount > 5)
				imageCount = 5;

			switch (imageCount) {
			case 1:
				var v1 = get1image (100, 0);
				Add (v1);
				slideHeight = 140 + 2 * height1;
				break;
			case 2:
				var v_1 = get2images (100,0,1); 
				Add (v_1); 
				slideHeight = 140 + separation + height1;
				break;
			case 3: 
				var v_2 = get3images (100,0,2); 
				Add (v_2);
				slideHeight = 140 + separation + height1;
				break;

			case 4: 
				var i_2 = get2images (100,0,1); 
				Add (i_2);
				var j_2 = get2images (100 + height1 + separation,2,3); 
				Add (j_2);
				slideHeight = 140 + separation + 2 * height1;
				break;


			case 5: 
				var i_3 = get2images (100,0,1 ); 
				Add (i_3);
				var j_3 = get3images (100 + height1 + separation,2,4); 
				Add (j_3);
				slideHeight = 140 + separation + 2 * height1;
				break;
			default:
				break;
			}


			var frame = new CGRect(0,slidePos,YConstants.DeviceWidht,slideHeight);
			Frame = frame;
		}


		UIView get1image (nfloat pos, int p)
		{
			var imageView =  new UIView (new CGRect(margin, pos , 800, height1*2 ));

			var img = new UIImageView (new CGRect(0,0,800, height1*2));
			img.ContentMode = UIViewContentMode.ScaleAspectFill;
			img.Layer.MasksToBounds = true;
			//img.Image = UIImage.FromFile ("MyImage.png");
			img.BackgroundColor = UIColor.Gray ;
			YConstants.DownloadImageAsync (source.Itemize[p].ImageUrl).ContinueWith ((task) => InvokeOnMainThread (() => {
				try { img.Image = task.Result; }
				catch{ }
			}));

			SetNeedsDisplay ();
			imageView.Add (img);

			return imageView;
		}

		UIView get2images(nfloat pos, int pi, int pf)
		{
			var images = new UIView (new CGRect(margin, pos , 800, height1 ));
			for (int i = pi; i <=pf ; i++) {
				var img = new UIImageView (new CGRect(i * (width1+separation),0,width1, height1));
				img.ContentMode = UIViewContentMode.ScaleAspectFill;
				img.Layer.MasksToBounds = true;
				//img.Image = UIImage.FromFile ("MyImage.png");

				img.BackgroundColor = UIColor.Gray ;
				YConstants.DownloadImageAsync (source.Itemize[i].ImageUrl).ContinueWith ((task) => InvokeOnMainThread (() => {
					try { img.Image = task.Result; }
					catch{ }
				}));

				SetNeedsDisplay ();
				images.Add (img);
			}
			return images;
		}


		UIView get3images (nfloat pos, int pi, int pf)
		{
			var images = new UIView (new CGRect(margin, pos , 800, height2 ));
			for (int i = pi; i <= pf; i++) {
				var img = new UIImageView (new CGRect(i * (width2+separation),0,width2, height2));
				img.ContentMode = UIViewContentMode.ScaleAspectFill;
				img.Layer.MasksToBounds = true;
				//img.Image = UIImage.FromFile ("MyImage.png");
				img.BackgroundColor = UIColor.Gray ;
				YConstants.DownloadImageAsync (source.Itemize[i].ImageUrl).ContinueWith ((task) => InvokeOnMainThread (() => {
					try { img.Image = task.Result; }
					catch{ }
				}));

				SetNeedsDisplay ();
				images.Add (img);
			}
			return images;
		}

	}
}

