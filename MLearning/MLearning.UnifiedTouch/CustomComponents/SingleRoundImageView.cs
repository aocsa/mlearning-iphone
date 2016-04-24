using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic; 

namespace MLearning.iPhone
{
	public class SingleRoundImageView : UIView
	{
		public SingleRoundImageView (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			initImage ();
		}

		UIImageView _image ;

		void initImage()
		{
			_image = getimage (0,0,44,44,"MLResources/default_img.png");
			Add (_image);
		}


	 
		byte[] _imagebytes;
		public byte[] ImageBytes
		{
			get{ return _imagebytes;}
			set{ _imagebytes = value;
				//getimage (16 , 24 , 52, 52, _sourcepath)
				if (_imagebytes != null) {
					_image.Image = MLConstants.BytesToUIImage (_imagebytes);
					_image.SetNeedsDisplay ();
					//getimagefrombytes (16, 24, 52, 52, _imagebytes); 

				}
			}
		}



		UIImageView getimage(float x , float y, float width , float height, string source )
		{
			var image = new UIImageView (new RectangleF(x,y,width,height)); 
			image.Layer.CornerRadius = image.Frame.Size.Width / 2;
			image.Layer.BorderWidth = 0.0f ;
			image.Layer.BorderColor = UIColor.White.CGColor;
			image.ContentMode = UIViewContentMode.ScaleToFill;
			image.Layer.MasksToBounds = true;
			//image.Image = UIImage.FromFile (source);
			image.SetNeedsDisplay ();  
			return image;
		}


		/*UIImageView getimagefrombytes(float x , float y, float width , float height, byte[] bytes )
		{ 
			var image = new UIImageView (new RectangleF(x,y,width,height));
			var img = ToUIImage (bytes);
			if (img != null)
				image.Image = img;
			image.Layer.CornerRadius = image.Frame.Size.Width / 2;
			image.Layer.BorderWidth = 0.0f ;
			image.Layer.BorderColor = UIColor.White.CGColor;
			image.ClipsToBounds = true;
			image.SetNeedsDisplay ();
			return image;
		}

		public UIImage ToUIImage (byte[] bytes)
		{
			if (bytes == null)
				return null;

			using (var data = NSData.FromArray (bytes)) {
				return UIImage.LoadFromData (data);
			}
		}*/

	}
}

