using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit; 

namespace MLReader
{
    public class LOSlideStyle
    {
        public LOSlideStyle()
        { }

        private UIColor _titlecolor;

		public UIColor TitleColor
        {
            get { return _titlecolor; }
            set { _titlecolor = value; }
        }


		private UIColor _contentcolor;

		public UIColor ContentColor
        {
            get { return _contentcolor; }
            set { _contentcolor = value; }
        }

		private UIColor _backgroundcolor;

		public UIColor BackgroundColor
        {
            get { return _backgroundcolor; }
            set { _backgroundcolor = value; }
        }

		private UIColor _bordercolor;

		public UIColor BorderColor
        {
            get { return _bordercolor; }
            set { _bordercolor = value; }
        }
        
        
    }
}
