using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;

namespace MLearning.iPhone
{
	public class LoadingOverlay : UIView 
	{
		// control declarations
		UIActivityIndicatorView activitySpinner;
		UILabel loadingLabel;


		public LoadingOverlay (CGRect frame) : base (frame)
		{
			// configurable bits
			BackgroundColor = UIColor.FromRGBA (0.0f,0.0f,0.0f,0.4f); //UIColor.Black;
			Alpha = 1.0f;
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			float labelHeight = 22.0f;
			float labelWidth = (float)Frame.Width - 20.0f;

			// derive the center x and y
			float centerX = (float)Frame.Width / 2.0f;
			float centerY = (float)Frame.Height / 2.0f;

			// create the activity spinner, center it horizontall and put it 5 points above center x
			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new CGRect (
				centerX - (activitySpinner.Frame.Width / 2.0f) ,
				centerY - activitySpinner.Frame.Height - 20.0f ,
				activitySpinner.Frame.Width ,
				activitySpinner.Frame.Height);
			activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (activitySpinner);
			activitySpinner.StartAnimating ();

			// create and configure the "Loading Data" label
			loadingLabel = new UILabel(new CGRect (
				centerX - (labelWidth / 2.0f),
				centerY + 20.0f ,
				labelWidth ,
				labelHeight
			));
			loadingLabel.BackgroundColor = UIColor.Clear;
			loadingLabel.TextColor = UIColor.White;
			loadingLabel.Text = "Loading...";
			loadingLabel.TextAlignment = UITextAlignment.Center;
			loadingLabel.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (loadingLabel);
		}

		public string OverlayText {
			get { return "";}
			set{ loadingLabel.Text = value; }
		}

		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Hide ()
		{
			UIView.Animate (
				0.5, // duration
				() => { Alpha = 0; },
				() => { RemoveFromSuperview(); }
			);
		}
	};
}

