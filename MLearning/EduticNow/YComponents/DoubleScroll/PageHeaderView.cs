﻿using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using System.Collections.ObjectModel;

namespace MLearning.UnifiedTouch
{
	public class PageHeaderView : UIView
	{
		public PageHeaderView ()
		{
		}


		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			var gctx = UIGraphics.GetCurrentContext ();

			// setting blend mode to clear and filling with
			// a clear color results in a transparent fill
			gctx.SetFillColor (UIColor.Purple.CGColor);
			gctx.FillRect (rect);

			gctx.SetBlendMode (CGBlendMode.Clear);
			UIColor.Clear.SetColor ();

			// create some cutout geometry
			var path = new CGPath ();   
			path.AddLines(new CGPoint[]{
				new CGPoint(100,200),
				new CGPoint(160,100), 
				new CGPoint(220,200)}); 
			path.CloseSubpath();

			gctx.AddPath(path);
			gctx.DrawPath(CGPathDrawingMode.Fill);  
		}
	}
}

