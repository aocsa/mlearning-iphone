using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public class FirstSlideType : UIView
	{
		LOSlideSource source;
		public LOSlideSource Source
		{
			get { return source; }
			set { source = value; initComponent(); }
		}

		UIColor titlecolor;
		public UIColor TitleColor
		{
			get { return titlecolor; }
			set { titlecolor = value; }
		}

		UIColor contentcolor;
		public UIColor ContentColor
		{
			get { return contentcolor; }
			set { contentcolor = value; }
		}

		UILabel title, paragraph;
		UIScrollView contentScroll;
		UIManipulableView view;
		UIImageView image;
		IconSlideBar iconBar;

		public FirstSlideType (int pos) : base()
		{
			var frame = Constants.ScreenFrame;
			frame.Y = pos * Constants.DeviceHeight;
			Frame = frame;


			contentScroll = new UIScrollView (new CGRect (200, 150, 380, 500)) 
			{
				Bounces = false,
				BackgroundColor = UIColor.Clear
			};

			title = Constants.makeLabel (new CGRect (0, 0, 380, 140), UIColor.White, UITextAlignment.Left, Font.Regular, 35);
			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (title);

			paragraph = Constants.makeLabel (new CGRect (0, 160, 380, 20), UIColor.White, UITextAlignment.Left, Font.Light, 18);
			paragraph.Lines = 0;
			paragraph.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (paragraph);

			view = new UIManipulableView ();
			view.setFrame (new CGRect (635, 245, 260, 320));

			view.Layer.CornerRadius = 4;


			image = new UIImageView (new CGRect (5, 5, 250, 310));
			image.ContentMode = UIViewContentMode.ScaleAspectFill;
			image.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			image.ClipsToBounds = true;
			view.Add (image);

			Add (view);
			Add (contentScroll);

		}
			
		void initComponent()
		{
			iconBar = new IconSlideBar();
			Add(iconBar);

			if (source.Type != 0)
			{
				if (source.Style.ColorNumber != 0)
					iconBar.ImageUrl = "iOS Resources/ricons/estilo" + source.Style.ID + "_color" + source.Style.ColorNumber + "-0" + source.Type + ".png";
				else
					iconBar.ImageUrl = "iOS Resources/ricons/tema5_colorblanco-0" + source.Type + ".png";
				iconBar.LineColor = source.Style.TitleColor;
			}

			title.Text = source.Title.ToUpper();
			paragraph.Text = source.Paragraph;

			/*resize height according to text*/
			int expectedHeight = Constants.resizeUILabelHeight (paragraph.Text, paragraph.Font, paragraph.Frame.Width);
			paragraph.Frame = new CGRect (0, 160, paragraph.Frame.Width, expectedHeight);


			contentScroll.ContentSize = new CGSize (380, 160 + expectedHeight + 10);
			title.TextColor = source.Style.TitleColor;
			paragraph.TextColor = source.Style.ContentColor;

			//var url = new NSUrl (Source.ImageUrl);
			//var data = NSData.FromUrl (url);
			//image.Image = UIImage.LoadFromData (data);


			//downloadImageWithURL (new NSUrl(Source.ImageUrl));

			Constants.DownloadImageAsync(Source.ImageUrl).ContinueWith((task) => InvokeOnMainThread(() =>
				{
					try { image.Image = task.Result; }
					catch{ }
				}));

			view.BackgroundColor = Source.Style.TitleColor;

			BackgroundColor = Source.Style.Background;
		}


	}
}

