using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation;
using CoreAnimation;
using CoreGraphics;
using CoreImage;
using MLearning.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System;
using System.Collections.Generic;
 
namespace MLearning.iPhone
{
	[Register("MainView")]
	public class MainView : MvxViewController
	{ 
		string fontname = "HelveticaNeue";
 
		UIView LeftView, CenterView ;
		UIButton MenuButton;
		bool IsMenuOpen = false ;

		LoadingOverlay LoadingView  ;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			View.BackgroundColor = UIColor.Black; 
			init_mainviews (); 
			init_left ();
			initCenter ();
 
		}

		 
 


		private void init_mainviews()
		{
			//init views
			LeftView = new UIView(new CGRect(0,0,204 , 568)){BackgroundColor = UIColor.Black};
			CenterView = new UIView (new CGRect(0,0,320, 568)){BackgroundColor = UIColor.Black	};
			View.Add (LeftView);
			View.Add (CenterView);

			//button controller
			var imgbt = new UIImageView(new CGRect(14,20,22,14)){ContentMode = UIViewContentMode.ScaleToFill};
			imgbt.Image = UIImage.FromFile ("MLResources/IconsMuro/menubutton");
			CenterView.Add (imgbt);

			MenuButton = new UIButton(new CGRect(0,0,44,46));  
			CenterView.Add (MenuButton);
			MenuButton.TouchUpInside += delegate {
				if(IsMenuOpen)
				{
					IsMenuOpen = false ;
					close_menu();
				}
				else
				{
					IsMenuOpen = true ;
					open_menu();
				}
			};

		}

		private void open_menu()
		{ 
			UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseOut,
					() => {
						CenterView.Center = new PointF ((float)CenterView.Center.X + 204.0f,
							(float)CenterView.Center.Y);
					}, null); 
		}

		private void close_menu()
		{ 
				UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn,
					() => {
						CenterView.Center = new PointF ((float)CenterView.Center.X - 204.0f,
							(float)CenterView.Center.Y);
					}, null); 
		}
 

		LOsScrollView los_scroll ;

		private void initCenter()
		{
			los_scroll = new LOsScrollView (new CGRect (0,48,320,198));
			los_scroll.DoOpenLO += (sender, lo_index) => {
				var vm1 = ViewModel as MainViewModel;
				vm1.OpenLOCommand.Execute(vm1.LearningOjectsList[0]);
				LoadingView = new LoadingOverlay(new CGRect(0,0,320,568));
				View.Add(LoadingView);
			};
			CenterView.Add (los_scroll);

			var vm = ViewModel as MainViewModel;
			if (vm.LearningOjectsList != null)
				los_scroll.LearningOjectsList = vm.LearningOjectsList;
			vm.PropertyChanged += (sender, e) => {
				los_scroll.LearningOjectsList = vm.LearningOjectsList;
			};
		}





		UIImageView _logo ;
		UIView CirclesView  ;
		UIButton _settingBT, _logoutBT ;
		UITextField _searchfield ;

		private void init_left()
		{
			var contentleft = new UIView (new CGRect (12, 48, 192, 462));
			contentleft.BackgroundColor = UIColor.FromRGBA (84.0f,84.0f,84.0f,0.16f);
			LeftView.Add (contentleft);

			//logo
			_logo = new UIImageView(new CGRect(36,26, 36,36)){ContentMode = UIViewContentMode.ScaleToFill};
			_logo.Image = UIImage.FromFile ("MLResources/Icons/logo.png");
			contentleft.Add (_logo);
			//main title
			var title = new UILabel(new CGRect(95,37,60,18)){ Text = "EDuTic" , TextColor = UIColor.White } ;
			title.Font = UIFont.FromName (fontname,12);
			contentleft.Add (title); 

			//search
			var searchimg = new UIImageView(new CGRect(28,96,10,10)){ContentMode = UIViewContentMode.ScaleToFill};
			searchimg.Image = UIImage.FromFile ("MLResources/IconsMuro/icon_search.png");
			contentleft.Add (searchimg);
			_searchfield = new UITextField (new CGRect(44,96,120,12)){BackgroundColor = UIColor.Clear};
			_searchfield.Placeholder = "Search";
			_searchfield.Font = UIFont.FromName (fontname, 11);
			_searchfield.TextColor = UIColor.White;
			contentleft.Add (_searchfield);

			//title circles
			var ctitle = new UILabel(new CGRect(28,130,80,16)){ Text = "CIRCULOS" , TextColor = UIColor.White } ;
			ctitle.Font = UIFont.FromName (fontname,12);
			contentleft.Add (ctitle);

			//down buttons
			var set_img = new UIImageView(new CGRect(28,432-48,20,20)){ContentMode = UIViewContentMode.ScaleToFill};
			set_img.Image = UIImage.FromFile ("MLResources/IconsMuro/icon_settings.png");
			contentleft.Add (set_img);

			var log_img = new UIImageView(new CGRect(28,456-46,18,18)){ContentMode = UIViewContentMode.ScaleToFill};
			log_img.Image = UIImage.FromFile ("MLResources/IconsMuro/icon_logout.png");
			contentleft.Add (log_img);

			var backButton = new UIButton (new CGRect (28,456-46,82,22));//{BackgroundColor = UIColor.Red} ; 
			contentleft.Add(backButton);
			backButton.TouchUpInside += (sender, e) => {
				var vm1 = ViewModel as MainViewModel ;
				vm1.LogoutCommand.Execute(null);
			};

			var set_label = new UILabel (new CGRect(56, 435-48,54,14)){TextColor = UIColor.White, Text="Settings"};
			set_label.Font = UIFont.FromName (fontname, 11);
			contentleft.Add (set_label);
			var log_label = new UILabel (new CGRect(56, 459-46,54,14)){TextColor = UIColor.White, Text="Logout"};
			log_label.Font = UIFont.FromName (fontname, 11);
			contentleft.Add (log_label);

			//circles view init
			CirclesView = new UIView(new CGRect(0,148, 192, 228)){BackgroundColor = UIColor.Clear};
			contentleft.Add (CirclesView);

			//addd
			addCircleScroll();
			addUsersScroll ();
			addComments ();
			initInputComment ();
		}

		CircleListView _circlelist ;
		void addCircleScroll()
		{
			_circlelist = new CircleListView (new CGRect(0,0,192,228));
			CirclesView.Add (_circlelist);

			var vm = ViewModel as MainViewModel;
			if (vm.CirclesList != null) _circlelist.CirclesList = vm.CirclesList;
			vm.PropertyChanged += (sender, e) => {
				if(e.PropertyName == "CirclesList") _circlelist.CirclesList = vm.CirclesList;
			};
				
		}


		UsersScrollView _userscrollview;
		void addUsersScroll()
		{
			_userscrollview = new UsersScrollView (new CGRect(0,246,320,100));
			CenterView.Add (_userscrollview);

			var vm = ViewModel as MainViewModel; 
			if (vm.UsersList != null)
				_userscrollview.UsersList = vm.UsersList;
			vm.PropertyChanged += (sender, e) => {
				if(e.PropertyName == "UsersList") _userscrollview.UsersList = vm.UsersList;
			};

		}

		CommentsScrollView _commentScroll;
		void addComments()
		{ 
			_commentScroll = new CommentsScrollView (new CGRect(0,346,320,167));
			CenterView.Add (_commentScroll);
			var vm = ViewModel as MainViewModel;
			if (vm.PostsList != null)
				_commentScroll.PostsList = vm.PostsList;
			vm.PropertyChanged += (sender, e) => {
				if(e.PropertyName == "PostsList") _commentScroll.PostsList = vm.PostsList ;
			};
		}

		UIView NewCommentView ;
		UIButton doCommentButon;
		UITextField inputComment ;
		private void initInputComment()
		{
			NewCommentView = new UIView (new CGRect(0,513,320,45));
			CenterView.Add (NewCommentView);

			var img = new UIImageView (new CGRect(20,12,22,22)){ContentMode = UIViewContentMode.ScaleToFill};
			img.Image = UIImage.FromFile ("MLResources/IconsMuro/icon_enter.png");
			NewCommentView.Add (img);

			inputComment = new UITextField (new CGRect (50,14,250,18)){ Placeholder = "Comment" };
			inputComment.Font = UIFont.FromName (fontname, 11);
			inputComment.TextColor = UIColor.White;
			NewCommentView.Add (inputComment);
		}


		 
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, true);
			if(LoadingView!=null)
				LoadingView.Hide ();
		}


	}
}