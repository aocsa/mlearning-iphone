using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation; 

namespace MLearning.iPhone
{
	public class MLSingleComment :UIView
	{
		string fontname = "HelveticaNeue";

		public MLSingleComment (CGRect frame) : base (frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions; 

			initComment ();
		}

		SingleRoundImageView _image ;
		UILabel _name, _comment , _time ;

		private void initComment()
		{
			_image = new SingleRoundImageView (new CGRect (20,12,44,44));
				//getimage (20, 12, 44, 44, "MLResources/default_img.png");
			Add (_image);

			string c = "";//This  a sample comment for ml project, lets talk a litle bit";
			_name = getTextLabel (82, 12, 158, 16, "Alicia Smith", 12);
			Add (_name);
			_comment = getTextLabel (82, 28, 212, 28, c , 11);
			Add (_comment);
			_time = getTextLabel (248, 12, 50, 16, "1.25 pm", 11);
			Add (_time);
		}


		public string NameText {
			get{ return "";}
			set{ _name.Text = value; }
		}

		public string CommentText {
			get{ return "";}
			set{ _comment.Text = value; }
		}

		public string TimeText {
			get{ return "";}
			set{ _time.Text = value;}
		}


		byte[] _imagebytes;
		public byte[] ImageBytes
		{
			get{ return _imagebytes;}
			set{ _imagebytes = value; 
				if (_imagebytes != null) {
					_image.ImageBytes = value;

				}
			}
		}

		UIImageView getimagefrombytes(float x , float y, float width , float height, byte[] bytes )
		{ 
			var image = new UIImageView (new RectangleF(x,y,width,height));
			var img = MLConstants.BytesToUIImage (bytes);
			if (img != null)
				image.Image = img;
			image.Layer.CornerRadius = image.Frame.Size.Width / 2;
			image.Layer.BorderWidth = 0.0f ;
			image.Layer.BorderColor = UIColor.White.CGColor;
			image.ClipsToBounds = true;
			image.SetNeedsDisplay ();
			return image;
		}

 

		UILabel getTextLabel(nfloat x , nfloat y, nfloat width , nfloat height, string text, int font )
		{
			var label = new UILabel (new CGRect(x,y,width,height)); 
			label.TextColor = UIColor.White;
			label.Font = UIFont.FromName (fontname, font);
			label.LineBreakMode = UILineBreakMode.WordWrap;
			label.Lines = 2;
			label.TextAlignment = UITextAlignment.Left;
			label.Text = text;
			return label;
		}

		UIImageView getimage(float x , float y, float width , float height, string source )
		{
			var image = new UIImageView (new RectangleF(x,y,width,height)); 
			image.Layer.CornerRadius = image.Frame.Size.Width / 2;
			image.Layer.BorderWidth = 0.0f ;
			image.Layer.BorderColor = UIColor.White.CGColor;
			image.ContentMode = UIViewContentMode.ScaleToFill;
			image.Layer.MasksToBounds = true;
			image.Image = UIImage.FromFile (source);
			image.SetNeedsDisplay ();  
			return image;
		}
	}
}

