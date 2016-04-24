using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public class FourthSlideType : UIView
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

		UILabel title;
		UIScrollView contentScroll;
		UIImageView rightAvatar;

		public FourthSlideType (int pos) : base()
		{
			var frame = Constants.ScreenFrame;
			frame.Y = pos * Constants.DeviceHeight;
			Frame = frame;


			contentScroll = new UIScrollView (new CGRect (170, 150, 380, 500)) 
			{
				Bounces = false,
				BackgroundColor = UIColor.Clear
			};

			title = Constants.makeLabel (new CGRect (0, 0, 380, 140), UIColor.White, UITextAlignment.Right, Font.Regular, 35);
			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (title);


			rightAvatar = new UIImageView (new CGRect (580, 144, 377, 480));
			rightAvatar.Image = UIImage.FromBundle ("iOS Resources/slides/rightimg.png");
			Add (rightAvatar);

			Add (contentScroll);
		}

		void initComponent()
		{
			var iconBar = new IconSlideBar();
			Add(iconBar);

			if (source.Type != 0)
			{
				if (source.Style.ColorNumber != 0)
					iconBar.ImageUrl = "iOS Resources/ricons/estilo" + source.Style.ID + "_color" + source.Style.ColorNumber + "-0" + source.Type + ".png";
				else
					iconBar.ImageUrl = "iOS Resources/ricons/tema5_colorblanco-0" + source.Type + ".png";
				iconBar.LineColor = source.Style.TitleColor;
			}

			//ITEMS
			title.Text = source.Title.ToUpper();

			int expectedHeight = Constants.resizeUILabelHeight (title.Text, title.Font, title.Frame.Width);
			title.Frame = new CGRect (0, 0, title.Frame.Width, expectedHeight);

			nfloat yPos = expectedHeight + 20;
			/*resize height according to text*/
			for (int i = 0; i < source.Itemize.Count; i++)
			{
				var item = new LOItem(yPos)
				{
					TextContent = source.Itemize[i].Text,
					TextColor = Source.Style.ContentColor,
					BulletColor = Source.Style.TitleColor
				};
				contentScroll.Add(item);
				yPos += item.Frame.Height;
			}


			contentScroll.ContentSize = new CGSize (380, yPos + 20);
			title.TextColor = source.Style.TitleColor;
			BackgroundColor = Source.Style.Background;
		}


	}
}

