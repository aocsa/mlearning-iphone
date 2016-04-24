using System;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public class Util
	{

		static List<CGColor> ColorList =  new List<CGColor>(){ UIColor.FromRGB(22,2,2).CGColor } ;

		public static CGColor GetColorByIndex(int index)
		{
			return ColorList [index];
		}
	}
}

