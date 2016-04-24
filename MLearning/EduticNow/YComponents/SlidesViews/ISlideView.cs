using System;
using UIKit;

namespace YComponents
{
	public interface ISlideView
	{
		nfloat GetHeight();
		nfloat GetPosition();

		//extra functions
		//void SetContent(string title, string author, string paragraph);
		//void SetImageSource(UIImage image);
	}
}

