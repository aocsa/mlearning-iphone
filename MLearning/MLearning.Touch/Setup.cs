 using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.CrossCore;
using MLearning.Core.File;
using MonoTouch.UIKit;
using MLearning.iPhone.File;

namespace MLearning.Touch
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override void InitializeLastChance()
		{
			Mvx.RegisterSingleton<IAsyncStorageService>(new AsyncStorageTouchService());
			base.InitializeLastChance();
		}

		protected override IMvxApplication CreateApp ()
		{
			return new Core.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
	}
}