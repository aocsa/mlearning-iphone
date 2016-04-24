using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public class SecondSlideType : UIView
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
		UIImageView leftAvatar;
		IconSlideBar iconBar;

		public SecondSlideType (int pos) : base()
		{
			var frame = Constants.ScreenFrame;
			frame.Y = pos * Constants.DeviceHeight;
			Frame = frame;


			contentScroll = new UIScrollView (new CGRect (509, 150, 380, 500)) 
			{
				Bounces = false,
				BackgroundColor = UIColor.Clear
			};

			title = Constants.makeLabel (new CGRect (0, 0, 380, 140), UIColor.White, UITextAlignment.Right, Font.Regular, 35);
			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (title);

			paragraph = Constants.makeLabel (new CGRect (0, 160, 380, 20), UIColor.White, UITextAlignment.Right, Font.Light, 18);
			paragraph.Lines = 0;
			paragraph.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (paragraph);

			leftAvatar = new UIImageView (new CGRect (90, 144, 376, 480));
			leftAvatar.Image = UIImage.FromBundle ("iOS Resources/slides/leftimg.png");
			Add (leftAvatar);

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

			int expectedHeight = Constants.resizeUILabelHeight (title.Text, title.Font, title.Frame.Width);
			title.Frame = new CGRect (0, 0, title.Frame.Width, expectedHeight);

			/*resize height according to text*/

			expectedHeight = Constants.resizeUILabelHeight (paragraph.Text, paragraph.Font, paragraph.Frame.Width);
			paragraph.Frame = new CGRect (0, 20 + title.Frame.Height, paragraph.Frame.Width, expectedHeight);


			contentScroll.ContentSize = new CGSize (380, paragraph.Frame.Y + expectedHeight + 10);
			title.TextColor = source.Style.TitleColor;
			paragraph.TextColor = source.Style.ContentColor;
			BackgroundColor = Source.Style.Background;

		}


	}
}

