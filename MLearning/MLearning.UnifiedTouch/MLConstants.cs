using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation; 

namespace MLearning.iPhone
{
	public class MLConstants
	{
		public MLConstants ()
		{
		}



		public static UIColor getUIColor(int id, nfloat alpha)
		{
			//RGB Colors
			List<nfloat> RList = new List<nfloat>(){192,101,0  ,255,255,255};
			List<nfloat> GList = new List<nfloat>(){50 ,201,198,210,150,56};
			List<nfloat> BList = new List<nfloat>(){242,33 ,255,0  ,0  ,145};

			var idx = id%6;
			//var color = UIColor.FromRGB (RList [idx], GList [idx], BList [idx]);//, alpha);
			return UIColor.FromRGB (RList [idx]/255, GList [idx]/255, BList [idx]/255);//, alpha);
		}

		public static CGColor getCGColor(int id, nfloat alpha)
		{
			//RGB Colors
			List<nfloat> RList = new List<nfloat>(){192,101,0  ,255,255,255};
			List<nfloat> GList = new List<nfloat>(){50 ,201,198,210,150,56};
			List<nfloat> BList = new List<nfloat>(){242,33 ,255,0  ,0  ,145};
 
			var idx = id%6;
			var color = new CGColor(RList [idx], GList [idx], BList [idx], alpha);
			return color; 
		}


		public static UIImage BytesToUIImage (byte[] bytes)
		{
			if (bytes == null)
				return null;

			using (var data = NSData.FromArray (bytes)) {
				return UIImage.LoadFromData (data);
			}
		}

	}
}

