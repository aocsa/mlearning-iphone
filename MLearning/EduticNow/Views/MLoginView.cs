using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation;
using MLearning.UnifiedTouch.CustomComponents;
using System;
using CoreAnimation;
using CoreGraphics;
using Core.ViewModels;
using YComponents;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Core.Repositories;
using Cirrious.CrossCore;
using MLearning.Core;

namespace MLearning.UnifiedTouch
{
	[Register("MLoginView")]
	public class MLoginView : MvxViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White;

			// ios7 layout
			if (RespondsToSelector (new Selector ("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			initButton ();
		}

		UIButton createButton ;

		void initButton()
		{
			createButton = new UIButton (UIButtonType.Custom);
			createButton.Frame = new CGRect (100,200,200,100);
			createButton.BackgroundColor = UIColor.Blue;
			View.Add (createButton);
			//Cambios 2 lineas >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			//var set = this.CreateBindingSet<MLoginView, MLoginViewModel>();  

			//set.Bind (createButton).To (vm=>vm.CreateConsumerCommand);

			//set.Apply ();
		}
	}
}

