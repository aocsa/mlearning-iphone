using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace ComponentsApp
{
	public class ReaderConstants
	{
		public ReaderConstants ()
		{
		}

		public static nfloat ResizeHeigthWithText(UILabel label,float maxHeight = 960f) 
		{
			nfloat width =  label.Frame.Width;  
			CGSize size = ((NSString)label.Text).StringSize(label.Font,constrainedToSize:new CGSize(width,maxHeight),
				lineBreakMode:UILineBreakMode.WordWrap);
			var labelFrame = label.Frame;
			label.Lines = (int)(size.Height / label.Font.CapHeight);
			labelFrame.Size = new CGSize(width,size.Height);
			var newHeight = size.Height;
			label.Frame = labelFrame;
			return newHeight;
		}

		public static UILabel GetNewTextLabel(nfloat x, nfloat y, nfloat w,nfloat h,nfloat textsize, UIColor col, int lines)
		{
			var label = new UILabel (new CGRect(x,y,w,h));
			label.Lines = lines;
			label.Font = UIFont.FromName ("HelveticaNeue", textsize);
			label.TextColor = col;
			return label;
		}

		public static string FontName = "HelveticaNeue" ;

	}
}

