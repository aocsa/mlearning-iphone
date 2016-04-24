using System;
using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch
{
	public class LOReaderScrollElement : UIScrollView
	{

		LOPageSource source;
		public LOPageSource Source 
		{
			get { return source; }
			set 
			{
				source = value;
				loadElement ();
			}
		}

		UIImageView backImage;
		CoverTextSlide cover;

		public LOReaderScrollElement (int position) : base()
		{
			PagingEnabled = true;
			Bounces = false;

			var elementFrame = Constants.ScreenFrame;
			elementFrame.X = Constants.DeviceWidth * position;
			Frame = elementFrame;
			backImage = new UIImageView (Constants.ScreenFrame);
			Add (backImage);
			cover = new CoverTextSlide ();
			Add (cover);
			ContentSize = new CGSize (Constants.DeviceWidth, Constants.DeviceHeight);
		}

		void loadElement()
		{
			backImage.Image = source.Cover;
			backImage.ContentMode = UIViewContentMode.ScaleAspectFill;
			backImage.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			backImage.ClipsToBounds = true;

			for (int i = 0; i < source.Slides.Count; i++) 
			{
				if (i == 0)
					cover.Source = source.Slides [0];
				else 
				{
					switch (source.Slides [i].Type) 
					{
					case 1: 
						FirstSlideType slide1 = new FirstSlideType(i) { Source = source.Slides[i] };
						Add(slide1);
						break;
					case 2:
						SecondSlideType slide2 = new SecondSlideType(i) { Source = source.Slides[i] };
						Add(slide2);
						break;
						//4 y 3 inverted on purpose (mistake naming classes)
					case 4:
						ThirdSlideType slide3 = new ThirdSlideType(i) { Source = source.Slides[i] };
						Add(slide3);
						break;
					case 3:
						FourthSlideType slide4 = new FourthSlideType(i) { Source = source.Slides[i] };
						Add(slide4);
						break;
					default:
						Add (new UIView (){ BackgroundColor = UIColor.LightGray, Frame = new CGRect (0, i * Constants.DeviceHeight, Constants.DeviceWidth, Constants.DeviceHeight)});
						break;
					}


				}
			}

			ContentSize = new CGSize (Constants.DeviceWidth, Constants.DeviceHeight * source.Slides.Count);

		}

	}
}

