using System;
using UIKit;
using CoreGraphics;
using System.Drawing;
using Foundation;
using CoreText;

namespace YComponents
{
	public class UIFormatedText : UIView
	{
		public UIFormatedText ()
		{
			//BackgroundColor = UIColor.FromRGBA(255,255,255,90);
		}

		  
		CGPath _path;
		public CGPath Path
		{
			get{ return _path;}
			set{ _path = value; SetNeedsDisplay ();}
		}

		string _text ;
		public string Text
		{
			get { return _text;}
			set { _text = value; SetNeedsDisplay ();}
		}


		UIColor _foreground  = UIColor.Black;
		public UIColor Foreground
		{
			get { return _foreground;}
			set { _foreground = value; SetNeedsDisplay ();}
		}


		CTTextAlignment _textAligment = CTTextAlignment.Left ;
		public CTTextAlignment TextAligment
		{
			get { return _textAligment; }
			set { _textAligment = value; SetNeedsDisplay(); }
		}


		string _fontName = "Verdana" ;
		public string FontName
		{
			get { return  _fontName;}
			set { _fontName = value; SetNeedsDisplay ();}
		}

		nfloat _fontSize = 20 ;
		public nfloat FontSize
		{
			get { return _fontSize; }
			set { _fontSize = value; SetNeedsDisplay ();}
		}

		nfloat _textHeight = 400 ;
		public nfloat TextHeight {
			get { return _textHeight ;}
			set { _textHeight = value; SetNeedsDisplay (); }
		}


		public override void Draw (CoreGraphics.CGRect rect)
		{
			base.Draw (rect);

			if (_path == null)
				return;

			using (CGContext g = UIGraphics.GetCurrentContext ()) {

				//set up drawing attributes
				g.SetLineWidth (1);
				UIColor.Clear.SetFill ();
				UIColor.Clear.SetStroke ();
 
				var attrString = new NSMutableAttributedString ("	");
				if (_text != null)
					attrString = new NSMutableAttributedString (_text);

				// We'll Set the justification to center so it'll
				// print the number in the center of the erect 
				CTParagraphStyle alignStyle = new CTParagraphStyle(new CTParagraphStyleSettings {
					Alignment = _textAligment //CTTextAlignment.Left
				});


				// Calculate the range of the attributed string
				NSRange stringRange = new NSRange(0, attrString.Length);

				var font = new CTFont (_fontName, _fontSize);
				// Add style attributes to the attributed string
				attrString.AddAttributes (new CTStringAttributes {
					Font = font,
					ParagraphStyle = alignStyle,
					ForegroundColor = Foreground.CGColor
				}, stringRange);

				// Create a container for the attributed string 
				using (var framesetter = new CTFramesetter(attrString)) {

					using (var frame = framesetter.GetFrame (stringRange, _path, null)) {
						g.SaveState ();

						g.TranslateCTM (0, TextHeight);
						g.ScaleCTM (1, -1); 
						//g.RotateCTM (0.2f );

						frame.Draw (g);
						g.RestoreState ();
					}
				}


			}
		}



	}
}

