using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace YComponents
{
	public class YConstants
	{
		public YConstants ()
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
		public static nfloat DeviceHeight = 768 ;
		public static nfloat DeviceWidht = 1024 ;

		public static List<UIColor> ColorList = new List<UIColor>{
			UIColor.FromRGBA(0,198,255,255),
			UIColor.FromRGBA(192,50,242,255),
			UIColor.FromRGBA(101,201,33,255),
			UIColor.FromRGBA(255,210,0,255),
			UIColor.FromRGBA(255,150,0,255),
			UIColor.FromRGBA(255,56,145,255)
		} ;

		public UIColor getColorbyID(int id)
		{ 
			return UIColor.Red;
		}



		public static async Task<UIImage> DownloadImageAsync(string imageUrl)
		{
			var httpclient = new HttpClient();
			Task <Byte[]> contentsTask = httpclient.GetByteArrayAsync (imageUrl);
			var contents = await contentsTask;
			return UIImage.LoadFromData(NSData.FromArray(contents));
		}


	}
}

